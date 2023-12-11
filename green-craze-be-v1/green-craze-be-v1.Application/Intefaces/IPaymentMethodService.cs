using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.PaymentMethod;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IPaymentMethodService
    {
        Task<long> CreatePaymentMethod(CreatePaymentMethodRequest request);

        Task<bool> UpdatePaymentMethod(UpdatePaymentMethodRequest request);

        Task<bool> DeletePaymentMethod(long id);

        Task<bool> DeleteListPaymentMethod(List<long> ids);

        Task<PaginatedResult<PaymentMethodDto>> GetListPaymentMethod(GetPaymentMethodPagingRequest request);

        Task<PaymentMethodDto> GetPaymentMethod(long id);
    }
}