using System.ComponentModel.DataAnnotations;

namespace Booker.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    public string DisplayName { get; set; }
    
    public string Email { get; set; }

    public string Password { get; set; }

    public string ImageUrl { get; set; }
    
    public virtual ICollection<Review> Reviews { get; set; }


}