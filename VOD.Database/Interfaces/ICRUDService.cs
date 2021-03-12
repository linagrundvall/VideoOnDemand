using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Database.Interfaces
{
    public interface ICRUDService
    {
        #region CRUD Methods
        Task<List<TDto>> GetAsync<TEntity, TDto>(bool include = false) 
            where TEntity : class 
            where TDto : class;
        Task<TDto> SingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class
            where TDto : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class;
        Task<int> CreateAsync<TDto, TEntity>(TDto item)
            where TDto : class
            where TEntity : class;
        Task<bool> UpdateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class;
        Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        #endregion
    }
}
