using PrettyNeatGenericAPI.Models.DbModels;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class ReturnLogs : AuditableEntity
    {   
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string PunchCode { get; set; }
        public string BagNumber { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string? Fault { get; set; }

    }
}
