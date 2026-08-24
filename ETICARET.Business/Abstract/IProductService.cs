using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface IProductService
    {
        Product? GetById (int id);
        List<Product> GetProductByCategory(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null);
        List<Product> GetRandomProducts(int count);
        List<Product> GetAll();
        Product? GetProductDetails(int id);
        void Create(Product entity);
        void Update(Product entity);
        void Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
        int GetCountByCategory(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null);

        Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Product>> GetProductByCategoryAsync(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default);
        Task<List<Product>> GetAllAsync(CancellationToken ct = default);
        Task<Product?> GetProductDetailsAsync(int id, CancellationToken ct = default);
        Task CreateAsync(Product entity, CancellationToken ct = default);
        Task UpdateAsync(Product entity, CancellationToken ct = default);
        Task DeleteAsync(Product entity, CancellationToken ct = default);
        Task<int> GetCountByCategoryAsync(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default);
    }
}
