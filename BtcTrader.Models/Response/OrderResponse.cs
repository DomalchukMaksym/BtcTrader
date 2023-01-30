using BtcTrader.Models.Enums;

namespace BtcTrader.Models.Response
{
	public class OrderResponse
	{
		public string OrderBookId { get; set; }

		public object Id { get; set; }

		public decimal Amount { get; set; }

		public decimal Price { get; set; }

	}
}
