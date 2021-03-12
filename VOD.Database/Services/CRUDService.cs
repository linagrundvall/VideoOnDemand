using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Database.Contexts;
using VOD.Database.Interfaces;

namespace VOD.Database.Services
{
    public class CRUDService : ICRUDService
    {
        private readonly IDbReadService _dbRead;
        private readonly VODContext _db;
        private readonly IMapper _mapper;

        public CRUDService(IDbReadService dbReadService, VODContext db, IMapper mapper)
        {
            _db = db;
            _dbRead = dbReadService;
            _mapper = mapper;
        }
        
        public async Task<List<TDto>> GetAsync<TEntity, TDto>(bool include = false)
            where TEntity : class
            where TDto : class
        {
            if (include) _dbRead.Include<TEntity>();
            var entities = await _dbRead.GetAsync<TEntity>();
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> SingleAsync<TEntity, TDto>(Expression<Func<TEntity, bool>> expression, bool include = false)
            where TEntity : class
            where TDto : class
        {
            if (include) _dbRead.Include<TEntity>();
            var entity = await _dbRead.SingleAsync(expression);
            return _mapper.Map<TDto>(entity);
        }
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _dbRead.AnyAsync(expression);
        }

        public async Task<int> CreateAsync<TDto, TEntity>(TDto item)
            where TDto : class
            where TEntity : class
        {
            try
            {
                var entity = _mapper.Map<TEntity>(item);
                _db.Add(entity);
                var succeeded = await _db.SaveChangesAsync() >= 0;
                if (succeeded) return (int)entity.GetType().GetProperty("Id").GetValue(entity);
            }
            catch (Exception ex)
            {
            }
            return -1;
        }

        public async Task<bool> UpdateAsync<TDto, TEntity>(TDto item) where TDto : class where TEntity : class
        {
            try
            {
                var entity = _mapper.Map<TEntity>(item);
                _db.Update(entity);
                return await _db.SaveChangesAsync() >= 0;
            }
            catch { }

            return false;
        }

        public async Task<bool> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            try
            {
                var entity = await _dbRead.SingleAsync(expression);
                _db.Remove(entity);
                return await _db.SaveChangesAsync() >= 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
