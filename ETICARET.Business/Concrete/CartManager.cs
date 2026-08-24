using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class CartManager(ICartDal cartDal) : ICartService
    {
        public void AddToCart(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Ürün adedi sıfırdan büyük olmalıdır.");
            }

            var cart = GetOrCreateCart(userId);
            var index = cart.CartItems.FindIndex(x => x.ProductId == productId);
            if (index < 0)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                });
            }
            else
            {
                cart.CartItems[index].Quantity += quantity;
            }

            cartDal.Update(cart);
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity, CancellationToken ct = default)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Ürün adedi sıfırdan büyük olmalıdır.");
            }

            var cart = await GetOrCreateCartAsync(userId, ct);
            var index = cart.CartItems.FindIndex(x => x.ProductId == productId);
            if (index < 0)
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    CartId = cart.Id
                });
            }
            else
            {
                cart.CartItems[index].Quantity += quantity;
            }

            await cartDal.UpdateAsync(cart, ct);
        }

        public void ClearCart(string cartId)=>cartDal.ClearCart(cartId);//Lambda expression ile hızlı fonksiyon tanımlama 
       

        public async Task ClearCartAsync(string cartId, CancellationToken ct = default)
        {
            await cartDal.ClearCartAsync(cartId, ct);
        }

        public void DeleteFromCart(string userId, int productId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null)
            {
                cartDal.DeleteFromCart(cart.Id, productId);
            }
        }

        public async Task DeleteFromCartAsync(string userId, int productId, CancellationToken ct = default)
        {
            var cart =await GetCartByUserIdAsync(userId);
            if (cart!=null)
            {
                await cartDal.DeleteFromCartAsync(cart.Id, productId, ct);
            }
        }

        public Cart? GetCartByUserId(string userId)=>cartDal.GetCartByUserId(userId);//DI 
        

        public async Task<Cart?> GetCartByUserIdAsync(string userId, CancellationToken ct = default)
        => await cartDal.GetCartByUserIdAsync(userId, ct);

        public void InitialCart(string userId)
        {
            if (GetCartByUserId(userId) is null)
            {
                cartDal.Create(new Cart { UserId = userId });
            }
        }

        public async Task InitialCartAsync(string userId, CancellationToken ct = default)
        {
            if (await GetCartByUserIdAsync(userId, ct) is null)
            {
                await cartDal.CreateAsync(new Cart { UserId = userId }, ct);
            }
        }

        private Cart GetOrCreateCart(string userId)
        {
            var cart = GetCartByUserId(userId);
            if (cart is not null)
            {
                return cart;
            }

            cartDal.Create(new Cart { UserId = userId });
            return GetCartByUserId(userId)
                ?? throw new InvalidOperationException("Kullanıcı sepeti oluşturulamadı.");
        }

        private async Task<Cart> GetOrCreateCartAsync(string userId, CancellationToken ct)
        {
            var cart = await GetCartByUserIdAsync(userId, ct);
            if (cart is not null)
            {
                return cart;
            }

            await cartDal.CreateAsync(new Cart { UserId = userId }, ct);
            return await GetCartByUserIdAsync(userId, ct)
                ?? throw new InvalidOperationException("Kullanıcı sepeti oluşturulamadı.");
        }
    }
}
