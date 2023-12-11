using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandsController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListBrand([FromQuery] GetBrandPagingRequest request)
        {
            var res = await _brandService.GetListBrand(request);

            return Ok(APIResponse<PaginatedResult<BrandDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrand([FromRoute] long id)
        {
            var res = await _brandService.GetBrand(id);

            return Ok(APIResponse<BrandDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandRequest request)
        {
            var brandId = await _brandService.CreateBrand(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/brands/{brandId}";

            return Created(url, new APIResponse<object>(new { id = brandId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] long id, [FromForm] UpdateBrandRequest request)
        {
            var res = await _brandService.UpdateBrand(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] long id)
        {
            var res = await _brandService.DeleteBrand(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListBrand([FromQuery] List<long> ids)
        {
            var res = await _brandService.DeleteListBrand(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}