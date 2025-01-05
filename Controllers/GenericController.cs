using Microsoft.AspNetCore.Mvc;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;
using System.Text.Json.Serialization;
using System.Text.Json;
using PrettyNeatGenericAPI.Data.Models;

namespace PrettyNeatGenericAPI.Controllers
{
    [ApiController]
    public abstract class GenericController<TEntity> : ControllerBase where TEntity : AuditableEntity
    {
        private readonly BaseRepo<TEntity> _repository;

        protected GenericController(BaseRepo<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

            };
            var entities = await _repository.GetAllAsync();
            return Results.Json(entities, options);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

            };

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return Results.NotFound();
            }
            return Results.Json(entity, options);
        }

        [HttpPost]
        public async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }

        [HttpPut]
        public async Task<IActionResult> Update(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.AnyByIdAsync(id);
            if (!entity)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<IResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc")
        {
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull

            };
            var entities = await _repository.GetPagedAsync(page, pageSize, sortBy, sortDirection);
            return Results.Json(entities, options);
        }

    }
}