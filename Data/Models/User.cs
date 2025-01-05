using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrettyNeatGenericAPI.Models.DbModels
{
    public class User : AuditableEntity
    {
        public string Username { get; set; }
        public string Role { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }
        [JsonIgnore]
        public byte[]? PasswordSalt { get; set; }

        [NotMapped]

        public string? NewPassword { get; set; }
    }
}
