using BtcTrader.ExchangeServices;
using BtcTrader.Models.Request;
using BtcTrader.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

		[HttpPost("bestorderstrategy")]
		[ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult CalculateBestOrderStrategy([FromBody] RequestInfo requestInfo)
		{
			try
			{
				var result = _orderCalculationService.CalculateBestStrategyWithMinimalInput(requestInfo);
				return Ok(result);
			}
			catch (ValidationException e)
			{
				return BadRequest(e.Message);
			}
		}

	}
}