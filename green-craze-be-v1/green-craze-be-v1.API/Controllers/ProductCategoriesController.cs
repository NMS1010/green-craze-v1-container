using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.ProductCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/product-categories")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductCategory([FromQuery] GetProductCategoryPagingRequest request)
        {
            var res = await _productCategoryService.GetListProductCategory(request);

            return Ok(APIResponse<PaginatedResult<ProductCategoryDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductCategory([FromRoute] long id)
        {
            var res = await _productCategoryService.GetProductCategory(id);

            return Ok(APIResponse<ProductCategoryDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCategoryBySlug([FromRoute] string slug)
        {
            var res = await _productCategoryService.GetProductCategoryBySlug(slug);

            return Ok(APIResponse<ProductCategoryDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory([FromForm] CreateProductCategoryRequest request)
        {
            var productCategoryId = await _productCategoryService.CreateProductCategory(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/product-categories/{productCategoryId}";

            return Created(url, new APIResponse<object>(new { id = productCategoryId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductCategory([FromRoute] long id, [FromForm] UpdateProductCategoryRequest request)
        {
            var res = await _productCategoryService.UpdateProductCategory(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] long id)
        {
            var res = await _productCategoryService.DeleteProductCategory(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListProductCategory([FromQuery] List<long> ids)
        {
            var res = await _productCategoryService.DeleteListProductCategory(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}