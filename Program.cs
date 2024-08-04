using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            CreateRolesAndAdminUser(app).Wait();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); 

            app.Run();
        }

        private static async Task CreateRolesAndAdminUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

              // rollerim
              // normal kullanýcý, içerik üretici ve admin
                string[] roleNames = { "NormalUser", "ContentCreator", "Admin" };
                IdentityResult roleResult;

                foreach (var roleName in roleNames)
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                //örnek user 
                var user = await userManager.FindByEmailAsync("admin@example.com");
                if (user == null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com"
                    };
                    await userManager.CreateAsync(user, "AdminPassword123!");
                }

                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
