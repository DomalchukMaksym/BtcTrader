using BtcTrader.ExchangeServices;
using BtcTrader.Models.Request;
using BtcTrader.Models.Response;
using CommandLine;
using CommandLine.Text;

namespace BtcTrader.ConsoleUI
{
	public class Service
	{
		private readonly OrderCalculationService _orderCalculationService;

		public Service(OrderCalculationService orderCalculationService)
		{
			_orderCalculationService = orderCalculationService;
		}

		public void Start(string[] args)
		{
			try
			{

				Parser parser = new(with =>
				{
					with.IgnoreUnknownArguments = true;
				});
				var parserResult = parser.ParseArguments<RequestInfo>(args);
				parserResult.WithParsed(t =>
						{
							var orderResponses = _orderCalculationService.CalculateBestStrategyWithMinimalInput(t);
							RenderOutputInfo(orderResponses);
							Console.ReadKey();
						});
				parserResult.WithNotParsed(errors => ThrowOnParseError(parserResult, errors));

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			Console.ReadKey();
		}
		private void RenderOutputInfo(List<OrderResponse> orderResponses)
		{
			if (orderResponses != null && orderResponses.Count > 0)
			{
				foreach (var order in orderResponses)
				{
					Console.WriteLine($"OrderBookId: {order.OrderBookId}; Amount: {order.Amount}; Price: {order.Price}");
				}
			}
			else
			{
				Console.WriteLine("Unable to find best strategy for this operation");
			}
			Console.WriteLine("Press any key to try again");
		}

		private static void ThrowOnParseError<T>(ParserResult<T> result, IEnumerable<Error> errors)
		{
			var builder = SentenceBuilder.Create();
			var errorMessages = HelpText.RenderParsingErrorsTextAsLines(result, builder.FormatError, builder.FormatMutuallyExclusiveSetErrors, 1);

			var excList = errorMessages.Select(msg => new ArgumentException(msg)).ToList();

			if (excList.Any())
				throw new AggregateException(excList);
		}
	}
}
