using AFSolution.Application.Interfaces;
using AFSolution.Domain.Entities;
using AFSolution.Domain.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Application.Services
{
    public abstract class BaseService<TEntity, TEntityDto> : IBaseService<TEntityDto>
        where TEntity : BaseEntity
        where TEntityDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TEntityDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TEntityDto>>(entities);
        }

        public virtual async Task<TEntityDto> CreateAsync(TEntityDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
     
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return _mapper.Map<TEntityDto>(entity);
        }

        public virtual async Task UpdateAsync(Guid id, TEntityDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            _mapper.Map(dto, entity);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task<PaginatedResult<TEntityDto>> GetPagedAsync(int pageIndex,int pageSize)
        {
            Expression<Func<TEntity, bool>> filter = null;
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null;
            var pagedEntities = await _repository.GetPagedAsync(pageIndex, pageSize, filter, orderBy);
            var pagedDtos = new PaginatedResult<TEntityDto>
            {
                Items = _mapper.Map<List<TEntityDto>>(pagedEntities.Items),
                PageIndex = pagedEntities.PageIndex,
                PageSize = pagedEntities.PageSize,
                TotalItems = pagedEntities.TotalItems
            };
            return pagedDtos;
        }
    }

}
