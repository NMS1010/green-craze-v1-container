using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductDto>> GetListProduct(GetProductPagingRequest request);

        Task<PaginatedResult<ProductDto>> GetListFilteringProduct(FilterProductPagingRequest request);

        Task<PaginatedResult<ProductDto>> GetListSearchingProduct(SearchProductPagingRequest request);

        Task<PaginatedResult<ProductDto>> GetListProductByCategory(GetProductPagingRequest request, string categorySlug);

        Task<ProductDto> GetProduct(long id);

        Task<ProductDto> GetProductBySlug(string slug);

        Task<long> CreateProduct(CreateProductRequest request);

        Task<bool> UpdateProduct(long id, UpdateProductRequest request);

        Task<bool> DeleteProduct(long id);

        Task<bool> DeleteListProduct(List<long> ids);
    }
}