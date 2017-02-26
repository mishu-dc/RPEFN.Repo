using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RPEFN.Data.Entities;

namespace RPEFN.WebService.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity:Entity
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IEnumerable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);
    }
}
