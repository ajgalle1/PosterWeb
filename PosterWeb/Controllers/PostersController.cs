using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PosterModels;
using PosterWeb.Data;
using PosterWebDBContext;

namespace PosterWeb.Controllers
{
    [Authorize]
    public class PostersController : Controller
    {
        private readonly PosterWebDbContext _context;
        private readonly SelectList _categories;
        public PostersController(PosterWebDbContext context)
        {
            _context = context;
            _categories = new SelectList(_context.Categories, "Id", "Name");

        }

        // GET: Posters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posters
                                      .Include(x=>x.Category)
                                      .ToListAsync());
        }

        // GET: Top 10 Posters
        public async Task<IActionResult> Top10()
        {
            return View(await _context.Posters
                                    .Include(x=>x.Category)
                                    .OrderBy(x => x.Title)
                                    .Take(10)
                                    .ToListAsync());
        }

        // GET: Posters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //select * from Posters where ID = id
            var poster = await _context.Posters
                .Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (poster == null)
            {
                return NotFound();
            }



            return View(poster);
        }

        // GET: Posters/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = _categories;
            return View();
        }

        // POST: Posters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Artist, ImgPath, CategoryId")] Poster poster)
        {
            if (poster.CategoryId is null)
            {
                ViewData["CategoryId"] = _categories;
                return View(poster);
            }

            if (ModelState.IsValid)
            {
                _context.Posters.Add(poster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = _categories;
            return View(poster);
        }

        // GET: Posters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poster = await _context.Posters.FindAsync(id);
            if (poster == null)
            {
                return NotFound();
            }
         
            ViewData["Categories"] = _categories;
            return View(poster);
        }

        // POST: Posters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Artist,CategoryId")] Poster poster)
        {
         
            
            if (id != poster.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Will replace next 2 lines with a single call to a service, passing the poster.
                    _context.Posters.Update(poster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosterExists(poster.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = _categories;
            return View(poster);
        }

        // GET: Posters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poster = await _context.Posters
                       .Include(x => x.Category)
                       .FirstOrDefaultAsync(m => m.ID == id);
            if (poster == null)
            {
                return NotFound();
            }

            return View(poster);
        }

        // POST: Posters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poster = await _context.Posters.FindAsync(id);
            if (poster != null)
            {
                _context.Posters.Remove(poster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PosterExists(int id)
        {
            return _context.Posters.Any(e => e.ID == id);
        }
    }
}
