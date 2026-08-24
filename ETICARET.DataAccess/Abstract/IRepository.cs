using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ETICARET.DataAccess.Abstract
{
    public interface IRepository<T> where T : class
    {
        T? GetById(int id);//Gövdesiz method.
        List<T> GetAll(Expression<Func<T, bool>>? filter = null);//Expression boşsa null dönsün
        void Create (T entity);
        void Update (T entity);
        void Delete (T entity);


        Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null,CancellationToken ct =default);
        Task CreateAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync (T entity, CancellationToken ct = default);
        Task DeleteAsync (T entity, CancellationToken ct = default);


    }
    /* Crud IRepository dizayn pattern
     * Generic repository pattern için temel create update read ve delete işlemleri
     * Canceletion uzun süren işlemleri iptal etmek için kullanılır
     * Async methodlar web uygulamasının performansını arttırmak için kullanılır
     
     */
}
