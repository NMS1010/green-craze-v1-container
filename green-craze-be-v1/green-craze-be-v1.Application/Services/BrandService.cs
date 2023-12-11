using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Specification.Brand;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class BrandService : IBrandService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadService;

		public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_uploadService = uploadService;
		}

		public async Task<PaginatedResult<BrandDto>> GetListBrand(GetBrandPagingRequest request)
		{
			var spec = new BrandSpecification(request, isPaging: true);
			var countSpec = new BrandSpecification(request);

			var brands = await _unitOfWork.Repository<Brand>().ListAsync(spec);
			var count = await _unitOfWork.Repository<Brand>().CountAsync(countSpec);
			var brandDtos = new List<BrandDto>();
			brands.ForEach(x => brandDtos.Add(_mapper.Map<BrandDto>(x)));

			return new PaginatedResult<BrandDto>(brandDtos, request.PageIndex, count, request.PageSize);
		}

		public async Task<BrandDto> GetBrand(long id)
		{
			var brand = await _unitOfWork.Repository<Brand>().GetById(id)
				 ?? throw new NotFoundException("Cannot find current brand");

			return _mapper.Map<BrandDto>(brand);
		}

		public async Task<long> CreateBrand(CreateBrandRequest request)
		{
			var brand = _mapper.Map<Brand>(request);
			brand.Image = _uploadService.UploadFile(request.Image).Result;
			await _unitOfWork.Repository<Brand>().Insert(brand);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot create entity");
			}

			return brand.Id;
		}

		public async Task<bool> UpdateBrand(long id, UpdateBrandRequest request)
		{
			var brand = await _unitOfWork.Repository<Brand>().GetById(id)
				?? throw new NotFoundException("Cannot find current brand");

			brand = _mapper.Map<UpdateBrandRequest, Brand>(request, brand);
			brand.Id = id;
			if (request.Image != null)
			{
				brand.Image = _uploadService.UploadFile(request.Image).Result;
			}

			_unitOfWork.Repository<Brand>().Update(brand);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteBrand(long id)
		{
			var brand = await _unitOfWork.Repository<Brand>().GetById(id)
				?? throw new NotFoundException("Cannot find current brand");

			brand.Status = false;
			_unitOfWork.Repository<Brand>().Update(brand);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteListBrand(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();

				foreach (var id in ids)
				{
					var brand = await _unitOfWork.Repository<Brand>().GetById(id)
						?? throw new NotFoundException("Cannot find current brand");

					brand.Status = false;
					_unitOfWork.Repository<Brand>().Update(brand);
				}
				var isSuccess = await _unitOfWork.Save() > 0;
				if (!isSuccess)
				{
					throw new Exception("Cannot update status of entities");
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
	}
}