using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Sale;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Sale;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
	public class SaleService : ISaleService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IUploadService _uploadService;
		private readonly INotificationService _notificationService;

		public SaleService(IUnitOfWork unitOfWork, IMapper mapper, IUploadService uploadService, INotificationService notificationService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_uploadService = uploadService;
			_notificationService = notificationService;
		}

		public async Task<PaginatedResult<SaleDto>> GetListSale(GetSalePagingRequest request)
		{
			var spec = new SaleSpecification(request, isPaging: true);
			var countSpec = new SaleSpecification(request);
			var sales = await _unitOfWork.Repository<Sale>().ListAsync(spec);
			var count = await _unitOfWork.Repository<Sale>().CountAsync(countSpec);

			var saleDtos = new List<SaleDto>();
			foreach (var sale in sales)
			{
				HashSet<ProductCategory> productCategories = new();
				var productCategoryDtos = new List<ProductCategoryDto>();
				if (!sale.All)
				{
					foreach (var p in sale.Products)
					{
						Product product =
							await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(p.Id));
						productCategories.Add(product.Category);
					}
					foreach (var productCategory in productCategories)
					{
						productCategoryDtos.Add(_mapper.Map<ProductCategoryDto>(productCategory));
					}
				}
				var saleDto = _mapper.Map<SaleDto>(sale);
				saleDto.ProductCategories = productCategoryDtos;
				saleDtos.Add(saleDto);
			}

			return new PaginatedResult<SaleDto>(saleDtos, request.PageIndex, count, request.PageSize);
		}

		public async Task<SaleDto> GetSale(long id)
		{
			var sale = await _unitOfWork.Repository<Sale>().GetEntityWithSpec(new SaleSpecification(id));
			HashSet<ProductCategory> productCategories = new();
			var productCategoryDtos = new List<ProductCategoryDto>();
			if (!sale.All)
			{
				foreach (var p in sale.Products)
				{
					Product product =
						await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(p.Id));
					productCategories.Add(product.Category);
				}
				foreach (var productCategory in productCategories)
				{
					productCategoryDtos.Add(_mapper.Map<ProductCategoryDto>(productCategory));
				}
			}
			var saleDto = _mapper.Map<SaleDto>(sale);
			saleDto.ProductCategories = productCategoryDtos;

			return saleDto;
		}

		public async Task<SaleDto> GetSaleLatest()
		{
			var sale = await _unitOfWork.Repository<Sale>().GetEntityWithSpec(new SaleSpecification(true));
			return _mapper.Map<SaleDto>(sale);
		}

		public async Task<long> CreateSale(CreateSaleRequest request)
		{
			var sale = _mapper.Map<Sale>(request);
			sale.Image = _uploadService.UploadFile(request.Image).Result;
			sale.Status = SALE_STATUS.INACTIVE;

			var products = new List<Product>();
			if (request.All)
			{
				products = await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification());
			}
			else
			{
				foreach (long categoryId in request.CategoryIds)
				{
					var listProduct = await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification(categoryId, true));
					listProduct.ForEach(p => products.Add(p));
				}
			}

			sale.Products = products;

			await _unitOfWork.Repository<Sale>().Insert(sale);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot create entity");
			}

			return sale.Id;
		}

		public async Task<bool> UpdateSale(long id, UpdateSaleRequest request)
		{
			var sale = await _unitOfWork.Repository<Sale>().GetById(id);
			sale = _mapper.Map<UpdateSaleRequest, Sale>(request, sale);
			sale.Id = id;

			if (request.Image != null)
			{
				sale.Image = _uploadService.UploadFile(request.Image).Result;
			}

			var products = new List<Product>();
			if (request.All)
			{
				products = await _unitOfWork.Repository<Product>().ListAsync(new ProductSpecification());
			}
			else
			{
				var listAllProduct = await _unitOfWork.Repository<Product>()
					.ListAsync(new ProductSpecification(id, true, true));

				foreach (long categoryId in request.CategoryIds)
				{
					var listProduct = await _unitOfWork.Repository<Product>()
						.ListAsync(new ProductSpecification(categoryId, true));
					listProduct.ForEach(p => products.Add(p));
				}

				foreach (var product in listAllProduct.Except(products).ToArray())
				{
					product.Sale = null;
					_unitOfWork.Repository<Product>().Update(product);
				}
			}

			sale.Products = products;

			_unitOfWork.Repository<Sale>().Update(sale);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteSale(long id)
		{
			var sale = await _unitOfWork.Repository<Sale>().GetById(id);
			sale.Status = SALE_STATUS.INACTIVE;
			_unitOfWork.Repository<Sale>().Update(sale);
			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entity");
			}

			return isSuccess;
		}

		public async Task<bool> DeleteListSale(List<long> ids)
		{
			try
			{
				await _unitOfWork.CreateTransaction();

				foreach (var id in ids)
				{
					var sale = await _unitOfWork.Repository<Sale>().GetById(id);
					sale.Status = SALE_STATUS.INACTIVE;
					_unitOfWork.Repository<Sale>().Update(sale);
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

		public async Task<bool> ApplySale(long id)
		{
			var sale = await _unitOfWork.Repository<Sale>().GetEntityWithSpec(new SaleSpecification(id))
				?? throw new NotFoundException("Cannot find current sale");

			if (sale.Status == SALE_STATUS.ACTIVE)
			{
				throw new SaleAppliedException("Sale has applied");
			}

			if (DateTime.Compare(sale.StartDate, DateTime.Now) > 0)
			{
				throw new SaleDateException("Sale date is not yet");
			}

			var products = sale.Products;
			if (products.Count == 0)
				throw new Exception("Cannot find any product applied this sale");

			foreach (var p in products)
			{
				Product product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(p.Id));
				foreach (var variant in product.Variants)
				{
					variant.PromotionalItemPrice = variant.ItemPrice - variant.ItemPrice * (decimal)sale.PromotionalPercent / 100;
					variant.TotalPromotionalPrice = variant.PromotionalItemPrice * variant.Quantity;
				}
				_unitOfWork.Repository<Product>().Update(product);
			}

			sale.Status = SALE_STATUS.ACTIVE;
			_unitOfWork.Repository<Sale>().Update(sale);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entities");
			}
			await _notificationService.CreateSaleNotification(new Application.Model.Notification.CreateNotificationRequest()
			{
				Content = $"Đợt khuyến mãi hiện đang có mặt tại cửa hàng, giảm giá lên tới {sale.PromotionalPercent}%",
				Type = NOTIFICATION_TYPE.SALE,
				Image = sale.Image,
				Status = false,
				Title = $"{sale.Name}",
				Anchor = "#"
			});
			return isSuccess;
		}

		public async Task<bool> CancelSale(long id)
		{
			var sale = await _unitOfWork.Repository<Sale>().GetEntityWithSpec(new SaleSpecification(id))
				?? throw new NotFoundException("Cannot find current sale");

			if (sale.Status == SALE_STATUS.INACTIVE || sale.Status == SALE_STATUS.EXPIRED)
			{
				throw new SaleAppliedException("Sale has not apply");
			}

			var products = sale.Products;
			if (products.Count == 0) return true;

			foreach (var p in products)
			{
				Product product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(p.Id));
				product.Sale = null;
				foreach (var variant in product.Variants)
				{
					variant.PromotionalItemPrice = null;
					variant.TotalPromotionalPrice = null;
				}
				_unitOfWork.Repository<Product>().Update(product);
			}

			sale.Status = SALE_STATUS.INACTIVE;
			_unitOfWork.Repository<Sale>().Update(sale);

			var isSuccess = await _unitOfWork.Save() > 0;
			if (!isSuccess)
			{
				throw new Exception("Cannot update status of entities");
			}

			return isSuccess;
		}
	}
}