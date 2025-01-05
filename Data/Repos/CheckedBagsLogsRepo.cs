using Microsoft.EntityFrameworkCore;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using PrettyNeatGenericAPI.Repositories;

namespace PrettyNeatGenericAPI.Data.Repos
{
    public class CheckedBagsLogsRepo : BaseRepo<CheckedBagsLogs>
    {

        private readonly PrettyNeatGenericAPIDbContext _dbContext;

        public CheckedBagsLogsRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<CheckedBagsLogs> GetByCheckedBagCheckedDateAsync(DateTime checkedDate)
        {
            return await _dbContext.Set<CheckedBagsLogs>().FirstOrDefaultAsync(c => c.CheckedDate == checkedDate);
        }



    }

}

