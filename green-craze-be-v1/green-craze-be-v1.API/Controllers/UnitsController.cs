using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListUnit([FromQuery] GetUnitPagingRequest request)
        {
            var res = await _unitService.GetListUnit(request);

            return Ok(APIResponse<PaginatedResult<UnitDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnit([FromRoute] long id)
        {
            var res = await _unitService.GetUnit(id);

            return Ok(new APIResponse<UnitDto>(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request)
        {
            var unitId = await _unitService.CreateUnit(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/units/{unitId}";

            return Created(url, new APIResponse<object>(new { id = unitId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit([FromRoute] long id, [FromBody] UpdateUnitRequest request)
        {
            var res = await _unitService.UpdateUnit(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit([FromRoute] long id)
        {
            var res = await _unitService.DeleteUnit(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteListUnit([FromQuery] List<long> ids)
        {
            var res = await _unitService.DeleteListUnit(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}