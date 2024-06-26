using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Models;
using WebShop.Application.Services;

namespace WebShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDto orderRequest)
        {
            var orderId = await _orderService.PlaceOrderAsync(orderRequest);
            return Ok(new { OrderId = orderId });
        }
    }
}
