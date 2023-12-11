using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Brand;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.ProductCategory;
using green_craze_be_v1.Application.Model.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IProductCategoryService
    {
        Task<PaginatedResult<ProductCategoryDto>> GetListProductCategory(GetProductCategoryPagingRequest request);

        Task<ProductCategoryDto> GetProductCategory(long id);

        Task<ProductCategoryDto> GetProductCategoryBySlug(string slug);

        Task<long> CreateProductCategory(CreateProductCategoryRequest request);

        Task<bool> UpdateProductCategory(long id, UpdateProductCategoryRequest request);

        Task<bool> DeleteProductCategory(long id);

        Task<bool> DeleteListProductCategory(List<long> ids);
    }
}
