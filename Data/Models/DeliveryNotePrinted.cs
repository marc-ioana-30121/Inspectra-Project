using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class DeliveryNotePrinted : AuditableEntity
    {
        
        public List<int> DeliveryNoteIds { get; set; }
        public List<string>? BagNumbers { get; set; }
        
    }
}
