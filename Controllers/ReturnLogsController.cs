namespace PrettyNeatGenericAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PrettyNeatGenericAPI.Models.TableModels;
    using PrettyNeatGenericAPI.Data.Models;
    using PrettyNeatGenericAPI.Data.Repos;
    using System.Text.RegularExpressions;
    using PrettyNeatGenericAPI.Models.DbModels;

    [ApiController]
    [Route("[controller]")]
    public class ReturnLogsController : GenericController<ReturnLogs>
    {
        private ReturnLogsRepo _returnLogsRepo;

        private IConfiguration _configuration;

        public ReturnLogsController(ReturnLogsRepo repository, IConfiguration configuration) : base(repository)
        {
            _returnLogsRepo = repository;
            _configuration = configuration;
        }


        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<ReturnLogs>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var returnLogs = await _returnLogsRepo.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(returnLogs);
        }





    }
}
