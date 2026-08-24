using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreFavoriteDal : EfCoreGenericRepository<Favorite, DataContext>, IFavoriteDal
    {
        public EfCoreFavoriteDal(DataContext context) : base(context)
        {
        }

        public Favorite? GetFavorite(string userId, int productId)
        {
            return context.Favorites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
        }

        public List<Favorite> GetFavoritesByUserId(string userId)
        {
            return context.Favorites
                .Include(f => f.Product)
                .ThenInclude(p => p.Images)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.AddedOn)
                .ToList();
        }
    }
}
