using Booker.Data;
using Booker.DTOs;
using Booker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Booker.Controllers;

public class ReviewController : Controller
{
    private readonly AppDbContext _db;
    public ReviewController(AppDbContext db)
    {
        _db = db;
    }

    
    [HttpPost]
    [Route("reviews")]
    public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null)
        {
            return BadRequest("Review data is missing.");
        }

        if (reviewDto.User == null || reviewDto.Book == null)
        {
            return BadRequest("User or Book data is missing.");
        }

        var review = new Review
        {
            ReviewText = reviewDto.ReviewText,
            ReviewRating = reviewDto.ReviewRating,
            UserId = reviewDto.User.UserId, 
            BookId = reviewDto.Book.BookId
        };

        try
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
        }
        catch (Exception ex)
        {
            // Log the exception here
            return StatusCode(500, "An error occurred while saving the review: " + ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReview(int id)
    {
        try
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error has occured while trying to retrieve the review: " + ex.Message);
        }
        
    }


}