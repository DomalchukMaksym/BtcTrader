using BtcTrader.Models;
using BtcTrader.Models.Enums;

namespace BtcTrader.ExchangeServices.Models
{
	public class OrderRequest
	{
		public decimal BTCAmount { get; set; }
		public OrderTypeEnum OrderType { get; set; }
		public List<ExchangerBalance> ExchangerBalances { get; set; } = new List<ExchangerBalance>();
	}
}
