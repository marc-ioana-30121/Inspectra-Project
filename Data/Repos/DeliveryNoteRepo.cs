using Microsoft.EntityFrameworkCore;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;

namespace PrettyNeatGenericAPI.Data.Repos
{
    public class DeliveryNoteRepo : BaseRepo<DeliveryNote>
    {
        private readonly PrettyNeatGenericAPIDbContext _dbContext;

        public DeliveryNoteRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<DeliveryNote>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc", string? searchQuery = null)
        {
            var query = _dbContext.DeliveryNote.AsQueryable();

            // Filter by search query if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.BagNumber.Contains(searchQuery) 
                                      || c.PartNumber.Contains(searchQuery)
                                      || c.Quality.ToString().Contains(searchQuery)
                                      || c.QuantityIssued.ToString().Contains(searchQuery)
                                      || c.QuantityPassed.ToString().Contains(searchQuery)
                                      || c.QuantityFailed.ToString().Contains(searchQuery)
                                      || c.Supplier.Contains(searchQuery)
                                      || c.InspectionType.Contains(searchQuery)
                                      || c.Notes.Contains(searchQuery)
                                      || c.DateDispatched.ToString().Contains(searchQuery));
            }

            // Get the total number of customers before pagination
            var totalDeliveryNotes = await query.CountAsync();

            query = sortOrder.ToLower() == "asc"
                ? query.OrderBy(x => EF.Property<string>(x, sortBy))
                : query.OrderByDescending(x => EF.Property<string>(x, sortBy));

            // Perform pagination
            var pagedDeliveryNotes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return a paginated list of customers
            return new PaginatedList<DeliveryNote>(pagedDeliveryNotes, totalDeliveryNotes, pageNumber, pageSize);
        }


    }
}
