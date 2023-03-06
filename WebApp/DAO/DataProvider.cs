using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface IDataProvider
    {
        IQueryable<TEntity> Select<TEntity>() where TEntity : class;
        Task Insert<TEntity>(TEntity entity) where TEntity : class;
        Task Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        Task Update<TEntity>(TEntity entity) where TEntity : class;
        Task Delete<TEntity>(TEntity entity) where TEntity : class;
        Task SaveAsync();
    }

    public class DataProvider : IDataProvider
    {
        readonly TodoDbContext _dbContext;

        public DataProvider(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> Select<TEntity>()
            where TEntity : class
        {
            
            return _dbContext.Set<TEntity>();
        }

        async public Task Insert<TEntity>(TEntity entity) where TEntity : class
        {            
            _dbContext.Entry(entity).State = EntityState.Added;
            await _dbContext.AddAsync(entity);
            //_dbContext.SaveChanges();
        }

        /// <summary>
        /// Запись нескольких полей в БД
        /// </summary>
        async public Task Inserts<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {

            // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
            //_dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

            foreach (TEntity entity in entities)
                _dbContext.Entry(entity).State = EntityState.Added;
            //_dbContext.SaveChanges();

            //_dbContext.ChangeTracker.AutoDetectChangesEnabled = true;            
        }

        async public Task Update<TEntity>(TEntity entity)
    where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            //_dbContext.SaveChanges();
        }

        async public Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        async public Task Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Entry<TEntity>(entity).State = EntityState.Deleted;
        }
    }
}
