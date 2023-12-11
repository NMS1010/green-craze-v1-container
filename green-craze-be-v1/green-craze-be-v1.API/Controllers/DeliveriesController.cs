using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Delivery;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveriesController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListDelivery([FromQuery] GetDeliveryPagingRequest request)
        {
            var resp = await _deliveryService.GetListDelivery(request);

            return Ok(new APIResponse<PaginatedResult<DeliveryDto>>(resp, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDelivery([FromRoute] long id)
        {
            var resp = await _deliveryService.GetDelivery(id);

            return Ok(new APIResponse<DeliveryDto>(resp, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromForm] CreateDeliveryRequest request)
        {
            var deliveryId = await _deliveryService.CreateDelivery(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/deliveries/{deliveryId}";

            return Created(url, new APIResponse<object>(new { id = deliveryId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDelivery([FromRoute] long id, [FromForm] UpdateDeliveryRequest request)
        {
            request.Id = id;
            var resp = await _deliveryService.UpdateDelivery(request);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery([FromRoute] long id)
        {
            var resp = await _deliveryService.DeleteDelivery(id);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListDelivery([FromQuery] List<long> ids)
        {
            var resp = await _deliveryService.DeleteListDelivery(ids);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }
    }
}