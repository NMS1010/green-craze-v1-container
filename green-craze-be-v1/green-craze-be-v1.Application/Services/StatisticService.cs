using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Statistic;
using green_craze_be_v1.Application.Specification.Docket;
using green_craze_be_v1.Application.Specification.Order;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Review;
using green_craze_be_v1.Application.Specification.Transaction;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace green_craze_be_v1.Application.Services
{
	public class StatisticService : IStatisticService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly IStatisticRepository _statisticRepository;

		public StatisticService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager, IStatisticRepository statisticRepository)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userManager = userManager;
			_statisticRepository = statisticRepository;
		}

		public async Task<List<StatisticOrderStatusResponse>> StatisticOrderStatus(StatisticOrderStatusRequest request)
		{
			var resp = new List<StatisticOrderStatusResponse>();
			foreach (var status in ORDER_STATUS.Status)
			{
				var count = await _unitOfWork.Repository<Order>()
					.CountAsync(new OrderSpecification(request.StartDate, request.EndDate, status));
				resp.Add(new StatisticOrderStatusResponse(status, count));
			}

			return resp;
		}

		public async Task<List<StatisticRatingResponse>> StatisticRating(StatisticRatingRequest request)
		{
			var resp = new List<StatisticRatingResponse>();
			for (int i = 1; i <= 5; i++)
			{
				var count = await _unitOfWork.Repository<Review>()
					.CountAsync(new ReviewSpecification(request.StartDate, request.EndDate, i));
				resp.Add(new StatisticRatingResponse(i + " Sao", count));
			}

			return resp;
		}

		public async Task<List<StatisticRevenueResponse>> StatisticRevenue(StatisticRevenueRequest request)
		{
			List<StatisticRevenueResponse> resp = new();
			for (int month = 1; month <= 12; month++)
			{
				DateTime firstDate = new(request.Year, month, 1);
				int lastDay = DateTime.DaysInMonth(request.Year, month);
				DateTime lastDate = new(request.Year, month, lastDay);

				var transactions = await _unitOfWork.Repository<Transaction>()
					.ListAsync(new TransactionSpecification(firstDate, lastDate));
				decimal revenue = 0;
				transactions.ForEach(transaction => revenue += transaction.TotalPay);

				var dockets = await _unitOfWork.Repository<Docket>()
					.ListAsync(new DocketSpecification(DOCKET_TYPE.IMPORT, firstDate, lastDate));
				decimal exspense = 0;
				dockets.ForEach(docket => exspense += docket.Quantity * docket.Product.Cost);

				resp.Add(new StatisticRevenueResponse("Tháng " + month, revenue, exspense));
			}

			return resp;
		}

		public async Task<List<StatisticTopSellingProductResponse>> StatisticTopSellingProduct(StatisticTopSellingProductRequest request)
		{
			return await _statisticRepository.StatisticTopSellingProduct(request);
		}

		public async Task<List<StatisticTopSellingProductYearResponse>> StatisticTopSellingProductYear(
			StatisticTopSellingProductYearRequest request)
		{
			List<StatisticTopSellingProductYearResponse> resp = new();
			var products = await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification(5, true));

			for (int month = 1; month <= 12; month++)
			{
				DateTime firstDate = new(request.Year, month, 1);
				int lastDay = DateTime.DaysInMonth(request.Year, month);
				DateTime lastDate = new(request.Year, month, lastDay);
				Dictionary<string, long> data = new();
				foreach (var product in products)
				{
					long sold = 0;
					foreach (var variant in product.Variants)
					{
						var orderItems = await _unitOfWork.Repository<OrderItem>()
							.ListAsync(new OrderItemSpecification(variant.Id, firstDate, lastDate, ORDER_STATUS.DELIVERED));
						orderItems.ForEach(orderItem => sold += orderItem.Quantity * orderItem.Variant.Quantity);
					}
					data[product.Name] = sold;
				}
				resp.Add(new StatisticTopSellingProductYearResponse("Tháng " + month, data));
			}

			return resp;
		}

		public async Task<StatisticTotalResponse> StatisticTotal()
		{
			var transactions = await _unitOfWork.Repository<Transaction>().ListAsync(new TransactionSpecification());
			decimal revenue = 0;
			transactions.ForEach(transaction => revenue += transaction.TotalPay);

			var dockets = await _unitOfWork.Repository<Docket>()
				.ListAsync(new DocketSpecification(DOCKET_TYPE.IMPORT.ToString()));
			decimal exspense = 0;
			dockets.ForEach(docket => exspense += docket.Quantity * docket.Product.Cost);

			var users = await _userManager.GetUsersInRoleAsync(USER_ROLE.USER);
			int countUser = users.Count();

			var orders = await _unitOfWork.Repository<Order>().CountAsync(new OrderSpecification(ORDER_STATUS.DELIVERED));

			return new StatisticTotalResponse(revenue, exspense, countUser, orders);
		}
	}
}