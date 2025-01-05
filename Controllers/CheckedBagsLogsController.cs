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
    public class CheckedBagsLogsController : GenericController<CheckedBagsLogs>
    {
        private CheckedBagsLogsRepo _checkedBagsLogsRepo;

        private IConfiguration _configuration;

        public CheckedBagsLogsController(CheckedBagsLogsRepo repository, IConfiguration configuration) : base(repository)
        {
            _checkedBagsLogsRepo = repository;
            _configuration = configuration;
        }


    
    }
}
