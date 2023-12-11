using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IReviewService
    {
        Task<PaginatedResult<ReviewDto>> GetListReview(GetReviewPagingRequest request);

        Task<List<long>> CountReview(long productId);

        Task<List<ReviewDto>> GetTop5ReviewLatest();

        Task<ReviewDto> GetReview(long id);

        Task<ReviewDto> GetReviewByOrderItem(long orderItemId, string userId);

        Task<long> CreateReview(CreateReviewRequest request);

        Task<bool> UpdateReview(UpdateReviewRequest request);

        Task<bool> ReplyReview(long id, ReplyReviewRequest request);

        Task<bool> DeleteReview(long id);

        Task<bool> ToggleReview(long id);

        Task<bool> DeleteListReview(List<long> ids);
    }
}