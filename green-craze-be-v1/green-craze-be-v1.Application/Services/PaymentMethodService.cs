using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.PaymentMethod;
using green_craze_be_v1.Application.Specification.PaymentMethod;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class PaymentMethodService : IPaymentMethodService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadService;

		public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_uploadService = uploadService;
		}

		public async Task<long> CreatePaymentMethod(CreatePaymentMethodRequest request)
		{
			var paymentMethod = _mapper.Map<PaymentMethod>(request);
			paymentMethod.Status = true;
			if (request.Image != null)
			{
				paymentMethod.Image = await _uploadService.UploadFile(request.Image);
			}
			await _unitOfWork.Repository<PaymentMethod>().Insert(paymentMethod);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to create payment method, an error has occured");
			}

			return paymentMethod.Id;
		}

		public async Task<bool> DeletePaymentMethod(long id)
		{
			var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(id);

			paymentMethod.Status = false;

			_unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to delete payment method, an error has occured");
			}

			return true;
		}

		public async Task<bool> DeleteListPaymentMethod(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();
				foreach (var id in ids)
				{
					var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(id);
					paymentMethod.Status = false;

					_unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);
				}
				var isSuccess = await _unitOfWork.Save() > 0;
				await _unitOfWork.Commit();
				if (!isSuccess)
				{
					throw new Exception("Cannot handle to delete list of payment method, an error has occured");
				}

				return true;
			}
			catch
			{
				await _unitOfWork.Rollback();
				throw;
			}
		}

		public async Task<PaymentMethodDto> GetPaymentMethod(long id)
		{
			var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(id);

			return _mapper.Map<PaymentMethodDto>(paymentMethod);
		}

		public async Task<PaginatedResult<PaymentMethodDto>> GetListPaymentMethod(GetPaymentMethodPagingRequest request)
		{
			var paymentMethods = await _unitOfWork.Repository<PaymentMethod>().ListAsync(new PaymentMethodSpecification(request, isPaging: true));
			var totalCount = await _unitOfWork.Repository<PaymentMethod>().CountAsync(new PaymentMethodSpecification(request));
			//if (request.Status)
			//    paymentMethods = paymentMethods.Where(x => x.Status == true).ToList();
			var paymentMethodDtos = new List<PaymentMethodDto>();
			paymentMethods.ForEach(x => paymentMethodDtos.Add(_mapper.Map<PaymentMethodDto>(x)));

			return new PaginatedResult<PaymentMethodDto>(paymentMethodDtos, request.PageIndex, totalCount, request.PageSize);
		}

		public async Task<bool> UpdatePaymentMethod(UpdatePaymentMethodRequest request)
		{
			var paymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetById(request.Id);
			var image = paymentMethod.Image;
			var url = "";
			_mapper.Map(request, paymentMethod);
			paymentMethod.Image = image;

			if (request.Image != null)
			{
				url = paymentMethod.Image;
				paymentMethod.Image = await _uploadService.UploadFile(request.Image);
			}

			_unitOfWork.Repository<PaymentMethod>().Update(paymentMethod);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot handle to update payment method, an error has occured");
			}
			if (!string.IsNullOrEmpty(url))
			{
				await _uploadService.DeleteFile(url);
			}

			return true;
		}
	}
}