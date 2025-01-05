namespace PrettyNeatGenericAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PrettyNeatGenericAPI.Models.TableModels;
    using PrettyNeatGenericAPI.Data.Models;
    using PrettyNeatGenericAPI.Data.Repos;
    using System.Text.RegularExpressions;

    [ApiController]
    [Route("[controller]")]
    public class PatientController : GenericController<Patient>
    {
        private PatientRepo _patientRepository;

        private IConfiguration _configuration;

        public PatientController(PatientRepo repository, IConfiguration configuration) : base(repository)
        {
            _patientRepository = (PatientRepo)repository;
            _configuration = configuration;
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<Patient>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var customers = await _patientRepository.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(customers);
        }


        [HttpGet("Barcodes")]

        public async Task<Dictionary<string,string>> GetAsyncBarcodes() 
        {
            
            Dictionary<string, string> formatedBarcodes = new Dictionary<string, string>();

            string[] barcodes = Directory.GetDirectories(@$"{_configuration.GetValue<string>("NetworkSharePath")}/barcodes");

            foreach(string barcode in barcodes)
            {
                var getLast = barcode.Split("/")[barcode.Split("/").Length - 1];
                formatedBarcodes[$@"{getLast.Substring(0, 1)}{getLast.Substring(3, 1)}{Regex.Match(getLast, @"\d+").Value}".ToUpper()] = getLast;

            }

            return formatedBarcodes;
        }
    }
}
