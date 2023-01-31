using BtcTrader.Models;
using BtcTrader.Models.Enums;

namespace BtcTrader.Models.Request
{
    public class OrderRequest
    {
        public decimal BTCAmount { get; set; }
        public OrderType OrderType { get; set; }
        public List<ExchangerBalance> ExchangerBalances { get; set; } = new List<ExchangerBalance>();
    }
}
