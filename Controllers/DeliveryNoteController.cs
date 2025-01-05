namespace PrettyNeatGenericAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PrettyNeatGenericAPI.Models.TableModels;
    using PrettyNeatGenericAPI.Data.Models;
    using PrettyNeatGenericAPI.Data.Repos;
    using System.Text.RegularExpressions;
    using PrettyNeatGenericAPI.Models.DbModels;
    using System.Text.Json.Serialization;
    using System.Text.Json;

    [ApiController]
    [Route("[controller]")]
    public class DeliveryNoteController : GenericController<DeliveryNote>
    {
        private DeliveryNoteRepo _deliveryNoteRepo;

        private IConfiguration _configuration;

        public DeliveryNoteController(DeliveryNoteRepo repository, IConfiguration configuration) : base(repository)
        {
            _deliveryNoteRepo= repository;
            _configuration = configuration;
        }


        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<Patient>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var customers = await _deliveryNoteRepo.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(customers);
        }

        [HttpGet("Valid")]
        public async Task<IActionResult> GetGoodDeliveryNotes()
        {
            var deliveryNotes = await _deliveryNoteRepo.GetAllAsync();

            if (deliveryNotes == null)
                return BadRequest("No delivery notes");

            // Use LINQ to filter the delivery notes based on conditions
            var goodDeliveryNotes = deliveryNotes
                .Where(deliveryNote => (deliveryNote.QuantityPassed != 0 || deliveryNote.QuantityFailed != 0) && (deliveryNote.Printed == false) )
                .ToList();

            return Ok(goodDeliveryNotes);
        }

        [HttpGet("Valid/{id}")]
        public async Task<IActionResult> GetValidDeliveryById(int id)
        {
            var deliveryNote = await _deliveryNoteRepo.GetByIdAsync(id);

            if (deliveryNote == null)
            {
                return BadRequest("DeliveryNote not found");
            }
            if (deliveryNote.QuantityPassed == 0 && deliveryNote.QuantityFailed == 0)
            {
                return BadRequest("DeliveryNote not valid");
            }

            return Ok(deliveryNote);
        }


        [HttpGet("Valid/GetSelectedRows")]
        public async Task<IActionResult> GetAndValidateSelectedRows([FromQuery] int[] ids)
        {
            var validDeliveryNotes = new List<DeliveryNote>();

            foreach (int id in ids)
            {
                var deliveryNote = await _deliveryNoteRepo.GetByIdAsync(id);

                if (deliveryNote == null)
                {
                    return BadRequest("DeliveryNote not found");
                }

                if (deliveryNote.QuantityPassed == 0 && deliveryNote.QuantityFailed == 0)
                {
                    return BadRequest("DeliveryNote not valid");
                }

                validDeliveryNotes.Add(deliveryNote);
            }

            return Ok(validDeliveryNotes);
        }


    }
}
