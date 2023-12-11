using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IInventoryService
    {
        Task<bool> ImportProduct(ImportProductRequest request);
        Task<List<DocketDto>> GetListDocketByProductId(long productId);
    }
}
