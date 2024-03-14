using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PosterModels;

namespace PosterWeb.Data
{
    
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Poster> Posters { get; set; }     
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    
public DbSet<PosterModels.Category> Category { get; set; } = default!;
    }
}
