using CommandLine;

namespace BtcTrader.Models
{
	public class DataPath
	{
		[Option('p',"path")]
		public string Path { get; set; }
	}
}
