using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class OrderManager(IOrderDal orderDal) : IOrderService
    {
        public void Create(Order entity)
        =>orderDal.Create(entity);

        public async Task CreateAsync(Order entity, CancellationToken ct = default)
       =>  await orderDal.CreateAsync(entity, ct);

        public List<Order> GetOrders(string userId)
       => orderDal.GetOrders(userId);

        public async Task<List<Order>> GetOrdersAsync(string userId, CancellationToken ct = default)
        => await orderDal.GetOrdersAsync(userId, ct);

        public void Update(Order entity)
        => orderDal.Update(entity);

        public Order GetOrderByConversionId(string conversionId)
        {
            return orderDal.GetAll(i => i.ConversionId == conversionId).FirstOrDefault();
        }
    }
}
