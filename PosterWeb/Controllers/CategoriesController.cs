using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PosterModels;
using PostersWebServiceLayer;
using PosterWeb.Data;
using PosterWeb.Models;
using PosterWebDBContext;

namespace PosterWeb.Controllers
{
    [Authorize]

    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMemoryCache _memoryCache;

        public CategoriesController(ICategoriesService categoriesService, IMemoryCache memoryCache)
        {
            _categoriesService = categoriesService;
            _memoryCache = memoryCache;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await GetCategoriesUsingCache();// new List<Category>();

            // Check if data is in cache, if so, get it from the cache
            if (!_memoryCache.TryGetValue(CacheConstants.CATEGORIES_KEY, out categories))
            {
                // If not in cache, get from service
                categories = await _categoriesService.GetAllAsync();

                // Store in cache
                _memoryCache.Set(CacheConstants.CATEGORIES_KEY, categories);
            }

            // Return the data
            return View(categories);
        }
        [Authorize(Roles = "Admin")]
        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await GetSingleCategoryById(id);
            //var category = await _categoriesService.GetAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "admin")]
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoriesService.AddOrUpdateAsync(category);
                InvalidateCache();
                //_memoryCache.Remove(CacheConstants.CATEGORIES_KEY); // Clear cache after adding new category
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        private void InvalidateCache()
        {
            _memoryCache.Remove(CacheConstants.CATEGORIES_KEY);
        }

        [Authorize(Roles = UserRolesService.ADMIN_ROLE_NAME)]
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _categoriesService.GetAsync(id.Value);
            var category = await GetSingleCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [Authorize(Roles = "Admin")]
        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriesService.AddOrUpdateAsync(category);
                    InvalidateCache();
                    //_memoryCache.Remove(CacheConstants.CATEGORIES_KEY); // Clear cache after updating category
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        [Authorize(Roles = UserRolesService.ADMIN_ROLE_NAME)]
        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? category = await GetSingleCategoryById(id);
            //var category = await _categoriesService.GetAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = UserRolesService.ADMIN_ROLE_NAME)]
        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoriesService.DeleteAsync(id);
                InvalidateCache();
                // _memoryCache.Remove(CacheConstants.CATEGORIES_KEY); // Clear cache after deleting category
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        #region "Helper Methods"
        private bool CategoryExists(int id)
        {
            var category = GetSingleCategoryById(id).Result;
            return category != null;
        }
        private async Task<List<Category>?> GetCategoriesUsingCache()
        {
            var categories = new List<Category>();

            //is the data in the cache? if so, get it from the cache
            if (!_memoryCache.TryGetValue(CacheConstants.CATEGORIES_KEY, out categories))
            {
                //if not, get it from the database
                //and set the local list
                categories = await _categoriesService.GetAllAsync();

                //store it in the cache
                _memoryCache.Set(CacheConstants.CATEGORIES_KEY, categories);
            }

            return categories;
        }

        private async Task<Category?> GetSingleCategoryById(int? id)
        {
            var categories = await GetCategoriesUsingCache();
            return categories?.SingleOrDefault(x => x.Id == id);
        }

        #endregion
    }
}
