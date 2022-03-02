using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Play.Common
{
    public interface IRepository<T> where T: IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAysnc(Guid id);
        Task<T> GetAysnc(Expression<Func<T, bool>> filter);
        Task RemoveAysnc(Guid id);
        Task UpdateAsync(T entity);
    }
}