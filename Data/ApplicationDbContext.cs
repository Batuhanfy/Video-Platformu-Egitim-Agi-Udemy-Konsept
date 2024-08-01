using Microsoft.EntityFrameworkCore;
using UdemyEgitimPlatformu.Models;


namespace UdemyEgitimPlatformu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<Settings> Settings { get; set; }

    }
}
