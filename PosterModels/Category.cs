using System.ComponentModel.DataAnnotations;

namespace PosterModels
{
    public class Category
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual List<Poster> Posters { get; set; } = new List<Poster>();
    }

}
