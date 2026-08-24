using ETICARET.Entities;
using System.Collections.Generic;

namespace ETICARET.DataAccess.Abstract
{
    public interface IFavoriteDal : IRepository<Favorite>
    {
        List<Favorite> GetFavoritesByUserId(string userId);
        Favorite? GetFavorite(string userId, int productId);
    }
}
