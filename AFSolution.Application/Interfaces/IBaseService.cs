using AFSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Application.Interfaces
{
    public interface IBaseService<TEntityDto>  where TEntityDto : class
    {
        Task<TEntityDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntityDto>> GetAllAsync();
        Task<TEntityDto> CreateAsync(TEntityDto dto);
        Task UpdateAsync(Guid id, TEntityDto dto);
        Task DeleteAsync(Guid id);

        Task<PaginatedResult<TEntityDto>> GetPagedAsync(int pageIndex, int pageSize);
        //Task<PaginatedResult<TEntityDto>> GetPagedAsync(
        //    int pageIndex,
        //    int pageSize,
        //    Expression<Func<TEntityDto, bool>> filter = null,
        //    Func<IQueryable<TEntityDto>, IOrderedQueryable<TEntityDto>> orderBy = null);
    }
}
