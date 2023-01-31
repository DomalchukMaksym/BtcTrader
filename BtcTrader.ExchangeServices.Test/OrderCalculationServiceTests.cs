using BtcTrader.Models;
using BtcTrader.Models.Enums;
using BtcTrader.Models.Request;
using BtcTrader.Models.Response;
using FluentAssertions;
using Moq;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BtcTrader.ExchangeServices.Test
{
	public class OrderCalculationServiceTests
	{

		private readonly Mock<IInputDataService> _inputDataServiceMock;
		private readonly OrderCalculationService _orderCalculationService;
		public OrderCalculationServiceTests()
		{
			_inputDataServiceMock = new Mock<IInputDataService>();
			_orderCalculationService = new OrderCalculationService(_inputDataServiceMock.Object);
		}

		[Fact]
		public void CalculateBestStrategy__ThrowsValidationExceptionIfEmptyBalances_Sell()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.5M,
				OrderType = OrderType.Sell,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id="1548759600.25189",
						BTCBalance=0,
						EuroBalance=0,
					},
					new ExchangerBalance()
					{
						Id="1548759601.33694",
						BTCBalance=0,
						EuroBalance=0,
					}
				}
			};

			List<OrderResponse> expectedResult = new();

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			Assert.Throws<ValidationException>(()=>_orderCalculationService.CalculateBestStrategy(orderRequest));
		}

		[Fact]
		public void CalculateBestStrategy__ThrowsExceptionIfEmptyBalances_Buy()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.5M,
				OrderType = OrderType.Buy,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id="1548759600.25189",
						BTCBalance=0,
						EuroBalance=0,
					},
					new ExchangerBalance()
					{
						Id="1548759601.33694",
						BTCBalance=0,
						EuroBalance=0,
					}
				}
			};

			List<OrderResponse> expectedResult = new();

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			Assert.Throws<Exception>(() => _orderCalculationService.CalculateBestStrategy(orderRequest));
		}

		[Fact]
		public void CalculateBestStrategy__ThrowsValidationExceptionIfTooSmallBalances_Sell()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.5M,
				OrderType = OrderType.Sell,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id = "1548759600.25189",
						BTCBalance = 0.1M,
						EuroBalance = 0,
					},
					new ExchangerBalance()
					{
						Id="1548759601.33694",
						BTCBalance = 0.2M,
						EuroBalance = 0,
					}
				}
			};

			List<OrderResponse> expectedResult = new();

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			Assert.Throws<ValidationException>(() => _orderCalculationService.CalculateBestStrategy(orderRequest));
		}

		[Fact]
		public void CalculateBestStrategy__ThrowsExceptionIfTooSmallBalances_Buy()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.5M,
				OrderType = OrderType.Buy,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id = "1548759600.25189",
						BTCBalance = 0,
						EuroBalance = 100,
					},
					new ExchangerBalance()
					{
						Id = "1548759601.33694",
						BTCBalance = 0,
						EuroBalance = 200,
					}
				}
			};

			List<OrderResponse> expectedResult = new();

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			Assert.Throws<Exception>(() => _orderCalculationService.CalculateBestStrategy(orderRequest));
		}

		[Fact]
		public void CalculateBestStrategy__ReturnsCorrectDataIfBestOptionsInOneExchanger_Sell()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.3M,
				OrderType = OrderType.Sell,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id="1548759600.25189",
						BTCBalance=0.3M,
						EuroBalance=0,
					},
					new ExchangerBalance()
					{
						Id="1548759601.33694",
						BTCBalance=0.2M,
						EuroBalance=0,
					}
				}
			};

			List<OrderResponse> expectedResult = new()
			{
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.2M,
					Price = 2964.64M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.1M,
					Price = 2964.44M
				}
			};

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			List<OrderResponse> orderResponses = _orderCalculationService.CalculateBestStrategy(orderRequest);

			expectedResult.Should().BeEquivalentTo(orderResponses);
		}

		[Fact]
		public void CalculateBestStrategy__ReturnsCorrectDataIfBestOptionsInSeveralExchangers_Sell()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.4M,
				OrderType = OrderType.Sell,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id="1548759600.25189",
						BTCBalance=0.3M,
						EuroBalance=0,
					},
					new ExchangerBalance()
					{
						Id="1548759601.33694",
						BTCBalance=0.2M,
						EuroBalance=0,
					}
				}
			};

			List<OrderResponse> expectedResult = new()
			{
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.2M,
					Price = 2964.64M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.1M,
					Price = 2964.44M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759601.33694",
					Id = null,
					Amount = 0.1M,
					Price = 2963.33M
				}
			};

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			List<OrderResponse> orderResponses = _orderCalculationService.CalculateBestStrategy(orderRequest);

			expectedResult.Should().BeEquivalentTo(orderResponses);
		}

		[Fact]
		public void CalculateBestStrategy__ReturnsCorrectDataIfBestOptionsInOneExchanger_Buy()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.4M,
				OrderType = OrderType.Buy,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id = "1548759600.25189",
						BTCBalance = 0,
						EuroBalance = 1500,
					},
					new ExchangerBalance()
					{
						Id = "1548759601.33694",
						BTCBalance = 0,
						EuroBalance = 1500,
					}
				}
			};

			List<OrderResponse> expectedResult = new()
			{
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.1M,
					Price = 2950.00M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.3M,
					Price = 2951.11M
				},
			};

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			List<OrderResponse> orderResponses = _orderCalculationService.CalculateBestStrategy(orderRequest);

			expectedResult.Should().BeEquivalentTo(orderResponses);
		}

		[Fact]
		public void CalculateBestStrategy__ReturnsCorrectDataIfBestOptionsInSeveralExchangers_Buy()
		{
			OrderRequest orderRequest = new()
			{
				BTCAmount = 0.6M,
				OrderType = OrderType.Buy,
				ExchangerBalances =
				{
					new ExchangerBalance()
					{
						Id = "1548759600.25189",
						BTCBalance = 0,
						EuroBalance = 1500,
					},
					new ExchangerBalance()
					{
						Id = "1548759601.33694",
						BTCBalance = 0,
						EuroBalance = 1500,
					}
				}
			};

			List<OrderResponse> expectedResult = new()
			{
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.1M,
					Price = 2950.00M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759600.25189",
					Id = null,
					Amount = 0.4M,
					Price = 2951.11M
				},
				new OrderResponse()
				{
					OrderBookId = "1548759601.33694",
					Id = null,
					Amount = 0.1M,
					Price = 2953.33M
				}
			};

			_inputDataServiceMock.Setup(m => m.GetCryptoExchangersByIdList(It.IsAny<List<string>>())).Returns(TestData.GetCryptoExchangers());

			List<OrderResponse> orderResponses = _orderCalculationService.CalculateBestStrategy(orderRequest);

			expectedResult.Should().BeEquivalentTo(orderResponses);
		}

	}
}