using System.Net;
using Booker.Data;
using Booker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booker.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace Booker.Controllers;

public class BookController : Controller
{
    private readonly AppDbContext _db;
    public BookController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Route("/books")]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var booksWithReviews = await _db.Books
                .Include(b => b.Reviews)
                .ThenInclude(c => c.User)
                .ToListAsync();

            var bookDtos = booksWithReviews.Select(b => new BookDto
            {
                BookId = b.BookId,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                ImageUrl = b.ImageUrl,
                Reviews = b.Reviews.Select(r => new ReviewDto{
                    ReviewId = r.ReviewId,
                    ReviewText = r.ReviewText,
                    ReviewRating = r.ReviewRating,
                    User = new UserDto
                    {
                        UserId =  r.UserId,
                        DisplayName = r.User.DisplayName, 
                        
                    }
                }).ToList()
            }).ToList();
            return Ok(bookDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error has occured while trying to retrieve the books: " + ex.Message);
        }
        
    }

    [HttpGet]
    [Route("/books/{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        try
        {
            var book = await _db.Books.Include(r => r.Reviews).SingleOrDefaultAsync(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            var bookFiltered = new BookDto
            {
                Author = book.Author,
                Title = book.Title,
                Genre = book.Genre,
                ImageUrl = book.ImageUrl,
                Reviews = book.Reviews.Select(r => new ReviewDto
                {
                    ReviewId = r.ReviewId,
                    ReviewRating = r.ReviewRating,
                    ReviewText = r.ReviewText
                }).ToList() ?? new List<ReviewDto>()
            };
        
            return Ok(bookFiltered);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error has occured while trying to get the book: " + ex.Message);
        }
        
    }

    [HttpPost]
    [Route("/books")]
    public async Task<IActionResult> PostBook([FromBody] BookDto bookDto)
    {
        try
        {
            var book = new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Genre = bookDto.Genre,
                ImageUrl = bookDto.ImageUrl
            };

            _db.Add(book);
            await _db.SaveChangesAsync();
        
            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, book);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occured while trying to add the book: " + ex.Message);
        }

      
    }

    [HttpPatch]
    [Route("/books/{id}")]
    public async Task<IActionResult> EditBook(int id, [FromBody] BookDto bookDto)
    {
        try
        {
            var book = await _db.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            if (bookDto.Title != null) book.Title = bookDto.Title;
            if (bookDto.Author != null) book.Author = bookDto.Author;
            if (bookDto.Genre != null) book.Genre = bookDto.Genre;
            if (bookDto.ImageUrl != null) book.ImageUrl = bookDto.ImageUrl;

            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occured while trying to update the book: " + ex.Message);
        }
        


    }
    
    [HttpDelete]
    [Route("/books/{id}")]
    public async Task<IActionResult>  DeleteBook(int id)
    {
        try
        {
            var book = await _db.Books.FindAsync(id);
        
            if (book == null)
            {
                return NotFound();
            }
        
            
            _db.Remove(book);
            await _db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error has occured while deleting the book " + ex.Message);
        }
       
    }
}