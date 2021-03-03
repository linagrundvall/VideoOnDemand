using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace VOD.Database.Services
{
    public interface IDbReadService
    {
        Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class;
        Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) 
            where TEntity : class;
        Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class;

        void Include<TEntity>() where TEntity : class;
        void Include<TEntity1, TEntity2>() 
            where TEntity1 : class
            where TEntity2 : class;

    }
}
