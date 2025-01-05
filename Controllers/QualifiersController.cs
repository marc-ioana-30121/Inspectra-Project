namespace PrettyNeatGenericAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PrettyNeatGenericAPI.Models.TableModels;
    using PrettyNeatGenericAPI.Data.Models;
    using PrettyNeatGenericAPI.Data.Repos;
    using System.Text.RegularExpressions;
    using PrettyNeatGenericAPI.Models.DbModels;
    using System.Data;
    using ClosedXML.Excel;
    using DocumentFormat.OpenXml.Spreadsheet;

    [ApiController]
    [Route("[controller]")]
    public class QualifiersController : GenericController<Qualifiers>
    {
        private QualifiersRepo _qualifiersRepo;
        private EmployeeRepo _employeeRepo;
        private BagInventoryRepo _bagInventoryRepo;
        private CheckedBagsLogsRepo _checkedBagsLogsRepo;

        private IConfiguration _configuration;

        public QualifiersController(QualifiersRepo repository, EmployeeRepo employeeRepo, BagInventoryRepo bagInventoryRepo, CheckedBagsLogsRepo checkedBagsLogsRepo, IConfiguration configuration) : base(repository)
        {
            _qualifiersRepo = repository;
            _employeeRepo = employeeRepo;
            _bagInventoryRepo = bagInventoryRepo;
            _checkedBagsLogsRepo = checkedBagsLogsRepo;
            _configuration = configuration;
        }

        [HttpGet("search")]
        public async Task<ActionResult<PaginatedList<Patient>>> GetPagedCustomer([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "Id", [FromQuery] string sortDirection = "asc", [FromQuery] string? searchQuery = "")
        {
            var customers = await _qualifiersRepo.GetPagedAsync(page, pageSize, sortBy, sortDirection, searchQuery);
            return Ok(customers);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateById([FromBody] QualifiersUpdate qualifierUpdate, int id)
        {
            var qualifier = await _qualifiersRepo.GetByIdAsync(id);

            if (qualifier == null) return BadRequest("No Qualifier found!");

            qualifier.ResponsiblePerson = qualifierUpdate.ResponsiblePerson;
            qualifier.DateOfIssue = qualifierUpdate.DateOfIssue;
            qualifier.PartNumber = qualifierUpdate.PartNumber;
            qualifier.Type = qualifierUpdate.Type;
            qualifier.TssCode = qualifierUpdate.TssCode;
            qualifier.ChargePerPiece = qualifierUpdate.ChargePerPiece;
            qualifier.PayPerPiece = qualifierUpdate.PayPerPiece;
            qualifier.ChargePerPieceDelay = qualifierUpdate.ChargePerPieceDelay;
            qualifier.PayPerPieceDelay = qualifierUpdate.PayPerPieceDelay;
            qualifier.ChargePerPieceExtraDelay = qualifierUpdate.ChargePerPieceExtraDelay;
            qualifier.PayPerPieceExtraDelay = qualifierUpdate.PayPerPieceExtraDelay;
            qualifier.EmployeePiecesPerHour = qualifierUpdate.EmployeePiecesPerHour;
            qualifier.ClientPiecesPerHour = qualifierUpdate.ClientPiecesPerHour;

            await _qualifiersRepo.UpdateAsync(qualifier);

            return Ok("Qualifier Updated!");

        }

        [HttpGet("EmployeeWageTable")]
        public async Task<IActionResult> CreateExcelForEmployeeExcel([FromQuery] string startDate, [FromQuery] string finalDate)
        {
            var empData = await GenerateEmployeeDataTable(startDate, finalDate);

            if (empData == null) return BadRequest("No checked bags betweend those dates!");

            var tssData = await GenerateTSSDataTable(startDate, finalDate);

            if (tssData == null) return BadRequest("No checked bags between those dates!");

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(empData, "Employee Salaries");
                wb.AddWorksheet(tssData, "Tss Charges");

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sample.xlsx");
                }
            }

        }


        [HttpGet("CHECK")]
        public async Task<DataTable> GenerateEmployeeDataTable(string startDate, string finalDate)
        {
            var dt = new DataTable();

            dt.TableName = "EmployeeWage";

            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Qty_in", typeof(int));
            dt.Columns.Add("Date_in", typeof(string));
            dt.Columns.Add("Price", typeof(float));
            dt.Columns.Add("Or_BagNumber", typeof(string));
            dt.Columns.Add("Or_MoNumber", typeof(string));
            dt.Columns.Add("Or_PartNumber", typeof(string));

            var _bagsListChecked = await _checkedBagsLogsRepo.GetAllAsync();
            var employeeList = await _employeeRepo.GetAllAsync();


            if (DateTime.TryParseExact(startDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var dateOfStart) &&
           DateTime.TryParseExact(finalDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var dateOfEnd))
            {
                var betweenDatesBags = _bagsListChecked
               .Where(bag => bag.CheckedDate.Date >= dateOfStart.Date && bag.CheckedDate.Date <= dateOfEnd.Date).ToList();

                // Gruparea și sortarea după coloana "Name"
                var groupedAndSortedData = betweenDatesBags
                    .GroupBy(bag => bag.Name)
                    .SelectMany(group => group.OrderBy(bag => bag.Name))
                    .ToList();


                foreach (var checkedBag in groupedAndSortedData)
                {
                    dt.Rows.Add($"{checkedBag.Name} {checkedBag.Surname} ({checkedBag.PunchCode})", checkedBag.QuantityIssued, checkedBag.CheckedDate, checkedBag.Price, checkedBag.BagNumber, checkedBag.MONumber, checkedBag.PartNumber);
                }

            }

            return dt;

        }


        [HttpGet("ChargeTss")]
        public async Task<DataTable> GenerateTSSDataTable(string startDate, string finalDate)
        {
            var dt = new DataTable();

            dt.TableName = "TSSCharges";

            dt.Columns.Add("Counter", typeof(int));
            dt.Columns.Add("Bag_Number", typeof(string));
            dt.Columns.Add("Supplier", typeof(string));
            dt.Columns.Add("Address1", typeof(string));
            dt.Columns.Add("Address2", typeof(string));
            dt.Columns.Add("Address3", typeof(string));
            dt.Columns.Add("Address4", typeof(string));
            dt.Columns.Add("Town", typeof(string));
            dt.Columns.Add("Tel_no", typeof(string));
            dt.Columns.Add("Vat_no", typeof(string));
            dt.Columns.Add("Invoice_no", typeof(string));
            dt.Columns.Add("Invoice_date", typeof(string));
            dt.Columns.Add("Part_Number", typeof(string));
            dt.Columns.Add("Mo_Number", typeof(string));
            dt.Columns.Add("Chit_Number", typeof(string));
            dt.Columns.Add("Qty_In", typeof(int));
            dt.Columns.Add("Date_in", typeof(string));
            dt.Columns.Add("Price", typeof(float));
            dt.Columns.Add("Value", typeof(float));


            var _bagsListChecked = await _checkedBagsLogsRepo.GetAllAsync();

            if (DateTime.TryParseExact(startDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var dateOfStart) &&
           DateTime.TryParseExact(finalDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out var dateOfEnd))
            {
                var betweenDatesBags = _bagsListChecked
               .Where(bag => bag.CheckedDate.Date >= dateOfStart.Date && bag.CheckedDate.Date <= dateOfEnd.Date).ToList();

             
                foreach (var checkedBag in betweenDatesBags)
                {
                    dt.Rows.Add(checkedBag.Id, checkedBag.BagNumber, checkedBag.Supplier, 
                                "INSPECTRA LIMITED","MRA 049 B","INDUSTRIAL ESTATE", " ","MARSA","21377934","M1743-4222","INVOICE", 
                                checkedBag.RequestedDate, checkedBag.PartNumber,checkedBag.MONumber,checkedBag.ChitNumber,
                                checkedBag.QuantityIssued, checkedBag.DateIssued,checkedBag.ChargePrice/checkedBag.QuantityIssued ,checkedBag.ChargePrice);
                }

            }

            return dt;

        }



    }

    public class QualifiersUpdate
    {

        public string? ResponsiblePerson { get; set; } // NAME : B
        public string? DateOfIssue { get; set; }    //DATE : C
        public string? PartNumber { get; set; } //PART NO   : D
        public string? Type { get; set; }   //TYPE + COMMENTS : E+F
        public string? TssCode { get; set; }    // ID + OD + FACE : H + I + J
        public float? ChargePerPiece { get; set; } //TSS RATE : K
        public float? PayPerPiece { get; set; } //INH RATE : L 
        public float? ChargePerPieceDelay { get; set; } // TSS RATE BX2 : M
        public float? PayPerPieceDelay { get; set; } // INH RATE BX2 : N
        public float? ChargePerPieceExtraDelay { get; set; } // TSS RATE BX3 : ?
        public float? PayPerPieceExtraDelay { get; set; } //INH RATE BX3 : ?
        public float? EmployeePiecesPerHour { get; set; } //INH 2023/1000PCS : O 
        public float? ClientPiecesPerHour { get; set; } //TSS 2023/1000PCS : P

    }
}
