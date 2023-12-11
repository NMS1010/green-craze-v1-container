using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Review;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Review;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<PaginatedResult<ReviewDto>> GetListReview(GetReviewPagingRequest request)
        {
            var spec = new ReviewSpecification(request, isPaging: true);
            var countSpec = new ReviewSpecification(request);

            var reviews = await _unitOfWork.Repository<Review>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Review>().CountAsync(countSpec);

            var reviewDtos = new List<ReviewDto>();
            reviews.ForEach(x =>
            {
                var dto = _mapper.Map<ReviewDto>(x);
                dto.VariantName = x?.OrderItem?.Variant?.Name;
                reviewDtos.Add(dto);
            });

            return new PaginatedResult<ReviewDto>(reviewDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<List<ReviewDto>> GetTop5ReviewLatest()
        {
            var reviewDtos = new List<ReviewDto>();
            var reviews = await _unitOfWork.Repository<Review>().ListAsync(new ReviewSpecification(5));
            reviews.ForEach(x => reviewDtos.Add(_mapper.Map<ReviewDto>(x)));
            return reviewDtos;
        }

        public async Task<ReviewDto> GetReview(long id)
        {
            var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(true, id)) ??
            throw new InvalidRequestException("Unexpected reviewId");

            var dto = _mapper.Map<ReviewDto>(review);
            dto.VariantName = review?.OrderItem?.Variant?.Name;

            return dto;
        }

        public async Task<long> CreateReview(CreateReviewRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var x = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(request.OrderItemId, request.UserId));

                if (x != null) throw new InvalidRequestException("Unexpected orderItemId, this order item has been reviewed");

                var review = _mapper.Map<Review>(request);
                if (request.Image != null)
                    review.Image = await _uploadService.UploadFile(request.Image);
                var product = await _unitOfWork.Repository<Product>().GetById(request.ProductId) ??
                    throw new InvalidRequestException("Unexpected productId");
                var user = await _unitOfWork.Repository<AppUser>().GetById(request.UserId) ??
                    throw new NotFoundException("Cannot found current user");
                var orderItem = await _unitOfWork.Repository<OrderItem>().GetEntityWithSpec(new OrderItemSpecification(request.OrderItemId, ORDER_STATUS.DELIVERED)) ??
                    throw new InvalidRequestException("Unexpected orderItemId, order has not been deliveried");

                review.Product = product;
                review.User = user;
                review.OrderItem = orderItem;
                await _unitOfWork.Repository<Review>().Insert(review);
                await _unitOfWork.Save();
                await CalculateProductReview(review.Product);
                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot create entity");
                }
                await _unitOfWork.Commit();

                return review.Id;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> ReplyReview(long id, ReplyReviewRequest request)
        {
            var review = await _unitOfWork.Repository<Review>().GetById(id);
            review.Reply = request.Reply;
            _unitOfWork.Repository<Review>().Update(review);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteReview(long id)
        {
            try
            {
                var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(true, id)) ??
                throw new InvalidRequestException("Unexpected reviewId");

                review.Status = false;
                _unitOfWork.Repository<Review>().Update(review);
                await _unitOfWork.Save();
                await CalculateProductReview(review.Product);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entity");
                }

                return isSuccess;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteListReview(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(true, id)) ??
                        throw new InvalidRequestException("Unexpected reviewId");
                    review.Status = false;

                    _unitOfWork.Repository<Review>().Update(review);
                    await _unitOfWork.Save();

                    await CalculateProductReview(review.Product);
                }

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
                }
                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        private async Task CalculateProductReview(Product product)
        {
            var reviews = await _unitOfWork.Repository<Review>().ListAsync(new ReviewSpecification(product.Id, true));
            product.Rating = reviews.Count == 0 ? 0 : reviews.Average(x => x.Rating);
            product.Rating = Math.Round(product.Rating.Value, 1);
            _unitOfWork.Repository<Product>().Update(product);
        }

        public async Task<bool> UpdateReview(UpdateReviewRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(request.UserId, request.Id))
                ?? throw new InvalidRequestException("Unexpected reviewId");

                review.Title = request.Title;
                review.Content = request.Content;
                review.Rating = request.Rating;
                if (request.Image != null)
                    review.Image = await _uploadService.UploadFile(request.Image);
                else if (request.IsDeleteImage)
                    review.Image = null;
                _unitOfWork.Repository<Review>().Update(review);
                await _unitOfWork.Save();
                await CalculateProductReview(review.Product);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update review");
                }
                await _unitOfWork.Commit();

                return true;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<ReviewDto> GetReviewByOrderItem(long orderItemId, string userId)
        {
            var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(orderItemId, userId));
            if (review == null)
                return null;
            var res = _mapper.Map<ReviewDto>(review);
            res.ProductId = review.Product.Id;

            return res;
        }

        public async Task<bool> ToggleReview(long id)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var review = await _unitOfWork.Repository<Review>().GetEntityWithSpec(new ReviewSpecification(true, id)) ??
                throw new InvalidRequestException("Unexpected reviewId");

                review.Status = !review.Status;
                _unitOfWork.Repository<Review>().Update(review);
                await _unitOfWork.Save();

                await CalculateProductReview(review.Product);

                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot toggle status of entity");
                }
                await _unitOfWork.Commit();
                return isSuccess;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<List<long>> CountReview(long productId)
        {
            var listProductReviews = await _unitOfWork.Repository<Review>().ListAsync(new ReviewSpecification(productId, true));
            var listReviews = new List<long>()
            {
                listProductReviews.Count,
                listProductReviews.Count(x => x.Rating == 5),
                listProductReviews.Count(x => x.Rating == 4),
                listProductReviews.Count(x => x.Rating == 3),
                listProductReviews.Count(x => x.Rating == 2),
                listProductReviews.Count(x => x.Rating == 1)
            };

            return listReviews;
        }
    }
}