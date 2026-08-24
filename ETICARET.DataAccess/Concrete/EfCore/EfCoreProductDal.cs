using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, DataContext>, IProductDal
    {
        public EfCoreProductDal(DataContext context) : base(context)
        {
        }

        public int GetCountByCategory(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                products = products.Where(i => 
                    (i.Name != null && i.Name.ToLower().Contains(lowerSearch)) || 
                    (i.Description != null && i.Description.ToLower().Contains(lowerSearch)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(i => i.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(i => i.Price <= maxPrice.Value);
            }

            return products.Count();
        }

        public async Task<int> GetCountByCategoryAsync(string category, string search = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default)
        {
            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                products = products.Where(i => 
                    (i.Name != null && i.Name.ToLower().Contains(lowerSearch)) || 
                    (i.Description != null && i.Description.ToLower().Contains(lowerSearch)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(i => i.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(i => i.Price <= maxPrice.Value);
            }

            return await products.CountAsync(ct);
        }

        public Product? GetProductDetails(int id)
        {
            return context.Products
                .Include(i => i.Images)
                .Include(i => i.ProductCategories)
                .ThenInclude(a => a.Category)
                .Include(i => i.Comments)
                .FirstOrDefault(i => i.Id == id);
        }

        public async Task<Product?> GetProductDetailsAsync(int id, CancellationToken ct = default)
        {
            return await context.Products
                .Include(i => i.Images)
                .Include(i => i.ProductCategories)
                .ThenInclude(a => a.Category)
                .Include(i => i.Comments)
                .FirstOrDefaultAsync(i => i.Id == id, ct);
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            var products = context.Products.Include("Images").AsQueryable();

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                products = products.Where(i => 
                    (i.Name != null && i.Name.ToLower().Contains(lowerSearch)) || 
                    (i.Description != null && i.Description.ToLower().Contains(lowerSearch)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(i => i.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(i => i.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "price_asc")
                {
                    products = products.OrderBy(i => i.Price);
                }
                else if (sort == "price_desc")
                {
                    products = products.OrderByDescending(i => i.Price);
                }
            }
            else
            {
                products = products.OrderBy(i => i.Id);
            }

            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> GetRandomProducts(int count)
        {
            var productIds = context.Products
                .OrderBy(_ => Guid.NewGuid())
                .Select(product => product.Id)
                .Take(count)
                .ToList();

            var productsById = context.Products
                .Include(product => product.Images)
                .Where(product => productIds.Contains(product.Id))
                .ToDictionary(product => product.Id);

            return productIds
                .Where(productsById.ContainsKey)
                .Select(id => productsById[id])
                .ToList();
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category, int page, int pageSize, string search = null, string sort = null, decimal? minPrice = null, decimal? maxPrice = null, CancellationToken ct = default)
        {
            var products = context.Products.Include("Images").AsQueryable();

            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                products = products
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
            }

            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                products = products.Where(i => 
                    (i.Name != null && i.Name.ToLower().Contains(lowerSearch)) || 
                    (i.Description != null && i.Description.ToLower().Contains(lowerSearch)));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(i => i.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(i => i.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort == "price_asc")
                {
                    products = products.OrderBy(i => i.Price);
                }
                else if (sort == "price_desc")
                {
                    products = products.OrderByDescending(i => i.Price);
                }
            }
            else
            {
                products = products.OrderBy(i => i.Id);
            }

            return await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);
        }
        public override void Update(Product entity)
        {
            var product = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == entity.Id);
            if (product != null)
            {
                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.Stock = entity.Stock;
                context.SaveChanges();
            }
        }
        public void Update(Product entity, int[] categoryIds)
        {
            var product = context.Products.Include(p => p.ProductCategories).FirstOrDefault(p => p.Id == entity.Id);
            if (product != null)
            {
                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.Stock = entity.Stock;
                product.ProductCategories = (categoryIds ?? []).Distinct().Select(id => new ProductCategory { ProductId = entity.Id, CategoryId = id }).ToList();
                context.SaveChanges();
            }
        }
        //async delete getall ve bunların async si yazılacak.
        public async Task UpdateAsync(Product entity, int[] categoryIds, CancellationToken ct = default)
        {
            var product = await context.Products.Include(p => p.ProductCategories).FirstOrDefaultAsync(p => p.Id == entity.Id, ct);
            if (product != null)
            {
                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.Stock = entity.Stock;
                product.ProductCategories = (categoryIds ?? []).Distinct().Select(id => new ProductCategory { ProductId = entity.Id, CategoryId = id }).ToList();
                await context.SaveChangesAsync(ct);
            }
        }
        public override void Delete(Product entity)
        {
            var product = context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == entity.Id);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
        public override List<Product> GetAll(Expression<Func<Product, bool>>? filter = null)
        {
            return filter == null ?
                context.Products.Include(i => i.Images).ToList()
                : context.Products.Include(i => i.Images).Where(filter).ToList();


        }
        public override async Task DeleteAsync(Product entity, CancellationToken ct = default)
        {
            var product = await context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == entity.Id, ct);
            if (product != null)
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync(ct);
            }

        }
        public override async Task<List<Product>> GetAllAsync(Expression<Func<Product, bool>>? filter = null, CancellationToken ct = default)
        {
            return filter == null ?
              await context.Products.Include(p => p.Images).ToListAsync(ct)
            : await context.Products.Include(p => p.Images).Where(filter).ToListAsync(ct);
        }
        public override async Task UpdateAsync(Product entity, CancellationToken ct = default)
        {
            var product = await context.Products.Include(p => p.Images).FirstOrDefaultAsync(p=>p.Id==entity.Id, ct);
            if (product != null)
            {
                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Price = entity.Price;
                product.Stock = entity.Stock;
                await context.SaveChangesAsync(ct);
            }
        }
    }
}
