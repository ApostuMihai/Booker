using System.ComponentModel.DataAnnotations.Schema;

namespace Booker.Models;

public class Book
{
    public int BookId { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }
}