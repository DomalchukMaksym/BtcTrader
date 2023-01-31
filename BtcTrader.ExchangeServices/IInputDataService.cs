using BtcTrader.ExchangeServices.Models;

namespace BtcTrader.ExchangeServices
{
	public interface IInputDataService
	{
		public List<CryptoExchanger> GetCryptoExchangersByIdList(List<string> ids);
		public List<CryptoExchanger> GetCryptoExchangers();
	}
}