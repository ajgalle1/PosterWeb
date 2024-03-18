using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PosterWebDBContext 
{
    public class PosterWebDbContext : DbContext
    {
        private static IConfigurationRoot _configuration;
        public PosterWebDbContext()
        {
            
        }

        public PosterWebDbContext(DbContextOptions<PosterWebDbContext> options)
            : base(options)
        {

        }

        //additional methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configuration = builder.Build();
                var cnstr = _configuration.GetConnectionString("PostersDataDbConnectionString"); 
                optionsBuilder.UseSqlServer(cnstr);
            }
        }

    }
}
