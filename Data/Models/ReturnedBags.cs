using PrettyNeatGenericAPI.Models.DbModels;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class ReturnedBags : AuditableEntity
    {
        public string EmployeeName { get; set; }
        public string BagNumber { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int CountReturned { get; set; }

    }
}
