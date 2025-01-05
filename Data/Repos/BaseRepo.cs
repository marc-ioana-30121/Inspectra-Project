using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrettyNeatGenericAPI.Repositories;

public abstract class BaseRepo<T> where T : AuditableEntity
{
    private readonly PrettyNeatGenericAPIDbContext _dbContext;

    private readonly IHttpContextAccessor _httpContextAccessor;


    public BaseRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task<bool> AnyByIdAsync(int id)
    {
        return await _dbContext.Set<T>().AnyAsync(c => c.Id == id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var userId = (_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        entity.CreatedBy = int.Parse(userId.Value);
        entity.CreatedDate = DateTime.Now;
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        var userId = (_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
        entity.UpdatedBy = int.Parse(userId.Value);
        entity.UpdatedDate = DateTime.Now;
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task<PaginatedList<T>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc")
    {
        IQueryable<T> entities = _dbContext.Set<T>();

        // Apply sorting if sortBy is specified
        if (!string.IsNullOrEmpty(sortBy))
        {
            var property = typeof(T).GetProperty(sortBy);

            if (property != null)
            {
                entities = sortOrder.ToLower() == "asc"
                    ? entities.OrderBy(x => EF.Property<string>(x, sortBy))
                    : entities.OrderByDescending(x => EF.Property<string>(x, sortBy));
            }
        }

        var count = await entities.CountAsync();
        var pagedEntities = await entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(pagedEntities, count, pageNumber, pageSize);
    }
}


