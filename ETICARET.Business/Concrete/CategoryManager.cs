using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
namespace ETICARET.Business.Concrete
{
    public class CategoryManager(ICategoryDal categoryDal) : ICategoryService
    {
        public void Create(Category entity)
       =>categoryDal.Create(entity);

        public async Task CreateAsync(Category entity, CancellationToken ct = default)
         => await categoryDal.CreateAsync(entity, ct);


        public void Delete(Category entity)
         =>categoryDal.Delete(entity);

        public async Task DeleteAsync(Category entity, CancellationToken ct = default)
       => await categoryDal.DeleteAsync(entity,ct);

        public List<Category> GetAll()
        => categoryDal.GetAllWithProductCount();


        public async Task<List<Category>> GetAllAsync(CancellationToken ct = default)
        => await categoryDal.GetAllWithProductCountAsync(ct);

        public Category? GetById(int id)
        {
            return categoryDal.GetById(id);
        }

        public async Task<Category?> GetByIdAsync(int categoryId, CancellationToken ct = default)

            => await categoryDal.GetByIdAsync(categoryId, ct);

        public  Category? GetCategoryWithProducts(int categoryId)
       =>  categoryDal.GetCategoryWithProducts(categoryId);

        public void Update(Category entity)
        => categoryDal.Update(entity);

        public async Task UpdateAsync(Category entity, CancellationToken ct = default)
       => await categoryDal.UpdateAsync(entity, ct);
    }
}
