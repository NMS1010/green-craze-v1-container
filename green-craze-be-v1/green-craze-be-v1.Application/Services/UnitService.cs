using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Specification.Unit;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class UnitService : IUnitService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<UnitDto>> GetListUnit(GetUnitPagingRequest request)
		{
			var spec = new UnitSpecification(request, isPaging: true);
			var countSpec = new UnitSpecification(request);
			var units = await _unitOfWork.Repository<Unit>().ListAsync(spec);
			var count = await _unitOfWork.Repository<Unit>().CountAsync(countSpec);
			var unitDtos = new List<UnitDto>();
			units.ForEach(x => unitDtos.Add(_mapper.Map<UnitDto>(x)));

			return new PaginatedResult<UnitDto>(unitDtos, request.PageIndex, count, request.PageSize);
		}

		public async Task<UnitDto> GetUnit(long id)
		{
			var unit = await _unitOfWork.Repository<Unit>().GetById(id)
				?? throw new NotFoundException("Cannot find current unit");

			return _mapper.Map<UnitDto>(unit);
		}

		public async Task<long> CreateUnit(CreateUnitRequest request)
		{
			var unit = _mapper.Map<Unit>(request);
			await _unitOfWork.Repository<Unit>().Insert(unit);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot create entity");
			}

			return unit.Id;
		}

		public async Task<bool> UpdateUnit(long id, UpdateUnitRequest request)
		{
			var unit = await _unitOfWork.Repository<Unit>().GetById(id)
				?? throw new NotFoundException("Cannot find current unit");

			unit = _mapper.Map<UpdateUnitRequest, Unit>(request, unit);
			unit.Id = id;

			_unitOfWork.Repository<Unit>().Update(unit);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteUnit(long id)
		{
			var unit = await _unitOfWork.Repository<Unit>().GetById(id)
				?? throw new NotFoundException("Cannot find current unit");

			unit.Status = false;
			_unitOfWork.Repository<Unit>().Update(unit);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteListUnit(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();

				foreach (var id in ids)
				{
					var unit = await _unitOfWork.Repository<Unit>().GetById(id)
						?? throw new NotFoundException("Cannot find current unit");

					unit.Status = false;
					_unitOfWork.Repository<Unit>().Update(unit);
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