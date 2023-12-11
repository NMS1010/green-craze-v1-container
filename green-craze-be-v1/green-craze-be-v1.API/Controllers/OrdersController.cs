using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICurrentUserService _currentUserService;

        public OrdersController(IOrderService orderService, ICurrentUserService currentUserService)
        {
            _orderService = orderService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var code = await _orderService.CreateOrder(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/orders/detail/{code}";

            return Created(url, new APIResponse<object>(new { code }, StatusCodes.Status201Created));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetListOrder([FromQuery] GetOrderPagingRequest request)
        {
            request.UserId = null;
            var orders = await _orderService.GetListOrder(request);

            return Ok(new APIResponse<PaginatedResult<OrderDto>>(orders, StatusCodes.Status200OK));
        }

        [HttpGet("me/list")]
        public async Task<IActionResult> GetListUserOrder([FromQuery] GetOrderPagingRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var orders = await _orderService.GetListUserOrder(request);

            return Ok(new APIResponse<PaginatedResult<OrderDto>>(orders, StatusCodes.Status200OK));
        }

        [HttpGet("top5-order-latest")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetTop5OrderLatest()
        {
            var resp = await _orderService.GetTop5OrderLatest();

            return Ok(new APIResponse<List<OrderDto>>(resp, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetOrder([FromRoute] long id)
        {
            var order = await _orderService.GetOrder(id);

            return Ok(new APIResponse<OrderDto>(order, StatusCodes.Status200OK));
        }

        [HttpGet("detail/{code}")]
        public async Task<IActionResult> GetOrderByCode([FromRoute] string code)
        {
            var order = await _orderService.GetOrderByCode(code, _currentUserService.UserId);

            return Ok(new APIResponse<OrderDto>(order, StatusCodes.Status200OK));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] long id, [FromBody] UpdateOrderRequest request)
        {
            request.OrderId = id;
            if (!_currentUserService.IsInRole(USER_ROLE.ADMIN))
                request.UserId = _currentUserService.UserId;

            var isSuccess = await _orderService.UpdateOrder(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }

        [HttpPut("paypal/{id}")]
        public async Task<IActionResult> CompletePaypalOrder([FromRoute] long id, [FromBody] CompletePaypalOrderRequest request)
        {
            request.OrderId = id;
            request.UserId = _currentUserService.UserId;

            var isSuccess = await _orderService.CompletePaypalOrder(request);

            return Ok(new APIResponse<bool>(isSuccess, StatusCodes.Status204NoContent));
        }
    }
}