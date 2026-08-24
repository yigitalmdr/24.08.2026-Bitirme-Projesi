using ETICARET.Entities;
using System.Collections.Generic;

namespace ETICARET.Business.Abstract
{
    public interface IFavoriteService
    {
        List<Favorite> GetFavoritesByUserId(string userId);
        Favorite? GetFavorite(string userId, int productId);
        void Create(Favorite entity);
        void Delete(Favorite entity);
    }
}
