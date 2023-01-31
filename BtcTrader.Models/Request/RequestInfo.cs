using BtcTrader.Models.Enums;
using CommandLine;

namespace BtcTrader.Models.Request
{
    public class RequestInfo
	{
		[Option("btcamount", Required = true, HelpText = "Input requested btc amount")]
		public decimal BTCAmount { get; set; }

		[Option("ordertype", Required = true, HelpText = "Input requested order type")]
		public OrderType OrderType { get; set; }

		[Option("btcbalance", Required = true, HelpText = "Input btc balance for all exchangers")]
		public decimal BTCBalance { get; set; }

		[Option("eurobalance", Required = true, HelpText = "Input euro balance for all exchangers")]
		public decimal EuroBalance { get; set; }
	}
}
