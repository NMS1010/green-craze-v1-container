using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.UserFollowProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/user-follow-products")]
    [ApiController]
    [Authorize]
    public class UserFollowProductController : ControllerBase
    {
        private readonly IUserFollowProductService _userFollowProductService;
        private readonly ICurrentUserService _currentUserService;

        public UserFollowProductController(IUserFollowProductService userFollowProductService, ICurrentUserService currentUserService)
        {
            _userFollowProductService = userFollowProductService;
            _currentUserService = currentUserService;
        }

        [HttpPost("like")]
        public async Task<ActionResult> UserLikeProduct([FromBody] FollowProductRequest request)
        {
            request.UserId = _currentUserService.UserId;
            await _userFollowProductService.LikeProduct(request);

            return Created("", new APIResponse<object>(new { id = request.ProductId }, StatusCodes.Status201Created));
        }

        [HttpPost("unlike")]
        public async Task<ActionResult> UserUnLikeProduct([FromBody] FollowProductRequest request)
        {
            request.UserId = _currentUserService.UserId;
            await _userFollowProductService.UnLikeProduct(request);

            return Created("", new APIResponse<object>(new { id = request.ProductId }, StatusCodes.Status201Created));
        }

        [HttpGet]
        public async Task<ActionResult> GetListFollowProduct([FromQuery] GetFollowProductPagingRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var resp = await _userFollowProductService.GetListFollowProduct(request);

            return Ok(new APIResponse<PaginatedResult<ProductDto>>(resp, StatusCodes.Status200OK));
        }
    }
}