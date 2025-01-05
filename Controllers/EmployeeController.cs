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
    public class EmployeeController : GenericController<Employee>
    {
        private EmployeeRepo _employeeRepository;

        private IConfiguration _configuration;

        public EmployeeController(EmployeeRepo repository, IConfiguration configuration) : base(repository)
        {
            _employeeRepository =  repository;
            _configuration = configuration;
        }


        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<Patient>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var customers = await _employeeRepository.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(customers);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateByNameAndSurname (int id, [FromBody]UpdateNameAndSurnameAndPunchCode updatedEmployee)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return BadRequest("I could not find id");

            employee.PunchCode = updatedEmployee.PunchCode;
            employee.Name = updatedEmployee.Name;
            employee.Surname = updatedEmployee.Surname;
            await _employeeRepository.UpdateAsync(employee);

            return NoContent();
        }

        public class UpdateNameAndSurnameAndPunchCode
        {   
            public string PunchCode { get; set; }
            public string Name { get; set; }
            public string Surname { get; set;}
        }








    }
}
