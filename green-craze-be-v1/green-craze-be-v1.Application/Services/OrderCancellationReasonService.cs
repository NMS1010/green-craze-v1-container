using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.OrderCancellationReason;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.OrderCancellationReason;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class OrderCancellationReasonService : IOrderCancellationReasonService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrderCancellationReasonService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<long> CreateOrderCancellationReason(CreateOrderCancellationReasonRequest request)
		{
			var orderCancellationReason = _mapper.Map<OrderCancellationReason>(request);
			orderCancellationReason.Status = true;
			if (orderCancellationReason.Note == null)
			{
				orderCancellationReason.Note = string.Empty;
			}

			await _unitOfWork.Repository<OrderCancellationReason>().Insert(orderCancellationReason);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to create orderCancellationReason, an error has occured");
			}

			return orderCancellationReason.Id;
		}

		public async Task<bool> DeleteOrderCancellationReason(long id)
		{
			var orderCancellationReason = await _unitOfWork.Repository<OrderCancellationReason>().GetById(id);

			orderCancellationReason.Status = false;

			_unitOfWork.Repository<OrderCancellationReason>().Update(orderCancellationReason);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to delete orderCancellationReason, an error has occured");
			}

			return true;
		}

		public async Task<bool> DeleteListOrderCancellationReason(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();
				foreach (var id in ids)
				{
					var orderCancellationReason = await _unitOfWork.Repository<OrderCancellationReason>().GetById(id);
					orderCancellationReason.Status = false;

					_unitOfWork.Repository<OrderCancellationReason>().Update(orderCancellationReason);
				}
				var isSuccess = await _unitOfWork.Save() > 0;
				await _unitOfWork.Commit();
				if (!isSuccess)
				{
					throw new Exception("Cannot handle to delete list of orderCancellationReason, an error has occured");
				}

				return true;
			}
			catch
			{
				await _unitOfWork.Rollback();
				throw;
			}
		}

		public async Task<OrderCancellationReasonDto> GetOrderCancellationReason(long id)
		{
			var orderCancellationReason = await _unitOfWork.Repository<OrderCancellationReason>().GetById(id);

			return _mapper.Map<OrderCancellationReasonDto>(orderCancellationReason);
		}

		public async Task<PaginatedResult<OrderCancellationReasonDto>> GetListOrderCancellationReason(GetOrderCancellationReasonPagingRequest request)
		{
			var orderCancellationReasons = await _unitOfWork.Repository<OrderCancellationReason>()
				.ListAsync(new OrderCancellationReasonSpecification(request, isPaging: true));

			var totalCount = await _unitOfWork.Repository<OrderCancellationReason>()
				.CountAsync(new OrderCancellationReasonSpecification(request));

			var orderCancellationReasonDtos = new List<OrderCancellationReasonDto>();
			//if (request.Status)
			//    orderCancellationReasons = orderCancellationReasons.Where(x => x.Status == true).ToList();
			orderCancellationReasons.ForEach(x => orderCancellationReasonDtos.Add(_mapper.Map<OrderCancellationReasonDto>(x)));

			return new PaginatedResult<OrderCancellationReasonDto>(orderCancellationReasonDtos, request.PageIndex,
				totalCount, request.PageSize);
		}

		public async Task<bool> UpdateOrderCancellationReason(UpdateOrderCancellationReasonRequest request)
		{
			var orderCancellationReason = await _unitOfWork.Repository<OrderCancellationReason>()
				.GetById(request.Id);

			_mapper.Map(request, orderCancellationReason);

			_unitOfWork.Repository<OrderCancellationReason>().Update(orderCancellationReason);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to update orderCancellationReason, an error has occured");
			}

			return true;
		}
	}
}