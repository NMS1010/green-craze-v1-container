using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Delivery;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Delivery;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class DeliveryService : IDeliveryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadService;

		public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_uploadService = uploadService;
		}

		public async Task<long> CreateDelivery(CreateDeliveryRequest request)
		{
			var delivery = _mapper.Map<Delivery>(request);
			delivery.Status = true;
			if (request.Image != null)
			{
				delivery.Image = await _uploadService.UploadFile(request.Image);
			}
			await _unitOfWork.Repository<Delivery>().Insert(delivery);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to create delivery, an error has occured");
			}

			return delivery.Id;
		}

		public async Task<bool> DeleteDelivery(long id)
		{
			var delivery = await _unitOfWork.Repository<Delivery>().GetById(id);

			delivery.Status = false;

			_unitOfWork.Repository<Delivery>().Update(delivery);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to delete delivery, an error has occured");
			}

			return true;
		}

		public async Task<bool> DeleteListDelivery(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();
				foreach (var id in ids)
				{
					var delivery = await _unitOfWork.Repository<Delivery>().GetById(id);
					delivery.Status = false;

					_unitOfWork.Repository<Delivery>().Update(delivery);
				}
				var isSuccess = await _unitOfWork.Save() > 0;
				await _unitOfWork.Commit();
				if (!isSuccess)
				{
					throw new Exception("Cannot handle to delete list of deliveries, an error has occured");
				}

				return true;
			}
			catch
			{
				await _unitOfWork.Rollback();
				throw;
			}
		}

		public async Task<DeliveryDto> GetDelivery(long id)
		{
			var delivery = await _unitOfWork.Repository<Delivery>().GetById(id);

			return _mapper.Map<DeliveryDto>(delivery);
		}

		public async Task<PaginatedResult<DeliveryDto>> GetListDelivery(GetDeliveryPagingRequest request)
		{
			var deliveries = await _unitOfWork.Repository<Delivery>().ListAsync(new DeliverySpecification(request, isPaging: true));
			var totalCount = await _unitOfWork.Repository<Delivery>().CountAsync(new DeliverySpecification(request));
			//if(request.Status)
			//    deliveries = deliveries.Where(x => x.Status == true).ToList();
			var deliveryDtos = new List<DeliveryDto>();
			deliveries.ForEach(x => deliveryDtos.Add(_mapper.Map<DeliveryDto>(x)));

			return new PaginatedResult<DeliveryDto>(deliveryDtos, request.PageIndex, totalCount, request.PageSize);
		}

		public async Task<bool> UpdateDelivery(UpdateDeliveryRequest request)
		{
			var delivery = await _unitOfWork.Repository<Delivery>().GetById(request.Id);
			var image = delivery.Image;
			var url = "";
			_mapper.Map(request, delivery);
			delivery.Image = image;
			if (request.Image != null)
			{
				url = delivery.Image;
				delivery.Image = await _uploadService.UploadFile(request.Image);
			}

			_unitOfWork.Repository<Delivery>().Update(delivery);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to update delivery, an error has occured");
			}
			if (!string.IsNullOrEmpty(url))
			{
				await _uploadService.DeleteFile(url);
			}

			return true;
		}
	}
}