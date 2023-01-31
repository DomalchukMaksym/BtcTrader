using BtcTrader.ExchangeServices.Models;
using BtcTrader.ExchangeServices.Validators;
using BtcTrader.Models;
using BtcTrader.Models.Enums;
using BtcTrader.Models.Request;
using BtcTrader.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace BtcTrader.ExchangeServices
{
    public class OrderCalculationService
	{
		private readonly IInputDataService _inputDataService;
		public OrderCalculationService(IInputDataService inputDataService)
		{
			_inputDataService = inputDataService;
		}
		public List<OrderResponse> CalculateBestStrategyWithMinimalInput(RequestInfo requestInfo, string? path = null)
		{
			List<CryptoExchanger> cryptoExchangers = _inputDataService.GetCryptoExchangers();

			var orderRequest = new OrderRequest()
			{
				OrderType = requestInfo.OrderType,
				BTCAmount = requestInfo.BTCAmount,
				ExchangerBalances = cryptoExchangers.Select(ex => new ExchangerBalance()
				{
					Id = ex.Id,
					BTCBalance = requestInfo.BTCBalance,
					EuroBalance = requestInfo.EuroBalance
				}).ToList(),
			};
			return CalculateBestStrategy(orderRequest);
		}

		public List<OrderResponse> CalculateBestStrategy(OrderRequest orderRequest)
		{
			(bool validationResult, string validationMessage) = OrderValidator.Validate(orderRequest);
			if (!validationResult)
				throw new ValidationException(validationMessage);

			List<CryptoExchanger> cryptoExchangers = _inputDataService.GetCryptoExchangersByIdList(orderRequest.ExchangerBalances.Select(b => b.Id).ToList());
			List<OrderResponse> possibleOrders = new();

			if (orderRequest.OrderType == OrderType.Sell)
			{
				foreach (var cryptoExchanger in cryptoExchangers)
				{
					List<OrderResponse> bidOffers = GetExchangerSellBestPossibleOrders(cryptoExchanger, orderRequest);
					if (bidOffers != null && bidOffers.Any())
						possibleOrders.AddRange(bidOffers);
				}
				possibleOrders = possibleOrders.OrderByDescending(o => o.Price)
					.ThenByDescending(o => o.Amount)
					.ToList();
			}
			else if (orderRequest.OrderType == OrderType.Buy)
			{
				foreach (var cryptoExchanger in cryptoExchangers)
				{
					List<OrderResponse> askOffers = GetExchangerBuyBestPossibleOrders(cryptoExchanger, orderRequest);
					if (askOffers != null && askOffers.Any())
						possibleOrders.AddRange(askOffers);
				}

				possibleOrders = possibleOrders.OrderBy(o => o.Price)
					.ThenByDescending(o => o.Amount)
					.ToList();
			}
			else
			{
				throw new Exception("Unknown order type");
			}

			return GetBestStrategyFromPossibleOrders(orderRequest.BTCAmount, possibleOrders);
		}

		private List<OrderResponse> GetBestStrategyFromPossibleOrders(decimal BTCAmount, List<OrderResponse> possibleOrders)
		{
			var result = new List<OrderResponse>(possibleOrders.Count);
			var currentAmount = 0M;

			foreach (var item in possibleOrders)
			{
				if (item.Amount <= BTCAmount - currentAmount)
				{
					result.Add(item);
					currentAmount += item.Amount;
				}
				else if (BTCAmount - currentAmount > 0)
				{
					item.Amount = BTCAmount - currentAmount;
					result.Add(item);
					currentAmount += item.Amount;
				}
				else break;
			}

			if (result.Count == 0 || currentAmount != BTCAmount)
			{
				throw new Exception("Unable to find best strategy for this operation");
			}

			return result;
		}

		private List<OrderResponse> GetExchangerBuyBestPossibleOrders(CryptoExchanger cryptoExchanger, OrderRequest orderRequest)
		{
			ExchangerBalance? exchangerBalance = orderRequest.ExchangerBalances.FirstOrDefault(b => b.Id == cryptoExchanger.Id);
			List<OrderResponse> result = new();
			var asks = cryptoExchanger.Asks.Select(b => new OrderResponse()
			{
				OrderBookId = cryptoExchanger.Id,
				Id = b.Id,
				Amount = b.Amount,
				Price = b.Price
			}).ToList();

			if (asks == null || !asks.Any() || exchangerBalance == null)
				return new();

			decimal currentAmount = 0;
			decimal currentEurBalance = exchangerBalance.EuroBalance;

			foreach (var ask in asks)
			{
				// need to buy entire ask and can do it
				if (currentEurBalance > 0
					&& currentEurBalance / ask.Price >= ask.Amount
					&& orderRequest.BTCAmount - currentAmount >= ask.Amount)
				{
					result.Add(ask);
					currentEurBalance -= ask.Amount * ask.Price;
					currentAmount += ask.Amount;
				}
				else if (currentEurBalance > 0 && orderRequest.BTCAmount - currentAmount > 0)
				{
					decimal maxAmount;
					// can`t buy amount that needed
					if (orderRequest.BTCAmount - currentAmount > currentEurBalance / ask.Price)
					{
						maxAmount = currentEurBalance / ask.Price;
					}
					// need only part of ask
					else
					{
						maxAmount = orderRequest.BTCAmount - currentAmount;
					}

					// too small numbers in balance left
					if (maxAmount == 0)
						break;

					ask.Amount = maxAmount;
					result.Add(ask);
					currentEurBalance -= ask.Amount * ask.Price;
					currentAmount += ask.Amount;

				}
				else
				{
					break;
				}
			}

			return result;
		}

		private List<OrderResponse> GetExchangerSellBestPossibleOrders(CryptoExchanger cryptoExchanger, OrderRequest orderRequest)
		{
			var bids = cryptoExchanger.Bids.Select(b => new OrderResponse()
			{
				OrderBookId = cryptoExchanger.Id,
				Id = b.Id,
				Amount = b.Amount,
				Price = b.Price
			}).ToList();
			ExchangerBalance? exchangerBalance = orderRequest.ExchangerBalances.FirstOrDefault(b => b.Id == cryptoExchanger.Id);
			List<OrderResponse> result = new();

			if (bids == null || !bids.Any() || exchangerBalance == null)
				return new();

			var amount = exchangerBalance.BTCBalance < orderRequest.BTCAmount ? exchangerBalance.BTCBalance : orderRequest.BTCAmount;

			foreach (var bid in bids)
			{
				if (amount >= bid.Amount)
				{
					result.Add(bid);
				}
				else if (amount > 0)
				{
					bid.Amount = amount;
					result.Add(bid);
				}
				else
				{
					break;
				}
				amount -= bid.Amount;
			}

			return result;
		}
	}
}
