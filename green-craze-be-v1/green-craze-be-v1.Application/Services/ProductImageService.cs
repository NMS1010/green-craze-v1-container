using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.ProductImage;
using green_craze_be_v1.Application.Specification.ProductImage;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class ProductImageService : IProductImageService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadService;

		public ProductImageService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_uploadService = uploadService;
		}

		public async Task<List<ProductImageDto>> GetListProductImage(long productId)
		{
			var productImages = await _unitOfWork.Repository<ProductImage>()
				.ListAsync(new ProductImageSpecification(productId));
			var productImageDtos = new List<ProductImageDto>();
			productImages.ForEach(productImage => productImageDtos.Add(_mapper.Map<ProductImageDto>(productImage)));

			return productImageDtos;
		}

		public async Task<ProductImageDto> GetProductImage(long id)
		{
			var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id)
				?? throw new NotFoundException("Cannot find current product image");

			return _mapper.Map<ProductImageDto>(productImage);
		}

		public async Task<long> CreateProductImage(CreateProductImageRequest request)
		{
			ProductImageDto productImageDto = new()
			{
				ProductId = request.ProductId,
				Image = _uploadService.UploadFile(request.Image).Result,
				Size = request.Image.Length,
				ContentType = request.Image.ContentType
			};
			var productImage = _mapper.Map<ProductImage>(productImageDto);
			productImage.Product = await _unitOfWork.Repository<Product>().GetById(request.ProductId)
				?? throw new NotFoundException("Cannot find current product");

			await _unitOfWork.Repository<ProductImage>().Insert(productImage);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot create entity");
			}
			return request.ProductId;
		}

		public async Task<bool> UpdateProductImage(long id, UpdateProductImageRequest request)
		{
			var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id)
				?? throw new NotFoundException("Cannot find current product image");

			productImage.Image = _uploadService.UploadFile(request.Image).Result;
			productImage.Size = request.Image.Length;
			productImage.ContentType = request.Image.ContentType;
			_unitOfWork.Repository<ProductImage>().Update(productImage);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> SetDefaultProductImage(long id, long productId)
		{
			var productImageDefault = await _unitOfWork.Repository<ProductImage>()
				.GetEntityWithSpec(new ProductImageSpecification(productId, true))
				?? throw new NotFoundException("Cannot find current product image");

			var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id)
				?? throw new NotFoundException("Cannot find current product image");

			productImageDefault.IsDefault = false;
			_unitOfWork.Repository<ProductImage>().Update(productImageDefault);

			productImage.IsDefault = true;
			_unitOfWork.Repository<ProductImage>().Update(productImage);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteProductImage(long id)
		{
			var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id)
				?? throw new NotFoundException("Cannot find current product image");

			if (productImage.IsDefault)
			{
				throw new Exception("Cannot delete product image default");
			}
			_unitOfWork.Repository<ProductImage>().Delete(productImage);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot delete product image default");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteListProductImage(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();

				foreach (var id in ids)
				{
					var productImage = await _unitOfWork.Repository<ProductImage>().GetById(id)
						?? throw new NotFoundException("Cannot find current product image");

					if (productImage.IsDefault)
					{
						throw new Exception("Cannot delete product image default");
					}
					_unitOfWork.Repository<ProductImage>().Delete(productImage);
				}
				var isSuccess = await _unitOfWork.Save() > 0;
				if (!isSuccess)
				{
					throw new Exception("Cannot delete product image default");
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
	}
}