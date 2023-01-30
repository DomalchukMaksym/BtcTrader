using BtcTrader.ExchangeServices;
using BtcTrader.Models;
using BtcTrader.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace BtcTrader.Controllers
{
	[Route("api/order")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly OrderCalculationService _orderCalculationService;

		public OrdersController(OrderCalculationService orderCalculationService)
		{
			_orderCalculationService = orderCalculationService;
		}

		[HttpPost("bestorderstarategy")]
		[ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult CaluculateBestOrderStrategy([FromBody] RequestInfo requestInfo)
		{
			try
			{
				var result = _orderCalculationService.CalculateBestStrategyWithMinimalInput(requestInfo);
				return Ok(result);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}

		}

	}
}