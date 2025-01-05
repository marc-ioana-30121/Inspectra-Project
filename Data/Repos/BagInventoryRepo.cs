using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;

namespace PrettyNeatGenericAPI.Data.Repos
{
    public class BagInventoryRepo : BaseRepo<BagInventory>
    {
        private readonly PrettyNeatGenericAPIDbContext _dbContext;

        public BagInventoryRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<BagInventory>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc", string? searchQuery = null)
        {
            var query = _dbContext.BagInventory.AsQueryable();

            // Filter by search query if provided
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(c => c.BagNumber.Contains(searchQuery)
                                      || c.MONumber.Contains(searchQuery)
                                      || c.ChitNumber.Contains(searchQuery)
                                      || c.PartNumber.Contains(searchQuery)
                                      || c.Supplier.Contains(searchQuery)
                                      || c.InspectionType.Contains(searchQuery)
                                      || c.Quality.ToString().Contains(searchQuery)
                                      || c.InspectionCode.Contains(searchQuery)
                                      || c.QuantityIssued.ToString().Contains(searchQuery)
                                      || c.DateIssued.ToString().Contains(searchQuery)
                                      || c.RequestedDate.ToString().Contains(searchQuery)
                                      || c.AssignedTo.ToString().Contains(searchQuery)
                                      || c.AssignedDate.ToString().Contains(searchQuery)
                                      || c.QuantityPassed.ToString().Contains(searchQuery)
                                      || c.QuantityFailed.ToString().Contains(searchQuery)
                                      || c.Notes.Contains(searchQuery)
                                      || c.ReturnedDate.ToString().Contains(searchQuery)
                                     );
            }

            // Get the total number of customers before pagination
            var totalBags = await query.CountAsync();

            query = sortOrder.ToLower() == "asc"
                ? query.OrderBy(x => EF.Property<string>(x, sortBy))
                : query.OrderByDescending(x => EF.Property<string>(x, sortBy));

            // Perform pagination
            var pagedBags = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Return a paginated list of customers
            return new PaginatedList<BagInventory>(pagedBags, totalBags, pageNumber, pageSize);
        }

        public virtual async Task<bool> AnyByBagNumberAsync(string bagNumber)
        {
            return await _dbContext.Set<BagInventory>().AnyAsync(c => c.BagNumber == bagNumber);
        }

        public virtual async Task<BagInventory> GetByBagNumberAsync(string bagNumber)
        {
            return await _dbContext.Set<BagInventory>().FirstOrDefaultAsync(c => c.BagNumber == bagNumber);
        }

        public virtual async Task<BagInventory> GetByDeliveryNoteIdAsync(int deliveryNoteId)
        {
            return await _dbContext.Set<BagInventory>().FirstOrDefaultAsync(c => c.DeliveryNoteId == deliveryNoteId);
        }







    }
}
