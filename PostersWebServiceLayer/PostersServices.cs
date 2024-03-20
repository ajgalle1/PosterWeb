using PosterModels;
using PostersWebDbLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PostersWebServiceLayer
{
    public class PostersServices : IPostersService
    {
        private readonly IPostersRepository _postersRepository;

        public PostersServices(IPostersRepository postersRepository)
        {
            _postersRepository = postersRepository;
        }
        public Task<List<Poster>> GetAllAsync()
        {
            return _postersRepository.GetAllAsync();
        }

        public Task<Poster?> GetAsync(int id)
        {
            return _postersRepository.GetAsync(id);
        }
        public Task<int> AddOrUpdateAsync(Poster poster)
        {
            return _postersRepository.AddOrUpdateAsync(poster);
        }

        public Task<int> DeleteAsync(Poster poster)
        {
            return _postersRepository.DeleteAsync(poster);
        }

        public Task<int> DeleteAsync(int id)
        {
            return _postersRepository.DeleteAsync(id);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _postersRepository.ExistsAsync(id);
        }


    }
}
