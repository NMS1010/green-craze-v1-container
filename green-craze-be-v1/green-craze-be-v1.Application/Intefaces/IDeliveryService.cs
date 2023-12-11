using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Delivery;
using green_craze_be_v1.Application.Model.Paging;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IDeliveryService
    {
        Task<long> CreateDelivery(CreateDeliveryRequest request);

        Task<bool> UpdateDelivery(UpdateDeliveryRequest request);

        Task<bool> DeleteDelivery(long id);

        Task<bool> DeleteListDelivery(List<long> ids);

        Task<PaginatedResult<DeliveryDto>> GetListDelivery(GetDeliveryPagingRequest request);

        Task<DeliveryDto> GetDelivery(long id);
    }
}