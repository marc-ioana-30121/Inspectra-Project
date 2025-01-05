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
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class DeliveryNotePrintedController : GenericController<DeliveryNotePrinted>
    {
        private DeliveryNotePrintedRepo _deliveryNotePrintedRepo;
        private DeliveryNoteRepo _deliveryNoteRepo;
        private BagInventoryRepo _bagInventoryRepo;

        private IConfiguration _configuration;

        public DeliveryNotePrintedController(DeliveryNotePrintedRepo repository, DeliveryNoteRepo deliveryNoteRepository, BagInventoryRepo bagInventoryRepo, IConfiguration configuration) : base(repository)
        {
            _deliveryNotePrintedRepo = repository;
            _configuration = configuration;
            _deliveryNoteRepo = deliveryNoteRepository;
            _bagInventoryRepo = bagInventoryRepo;
        }

        [HttpGet("GetLastRecordId")]
        public async Task<int> GetLastUpdatedRecordAsync()
        {
            Thread.Sleep(200);
            var deliveryNotesPrinted = await _deliveryNotePrintedRepo.GetAllAsync();

            var lastCreatedDeliveryNotePrinted = deliveryNotesPrinted
                .OrderByDescending(deliveryNotePrinted => deliveryNotePrinted.CreatedDate) // Assuming there's a CreatedDate property
                .FirstOrDefault();

            if (lastCreatedDeliveryNotePrinted == null) return -1; 

            return lastCreatedDeliveryNotePrinted.Id;
        }

        [HttpPost("CreateDeliveryNotePrinted")]
        public async Task<ActionResult> CreateDeliveryNotePrinted(DeliveryNotePrinted deliveryNotePrinted)
        {
            await _deliveryNotePrintedRepo.AddAsync(deliveryNotePrinted);

            foreach (var id in deliveryNotePrinted.DeliveryNoteIds)
            {
                var deliveryNote = await _deliveryNoteRepo.GetByIdAsync(id);
                deliveryNote.Printed = true;
                await _deliveryNoteRepo.UpdateAsync(deliveryNote);

                var bag = await _bagInventoryRepo.GetByDeliveryNoteIdAsync(id);
                bag.IsFirstTime = false;
                bag.IsChecked = true;
                bag.IsReturned = false;
                bag.IsDelivered = true;
                await _bagInventoryRepo.UpdateAsync(bag);
            }

            return CreatedAtAction(nameof(GetById), new { id = deliveryNotePrinted.Id }, deliveryNotePrinted);
        }

        [HttpGet("GetDeliveryNotes/{deliveryNotePrintedId}")]
        public async Task<ActionResult> GetDeliveryNotesFromDeliveryNotePrinted(int deliveryNotePrintedId)
        {
            var deliveryNotePrinted = await _deliveryNotePrintedRepo.GetByIdAsync(deliveryNotePrintedId);

            if (deliveryNotePrinted == null)
                return BadRequest("No Delivery Note Printed found!");

            var deliveryNotes = new List<DeliveryNote>();

            foreach (var deliveryNoteId in deliveryNotePrinted.DeliveryNoteIds)
            {
                deliveryNotes.Add(await _deliveryNoteRepo.GetByIdAsync(deliveryNoteId));
            }

            if (!deliveryNotes.Any())
                return BadRequest("No Delivery Notes in the one printed! Something has gone wrong!");

            return Ok(deliveryNotes);
        }









    }
}
