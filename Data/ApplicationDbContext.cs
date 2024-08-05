using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UdemyEgitimPlatformu.Models;

namespace UdemyEgitimPlatformu.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SmtpSettings> SmtpSettings { get; set; }

        public DbSet<Settings> Settings { get; set; }
        public DbSet<Yorumlar> Yorumlar { get; set; }
        public DbSet<Menuler> Menuler { get; set; }

        public DbSet<AlinanVideolar> AlinanVideolar { get; set; }
        public DbSet<Kategoriler> Kategoriler { get; set; }
        public DbSet<Videolar> Videolar { get; set; }
        public DbSet<ApplicationRequest> ApplicationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ekstra konfigürasyonlar buraya eklenebilir
            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable(name: "AspNetUsers");
            });
            builder.Entity<IdentityRole>(entity => {
                entity.ToTable(name: "AspNetRoles");
            });
            builder.Entity<IdentityUserRole<string>>(entity => {
                entity.ToTable("AspNetUserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity => {
                entity.ToTable("AspNetUserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity => {
                entity.ToTable("AspNetUserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity => {
                entity.ToTable("AspNetRoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity => {
                entity.ToTable("AspNetUserTokens");
            });

            builder.Entity<Menuler>()
          .HasOne(m => m.Category)
          .WithMany(c => c.Menuler)
          .HasForeignKey(m => m.CategoryId);
        }
    }
}
