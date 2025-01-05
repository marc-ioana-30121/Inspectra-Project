using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class BagInventory : AuditableEntity
    {
            
        public string BagNumber { get; set; }

        public string MONumber { get; set; }

        public string ChitNumber { get; set; }

        public string PartNumber { get; set; }

        public string Supplier { get; set; }

        public string InspectionType { get; set; }

        public string Quality { get; set; }

        public string? InspectionCode { get; set; }

        public int QuantityIssued { get; set; }
   
        public string DateIssued { get; set; }
        
        public string RequestedDate { get; set; }

        public bool? Assigned { get; set; } = false;

        public int? AssignedTo { get; set; }
        public int? LastAssignedTo { get; set; }

        public DateTime? AssignedDate { get; set; }
        public DateTime? CheckedDate { get;set; }
        public int? QuantityPassed { get; set; }

        public int? QuantityFailed { get; set; }

        public string? Notes { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public bool? IsFirstTime { get; set; } 

        public bool? IsChecked { get; set; }

        public bool? IsReturned { get; set; }

        public bool? IsDelivered { get; set; }
        public bool? TssFault { get; set; } = null;

        // Add foreign key property
        [ForeignKey("DeliveryNote")] // Specify the navigation property name
        public int DeliveryNoteId { get; set; }
        public DeliveryNote? DeliveryNote { get; set; }
    }
}
