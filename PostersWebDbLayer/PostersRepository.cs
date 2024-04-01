using Microsoft.EntityFrameworkCore;
using PosterModels;
using PosterWebDBContext;
using System.Reflection;

namespace PostersWebDbLayer
{
    public class PostersRepository : IPostersRepository
    {
        private readonly PosterWebDbContext _context;

        public PostersRepository(PosterWebDbContext context)
        {
            _context = context;
        }
        public async Task<List<Poster>> GetAllAsync()
        {
            return await _context.Posters
                            .Include(x => x.Category)
                            .ToListAsync();

        }
        public async Task<Poster?> GetAsync(int id)
        {
            if (id <= 0)
            {
                return null; //Caller will have to know what to do with Null
            }
        
          return await _context.Posters
                .Include(x => x.Category)
                .FirstOrDefaultAsync(m => m.ID == id); 
        }
        public async Task<int> AddOrUpdateAsync(Poster poster)
        {
            //upsert is when update or insert depending on needs;
            if(poster.ID == null)
            {
                throw new ArgumentNullException(nameof(poster));
            }
            if(poster.ID == 0)
            {
                //if 0, this is an Add. 
                return await Add(poster);
            }
            else
            {
                return await Update(poster);
            }


        }

        private async Task<int> Update(Poster poster)
        {
            var existing = await _context.Posters.FirstOrDefaultAsync(x=>x.ID==poster.ID);
            if (existing == null)
            {
                throw new ArgumentException($"Poster ID {poster.ID} Not found");
            }
            existing.Artist = poster.Artist;
            existing.Title = poster.Title;
            existing.CategoryId = poster.CategoryId;
            existing.ImgPath = poster.ImgPath;
            existing.BinaryVersionOfImage = poster.BinaryVersionOfImage;

            await _context.SaveChangesAsync();
            return existing.ID;

        }

        private async Task<int> Add(Poster poster)
        {
            _context.Posters.Add(poster);
            await _context.SaveChangesAsync();
            return poster.ID; 
        }

        public async Task<int> DeleteAsync(Poster poster)
        {
            return await DeleteAsync(poster.ID);
        }

        public async  Task<int> DeleteAsync(int id)
        {
            var existing = await _context.Posters
                                    .FirstOrDefaultAsync(m => m.ID == id);
            _context.Posters.Remove(existing);
            await _context.SaveChangesAsync();

            return existing.ID;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Posters.FirstOrDefaultAsync(m => m.ID == id)!=null;
       }


    }
}
