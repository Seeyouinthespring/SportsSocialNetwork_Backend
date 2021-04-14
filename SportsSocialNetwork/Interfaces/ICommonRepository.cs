using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SportsSocialNetwork.Interfaces
{
    public interface ICommonRepository
    {
        IQueryable<T> GetAll<T>() where T : class;
        Task<List<T>> GetAllAsync<T>() where T : class;
        IQueryable<T> FindByCondition<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<T> FindByConditionNoTracking<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<List<T>> FindByConditionAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<List<T>> FindByConditionNoTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        T FindFirstByCondition<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FindFirstByConditionAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FindFirstByConditionWithTrackAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        bool Any<T>(Expression<Func<T, bool>> expression) where T : class;
        T FindById<T>(long id) where T : class;
        Task<T> FindByIdAsync<T>(long id) where T : class;
        Task<T> FindByIdAsync<T>(string id) where T : class;
        T FindById<T>(long[] ids) where T : class;
        T FindById<T>(int id) where T : class;
        Task<T> FindByIdAsync<T>(int id) where T : class;
        void Add<T>(T entity) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        void AddRange<T>(IEnumerable<T> entities) where T : class;
        Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
        void Update<T>(T entity) where T : class;
        void UpdateRange<T>(IEnumerable<T> entity) where T : class;
        void Update<T, TProperty>(T entity, Expression<Func<T, TProperty>> dontModifyPropertyExpression) where T : class;
        void Update<T>(T entity, params string[] dontModifyProperties) where T : class;
        void Delete<T>(T entity) where T : class;
        Task DeleteAsync<T>(long id) where T : class;
        void Delete<T>(IEnumerable<T> entities) where T : class;
        void DeleteAll<T>(IEnumerable<T> entities) where T : class;
        void Save();
        Task SaveAsync();
    }
}
