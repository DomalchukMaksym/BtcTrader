
namespace BtcTrader.ExchangeServices.Models
{
	internal class OrderBook
	{
		public DateTimeOffset AcqTime { get; set; }
		public BidsAsks[]? Bids { get; set; }
		public BidsAsks[]? Asks { get; set; }
	}
}
