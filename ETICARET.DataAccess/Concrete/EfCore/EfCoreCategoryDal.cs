using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, DataContext>, ICategoryDal
    {
        public EfCoreCategoryDal(DataContext context) : base(context)
        {
        }

        public List<Category> GetAllWithProductCount()
        {
            return context.Categories.Include(c => c.ProductCategories).ToList();
        }

        public async Task<List<Category>> GetAllWithProductCountAsync(CancellationToken ct = default)
        {
            return await context.Categories.Include(c => c.ProductCategories).ToListAsync(ct);
        }

        public Category? GetCategoryWithProducts(int categoryId)
        {
            return context.Categories
                .Where(i => i.Id == categoryId)
                .Include(i => i.ProductCategories)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.Images).FirstOrDefault();
        }

        public override void Delete(Category entity)
        {
            var category = context.Categories.Include(c => c.ProductCategories).FirstOrDefault(c => c.Id == entity.Id);
            if (category != null)
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }
        public override async Task DeleteAsync(Category entity, CancellationToken ct = default)
        {
            var category = await context.Categories.Include(c => c.ProductCategories).FirstOrDefaultAsync(c => c.Id == entity.Id, ct);
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync(ct);
            }
        }
    }
}
