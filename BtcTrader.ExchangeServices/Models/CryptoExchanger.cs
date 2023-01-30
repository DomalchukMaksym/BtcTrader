namespace BtcTrader.ExchangeServices.Models
{
	public class CryptoExchanger
	{
		public string Id { get; set; }
		public DateTimeOffset AcqTime { get; set; }
		public Order[]? Bids { get; set; }
		public Order[]? Asks { get; set; }
	}
}
