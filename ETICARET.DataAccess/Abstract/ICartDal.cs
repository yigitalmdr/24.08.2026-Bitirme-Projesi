using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface ICartDal:IRepository<Cart>
    {
        Cart? GetCartByUserId(string userId);
        void DeleteFromCart(int cartId, int productId);
        void ClearCart(string cartId);

        Task<Cart?> GetCartByUserIdAsync(string userId, CancellationToken ct = default);
        Task DeleteFromCartAsync(int cartId, int productId, CancellationToken ct = default);
        Task ClearCartAsync(string cartId, CancellationToken ct = default);
    }
}
