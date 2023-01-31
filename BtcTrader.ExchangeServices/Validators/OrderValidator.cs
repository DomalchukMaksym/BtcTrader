using BtcTrader.Models.Enums;
using BtcTrader.Models.Request;

namespace BtcTrader.ExchangeServices.Validators
{
    public class OrderValidator
    {
        public static (bool, string) Validate(OrderRequest orderRequest)
        {
            if (orderRequest.BTCAmount < 0)
                return (false, "BTC amount cannot be less than 0");

            if (orderRequest.ExchangerBalances.Any(e => e.EuroBalance < 0 || e.BTCBalance < 0))
                return (false, "Exchanger cannot have euro or BTC balance less then 0");

            if (orderRequest.OrderType == OrderType.Sell && orderRequest.ExchangerBalances.Sum(e => e.BTCBalance) < orderRequest.BTCAmount)
                return (false, "Total BTC balance cannot be less than order BTC amount");

            return (true, "");
        }
    }
}
