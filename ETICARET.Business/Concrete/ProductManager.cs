using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class ProductManager:IProductService
    {
        private readonly IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal= productDal;
        }

        public void Create(Product entity)
         =>_productDal.Create(entity);

        public async Task CreateAsync(Product entity, CancellationToken ct = default)
       => await _productDal.CreateAsync(entity, ct);

        public void Delete(Product entity)
        =>_productDal.Delete(entity);

        public async Task DeleteAsync(Product entity, CancellationToken ct = default)
       => await _productDal.DeleteAsync(entity,ct);

        public List<Product> GetAll()
       => _productDal.GetAll();

        public async Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => await _productDal.GetAllAsync(null,ct);

        public Product? GetById(int id)
       => _productDal.GetById(id);
        public async Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _productDal.GetByIdAsync(id, ct);

        public int GetCountByCategory(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null)
       => _productDal.GetCountByCategory(category, search, minPrice, maxPrice);

        public async Task<int> GetCountByCategoryAsync(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default)
       => await _productDal.GetCountByCategoryAsync(category, search, minPrice, maxPrice, ct);

        public List<Product> GetProductByCategory(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null)
        => _productDal.GetProductsByCategory(category, page, pageSize, search, sort, minPrice, maxPrice);

        public List<Product> GetRandomProducts(int count)
        => _productDal.GetRandomProducts(count);

        public async Task<List<Product>> GetProductByCategoryAsync(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default)
        => await _productDal.GetProductsByCategoryAsync(category, page, pageSize, search, sort, minPrice, maxPrice, ct);

        public Product? GetProductDetails(int id)
        => _productDal.GetProductDetails(id);
        public async Task<Product?> GetProductDetailsAsync(int id, CancellationToken ct = default)
       => await _productDal.GetProductDetailsAsync(id, ct);

        public void Update(Product entity)
        => _productDal.Update(entity);

        public void Update(Product entity, int[] categoryIds)
        => _productDal.Update(entity, categoryIds);

        public async Task UpdateAsync(Product entity, CancellationToken ct = default)
      => await _productDal.UpdateAsync(entity, ct);
    }
}
