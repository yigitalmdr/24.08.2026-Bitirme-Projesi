using ETICARET.Business.Abstract;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class CommentManager(ICommentDal commentDal) : ICommentService
    {
        public void Create(Comment entity)
       => commentDal.Create(entity);
        public async Task CreateAsync(Comment entity, CancellationToken ct = default)
     => await commentDal.CreateAsync(entity, ct);

        public void Delete(Comment entity)
       => commentDal.Delete(entity);

        public async Task DeleteAsync(Comment entity, CancellationToken ct = default)
        => await commentDal.DeleteAsync(entity, ct);

        public List<Comment> GetAll()
       => commentDal.GetAll();

        public async Task<List<Comment>> GetAllAsync(CancellationToken ct = default)
       => await commentDal.GetAllAsync(null, ct);

        public Comment? GetById(int id)
       => commentDal.GetById(id);

        public async Task<Comment?> GetByIdAsync(int id, CancellationToken ct = default)
        => await commentDal.GetByIdAsync(id, ct);

        public async Task<List<Comment>> GetCommentsByProductIdAsync(int productId, CancellationToken ct = default)
      => await commentDal.GetCommentsByProductIdAsync(productId, ct);

        public void Update(Comment entity)
        => commentDal.Update(entity);
    }
}
