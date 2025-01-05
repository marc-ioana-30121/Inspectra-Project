using Microsoft.EntityFrameworkCore;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;

namespace PrettyNeatGenericAPI.Data.Repos
{
    public class ReturnLogsRepo : BaseRepo<ReturnLogs>
    {
        private readonly PrettyNeatGenericAPIDbContext _dbContext;

        public ReturnLogsRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<ReturnLogs>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc", string? searchQuery = null)
        {
            var query = _dbContext.ReturnLogs.AsQueryable();

            // Filter by search query if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.BagNumber.Contains(searchQuery)
                                      || c.EmployeeName.Contains(searchQuery)
                                      || c.ReturnedDate.ToString().Contains(searchQuery)
                                      || c.Fault.Contains(searchQuery)
                                    );
            }

            // Get the total number of customers before pagination
            var totalReturnLogs = await query.CountAsync();

            query = sortOrder.ToLower() == "asc"
                ? query.OrderBy(x => EF.Property<string>(x, sortBy))
                : query.OrderByDescending(x => EF.Property<string>(x, sortBy));

            // Perform pagination
            var pagedReturnLogs = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return a paginated list of customers
            return new PaginatedList<ReturnLogs>(pagedReturnLogs, totalReturnLogs, pageNumber, pageSize);
        }
    }
}
