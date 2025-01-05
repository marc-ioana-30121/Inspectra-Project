using PrettyNeatGenericAPI.Models.DbModels;

namespace PrettyNeatGenericAPI.Data.Models
{

    public class Patient : AuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string IDCard { get; set; }
        public string UnderlyingConditions { get; set; }
        public string Notes { get; set; }

    }
}
