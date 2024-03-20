using Microsoft.EntityFrameworkCore;
using PosterModels;
using PosterWebDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostersWebDbLayer
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly PosterWebDbContext _context;

        public CategoriesRepository(PosterWebDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                                 .ToListAsync();
        }
        public async Task<Category?> GetAsync(int id)
        {
            if (id <= 0)
            {
                return null; //Caller will have to know what to do with Null
            }

            return await _context.Categories
                                  .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<int> AddOrUpdateAsync(Category category)
        {
            //upsert is when update or insert depending on needs;
            if (category.Id == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (category.Id == 0)
            {
                //if 0, this is an Add. 
                return await Add(category);
            }
            else
            {
                return await Update(category);
            }

        }

        private async Task<int> Update(Category category)
        {
            var existing = await _context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existing == null)
            {
                throw new ArgumentException($"Category ID {category.Id} Not found");
            }
            existing.Name = category.Name;

            await _context.SaveChangesAsync();
            return existing.Id;
        }

        private async Task<int> Add(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category.Id;
        }

        public async Task<int> DeleteAsync(Category category)
        {
            return await DeleteAsync(category.Id); ;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existing = await _context.Categories
                                            .FirstOrDefaultAsync(m => m.Id == id);
            _context.Categories.Remove(existing);
            await _context.SaveChangesAsync();

            return existing.Id;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(m => m.Id == id) != null;
        }


    }
}
