using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface IOrderDal:IRepository<Order>
    {
        List<Order> GetOrders(string userId);
        Task<List<Order>> GetOrdersAsync(string userId, CancellationToken ct = default);
    }
}
