using Booker.Models;

namespace Booker.DTOs;

public class BookDto
{
    public int BookId { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Genre { get; set; }
    
    public string ImageUrl { get; set; }

    public virtual List<ReviewDto> Reviews { get; set; }
}