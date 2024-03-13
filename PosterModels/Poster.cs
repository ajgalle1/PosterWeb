using System.ComponentModel.DataAnnotations;

namespace PosterModels
{
    public class Poster
    {
        public int ID { get; set; }

        //default is NVARCHAR max, which  is too much for our database. Annotations will help keep sizes reasonable.
        [StringLength(250)]
        public string Title { get; set; }
        public string Artist { get; set; }

        [StringLength(2048)] //Typically  the maximum for browsers to accept this length (not always)
        public string ImgPath { get; set; }//use if something like Azure storage account

        public byte[]? BinaryVersionOfImage { get; set; } // For storing directly on my SQL Server
    }
}
