using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Variant;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class VariantService : IVariantService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public VariantService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<VariantDto>> GetListVariant(GetVariantPagingRequest request)
		{
			var spec = new VariantSpecification(request, isPaging: true);
			var countSpec = new VariantSpecification(request);
			var variants = await _unitOfWork.Repository<Variant>().ListAsync(spec);
			var count = await _unitOfWork.Repository<Variant>().CountAsync(countSpec);
			var variantDtos = new List<VariantDto>();
			variants.ForEach(variant =>
			{
				var variantDto = _mapper.Map<VariantDto>(variant);
				variantDtos.Add(variantDto);
			});

			return new PaginatedResult<VariantDto>(variantDtos, request.PageIndex, count, request.PageSize);
		}

		public async Task<List<VariantDto>> GetListVariantByProductId(long productId)
		{
			var variants = await _unitOfWork.Repository<Variant>().ListAsync(new VariantSpecification(productId, true));
			var variantDtos = new List<VariantDto>();
			variants.ForEach(variant => variantDtos.Add(_mapper.Map<VariantDto>(variant)));

			return variantDtos;
		}

		public async Task<VariantDto> GetVariant(long id)
		{
			var spec = new VariantSpecification(id);
			var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(spec)
				?? throw new NotFoundException("Cannot find current variant");

			var variantDto = _mapper.Map<VariantDto>(variant);

			return variantDto;
		}

		public async Task<long> CreateVariant(CreateVariantRequest request)
		{
			var variant = _mapper.Map<Variant>(request);
			variant.Product = await _unitOfWork.Repository<Product>().GetById(request.ProductId)
				?? throw new NotFoundException("Cannot find current product");

			variant.Status = VARIANT_STATUS.ACTIVE;
			await _unitOfWork.Repository<Variant>().Insert(variant);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot create entity");
			}

			return variant.Id;
		}

		public async Task<bool> UpdateVariant(long id, UpdateVariantRequest request)
		{
			var variant = await _unitOfWork.Repository<Variant>().GetById(id)
				?? throw new NotFoundException("Cannot find current variant");

			variant = _mapper.Map<UpdateVariantRequest, Variant>(request, variant);
			variant.Id = id;
			variant.Status = variant.Status switch
			{
				VARIANT_STATUS.ACTIVE => VARIANT_STATUS.ACTIVE,
				VARIANT_STATUS.INACTIVE => VARIANT_STATUS.INACTIVE,
				_ => throw new InvalidRequestException("Unexpected variant status: " + request.Status),
			};
			_unitOfWork.Repository<Variant>().Update(variant);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteVariant(long id)
		{
			var variant = await _unitOfWork.Repository<Variant>().GetById(id)
				?? throw new NotFoundException("Cannot find current variant");

			variant.Status = VARIANT_STATUS.INACTIVE;
			_unitOfWork.Repository<Variant>().Update(variant);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteListVariant(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();

				foreach (var id in ids)
				{
					var variant = await _unitOfWork.Repository<Variant>().GetById(id)
						?? throw new NotFoundException("Cannot find current variant");

					variant.Status = VARIANT_STATUS.INACTIVE;
					_unitOfWork.Repository<Variant>().Update(variant);
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
	}
}