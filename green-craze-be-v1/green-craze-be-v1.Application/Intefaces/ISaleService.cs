using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface ISaleService
    {
        Task<PaginatedResult<SaleDto>> GetListSale(GetSalePagingRequest request);

        Task<SaleDto> GetSale(long id);

        Task<SaleDto> GetSaleLatest();

        Task<long> CreateSale(CreateSaleRequest request);

        Task<bool> UpdateSale(long id, UpdateSaleRequest request);

        Task<bool> DeleteSale(long id);

        Task<bool> DeleteListSale(List<long> ids);

        Task<bool> ApplySale(long id);

        Task<bool> CancelSale(long id);
    }
}
