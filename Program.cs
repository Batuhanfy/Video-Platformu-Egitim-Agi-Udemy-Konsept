using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.Services;

namespace BidemyLearning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Database Not Connection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity servislerini ekleyin
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            builder.Services.AddHostedService<BackgroundWorker>();


            builder.Services.AddScoped<SmtpSettings>(provider =>
            {
                var dbContext = provider.GetService<ApplicationDbContext>();
                return dbContext?.SmtpSettings.FirstOrDefault() ?? new SmtpSettings(); // Default deðer ile boþ kontrolü
            });
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Kimlik doðrulama middleware'ini ekleyin
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); // Razor Pages'ý ekleyin

            app.Run();
        }
    }
}
