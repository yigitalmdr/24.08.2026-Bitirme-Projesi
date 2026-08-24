using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System.Collections.Generic;

namespace ETICARET.Business.Concrete
{
    public class FavoriteManager : IFavoriteService
    {
        private readonly IFavoriteDal _favoriteDal;

        public FavoriteManager(IFavoriteDal favoriteDal)
        {
            _favoriteDal = favoriteDal;
        }

        public void Create(Favorite entity)
        {
            _favoriteDal.Create(entity);
        }

        public void Delete(Favorite entity)
        {
            _favoriteDal.Delete(entity);
        }

        public Favorite? GetFavorite(string userId, int productId)
        {
            return _favoriteDal.GetFavorite(userId, productId);
        }

        public List<Favorite> GetFavoritesByUserId(string userId)
        {
            return _favoriteDal.GetFavoritesByUserId(userId);
        }
    }
}
