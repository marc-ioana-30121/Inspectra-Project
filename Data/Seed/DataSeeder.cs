using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PrettyNeatGenericAPI.Data.Seed
{
    public static class DataSeeder
    {

        public static void Nuke(PrettyNeatGenericAPIDbContext dbContext)
        {
            try
            {
                dbContext.Database.EnsureDeleted();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            dbContext.Database.Migrate();
        }

        public static void Seed(PrettyNeatGenericAPIDbContext dbContext)
        {

 

            // Seed Patients
            var patients = new List<Patient>
        {
            new Patient
            {
                Name = "Alice",
                Description = "Patient with respiratory issues",
                Email = "alice@example.com",
                PhoneNumber = "1234567890",
                Sex = "Female",
                Address = "123 Main St",
                Citizenship = "USA",
                IDCard = "A12345678",
                UnderlyingConditions = "Asthma",
                Notes = "Regular check-up required"
            },
            new Patient
            {
                Name = "Bob",
                Description = "Patient with diabetes",
                Email = "bob@example.com",
                PhoneNumber = "9876543210",
                Sex = "Male",
                Address = "456 Elm St",
                Citizenship = "USA",
                IDCard = "B98765432",
                UnderlyingConditions = "Type 2 diabetes",
                Notes = "Requires insulin treatment"
            }
        };
            dbContext.Set<Patient>().AddRange(patients);

        }
    }

}
