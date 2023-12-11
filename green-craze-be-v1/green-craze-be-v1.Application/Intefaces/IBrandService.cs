using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IBrandService
    {
        Task<PaginatedResult<BrandDto>> GetListBrand(GetBrandPagingRequest request);

        Task<BrandDto> GetBrand(long id);

        Task<long> CreateBrand(CreateBrandRequest request);

        Task<bool> UpdateBrand(long id, UpdateBrandRequest request);

        Task<bool> DeleteBrand(long id);

        Task<bool> DeleteListBrand(List<long> ids);
    }
}
