using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        public UsersController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var res = await _userService.ChangePassword(request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status200OK));
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ToggleUserStatus([FromRoute] string userId)
        {
            var res = await _userService.ToggleUserStatus(userId);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status200OK));
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DisableListUsersStatus([FromQuery] List<string> ids)
        {
            var res = await _userService.DisableListUserStatus(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status200OK));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var res = await _userService.UpdateUser(request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            var res = await _userService.GetUser(userId);

            return Ok(new APIResponse<UserDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet("profile/me")]
        public async Task<IActionResult> GetMe()
        {
            var res = await _userService.GetUser(_currentUserService.UserId);

            return Ok(new APIResponse<UserDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetListUser([FromQuery] GetUserPagingRequest request)
        {
            var res = await _userService.GetListUser(request);

            return Ok(new APIResponse<PaginatedResult<UserDto>>(res, StatusCodes.Status200OK));
        }
    }
}