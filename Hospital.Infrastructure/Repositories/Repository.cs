using Hospital.Core.DTOs.Common;
using Hospital.Core.Interfaces.Repositories;
using Hospital.Infrastructure.Data;
using Hospital.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hospital.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly HospitalDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public Repository(HospitalDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        // ---------------------------------------------------------
        // PAGINATION SUPPORT: Used by all repositories
        // ---------------------------------------------------------
        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize)
        {
            return await _dbSet
                .AsNoTracking()
                .ToPagedResultAsync(page, pageSize);
        }
    }
}
