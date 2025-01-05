namespace PrettyNeatGenericAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PrettyNeatGenericAPI.Models.TableModels;
    using PrettyNeatGenericAPI.Data.Models;
    using PrettyNeatGenericAPI.Data.Repos;
    using System.Text.RegularExpressions;
    using PrettyNeatGenericAPI.Models.DbModels;
    using Microsoft.AspNetCore.JsonPatch;
    using System.ComponentModel.DataAnnotations.Schema;
    using ClosedXML.Excel;
    using System.IO;
    using Microsoft.EntityFrameworkCore;
    using PrettyNeatGenericAPI.Migrations;
    using DocumentFormat.OpenXml.Wordprocessing;

    [ApiController]
    [Route("[controller]")]
    public class BagInventoryController : GenericController<BagInventory>
    {
        private BagInventoryRepo _bagInventoryRepository;
        private DeliveryNoteRepo _deliveryNoteRepository;
        private EmployeeRepo _employeeRepository;
        private ReturnLogsRepo _returnLogsRepository;
        private CheckedBagsLogsRepo _checkedBagsLogsRepo;
        private QualifiersRepo _qualifiersRepo;

        private IConfiguration _configuration;

        public BagInventoryController(DeliveryNoteRepo deliveryRepository, BagInventoryRepo bagRepository, EmployeeRepo employeeRepository, ReturnLogsRepo returnLogsRepo, CheckedBagsLogsRepo checkedBagsLogsRepo, QualifiersRepo qualifiersRepo, IConfiguration configuration) : base(bagRepository)
        {
            _deliveryNoteRepository = deliveryRepository;
            _bagInventoryRepository = bagRepository;
            _employeeRepository = employeeRepository;
            _returnLogsRepository = returnLogsRepo;
            _checkedBagsLogsRepo = checkedBagsLogsRepo;
            _qualifiersRepo = qualifiersRepo;
            _configuration = configuration;
        }


        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<BagInventory>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var customers = await _bagInventoryRepository.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(customers);
        }


        //Return Bag PAGE 

        //After you search update the attributes for returned and checked
        [HttpPut("ReturnBagFault/{bagNumber}")]
        public async Task<IActionResult> UpdateReturnedBagFault(string bagNumber, [FromBody] FaultClass faultObj)
        {   
            var returningDate = DateTime.Now;
            
            var bag = await _bagInventoryRepository.GetByBagNumberAsync(bagNumber);

            if (bag == null)
                return BadRequest("Failed to find a bag by the Bag Number provided");

            if (bag.IsFirstTime == true)
                return BadRequest("You cannot Return something that it has not been sent");

            if (faultObj == null)
                return BadRequest("No fault specified!");

            var employee = await _employeeRepository.GetByIdAsync((int)bag.AssignedTo);

            if (employee == null) return BadRequest("No Employee found!");

            var newReturnLog = new ReturnLogs
            {
                EmployeeName = employee.Name,
                EmployeeSurname = employee.Surname,
                PunchCode = employee.PunchCode,
                BagNumber = bag.BagNumber,
                ReturnedDate = returningDate,
                Fault = faultObj.Fault
            };

            var checkedBagAlready = await _checkedBagsLogsRepo.GetByCheckedBagCheckedDateAsync((DateTime)bag.CheckedDate);

            if (faultObj.Fault == "Employee Fault")
                { checkedBagAlready.Price = 0; checkedBagAlready.ChargePrice = 0; }

            await _checkedBagsLogsRepo.UpdateAsync(checkedBagAlready);

            bag.IsChecked = false;
            bag.IsReturned = true;
            bag.QuantityFailed = 0;
            bag.QuantityPassed = 0;
            bag.ReturnedDate = returningDate;
            bag.CheckedDate = null;
            bag.IsDelivered = false;
            bag.AssignedTo = null;
            bag.Assigned = false;
            bag.AssignedDate = null;

            if (faultObj.Fault == "Employee Fault")
                bag.TssFault = false;
            else
                bag.TssFault = true;

            var deliveryNote = await _deliveryNoteRepository.GetByIdAsync(bag.DeliveryNoteId);

            if (deliveryNote == null)
                return BadRequest("Dlivery Note not found!");

            await UpdateDeliveryNoteWithBagProperties(deliveryNote, bag);

            deliveryNote.Printed = false;

            await _bagInventoryRepository.UpdateAsync(bag);
            await _deliveryNoteRepository.UpdateAsync(deliveryNote);
            await _returnLogsRepository.AddAsync(newReturnLog);

            return Ok();
        }

        [HttpGet("GetReturnedBags")]
        public async Task<IActionResult> GetReturnedBags()
        {
            var bags = await _bagInventoryRepository.GetAllAsync();

            if (bags == null)
                return BadRequest("No bags");

            // Use LINQ to filter the delivery notes based on conditions
            var returnedBags = bags
                .Where(returnedBag => returnedBag.IsFirstTime == false && returnedBag.IsChecked == false && returnedBag.IsReturned == true)
                .ToList();

            return Ok(returnedBags);
        }

        //WHAT IS IN STOCK BAG

        [HttpGet("GetWhatIsInStock")]
        public async Task<IActionResult> GetWhatIsInStock()
        {
            var bags = await _bagInventoryRepository.GetAllAsync();

            if (bags == null)
                return BadRequest("No bags");

            // Use LINQ to filter the delivery notes based on conditions

            var stockBags = bags
                .Where(stockBag => stockBag.IsDelivered == false)
                .ToList();

            return Ok(stockBags);
        }



        //UPLOAD BAG

        private async Task <IFormFile> ChangeFileExtension(IFormFile originalFile, string newExtension)
        {
            var originalFileName = Path.GetFileNameWithoutExtension(originalFile.FileName);
            var newFileName = $"{originalFileName}.{newExtension}";

            // Create a MemoryStream to copy the content of the original IFormFile
            var memoryStream = new MemoryStream();
            originalFile.CopyTo(memoryStream);

                // Create a new IFormFile with the modified extension
                var newFile = new FormFile(memoryStream, 0, memoryStream.Length, originalFileName, newFileName)
                {
                    Headers = originalFile.Headers
                };

                return newFile;
            
        }

        [HttpPost("UploadBag")]
        public async Task <ActionResult> SaveDatabase(IFormFile formFile)
        {   
            //Change it to xlsx format because of the corrupted data 
            //XLWorkbook does not really work with excel 97
            var extension = "xlsx";
            var fileInGoodFormat = await ChangeFileExtension(formFile,extension);
            fileInGoodFormat.ContentDisposition.ToString().Replace(".xls", ".xlsx");

            var savedBags = new List<BagInventory>();
            var alreadyExistingBags = new List<BagInventory>();

            try
            {
                using (XLWorkbook excelWorkbook = new XLWorkbook(fileInGoodFormat.OpenReadStream()))
                {
                    var nonEmptyDataRows = excelWorkbook.Worksheet(1).RowsUsed();

                    foreach (var dataRow in nonEmptyDataRows.Skip(1))
                    {

                        var newbag = new BagInventory
                        {
                            BagNumber = dataRow.Cell(1).GetString(),
                            MONumber = dataRow.Cell(2).GetString(),
                            ChitNumber = dataRow.Cell(3).GetString(),
                            PartNumber = dataRow.Cell(4).GetString(),
                            Supplier = dataRow.Cell(5).GetString(),
                            InspectionType = dataRow.Cell(6).GetString(),
                            Quality = dataRow.Cell(7).GetString(),
                            QuantityIssued = Convert.ToInt32(dataRow.Cell(8).GetString()),
                            DateIssued = DateTime.FromOADate(Convert.ToDouble(dataRow.Cell(9).GetString())).ToString("dd/MM/yyyy"),
                            RequestedDate = DateTime.FromOADate(Convert.ToDouble(dataRow.Cell(10).GetString())).ToString("dd/MM/yyyy"),
                            IsFirstTime = true,
                            IsChecked = false,
                            IsReturned = false,
                            IsDelivered = false,
                            DeliveryNoteId = 0,
                            DeliveryNote = new DeliveryNote { Id = 0 } 
                            
                        };

                        //check if the BagNumber exists in Database, otherwise add it to savedBags

                        if (await _bagInventoryRepository.AnyByBagNumberAsync(newbag.BagNumber) == true)
                        {
                            alreadyExistingBags.Add(newbag);
                        }
                        else
                        {
                            savedBags.Add(newbag);
                        }

                    }

                    if (savedBags.Count == 0)
                        return BadRequest("File Has Been Already Uploaded / all the BagNumbers are in the database");

                    foreach (var bag in savedBags)
                     {
                        if (bag.Quality.Length == 1) bag.Quality = "00" + bag.Quality;
                        if (bag.Quality.Length == 2) bag.Quality = "0" + bag.Quality;
                        
                            
                        await _bagInventoryRepository.AddAsync(bag);

                        //update the delivery note with data from the new bag
                        var deliveryNote = await _deliveryNoteRepository.GetByIdAsync(bag.DeliveryNoteId);
                        await UpdateDeliveryNoteWithBagProperties(deliveryNote, bag);
                        }

                }

                return Ok(alreadyExistingBags);
            }

            catch (Exception e)
            {
                return BadRequest("Something did not work well!" + e);
            }
        }

        [HttpGet("GetUploadedBags")]
        public async Task<IActionResult> GetOnlyFirstTimeUploadedBags()
        {
            var bags = await _bagInventoryRepository.GetAllAsync();

            if (bags == null)
                return BadRequest("No bags");

            // Use LINQ to filter the delivery notes based on conditions
            var goodBags = bags
                .Where(bag => bag.IsFirstTime == true && bag.IsChecked == false)
                .ToList();

            return Ok(goodBags);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById([FromBody] BagInventoryUpdate bagInventoryUpdate, int id)
        {

            var bag = await _bagInventoryRepository.GetByIdAsync(id);

            if (bag == null)
                return BadRequest("I could not find id");

            var bagProperties = typeof(BagInventoryUpdate).GetProperties();

            if(bagInventoryUpdate.QuantityIssued <0)
            {
                return BadRequest("Quantity impossible to be negative!");
            }

            if (bag.QuantityIssued != bagInventoryUpdate.QuantityIssued)
            {
                bag.QuantityFailed = 0;
                bag.QuantityPassed = 0;
            }

            foreach (var property in bagProperties)
            {
                var newValue = property.GetValue(bagInventoryUpdate);

                if (newValue != null)
                {
                    var bagProperty = typeof(BagInventory).GetProperty(property.Name);
                    bagProperty.SetValue(bag, newValue);
                }
            }

          

            await _bagInventoryRepository.UpdateAsync(bag);

            var deliveryNote = await _deliveryNoteRepository.GetByIdAsync(bag.DeliveryNoteId);

            //this is a bit changed

            await UpdateDeliveryNoteWithBagProperties(deliveryNote, bag);

            
            return NoContent();
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBagAndDelivery(int id)
        {
            var bag = await _bagInventoryRepository.GetByIdAsync(id);
            if (bag == null)
            {
                return NotFound();
            }


            await _bagInventoryRepository.DeleteAsync(id);
            await _deliveryNoteRepository.DeleteAsync(bag.DeliveryNoteId);

            return NoContent();
        }


        [HttpPut("UpdateBagQuantities")]
        public async Task<IActionResult> UpdateBagQuantities([FromBody] BagQuantities bagQuantitiesUpdate)
        {
           var checkDate = DateTime.Now;
           var bag = await _bagInventoryRepository.GetByIdAsync(bagQuantitiesUpdate.Id);
           var qualifiers = await _qualifiersRepo.GetAllAsync();

            if (qualifiers == null)
            {
                return BadRequest("No qualifiers Uploaded");
            }

            if (bag == null)
                return BadRequest("I could not find id");

            if (bag.QuantityIssued != bagQuantitiesUpdate.QuantityFailed + bagQuantitiesUpdate.QuantityPassed)
            {
                bag.CheckedDate = null;
                return BadRequest("Quantity issued must be equal to failed and passed"); 
            }

            var matchingQualifiers = qualifiers.Where(q =>
                                q.PartNumber == bag.PartNumber &&
                                q.TssCode == bag.Quality
                                ).ToList();

            if (!matchingQualifiers.Any())
            {
                return BadRequest("No Qualifier found!");
            }
            
            bag.Notes = bagQuantitiesUpdate.Notes;
            bag.QuantityPassed = bagQuantitiesUpdate.QuantityPassed;
            bag.QuantityFailed = bagQuantitiesUpdate.QuantityFailed;

            bag.IsChecked = true;
            bag.CheckedDate = checkDate;

            var employee = await _employeeRepository.GetByIdAsync((int)bag.AssignedTo);

            var checkedBag = new CheckedBagsLogs
            {
                Name = employee.Name,
                Surname = employee.Surname,
                PunchCode = employee.PunchCode,
                QuantityIssued = bag.QuantityIssued,
                CheckedDate = checkDate,
                Price = matchingQualifiers[0].PayPerPiece * bag.QuantityIssued,
                ChargePrice = matchingQualifiers[0].ChargePerPiece * bag.QuantityIssued,
                BagNumber = bag.BagNumber,
                MONumber = bag.MONumber,
                PartNumber = bag.PartNumber,
                ChitNumber = bag.ChitNumber,
                Quality = bag.Quality,
                DateIssued = bag.DateIssued,
                RequestedDate = bag.RequestedDate,
                Supplier = bag.Supplier,
            };

            await _checkedBagsLogsRepo.AddAsync(checkedBag);
            
            var deliveryNote = await _deliveryNoteRepository.GetByIdAsync(bag.DeliveryNoteId);

            await _bagInventoryRepository.UpdateAsync(bag);
            await UpdateDeliveryNoteWithBagProperties(deliveryNote,bag);

           return Ok(200);
        }


        [HttpPut("AssignEmployee")]
        public async Task<IActionResult> UpdateEmployeeById([FromBody] AssignEmployeeRequest data) 
        {
            
            var bag = await  _bagInventoryRepository.GetByIdAsync(data.BagInventoryId);
            var employee = await _employeeRepository.GetByIdAsync(data.EmployeeId);

            if (bag == null)
                return BadRequest("Bag not found!");

            if ((int)data.EmployeeId != 0)
            {   

                //daca este vina employee stergem ultima aparitie a in assignbags pt acel bag
                if (bag.TssFault == false || bag.Assigned == true) 
                {   
                    var updateEmployee = await _employeeRepository.GetByIdAsync((int)bag.LastAssignedTo);

                    if (updateEmployee != null && updateEmployee.AssignedBag.Contains(bag.BagNumber))
                    {
                        updateEmployee.AssignedBag.Remove(bag.BagNumber);
                        await _employeeRepository.UpdateAsync(updateEmployee);
                    }
                }

                
                bag.AssignedTo = data.EmployeeId;
                bag.Assigned = true;
                bag.AssignedDate = (DateTime.Now);

                if (employee.AssignedBag == null)
                {
                    employee.AssignedBag = new List<string>();
                }

                employee.AssignedBag.Add(bag.BagNumber);
                await _employeeRepository.UpdateAsync(employee);
            }
            else if(bag.LastAssignedTo != 0)
            {
                var updateEmployee = await _employeeRepository.GetByIdAsync((int)bag.LastAssignedTo);
                updateEmployee.AssignedBag.Remove(bag.BagNumber);
                await _employeeRepository.UpdateAsync(updateEmployee);

                bag.Assigned = false;
                bag.AssignedTo = 0;
                bag.AssignedDate = null;
                
            }
            
            else { return NoContent(); }

            bag.LastAssignedTo = data.EmployeeId;

            await _bagInventoryRepository.UpdateAsync(bag);

            return NoContent();
        }

        private async Task UpdateDeliveryNoteWithBagProperties(DeliveryNote deliveryNote, BagInventory bag)
        {
        
            if (deliveryNote == null)
                throw new Exception("Delivery Note NOT FOUND");

            deliveryNote.BagNumber = bag.BagNumber;
            deliveryNote.PartNumber = bag.PartNumber;
            deliveryNote.ChitNumber = bag.ChitNumber;
            deliveryNote.MONumber = bag.MONumber;
            deliveryNote.Quality = bag.Quality;
            deliveryNote.QuantityIssued = bag.QuantityIssued;
            deliveryNote.QuantityPassed = bag.QuantityPassed ?? 0;
            deliveryNote.QuantityFailed = bag.QuantityFailed ?? 0;
            deliveryNote.Supplier = bag.Supplier;
            deliveryNote.InspectionType = bag.InspectionType;
            deliveryNote.Notes = bag.Notes;

            await _deliveryNoteRepository.UpdateAsync(deliveryNote);
        }

    }

    public class FaultClass
    {
        public string Fault { get; set; }

    }

    public class AssignEmployeeRequest
    {   
        public int EmployeeId { get; set; }
        public int BagInventoryId { get; set; }
    }

    public class BagQuantities 
    {   public int Id { get; set; }
        public int? QuantityPassed { get; set; }

        public int? QuantityFailed { get; set; }

        public string? Notes { get; set; }
    }

    public class BagInventoryUpdate
    {   


        public string BagNumber { get; set; }

        public string MONumber { get; set; }

        public string ChitNumber { get; set; }

        public string PartNumber { get; set; }

        public string Supplier { get; set; }

        public string InspectionType { get; set; }

        public string Quality { get; set; }

        public int QuantityIssued { get; set; }

        public string DateIssued { get; set; }

        public string RequestedDate { get; set; }

        public bool? Assigned { get; set; } 

        public int? AssignedTo { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public bool? Returned { get; set; }

    }

    public class DeliveryNoteFromBag
    {

        public string BagNumber { get; set; }

        public string PartNumber { get; set; }

        public string Supplier { get; set; }

        public string InspectionType { get; set; }

        public string Quality { get; set; }

        public int QuantityIssued { get; set; }

        public int? QuantityPassed { get; set; }

        public int? QuantityFailed { get; set; }

        public string? Notes { get; set; }

    }
}
