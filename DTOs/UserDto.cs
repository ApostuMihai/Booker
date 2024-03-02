using Booker.Models;

namespace Booker.DTOs;

public class UserDto
{
    public int UserId { get; set; }
    
    public string DisplayName { get; set; }
    
    public string Email { get; set; }

    public string ImageUrl { get; set; }
    

}