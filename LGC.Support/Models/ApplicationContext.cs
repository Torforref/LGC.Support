using LGC.Suport.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LGC.Support.Models
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions options)
       : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<UserData> Users { get; set; }

        public DbSet<ProductData> Products { get; set; }
        public DbSet<CustomerData> Customers { get; set; }
        public DbSet<JobData> Jobs { get; set; }
        public DbSet<ServiceData> Services { get; set; }
    }
}
