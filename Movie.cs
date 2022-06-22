using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Shared
{
    public class Movie
    {
        public string MovieId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }

        //[NotMapped]
        public DateTime DateReleased { get; set; }
    }
}
