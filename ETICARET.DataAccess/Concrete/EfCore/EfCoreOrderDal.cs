using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreOrderDal : EfCoreGenericRepository<Order, DataContext>, IOrderDal
    {
        public EfCoreOrderDal(DataContext context) : base(context)
        {
        }

        public List<Order> GetOrders(string userId)
        {
            var orders = context.Orders
                .Include(i => i.OrderItems)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i!.Images)
                .AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                orders=orders.Where(i => i.UserId == userId);
            }
            return orders.OrderByDescending(order => order.OrderDate).ToList();
        }

        public async Task<List<Order>> GetOrdersAsync(string userId, CancellationToken ct = default)
        {
            var orders = context.Orders
                .Include(i => i.OrderItems)
                .ThenInclude(i => i.Product).
                ThenInclude(i => i!.Images)
                .AsQueryable();
            if (!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(i => i.UserId == userId);
            }
            return await orders.OrderByDescending(order => order.OrderDate).ToListAsync(ct);
        }
    }
}
