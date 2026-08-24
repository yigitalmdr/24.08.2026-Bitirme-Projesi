using ETICARET.Entities;
namespace ETICARET.Business.Abstract
{
    public interface ICategoryService
    {
        Category? GetById(int id);
        List<Category> GetAll();
        void Create(Category entity);
        void Update(Category entity);
        void Delete(Category entity);
        Category? GetCategoryWithProducts(int categoryId);

        Task<Category?> GetByIdAsync(int categoryId,CancellationToken ct=default);
        Task<List<Category>> GetAllAsync(CancellationToken ct=default);
        Task CreateAsync(Category entity,CancellationToken ct=default);
        Task UpdateAsync(Category entity, CancellationToken ct = default);

        Task DeleteAsync(Category entity, CancellationToken ct = default);
    }
}
