using BtcTrader.ExchangeServices;
using BtcTrader.ExchangeServices.Models;
using BtcTrader.Models;
using BtcTrader.Models.Enums;
using BtcTrader.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace BtcTrader.ConsoleUI
{
	public class Service
	{
		private readonly OrderCalculationService _orderCalculationService;
		private readonly RequestInfo _orderRequest = new();

		public Service(OrderCalculationService orderCalculationService)
		{
			_orderCalculationService= orderCalculationService;
		}

		public void Start()
		{
			while (true)
			{
				try
				{
					InputOrderType();
					InputOrderBTCAmount();
					InputBalance(CurrencyTypeEnum.EUR);
					InputBalance(CurrencyTypeEnum.BTC);
					var orderResponses = _orderCalculationService.CalculateBestStrategyWithMinimalInput(_orderRequest);

					RenderOutputInfo(orderResponses);
					Console.ReadKey();
					Console.Clear();
				}
				catch (Exception ex)
				{
					Console.Clear();
					Console.WriteLine(ex.Message);
				}
			}
		}
		private void RenderOutputInfo(List<OrderResponse> orderResponses)
		{
			if (orderResponses!=null && orderResponses.Count > 0)
			{
				foreach (var order in orderResponses)
				{
					Console.WriteLine($"OrderBookId: {order.OrderBookId}; Amount: {order.Amount}; Price: {order.Price}");
				}
			}
			else
			{
				Console.WriteLine("Unable to find best strategy for this operation");
			}
			Console.WriteLine("Press any key to try again");
		}

		private void InputOrderType()
		{
			Console.WriteLine("Inpur order type:");
			string? orderType = Console.ReadLine();

			if (OrderTypeEnum.Buy.ToString().ToLower() == orderType?.ToLower())
			{
				_orderRequest.OrderType = OrderTypeEnum.Buy;

			}
			else if (OrderTypeEnum.Sell.ToString().ToLower() == orderType?.ToLower())
			{
				_orderRequest.OrderType = OrderTypeEnum.Sell;
			}
			else
			{
				throw new ValidationException("Not existing order type");
			}
		}

		private void InputOrderBTCAmount()
		{
			Console.WriteLine("Inpur order BTC amount:");
			string? inputString = Console.ReadLine();

			if (string.IsNullOrEmpty(inputString) || !decimal.TryParse(inputString, out decimal btcAmount))
			{
				throw new ValidationException("Please enter a number for BTC amount");
			}

			_orderRequest.BTCAmount = btcAmount;
		}

		private void InputBalance(CurrencyTypeEnum currencyType)
		{

			Console.WriteLine($"Inpur {currencyType} balance:");
			string? inputString = Console.ReadLine();

			if (string.IsNullOrEmpty(inputString) || !decimal.TryParse(inputString, out decimal currencyBalance))
			{
				throw new ValidationException($"Please enter a number for {currencyType} balance");
			}

			if (currencyType == CurrencyTypeEnum.EUR)
			{
				_orderRequest.EuroBalance = currencyBalance;
			}
			else if (currencyType == CurrencyTypeEnum.BTC)
			{
				_orderRequest.BTCBalance = currencyBalance;
			}
			else
			{
				throw new ValidationException("Unknown currency type");
			}

		}
	}
}
