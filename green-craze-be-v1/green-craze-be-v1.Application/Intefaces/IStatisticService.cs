using green_craze_be_v1.Application.Model.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IStatisticService
    {
        Task<StatisticTotalResponse> StatisticTotal();

        Task<List<StatisticRevenueResponse>> StatisticRevenue(StatisticRevenueRequest request);

        Task<List<StatisticTopSellingProductYearResponse>> StatisticTopSellingProductYear(
            StatisticTopSellingProductYearRequest request);

        Task<List<StatisticTopSellingProductResponse>> StatisticTopSellingProduct(
            StatisticTopSellingProductRequest request);

        Task<List<StatisticOrderStatusResponse>> StatisticOrderStatus(
            StatisticOrderStatusRequest request);

        Task<List<StatisticRatingResponse>> StatisticRating(StatisticRatingRequest request);
    }
}
