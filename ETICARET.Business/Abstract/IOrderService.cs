using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface IOrderService
    {
        void Create(Order entity);
        List<Order> GetOrders(string userId);
        Task CreateAsync(Order entity, CancellationToken ct = default);
        Task<List<Order>> GetOrdersAsync(string userId, CancellationToken ct = default);
        void Update(Order entity);
        Order GetOrderByConversionId(string conversionId);
    }
}
