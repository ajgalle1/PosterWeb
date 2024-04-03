using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PosterWeb.Data;
using PosterWeb.Models;
using PosterWebDBContext;
using System.Diagnostics;

namespace PosterWeb.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRolesService _userRolesService;

       // private readonly PosterWebDbContext _context;


        public HomeController(ILogger<HomeController> logger, IUserRolesService userRolesService)
        {
            _logger = logger;
            _userRolesService = userRolesService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*  public async Task<IActionResult> IndexAsync()
      {
          var posters = await _context.Posters
                                      .Include(x => x.Category)
                                      .OrderBy(x => x.Title)
                                      .Take(5)
                                      .ToListAsync();

          return View(posters);
      }

      */


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> EnsureAdminUserIsCreated()
        {
            await _userRolesService.EnsureUsersAndRoles();
            return RedirectToAction("Index");
        }

    }
}
