using BtcTrader.Models.Enums;

namespace BtcTrader.Models
{
	public class RequestInfo
	{
		public decimal BTCAmount { get; set; }
		public OrderTypeEnum OrderType { get; set; }
		public decimal BTCBalance { get; set; }
		public decimal EuroBalance { get; set; }
	}
}
