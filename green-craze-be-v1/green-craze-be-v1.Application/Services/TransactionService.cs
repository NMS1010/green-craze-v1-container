using AutoMapper;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Statistic;
using green_craze_be_v1.Application.Model.Transaction;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Transaction;
using green_craze_be_v1.Application.Specification.User;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatedResult<TransactionDto>> GetListTransaction(GetTransactionPagingRequest request)
		{
			var transactions = await _unitOfWork.Repository<Domain.Entities.Transaction>()
				.ListAsync(new TransactionSpecification(request, isPaging: true));
			var count = await _unitOfWork.Repository<Domain.Entities.Transaction>()
				.CountAsync(new TransactionSpecification(request));

			return new PaginatedResult<TransactionDto>(transactions.Select(x =>
			{
				var res = _mapper.Map<TransactionDto>(x);
				res.OrderCode = x.Order.Code;
				return res;
			}).ToList(),
				request.PageIndex, count, request.PageSize);
		}

		public async Task<List<StatisticTransactionResponse>> GetTop5TransactionLatest()
		{
			List<StatisticTransactionResponse> transactionDtos = new();
			var transactions = await _unitOfWork.Repository<Transaction>().ListAsync(new TransactionSpecification(5));
			foreach (var transaction in transactions)
			{
				var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderSpecification(transaction.OrderId));
				var user = await _unitOfWork.Repository<AppUser>().GetEntityWithSpec(new UserSpecification(order.User.Id));
				var userDto = _mapper.Map<UserDto>(user);
				var transactionDto = _mapper.Map<TransactionDto>(transaction);
				transactionDtos.Add(new StatisticTransactionResponse(transactionDto, userDto));
			}
			return transactionDtos;
		}

		public async Task<TransactionDto> GetTransaction(long id)
		{
			var transaction = await _unitOfWork.Repository<Domain.Entities.Transaction>()
				.GetEntityWithSpec(new TransactionSpecification(id));

			var dto = _mapper.Map<TransactionDto>(transaction);
			dto.OrderCode = transaction.Order.Code;

			return dto;
		}
	}
}