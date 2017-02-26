using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using RPEFN.Data.Entities;
using RPEFN.WebService.Infrastructure.Interfaces;

namespace RPEFN.WebService.Infrastructure.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity:Entity
    {
        protected readonly DbContext Context = null;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }
        
        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public IEnumerable<TEntity> Find(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public IEnumerable<TEntity> Get()
        {
            return Context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>>  GetAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }
}