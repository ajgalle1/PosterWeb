using PosterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostersWebDbLayer
{
    internal interface ICategoriesRepository
    {
        //CRUD Operatiosn
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetAsync(int id);

        Task<int> AddOrUpdateAsync(Category category);
        Task<int> DeleteAsync(Category category);
        Task<int> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
