using PrettyNeatGenericAPI.Models.DbModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrettyNeatGenericAPI.Data.Models
{
    public class Employee : AuditableEntity
    { 

        public string Name { get; set; }

        public string Surname { get; set; }

        public string? PunchCode { get; set; }

        public List<string>? AssignedBag { get; set; }

       /* // Add foreign key property
        [ForeignKey("BagInventory")] // Specify the navigation property name
        public int BagInventoryId { get; set; }
        public BagInventory? BagInventory { get; set; }
       */

    }
}
