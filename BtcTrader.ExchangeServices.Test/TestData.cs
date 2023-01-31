using BtcTrader.ExchangeServices.Models;
using BtcTrader.Models.Enums;

namespace BtcTrader.ExchangeServices.Test
{
	public class TestData
	{
		public static List<CryptoExchanger> GetCryptoExchangers()
		{
			return new List<CryptoExchanger>
			{
				new CryptoExchanger()
				{
					Id = "1548759600.25189",
					AcqTime = DateTime.Today,
					Bids = new Order[]
					{
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 0.2M,
								Price = 2964.64M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 0.1M,
								Price = 2964.44M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 0.3M,
								Price = 2960.64M
						},
					},
					Asks = new Order[]
					{
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.1M,
								Price = 2950.00M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.4M,
								Price = 2951.11M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.1M,
								Price = 2957.77M
						},
					},
				},
				new CryptoExchanger()
				{
					Id = "1548759601.33694",
					AcqTime = DateTime.Today,
					Bids = new Order[]
					{
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 0.8M,
								Price = 2963.33M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 2.1M,
								Price = 2961.11M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Buy,
								Kind = OrderKind.Limit,
								Amount = 1.3M,
								Price = 2955.55M
						},
					},
					Asks = new Order[]
					{
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.1M,
								Price = 2953.33M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.4M,
								Price = 2954.44M
						},
						new Order()
						{
								Id = null,
								Time = DateTime.Today,
								Type = OrderType.Sell,
								Kind = OrderKind.Limit,
								Amount = 0.7M,
								Price = 2956.66M
						},
					},
				}
			};
		}
	}
}
