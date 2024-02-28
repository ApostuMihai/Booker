using Booker.Data;
using Booker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var booksWithReviews = _db.Books.Include(b => b.Reviews).ThenInclude(c => c.User).ToList();
        return Ok(booksWithReviews);
    }
        
}