using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Variant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class VariantsController : ControllerBase
    {
        private readonly IVariantService _variantService;

        public VariantsController(IVariantService variantService)
        {
            _variantService = variantService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetListVariant([FromQuery] GetVariantPagingRequest request)
        //{
        //    var res = await _variantService.GetListVariant(request);

        //    return Ok(APIResponse<PaginatedResult<VariantDto>>.Initialize(res, StatusCodes.Status200OK));
        //}

        [HttpGet]
        public async Task<IActionResult> GetListVariant([FromQuery] long productId)
        {
            var res = await _variantService.GetListVariantByProductId(productId);

            return Ok(APIResponse<List<VariantDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVariant([FromRoute] long id)
        {
            var res = await _variantService.GetVariant(id);

            return Ok(APIResponse<VariantDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant([FromBody] CreateVariantRequest request)
        {
            var variantId = await _variantService.CreateVariant(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/variants/{variantId}";

            return Created(url, new APIResponse<object>(new { id = variantId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVariant([FromRoute] long id, [FromBody] UpdateVariantRequest request)
        {
            var res = await _variantService.UpdateVariant(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariant([FromRoute] long id)
        {
            var res = await _variantService.DeleteVariant(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListVariant([FromQuery] List<long> ids)
        {
            var res = await _variantService.DeleteListVariant(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}