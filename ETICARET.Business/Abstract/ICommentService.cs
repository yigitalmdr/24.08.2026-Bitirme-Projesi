using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface ICommentService
    {
        Comment? GetById(int id);
        List<Comment> GetAll();
        void Create(Comment entity);
        void Update(Comment entity);
        void Delete(Comment entity);

        Task<Comment?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Comment>> GetAllAsync(CancellationToken ct = default);
        Task<List<Comment>> GetCommentsByProductIdAsync(int productId, CancellationToken ct = default);
        Task CreateAsync(Comment entity, CancellationToken ct = default);
        Task DeleteAsync(Comment entity, CancellationToken ct = default);
    }
}
