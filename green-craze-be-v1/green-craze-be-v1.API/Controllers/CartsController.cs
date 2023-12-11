using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CartsController : ControllerBase
	{
		private readonly ICartService _cartService;
		private readonly ICurrentUserService _currentUserService;

		public CartsController(ICartService cartService, ICurrentUserService currentUserService)
		{
			_cartService = cartService;
			_currentUserService = currentUserService;
		}

		[HttpPost]
		public async Task<IActionResult> AddVariantItemToCart([FromBody] CreateCartItemRequest request)
		{
			request.UserId = _currentUserService.UserId;
			var res = await _cartService.AddVariantItemToCart(request);

			return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
		}

		[HttpPut("{cartItemId}")]
		public async Task<IActionResult> UpdateCartItemQuantity([FromRoute] long cartItemId, [FromBody] UpdateCartItemRequest request)
		{
			request.CartItemId = cartItemId;
			request.UserId = _currentUserService.UserId;
			var resp = await _cartService.UpdateCartItemQuantity(request);

			return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
		}

		[HttpDelete("{cartItemId}")]
		public async Task<IActionResult> DeleteCartItem([FromRoute] long cartItemId)
		{
			var resp = await _cartService.DeleteCartItem(cartItemId, _currentUserService.UserId);

			return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteListCartItem([FromQuery] List<long> ids)
		{
			var resp = await _cartService.DeleteListCartItem(ids, _currentUserService.UserId);

			return Ok(new APIResponse<bool>(resp, StatusCodes.Status204NoContent));
		}

		[HttpGet]
		public async Task<IActionResult> GetCartByUser([FromQuery] GetCartPagingRequest request)
		{
			request.UserId = _currentUserService.UserId;
			var resp = await _cartService.GetCartByUser(request);

			return Ok(new APIResponse<PaginatedResult<CartItemDto>>(resp, StatusCodes.Status200OK));
		}

		[HttpGet("list-ids")]
		public async Task<IActionResult> GetCartItemByIds([FromQuery] List<long> ids)
		{
			var resp = await _cartService.GetCartItemByIds(ids, _currentUserService.UserId);

			return Ok(new APIResponse<List<CartItemDto>>(resp, StatusCodes.Status200OK));
		}
	}
}