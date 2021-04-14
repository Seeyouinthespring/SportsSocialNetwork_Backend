using Microsoft.EntityFrameworkCore;
using SportsSocialNetwork.DataBaseModels;
using SportsSocialNetwork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SportsSocialNetwork.DataAccessLayer
{
    public class CommonRepository : ICommonRepository
    {
        private readonly DataBaseContext _context;

        public CommonRepository(DataBaseContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        public IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _context.Set<T>().Where(expression);
        }
        public IQueryable<T> FindByConditionNoTracking<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _context.Set<T>().AsNoTracking().Where(expression);
        }
        public async Task<List<T>> FindByConditionAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }
        public async Task<List<T>> FindByConditionNoTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _context.Set<T>().AsNoTracking().Where(expression).ToListAsync();
        }
        public T FindFirstByCondition<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _context.Set<T>().Where(expression).FirstOrDefault();
        }
        public async Task<T> FindFirstByConditionAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);
        }
        public async Task<T> FindFirstByConditionWithTrackAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }
        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await _context.Set<T>().AnyAsync(expression);
        }
        public bool Any<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _context.Set<T>().Any(expression);
        }

        public T FindById<T>(long id) where T : class
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T> FindByIdAsync<T>(long id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public T FindById<T>(long[] ids) where T : class
        {
            return _context.Set<T>().Find(ids);
        }
        public T FindById<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T> FindByIdAsync<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().AddRange(entities);
        }
        public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }
        public void UpdateRange<T>(IEnumerable<T> entity) where T : class
        {
            _context.Set<T>().UpdateRange(entity);
        }
        public void Update<T, TProperty>(T entity, Expression<Func<T, TProperty>> dontModifyPropertyExpression) where T : class
        {
            _context.Set<T>().Update(entity).Property(dontModifyPropertyExpression).IsModified = false;
        }
        public void Update<T>(T entity, params string[] dontModifyProperties) where T : class
        {
            var excludePropertieNames = new HashSet<string>(dontModifyProperties);
            _context.Set<T>().Update(entity).Properties.Where(x => excludePropertieNames.Contains(x.Metadata.Name))
                .ToList().ForEach(x => x.IsModified = false);
        }

        public void Delete<T>(T entity) where T : class
        {
            if (entity == null)
                return;
            _context.Set<T>().Remove(entity);
        }
        public async Task DeleteAsync<T>(long id) where T : class
        {
            T entity = await FindByIdAsync<T>(id);
            if (entity == null) return;

            _context.Set<T>().Remove(entity);
        }
        [Obsolete("Use DeleteAll")]
        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void DeleteAll<T>(IEnumerable<T> entities) where T : class
        {
            if (entities == null || !entities.Any())
                return;
            _context.Set<T>().RemoveRange(entities);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> FindByIdAsync<T>(string id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
