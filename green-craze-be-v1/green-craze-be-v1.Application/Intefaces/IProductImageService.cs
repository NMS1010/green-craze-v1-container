using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.ProductImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetListProductImage(long productId);
        Task<ProductImageDto> GetProductImage(long id);
        Task<long> CreateProductImage(CreateProductImageRequest request);
        Task<bool> UpdateProductImage(long id, UpdateProductImageRequest request);
        Task<bool> SetDefaultProductImage(long id, long productId);
        Task<bool> DeleteListProductImage(List<long> ids);
        Task<bool> DeleteProductImage(long id);
    }
}
