using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Product;
using green_craze_be_v1.Application.Model.Variant;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace green_craze_be_v1.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductImageService _productImageService;
        private readonly IUploadService _uploadService;

        public ProductService(
            IUnitOfWork unitOfWork, IMapper mapper, IProductImageService productImageService, IUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productImageService = productImageService;
            _uploadService = uploadService;
        }

        public async Task<PaginatedResult<ProductDto>> GetListProduct(GetProductPagingRequest request)
        {
            var spec = new ProductSpecification(request, isPaging: true);
            var countSpec = new ProductSpecification(request);
            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);
            var productDtos = new List<ProductDto>();
            products.ForEach(product =>
            {
                var productDto = _mapper.Map<ProductDto>(product);
                productDtos.Add(productDto);
            });

            return new PaginatedResult<ProductDto>(productDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<ProductDto> GetProduct(long id)
        {
            var spec = new ProductSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec)
                ?? throw new NotFoundException("Cannot find current product");

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<long> CreateProduct(CreateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.Category = await _unitOfWork.Repository<ProductCategory>().GetById(request.CategoryId)
                ?? throw new NotFoundException("Cannot find current product category");

            product.Brand = await _unitOfWork.Repository<Brand>().GetById(request.BrandId)
                ?? throw new NotFoundException("Cannot find current brand");

            product.Unit = await _unitOfWork.Repository<Unit>().GetById(request.UnitId)
                ?? throw new NotFoundException("Cannot find current unit");

            product.Quantity = 0;
            product.ActualInventory = 0;
            product.Sold = 0;
            product.Rating = 5;

            if (request.SaleId != null)
            {
                product.Sale = await _unitOfWork.Repository<Sale>().GetById(request.SaleId)
                    ?? throw new NotFoundException("Cannot find current sale");
            }
            product.Status = PRODUCT_STATUS.INACTIVE;

            List<ProductImage> productImages = new();
            foreach (IFormFile image in request.ProductImages)
            {
                ProductImage productImage = new()
                {
                    Image = _uploadService.UploadFile(image).Result,
                    Size = image.Length,
                    ContentType = image.ContentType
                };
                productImages.Add(productImage);
            }
            productImages[0].IsDefault = true;
            product.Images = productImages;

            List<Variant> variants = new();
            foreach (string v in request.Variants)
            {
                var variant = _mapper.Map<Variant>(JObject.Parse(v).ToObject<CreateVariantRequest>());
                variant.Status = VARIANT_STATUS.ACTIVE;
                variants.Add(variant);
            }
            product.Variants = variants;

            await _unitOfWork.Repository<Product>().Insert(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot create entity");
            }

            return product.Id;
        }

        public async Task<bool> UpdateProduct(long id, UpdateProductRequest request)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id)
                ?? throw new NotFoundException("Cannot find current product");

            product = _mapper.Map(request, product);
            product.Id = id;

            product.Category = await _unitOfWork.Repository<ProductCategory>().GetById(request.CategoryId)
                ?? throw new NotFoundException("Cannot find current product category");

            product.Brand = await _unitOfWork.Repository<Brand>().GetById(request.BrandId)
                ?? throw new NotFoundException("Cannot find current product brand");

            product.Unit = await _unitOfWork.Repository<Unit>().GetById(request.UnitId)
                ?? throw new NotFoundException("Cannot find current unit");

            if (request.SaleId != null)
            {
                product.Sale = await _unitOfWork.Repository<Sale>().GetById(request.SaleId)
                    ?? throw new NotFoundException("Cannot find current sale");
            }

            product.Status = product.Status switch
            {
                PRODUCT_STATUS.ACTIVE => PRODUCT_STATUS.ACTIVE,
                PRODUCT_STATUS.INACTIVE => PRODUCT_STATUS.INACTIVE,
                PRODUCT_STATUS.SOLD_OUT => PRODUCT_STATUS.SOLD_OUT,
                _ => throw new InvalidRequestException("Unexpected product status: " + request.Status),
            };
            _unitOfWork.Repository<Product>().Update(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id)
                ?? throw new NotFoundException("Cannot find current product");

            product.Status = PRODUCT_STATUS.INACTIVE;
            _unitOfWork.Repository<Product>().Update(product);

            var isSuccess = await _unitOfWork.Save() > 0;
            if (!isSuccess)
            {
                throw new Exception("Cannot update status of entity");
            }

            return isSuccess;
        }

        public async Task<bool> DeleteListProduct(List<long> ids)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                foreach (var id in ids)
                {
                    var product = await _unitOfWork.Repository<Product>().GetById(id)
                        ?? throw new NotFoundException("Cannot find current product");

                    product.Status = PRODUCT_STATUS.INACTIVE;
                    _unitOfWork.Repository<Product>().Update(product);
                }
                var isSuccess = await _unitOfWork.Save() > 0;
                if (!isSuccess)
                {
                    throw new Exception("Cannot update status of entities");
                }

                await _unitOfWork.Commit();

                return isSuccess;
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<ProductDto> GetProductBySlug(string slug)
        {
            var spec = new ProductSpecification(slug);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec)
                ?? throw new NotFoundException("Cannot find current product");

            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public Task<PaginatedResult<ProductDto>> GetListProductByCategory(GetProductPagingRequest request, string categorySlug)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedResult<ProductDto>> GetListFilteringProduct(FilterProductPagingRequest request)
        {
            var spec = new ProductFilterSpecification(request, isPaging: true);
            var countSpec = new ProductFilterSpecification(request);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var productDtos = products.Select(x => _mapper.Map<ProductDto>(x)).ToList();

            return new PaginatedResult<ProductDto>(productDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<PaginatedResult<ProductDto>> GetListSearchingProduct(SearchProductPagingRequest request)
        {
            var spec = new ProductSearchSpecification(request, isPaging: true);
            var countSpec = new ProductSearchSpecification(request);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var productDtos = products.Select(x => _mapper.Map<ProductDto>(x)).ToList();

            return new PaginatedResult<ProductDto>(productDtos, request.PageIndex, count, request.PageSize);
        }
    }
}