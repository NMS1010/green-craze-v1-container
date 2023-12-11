using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.PaymentMethod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/payment-methods")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListPaymentMethod([FromQuery] GetPaymentMethodPagingRequest request)
        {
            var resp = await _paymentMethodService.GetListPaymentMethod(request);

            return Ok(new APIResponse<PaginatedResult<PaymentMethodDto>>(resp, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethod([FromRoute] long id)
        {
            var resp = await _paymentMethodService.GetPaymentMethod(id);

            return Ok(new APIResponse<PaymentMethodDto>(resp, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod([FromForm] CreatePaymentMethodRequest request)
        {
            var paymentMethodId = await _paymentMethodService.CreatePaymentMethod(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/paymentmethods/{paymentMethodId}";

            return Created(url, new APIResponse<object>(new { id = paymentMethodId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] long id, [FromForm] UpdatePaymentMethodRequest request)
        {
            request.Id = id;
            var resp = await _paymentMethodService.UpdatePaymentMethod(request);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] long id)
        {
            var resp = await _paymentMethodService.DeletePaymentMethod(id);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListPaymentMethod([FromQuery] List<long> ids)
        {
            var resp = await _paymentMethodService.DeleteListPaymentMethod(ids);

            return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
        }
    }
}