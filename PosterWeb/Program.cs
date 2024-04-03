using Microsoft.AspNetCore.Identity; //
using Microsoft.EntityFrameworkCore; //
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PostersWebDbLayer;//
using PostersWebServiceLayer;//
using PosterWeb.Data; //
using PosterWebDBContext;
using PosterWebDBLibrary;

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
                 .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            // Add our own services:
            builder.Services.AddScoped<IPostersRepository, PostersRepository>();
            builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            builder.Services.AddScoped<IPostersService, PostersServices>();
            builder.Services.AddScoped<ICategoriesService, CategoriesService>();

            builder.Services.AddScoped<IUserRolesService, UserRolesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
