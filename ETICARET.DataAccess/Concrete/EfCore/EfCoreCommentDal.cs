using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCommentDal : EfCoreGenericRepository<Comment, DataContext>, ICommentDal
    {
        public EfCoreCommentDal(DataContext context) : base(context)
        {
        }

        public async Task<List<Comment>> GetCommentsByProductIdAsync(int productId, CancellationToken ct = default)
        {
            return await context.Comments
                .Where(c=>c.ProductId==productId)
                .OrderByDescending(c=>c.CreateOn)
                .ToListAsync(ct);
        }
    }
}
