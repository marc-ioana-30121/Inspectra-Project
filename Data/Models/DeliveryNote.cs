using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class DeliveryNote : AuditableEntity
    {

        public string? BagNumber { get; set; }

        public string? MONumber { get; set; }

        public string? ChitNumber { get; set; }

        public string? PartNumber { get; set; }
        

        public string? Quality { get; set; }

        public int? QuantityIssued { get; set; }

        public int? QuantityPassed { get; set; }

        public int? QuantityFailed { get; set; }

        public string? Supplier { get; set; }

        public string? InspectionType { get; set; }

        public string? Notes { get; set; }

        public DateOnly? DateDispatched { get; set; }

        public bool? Printed { get; set; } = false;
    }
}
