using ETICARET.Entities;
namespace ETICARET.DataAccess.Abstract
{
    public interface ICategoryDal:IRepository<Category>
    {

        Category? GetCategoryWithProducts(int categoryId);
        List<Category> GetAllWithProductCount();
        Task<List<Category>> GetAllWithProductCountAsync(CancellationToken ct = default);
    }
}
