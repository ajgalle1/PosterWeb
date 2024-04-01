using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PosterWeb.Models;
using PosterWebDBContext;
using System.Diagnostics;

namespace PosterWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly PosterWebDbContext _context;

        
        public HomeController(ILogger<HomeController> logger, PosterWebDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var posters = await _context.Posters
                                        .Include(x => x.Category)
                                        .OrderBy(x => x.Title)
                                        .Take(5)
                                        .ToListAsync();

            return View(posters);
        }
        
        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
