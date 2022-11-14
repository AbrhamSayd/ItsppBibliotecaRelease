using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBiblioteca.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Editorial { get; set; }
        public int PublishedYear { get; set; }
        public int Stock { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
