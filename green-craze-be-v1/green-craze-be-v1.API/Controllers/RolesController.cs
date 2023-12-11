using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListRole([FromQuery] GetRolePagingRequest request)
        {
            var roles = await _roleService.GetListRole(request);

            return Ok(new APIResponse<PaginatedResult<RoleDto>>(roles, StatusCodes.Status200OK));
        }

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole([FromRoute] string roleId)
        {
            var role = await _roleService.GetRole(roleId);

            return Ok(new APIResponse<RoleDto>(role, StatusCodes.Status200OK));
        }
    }
}