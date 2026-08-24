using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface IProductDal:IRepository<Product>
    {
        List<Product> GetProductsByCategory(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null);
        List<Product> GetRandomProducts(int count);
        Product? GetProductDetails(int id);
        int GetCountByCategory(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null);
        void Update(Product entity, int[] categoryIds);

        Task<List<Product>> GetProductsByCategoryAsync(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default);
        Task<Product?> GetProductDetailsAsync(int id, CancellationToken ct = default);
        Task<int> GetCountByCategoryAsync(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default);
        Task UpdateAsync(Product entity, int[] categoryIds, CancellationToken ct = default);

    }
}
