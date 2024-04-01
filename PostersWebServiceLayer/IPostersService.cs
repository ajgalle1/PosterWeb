using PosterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostersWebServiceLayer
{
    public interface IPostersService
    {
        Task<List<Poster>> GetAllAsync();
        Task<Poster?> GetAsync(int id);

        Task<int> AddOrUpdateAsync(Poster poster);
        Task<int> DeleteAsync(Poster poster);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

    }
}
