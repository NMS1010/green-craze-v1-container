using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Order;
using green_craze_be_v1.Application.Model.Paging;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IOrderService
    {
        Task<string> CreateOrder(CreateOrderRequest request);

        Task<bool> CompletePaypalOrder(CompletePaypalOrderRequest request);

        Task<bool> UpdateOrder(UpdateOrderRequest request);

        Task<OrderDto> GetOrder(long id);

        Task<OrderDto> GetOrderByCode(string code, string userId);

        Task<PaginatedResult<OrderDto>> GetListOrder(GetOrderPagingRequest request);

        Task<List<OrderDto>> GetTop5OrderLatest();

        Task<PaginatedResult<OrderDto>> GetListUserOrder(GetOrderPagingRequest request);
    }
}