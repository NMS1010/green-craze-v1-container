using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN, STAFF")]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoriesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult> GetListDocketByProductId([FromRoute] long productId)
        {
            var res = await _inventoryService.GetListDocketByProductId(productId);
            return Ok(new APIResponse<List<DocketDto>>(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<ActionResult> ImportProduct(ImportProductRequest request)
        {
            var res = await _inventoryService.ImportProduct(request);
            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}