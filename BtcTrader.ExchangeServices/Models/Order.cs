using BtcTrader.Models.Enums;

namespace BtcTrader.ExchangeServices.Models
{
	public class Order
	{
		public string Id { get; set; }

		public DateTime Time { get; set; }

		public OrderType Type { get; set; }

		public OrderKind Kind { get; set; }

		public decimal Amount { get; set; }

		public decimal Price { get; set; }
	}
}
