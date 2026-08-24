using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface ICartService
    {
        void InitialCart(string userId);
        Cart? GetCartByUserId(string userId);
        void AddToCart(string userId, int productId, int quantity);
        void DeleteFromCart(string userId, int productId);
        void ClearCart(string cartId);

        Task InitialCartAsync(string userId, CancellationToken ct = default);
        Task<Cart?> GetCartByUserIdAsync(string userId, CancellationToken ct = default);
        Task  AddToCartAsync(string userId, int productId, int quantity,CancellationToken ct=default);
        Task DeleteFromCartAsync(string userId, int productId,CancellationToken ct=default);
        Task ClearCartAsync(string cartId ,CancellationToken ct=default);

    }
}
