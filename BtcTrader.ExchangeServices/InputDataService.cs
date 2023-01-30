using BtcTrader.ExchangeServices.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace BtcTrader.ExchangeServices
{
	public class InputDataService
	{
		private readonly List<CryptoExchanger> _orderBooks = new List<CryptoExchanger>();
		private static readonly Regex pattern = new(@"(?<id>\d+(\.+\d+)*)\s*(?<json>\{(.+|\s+)})");

		public void ParseData(string path)
		{
			if (_orderBooks.Any())
				return;

			var text = File.ReadAllText(path);

			var matchCollection = pattern.Matches(text);

			if (matchCollection == null)
				throw new Exception("The data file does not match the regular expression");

			foreach (Match match in matchCollection.Cast<Match>())
			{
				string id = match.Groups["id"].Value;
				string json = match.Groups["json"].Value;

				OrderBook? orderBook = JsonConvert.DeserializeObject<OrderBook>(json);
				if (orderBook != null)
					_orderBooks.Add(new CryptoExchanger()
					{
						Id = id,
						AcqTime = orderBook.AcqTime,
						Bids = orderBook.Bids?.Select(x => x.Order).OrderByDescending(x => x.Price).ThenByDescending(x => x.Amount).ToArray(),
						Asks = orderBook.Asks?.Select(x => x.Order).OrderBy(x => x.Price).ThenByDescending(x => x.Amount).ToArray(),
					});
			}
		}

		public List<CryptoExchanger> GetCryptoExchangers()
		{
			return _orderBooks;
		}

		public List<CryptoExchanger> GetCryptoExchangersByIdList(List<string> ids)
		{
			return _orderBooks.Where(book => ids.Exists(id => id == book.Id)).ToList();
		}
	}
}