using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.OrderCancellationReason;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IOrderCancellationReasonService
    {
        Task<long> CreateOrderCancellationReason(CreateOrderCancellationReasonRequest request);

        Task<bool> UpdateOrderCancellationReason(UpdateOrderCancellationReasonRequest request);

        Task<bool> DeleteOrderCancellationReason(long id);

        Task<bool> DeleteListOrderCancellationReason(List<long> ids);

        Task<PaginatedResult<OrderCancellationReasonDto>> GetListOrderCancellationReason(GetOrderCancellationReasonPagingRequest request);

        Task<OrderCancellationReasonDto> GetOrderCancellationReason(long id);
    }
}