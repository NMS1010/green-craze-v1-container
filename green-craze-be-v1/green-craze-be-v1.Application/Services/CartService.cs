using AutoMapper;
using green_craze_be_v1.Application.Common.Enums;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Cart;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Cart;
using green_craze_be_v1.Application.Specification.Product;
using green_craze_be_v1.Application.Specification.Variant;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<bool> AddVariantItemToCart(CreateCartItemRequest request)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of current user");

            var cartItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpec(new CartItemSpecification(cart.Id, request.VariantId));

            var ci = new CartItem();

            var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(request.VariantId))
                ?? throw new InvalidRequestException("Unexpected variantId");

            if (variant.Product.Status != PRODUCT_STATUS.ACTIVE)
                throw new InvalidRequestException("Unexpected variantId, product is not active");

            if (request.Quantity <= 0)
                throw new InvalidRequestException("Unexpected quantity, it must be a positive number");

            var quantity = variant?.Product?.ActualInventory;
            if (quantity < (request.Quantity * variant.Quantity))
                throw new InvalidRequestException("Unexpected quantity, it must be less than or equal to product in inventory");

            if (cartItem == null)
            {
                ci.Quantity = request.Quantity;
                ci.Variant = variant;
                cart.CartItems.Add(ci);
                _unitOfWork.Repository<Cart>().Update(cart);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
                _unitOfWork.Repository<CartItem>().Update(cartItem);
            }

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to add product to your cart, an error has occured");

            return true;
        }

        public async Task<bool> DeleteCartItem(long cartItemId, string userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(userId))
                ?? throw new NotFoundException("Cannot find cart of current user");
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == cartItemId)
                ?? throw new InvalidRequestException("Unexpected cartItemId");

            _unitOfWork.Repository<CartItem>().Delete(cartItem);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to remove product from your cart, an error has occured");

            return true;
        }

        public async Task<PaginatedResult<CartItemDto>> GetCartByUser(GetCartPagingRequest request)
        {
            var cartItems = await _unitOfWork.Repository<CartItem>().ListAsync(new CartItemSpecification(request, isPaging: true));
            var count = await _unitOfWork.Repository<CartItem>().CountAsync(new CartItemSpecification(request));

            var cartDtos = new List<CartItemDto>();
            foreach (var cartItem in cartItems)
            {
                cartDtos.Add(await GetCartItemDto(cartItem));
            }

            return new PaginatedResult<CartItemDto>(cartDtos, request.PageIndex, count, request.PageSize);
        }

        private async Task<CartItemDto> GetCartItemDto(CartItem cartItem)
        {
            var isPromotion = cartItem.Variant.PromotionalItemPrice.HasValue;
            var variant = await _unitOfWork.Repository<Variant>().GetEntityWithSpec(new VariantSpecification(cartItem.Variant.Id))
                ?? throw new NotFoundException("Cannot find varaint item");

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(new ProductSpecification(variant.Product.Id))
                ?? throw new NotFoundException("Cannot find product of variant item");

            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);

            cartItemDto.VariantId = variant.Id;
            cartItemDto.Quantity = cartItem.Quantity;
            cartItemDto.TotalPrice = variant.Quantity * variant.ItemPrice;
            cartItemDto.TotalPromotionalPrice = isPromotion ? variant.Quantity * variant.PromotionalItemPrice.Value : null;
            cartItemDto.Sku = variant.Sku;
            cartItemDto.VariantName = variant.Name;
            cartItemDto.VariantPrice = variant.ItemPrice;
            cartItemDto.VariantPromotionalPrice = isPromotion ? variant.PromotionalItemPrice.Value : null;
            cartItemDto.IsPromotion = isPromotion;
            cartItemDto.VariantQuantity = variant.Quantity;
            cartItemDto.ProductName = product.Name;
            cartItemDto.ProductSlug = product.Slug;
            cartItemDto.ProductUnit = product.Unit.Name;
            cartItemDto.ProductImage = product.Images.FirstOrDefault(x => x.IsDefault)?.Image ?? product.Images.FirstOrDefault()?.Image;

            return cartItemDto;
        }

        public async Task<bool> UpdateCartItemQuantity(UpdateCartItemRequest request)
        {
            if (request.Quantity <= 0)
                throw new InvalidRequestException("Unexpected quantity, it must be a positive number");
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(request.UserId))
                ?? throw new NotFoundException("Cannot find cart of current user");

            var cartItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpec(new CartItemSpecification(request.CartItemId, request.UserId))
                ?? throw new InvalidRequestException("Unexpected cartItemId");

            var quantity = cartItem?.Variant?.Product?.ActualInventory;
            if (quantity < (request.Quantity * cartItem?.Variant?.Quantity))
                throw new InvalidRequestException("Unexpected quantity, it must be less than or equal to product in inventory");

            cartItem.Quantity = request.Quantity;

            _unitOfWork.Repository<Cart>().Update(cart);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to update product quantity, an error has occured");

            return true;
        }

        public async Task<bool> DeleteListCartItem(List<long> ids, string userId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetEntityWithSpec(new CartSpecification(userId))
                ?? throw new NotFoundException("Cannot find cart of current user");

            foreach (var id in ids)
            {
                var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == id)
                    ?? throw new InvalidRequestException("Unexpected cartItemId");

                _unitOfWork.Repository<CartItem>().Delete(cartItem);
            }

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess) throw new Exception("Cannot handle to remove list of product from your cart, an error has occured");

            return true;
        }

        public async Task<List<CartItemDto>> GetCartItemByIds(List<long> ids, string userId)
        {
            var res = new List<CartItemDto>();
            foreach (var id in ids)
            {
                var cartItem = await _unitOfWork.Repository<CartItem>().GetEntityWithSpec(new CartItemSpecification(id, userId))
                    ?? throw new NotFoundException("Cannot find cart item");

                res.Add(await GetCartItemDto(cartItem));
            }

            return res;
        }
    }
}