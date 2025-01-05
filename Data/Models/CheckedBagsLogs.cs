using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class CheckedBagsLogs : AuditableEntity
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PunchCode { get; set; }

        public int QuantityIssued { get; set; }
        public DateTime CheckedDate { get; set; }
        public float? Price { get; set; }
        public float? ChargePrice { get; set; }
        public string BagNumber { get; set; }
        public string MONumber { get; set; }
        public string PartNumber { get; set; }
        public string ChitNumber { get; set; }
        public string Quality { get; set; }
        public string Supplier { get; set; }
        public string DateIssued { get; set; } 
        public string RequestedDate { get; set; } 

    }
}
