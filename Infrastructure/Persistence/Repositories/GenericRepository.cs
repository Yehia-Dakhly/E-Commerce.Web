using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _DbContext;

        public GenericRepository(StoreDbContext storeDbContext)
        {
            _DbContext = storeDbContext;
        }
        public async Task AddAsync(TEntity entity)
            => await _DbContext.AddAsync(entity);
        public void Delete(TEntity entity)
            => _DbContext.Remove(entity);
        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _DbContext.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetbyIdAsync(TKey id)
            => await _DbContext.Set<TEntity>().FindAsync(id);
        public void Update(TEntity entity)
            => _DbContext.Set<TEntity>().Update(entity);

        #region With Specifications
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_DbContext.Set<TEntity>(), specifications).ToListAsync();
        }


        public async Task<TEntity?> GetbyIdAsync(ISpecifications<TEntity, TKey> specifications)
        {
            return await SpecificationEvaluator.CreateQuery(_DbContext.Set<TEntity>(), specifications).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> specifications)
        => await SpecificationEvaluator.CreateQuery(_DbContext.Set<TEntity>(), specifications).CountAsync();
        #endregion

    }
}
