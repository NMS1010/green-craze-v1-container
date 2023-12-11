using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.ProductImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;

        public ProductsController(IProductImageService productImageService, IProductService productService)
        {
            _productImageService = productImageService;
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProduct([FromQuery] GetProductPagingRequest request)
        {
            var res = await _productService.GetListProduct(request);

            return Ok(APIResponse<PaginatedResult<ProductDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListFilteringProduct([FromQuery] FilterProductPagingRequest request)
        {
            var res = await _productService.GetListFilteringProduct(request);

            return Ok(APIResponse<PaginatedResult<ProductDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListSearchingProduct([FromQuery] SearchProductPagingRequest request)
        {
            var res = await _productService.GetListSearchingProduct(request);

            return Ok(APIResponse<PaginatedResult<ProductDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            var res = await _productService.GetProduct(id);

            return Ok(APIResponse<ProductDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("detail/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductBySlug([FromRoute] string slug)
        {
            var res = await _productService.GetProductBySlug(slug);

            return Ok(APIResponse<ProductDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var productId = await _productService.CreateProduct(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/products/{productId}";

            return Created(url, new APIResponse<object>(new { id = productId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] long id, [FromForm] UpdateProductRequest request)
        {
            var res = await _productService.UpdateProduct(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            var res = await _productService.DeleteProduct(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListProduct([FromQuery] List<long> ids)
        {
            var res = await _productService.DeleteListProduct(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("images")]
        public async Task<IActionResult> GetListProductImage([FromQuery] long productId)
        {
            var res = await _productImageService.GetListProductImage(productId);

            return Ok(APIResponse<List<ProductImageDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("images/{id}")]
        public async Task<IActionResult> GetProductImage([FromRoute] long id)
        {
            var res = await _productImageService.GetProductImage(id);

            return Ok(APIResponse<ProductImageDto>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpPost("images")]
        public async Task<IActionResult> CreateProductImage([FromForm] CreateProductImageRequest request)
        {
            var resp = await _productImageService.CreateProductImage(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/products/images";

            return Created(url, new APIResponse<object>(new { id = resp }, StatusCodes.Status201Created));
        }

        [HttpPut("images/{id}")]
        public async Task<IActionResult> UpdateProductImage([FromRoute] long id, [FromForm] UpdateProductImageRequest request)
        {
            var res = await _productImageService.UpdateProductImage(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpPatch("images/{id}")]
        public async Task<IActionResult> SetDefaultProductImage([FromRoute] long id, [FromForm] long productId)
        {
            var res = await _productImageService.SetDefaultProductImage(id, productId);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("images/{id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute] long id)
        {
            var res = await _productImageService.DeleteProductImage(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("images")]
        public async Task<IActionResult> DeleteListProductImage([FromQuery] List<long> ids)
        {
            var res = await _productImageService.DeleteListProductImage(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}