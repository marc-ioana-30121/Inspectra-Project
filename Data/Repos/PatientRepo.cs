using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PrettyNeatGenericAPI.Data.Repos
{
    public class PatientRepo : BaseRepo<Patient>
    {
        private readonly PrettyNeatGenericAPIDbContext _dbContext;

        public PatientRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<Patient>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc", string? searchQuery = null)
        {
            var query = _dbContext.Patient.AsQueryable();

            // Filter by search query if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.Name.Contains(searchQuery) || c.Email.Contains(searchQuery));
            }

            // Get the total number of customers before pagination
            var totalPatients = await query.CountAsync();

            query = sortOrder.ToLower() == "asc"
                ? query.OrderBy(x => EF.Property<string>(x, sortBy))
                : query.OrderByDescending(x => EF.Property<string>(x, sortBy));

            // Perform pagination
            var pagedPatients = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return a paginated list of customers
            return new PaginatedList<Patient>(pagedPatients, totalPatients, pageNumber, pageSize);
        }
    }

}
