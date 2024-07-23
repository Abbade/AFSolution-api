using AFSolution.Domain.Entities;
using AFSolution.Domain.Interfaces;
using AFSolution.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity is BaseEntity entityWithGuid && entityWithGuid.Id == Guid.Empty)
            {
                entityWithGuid.Id = Guid.NewGuid();
            }


            await _dbSet.AddAsync(entity);
        }

        public virtual  void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
        }

        public async Task<PaginatedResult<T>> GetPagedAsync(
                int pageIndex, int pageSize,
                Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalItems = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<T>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
        public  async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual void Remove(T entity)
        {
            if (entity is BaseEntity entityWithGuid)
            {
                entityWithGuid.IsActive = false;
                _dbSet.Update(entity);
                return;
            }
            _dbSet.Remove(entity);
        }

    }
}
