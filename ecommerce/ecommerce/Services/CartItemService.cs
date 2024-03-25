﻿using ecommerce.DTO;
using ecommerce.Middleware;
using ecommerce.Models;
using ecommerce.Repository.Interface;
using ecommerce.Services.Interface;

namespace ecommerce.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }
        public async Task AddCartItemAsync(CartItemDto cartItem)
        {
            // find cart item by cart id and product id
            var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product == null)
            {
                throw new CustomException("Product not found", 404);
            }
            var cartItemModel = new CartItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                CartId = cartItem.CartId,
                Product = product
            };
            _cartItemRepository.AddCartItem(cartItemModel);
        }

        public async Task<ApiResponse<int>> DeleteCartItemAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "No Cart Item found",
                    Status = false
                };
            }
            _cartItemRepository.DeleteCartItem(cartItem);
            return new ApiResponse<int>
            {
                Data = id,
                Message = "Cart Item deleted",
                Status = true
            };
        }

        public async Task<ApiResponse<int>> DeleteCartItemsByCartIdAsync(IEnumerable<CartItem> cartItems)
        {
            try
            {
                _cartItemRepository.DeleteCartItemsByCartId(cartItems);
                return new ApiResponse<int>
                {
                    Data = 0,
                    Message = "Cart Items deleted",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = 0,
                    Message = ex.Message,
                    Status = false
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CartItemDto>>> GetAllCartItemsAsync()
        {
            var cartItems = await _cartItemRepository.GetAllCartItemsAsync();
            if (cartItems == null)
            {
                return new ApiResponse<IEnumerable<CartItemDto>>
                {
                    Data = null,
                    Message = "No Cart Item found",
                    Status = false
                };
            }
            return new ApiResponse<IEnumerable<CartItemDto>>
            {
                Data = cartItems.Select(x => new CartItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    CartId = x.CartId
                }),
                Message = "Cart Items found",
                Status = true
            };

        }

        public async Task<ApiResponse<CartItemDto>> GetCartItemByIdAsync(int id)
        {
            var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
            if (cartItem == null)
            {
                return new ApiResponse<CartItemDto>
                {
                    Data = null,
                    Message = "Cart Item not found",
                    Status = false
                };
            }
            return new ApiResponse<CartItemDto>
            {
                Data = new CartItemDto
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    CartId = cartItem.CartId
                },
                Message = "Cart Item found",
                Status = true
            };
        }

        public async Task<ApiResponse<IEnumerable<CartItemDto>>> GetCartItemsByCartIdAsync(int cartId, int productId)
        {
            var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cartId, productId);
            if (cartItems == null)
            {
                return new ApiResponse<IEnumerable<CartItemDto>>
                {
                    Data = null,
                    Message = "No Cart Item found",
                    Status = false
                };
            }
            return new ApiResponse<IEnumerable<CartItemDto>>
            {
                Data = cartItems.Select(x => new CartItemDto
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    CartId = x.CartId
                }),
                Message = "Cart Items found",
                Status = true
            };
        }

        public async Task<ApiResponse<int>> UpdateCartItemAsync(int id, CartItemDto cartItem)
        {
            var cartItemModel = new CartItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                CartId = cartItem.CartId
            };
            var cartItemToUpdate = await _cartItemRepository.GetCartItemByIdAsync(id);
            if (cartItemToUpdate == null)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "No Cart Item found",
                    Status = false
                };
            }
            _cartItemRepository.UpdateCartItem(cartItemToUpdate, cartItemModel);
            return new ApiResponse<int>
            {
                Data = id,
                Message = "Cart Item updated",
                Status = true
            };
        }

        public async Task<ApiResponse<int>> UpdateCartItemsAsync(int id, List<CartItemDto> cartItem)
        {
            var cartItems = cartItem.Select(x => new CartItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                CartId = x.CartId
            });
            try
            {
                var cartItemsToUpdate = await _cartItemRepository.GetCartItemsByCartIdAsync(id, cartItem[0].ProductId);
                if (cartItemsToUpdate == null)
                {
                    return new ApiResponse<int>
                    {
                        Data = id,
                        Message = "No Cart Item found",
                        Status = false
                    };
                }
                _cartItemRepository.UpdateCartsItem(cartItemsToUpdate, cartItems);
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = "Cart Items updated",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Data = id,
                    Message = ex.Message,
                    Status = false
                };
            }
        }
    }
}
