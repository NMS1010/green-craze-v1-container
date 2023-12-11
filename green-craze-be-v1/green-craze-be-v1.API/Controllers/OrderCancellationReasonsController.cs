using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.OrderCancellationReason;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/order-cancel-reasons")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class OrderCancellationReasonsController : ControllerBase
    {
        private readonly IOrderCancellationReasonService _service;

        public OrderCancellationReasonsController(IOrderCancellationReasonService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListOrderCancellationReason([FromQuery] GetOrderCancellationReasonPagingRequest request)
        {
            var resp = await _service.GetListOrderCancellationReason(request);

            return Ok(new APIResponse<PaginatedResult<OrderCancellationReasonDto>>(resp, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderCancellationReason([FromRoute] long id)
        {
            var resp = await _service.GetOrderCancellationReason(id);

            return Ok(new APIResponse<OrderCancellationReasonDto>(resp, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderCancellationReason([FromBody] CreateOrderCancellationReasonRequest request)
        {
            var id = await _service.CreateOrderCancellationReason(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/ordercancellationreasons/{id}";

            return Created(url, new APIResponse<object>(new { id }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderCancellationReason([FromRoute] long id, [FromBody] UpdateOrderCancellationReasonRequest request)
        {
            request.Id = id;
            var resp = await _service.UpdateOrderCancellationReason(request);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderCancellationReason([FromRoute] long id)
        {
            var resp = await _service.DeleteOrderCancellationReason(id);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListOrderCancellationReason([FromQuery] List<long> ids)
        {
            var resp = await _service.DeleteListOrderCancellationReason(ids);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }
    }
}