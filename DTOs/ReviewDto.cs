using System.Text.Json.Serialization;

namespace Booker.DTOs;

public class ReviewDto
{
    public int ReviewId { get; set; }

    public string ReviewText { get; set; }

    public int ReviewRating { get; set; }
    
    [JsonIgnore]
    public BookDto Book { get; set; }
    public UserDto User { get; set; }
}