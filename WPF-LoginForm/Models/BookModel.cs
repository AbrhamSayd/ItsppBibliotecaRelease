using System;

namespace WPFBiblioteca.Models; 

public class BookModel
{
    public int Id { get; set; }
    public string Isbn { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string Editorial { get; set; }
    public int? PublishedYear { get; set; }
    public int Stock { get; set; }
    public string Color { get; set; }
    public int CategoryId { get; set; } //category id inside DB, Use Inner Joint on CategoryId Id
    public string Location { get; set; }
    public string Remarks { get; set; }
    public string CategoryDescription { get; set; }
}