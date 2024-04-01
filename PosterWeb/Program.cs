using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PosterWeb.Data;
using PosterWebDBContext;

namespace PosterWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var PostersDbConnectionString = builder.Configuration.GetConnectionString("PostersDataDbConnectionString") ?? throw new InvalidOperationException("Connection string 'PostersDataDbConnectionString' not found.");

            // Configure and add PosterWebDbContext
            builder.Services.AddDbContext<PosterWebDbContext>(options =>
                options.UseSqlServer(PostersDbConnectionString));

            // Configure DbContext for Identity
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            //BEGIN Code for options when pivoting to Azure SQL database

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options;
            using (var context = new ApplicationDbContext(contextOptions))
            {
                context.Database.Migrate();
            }

            var contextOptions2 = new DbContextOptionsBuilder<PosterWebDbContext>().UseSqlServer(PostersDbConnectionString).Options;
            using (var context = new PosterWebDbContext(contextOptions2))
            {
                context.Database.Migrate();
            }

            //END Code for options when pivoting to Azure SQL database
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
