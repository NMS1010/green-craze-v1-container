using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Statistic;
using green_craze_be_v1.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace green_craze_be_v1.Infrastructure.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AppDBContext _context;

        public StatisticRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<StatisticTopSellingProductResponse>> StatisticTopSellingProduct(StatisticTopSellingProductRequest request)
        {
            var res = await _context.OrderItems
                .Include(x => x.Order)
                .ThenInclude(x => x.Transaction)
                .Include(x => x.Variant)
                .ThenInclude(x => x.Product)
                .Where(x => x.Order.Status == ORDER_STATUS.DELIVERED
                    && x.Order.Transaction.CompletedAt >= request.StartDate
                    && x.Order.Transaction.CompletedAt <= request.EndDate)
                .GroupBy(x => x.Variant.Product.Id)
                .Select(x => new StatisticTopSellingProductResponse()
                {
                    Name = x.FirstOrDefault().Variant.Product.Name,
                    Value = x.Sum(x => x.Quantity * x.Variant.Quantity)
                })
                .OrderByDescending(x => x.Value)
                .Take(request.Top)
                .ToListAsync();

            return res;
        }
    }
}