using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.UserFollowProduct;
using green_craze_be_v1.Application.Specification.UserFollowProduct;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class UserFollowProductService : IUserFollowProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UserFollowProductService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<ProductDto>> GetListFollowProduct(GetFollowProductPagingRequest request)
		{
			var list = await _unitOfWork.Repository<UserFollowProduct>().ListAsync(new UserFollowProductSpecification(request, isPaging: true));
			var count = await _unitOfWork.Repository<UserFollowProduct>().CountAsync(new UserFollowProductSpecification(request));

			var productDtos = new List<ProductDto>();
			foreach (var item in list)
			{
				productDtos.Add(_mapper.Map<ProductDto>(item.Product));
			}

			return new PaginatedResult<ProductDto>(productDtos, request.PageIndex, count, request.PageSize);
		}

		public async Task<bool> LikeProduct(FollowProductRequest request)
		{
			var product = await _unitOfWork.Repository<Product>().GetById(request.ProductId);
			var user = await _unitOfWork.Repository<AppUser>().GetById(request.UserId);

			var res = await _unitOfWork.Repository<UserFollowProduct>().GetEntityWithSpec(new UserFollowProductSpecification(request.UserId, request.ProductId));

			if (res != null)
			{
				throw new InvalidRequestException("User has already liked this product");
			}

			UserFollowProduct userFollowProduct = new()
			{
				Product = product,
				User = user
			};
			await _unitOfWork.Repository<UserFollowProduct>().Insert(userFollowProduct);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot save of entities");
			}

			return isSuccess;
		}

		public async Task<bool> UnLikeProduct(FollowProductRequest request)
		{
			var userFollowProduct = await _unitOfWork.Repository<UserFollowProduct>()
				.GetEntityWithSpec(new UserFollowProductSpecification(request.UserId, request.ProductId))
				?? throw new NotFoundException("Cannot find follow product of user");

			_unitOfWork.Repository<UserFollowProduct>().Delete(userFollowProduct);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot save of entities");
			}

			return isSuccess;
		}
	}
}