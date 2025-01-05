using PrettyNeatGenericAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace PrettyNeatGenericAPI.Models.DbModels
{
    public class PrettyNeatGenericAPIDbContext : DbContext
    {
        public PrettyNeatGenericAPIDbContext(DbContextOptions<PrettyNeatGenericAPIDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Employee> Employee{ get; set; }
        public DbSet<DeliveryNote> DeliveryNote { get; set; }
        public DbSet<BagInventory> BagInventory{ get; set; }
        public DbSet<ReturnLogs> ReturnLogs{ get; set; }
        public DbSet<ReturnedBags> ReturnBags{ get; set; }
        public DbSet<DeliveryNotePrinted> DeliveryNotePrinted{ get; set; }
        public DbSet<Qualifiers> Qualifiers{ get; set; }
        public DbSet<CheckedBagsLogs> CheckedBagsLogs { get; set; }


    }
}