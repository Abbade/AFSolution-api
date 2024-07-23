using AFSolution.Application.Services;
using AFSolution.Application.Interfaces;
using AFSolution.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AFSolution.Api.Controllers
{
    public abstract class BaseController<TDto> : ControllerBase
        where TDto : class
    {
        protected readonly IBaseService<TDto> _service;

        public BaseController(IBaseService<TDto> service)
        {
            _service = service;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (result == null)
            {
                return BadRequest("Unable to create the entity.");
            }

            return Created(string.Empty, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, TDto dto)
        {
            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("Paged")]
        public async Task<IActionResult> GetPaged(int pageIndex, int pageSize)
        {
            var result = await _service.GetPagedAsync(pageIndex, pageSize);
            return Ok(result);
        }
    }
}
