using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booker.Models;

public class Review
{
    [Key] 
    public int ReviewId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [ForeignKey("Book")] 
    public int BookId { get; set; }
    public virtual Book Book { get; set; }

    public string ReviewText { get; set; }

    public int ReviewRating { get; set; }



}