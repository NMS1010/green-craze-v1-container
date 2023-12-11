using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.Paging;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IAddressService
    {
        Task<long> CreateAddress(CreateAddressRequest request);

        Task<bool> UpdateAddress(UpdateAddressRequest request);

        Task<bool> DeleteAddress(long id, string userId);

        Task<AddressDto> GetAddress(long id, string userId);

        Task<AddressDto> GetDefaultAddress(string userId);

        Task<PaginatedResult<AddressDto>> GetListAddress(GetAddressPagingRequest request);

        Task<bool> SetAddressDefault(long id, string userId);

        Task<List<ProvinceDto>> GetListProvince();

        Task<List<DistrictDto>> GetListDistrictByProvince(long provinceId);

        Task<List<WardDto>> GetListWardByDistrict(long districtId);
    }
}