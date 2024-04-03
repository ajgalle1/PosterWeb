using System.ComponentModel.DataAnnotations;

namespace PosterModels
{
    public class Poster
    {
        public int ID { get; set; }

        //default is NVARCHAR max, which  is too much for our database. Annotations will help keep sizes reasonable.
        [StringLength(PosterConstants.POSTER_TITLE_LENGTH)]
        public string Title { get; set; }

        [StringLength(PosterConstants.POSTER_ARTIST_LENGTH)]
        public string Artist { get; set; }

        [StringLength(PosterConstants.POSTER_IMGPATH_LENGTH)] //Typically  the maximum for browsers to accept this length (not always)
        public string ImgPath { get; set; }//use if something like Azure storage account


        public byte[]? BinaryVersionOfImage { get; set; } // For storing directly on my SQL Server


        public virtual int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }

}
