using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Entities;

namespace WebApi.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext()
        {
        }

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database

            options.UseSqlServer("");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NewAccessUser> NewAccessUsers { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Moment> Moments { get; set; }
        public DbSet<Register> Registers { get; set; }
        public DbSet<Report> Reports { get; set; }

    }
}
