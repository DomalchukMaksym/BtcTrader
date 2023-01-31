using BtcTrader.ExchangeServices.Models;
using BtcTrader.Models;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace BtcTrader.ExchangeServices
{
	public class InputDataService : IInputDataService
	{
		private readonly List<CryptoExchanger> _cryptoExchangers = new();
		private static readonly Regex _pattern = new(@"(?<id>\d+(\.+\d+)*)\s*(?<json>\{(.+|\s+)})");

		private string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\order_books_data");

		public InputDataService(IConfiguration configuration)
		{
			string? configPath = configuration["DataPath"];
			// set path from configuration
			if (!string.IsNullOrEmpty(configPath))
			{
				_path = configPath;
			}
			// set path from command line or leave default
			else
			{
				string[] args = Environment.GetCommandLineArgs();
				Parser parser = new(with =>
				{
					with.IgnoreUnknownArguments = true;
				});
				parser.ParseArguments<DataPath>(args)
						.WithParsed(o => _path = o.Path ?? _path);
			}

			ParseData();
		}

		private void ParseData()
		{
			var text = File.ReadAllText(_path);

			var matchCollection = _pattern.Matches(text);

			if (matchCollection == null)
				throw new Exception("The data file does not match the regular expression");

			foreach (Match match in matchCollection.Cast<Match>())
			{
				string id = match.Groups["id"].Value;
				string json = match.Groups["json"].Value;

				OrderBook? orderBook = JsonConvert.DeserializeObject<OrderBook>(json);
				if (orderBook != null)
					_cryptoExchangers.Add(new CryptoExchanger()
					{
						Id = id,
						AcqTime = orderBook.AcqTime,
						Bids = orderBook.Bids.Select(x => x.Order).OrderByDescending(x => x.Price).ThenByDescending(x => x.Amount).ToArray(),
						Asks = orderBook.Asks.Select(x => x.Order).OrderBy(x => x.Price).ThenByDescending(x => x.Amount).ToArray(),
					});
			}
		}

		public List<CryptoExchanger> GetCryptoExchangers()
		{
			return _cryptoExchangers;
		}

		public List<CryptoExchanger> GetCryptoExchangersByIdList(List<string> ids)
		{
			return _cryptoExchangers.Where(book => ids.Exists(id => id == book.Id)).ToList();
		}
	}
}