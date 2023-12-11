using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ICurrentUserService _currentUserService;

        public ReviewsController(IReviewService reviewService, ICurrentUserService currentUserService)
        {
            _reviewService = reviewService;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetListReview([FromQuery] GetReviewPagingRequest request)
        {
            var res = await _reviewService.GetListReview(request);

            return Ok(APIResponse<PaginatedResult<ReviewDto>>.Initialize(res, StatusCodes.Status200OK));
        }

        [HttpGet("top5-review-latest")]
        public async Task<IActionResult> GetTop5ReviewLatest()
        {
            var res = await _reviewService.GetTop5ReviewLatest();

            return Ok(new APIResponse<List<ReviewDto>>(res, StatusCodes.Status200OK));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview([FromRoute] long id)
        {
            var res = await _reviewService.GetReview(id);

            return Ok(new APIResponse<ReviewDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet("order-item/{orderItemId}")]
        public async Task<IActionResult> GetReviewByOrderItem([FromRoute] long orderItemId)
        {
            var res = await _reviewService.GetReviewByOrderItem(orderItemId, _currentUserService.UserId);

            return Ok(new APIResponse<ReviewDto>(res, StatusCodes.Status200OK));
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] CreateReviewRequest request)
        {
            request.UserId = _currentUserService.UserId;
            var reviewId = await _reviewService.CreateReview(request);
            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/reviews/{reviewId}";

            return Created(url, new APIResponse<object>(new { id = reviewId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview([FromForm] UpdateReviewRequest request, [FromRoute] long id)
        {
            request.Id = id;
            request.UserId = _currentUserService.UserId;
            var res = await _reviewService.UpdateReview(request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpPut("reply/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ReplyReview([FromBody] ReplyReviewRequest request, [FromRoute] long id)
        {
            var res = await _reviewService.ReplyReview(id, request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("count/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CountReview([FromRoute] long productId)
        {
            var res = await _reviewService.CountReview(productId);

            return Ok(new APIResponse<List<long>>(res, StatusCodes.Status200OK));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteReview([FromRoute] long id)
        {
            var res = await _reviewService.DeleteReview(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpPatch("toggle/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ToggleReview([FromRoute] long id)
        {
            var res = await _reviewService.ToggleReview(id);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpDelete]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteListReview([FromQuery] List<long> ids)
        {
            var res = await _reviewService.DeleteListReview(ids);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }
    }
}