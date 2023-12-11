using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;
using green_craze_be_v1.Application.Model.Variant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IVariantService
    {
        Task<PaginatedResult<VariantDto>> GetListVariant(GetVariantPagingRequest request);

        Task<VariantDto> GetVariant(long id);

        Task<List<VariantDto>> GetListVariantByProductId(long productId);

        Task<long> CreateVariant(CreateVariantRequest request);

        Task<bool> UpdateVariant(long id, UpdateVariantRequest request);

        Task<bool> DeleteVariant(long id);

        Task<bool> DeleteListVariant(List<long> ids);
    }
}
