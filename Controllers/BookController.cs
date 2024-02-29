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
    public IActionResult GetBooks()
    {
        var booksWithReviews = _db.Books
            .Include(b => b.Reviews)
            .ThenInclude(c => c.User)
            .ToList();

        var bookDtos = booksWithReviews.Select(b => new BookDto
        {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            Genre = b.Genre,
            Reviews = b.Reviews.Select(r => new ReviewDto{
                ReviewId = r.ReviewId,
                ReviewText = r.ReviewText,
                ReviewRating = r.ReviewRating,
                User = new UserDto
                    {
                     UserId =  r.UserId,
                     DisplayName = r.User.DisplayName, 
                     Email = r.User.Email,
                     ImageUrl = r.User.ImageUrl
                    }
                }).ToList()
        }).ToList();
        return Ok(bookDtos);
    }

    [HttpGet]
    [Route("/books/{id}")]
    public IActionResult GetBook(int id)
    {
        var book = _db.Books.Include(r => r.Reviews).SingleOrDefault(b => b.BookId == id);
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

    [HttpPost]
    [Route("/books")]
    public IActionResult PostBook([FromBody] BookDto bookDto)
    {

        var book = new Book
        {
            Title = bookDto.Title,
            Author = bookDto.Author,
            Genre = bookDto.Genre,
            ImageUrl = bookDto.ImageUrl
        };

        _db.Add(book);
        _db.SaveChanges();
        
        return Ok(book);
    }

    [HttpPatch]
    [Route("/books/{id}")]
    public IActionResult EditBook(int id, [FromBody] BookDto bookDto)
    {
        var book = _db.Books.Find(id);

        if (book == null)
        {
            return NotFound();
        }
        if (bookDto.Title != null) book.Title = bookDto.Title;
        if (bookDto.Author != null) book.Author = bookDto.Author;
        if (bookDto.Genre != null) book.Genre = bookDto.Genre;
        if (bookDto.ImageUrl != null) book.ImageUrl = bookDto.ImageUrl;

        _db.SaveChanges();

        return Ok(book);


    }
    
    [HttpDelete]
    [Route("/books/{id}")]
    public IActionResult DeleteBook(int id)
    {

        var book = _db.Books.Find(id);
        
        if (book == null)
        {
            return NotFound();
        }
        else if (book != null)
        {
            
        _db.Remove(book);
        _db.SaveChanges();
        }
        return Ok();
    }
}