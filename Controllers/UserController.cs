using Booker.Data;
using Booker.DTOs;
using Booker.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booker.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }


    [HttpGet]
    [Route("users")]
    public IActionResult GetUsers()
    {
        var users = _db.Users.Include(r => r.Reviews).ToList();

        var usersDto = users.Select(u => new UserDto
        {
            UserId = u.UserId,
            DisplayName = u.DisplayName,
            Email = u.Email,
            ImageUrl = u.ImageUrl,
            Reviews = u.Reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ReviewRating = r.ReviewRating,
                ReviewText = r.ReviewText
            }).ToList()
        });

        return Ok(usersDto);
    }

    [HttpGet]
    [Route("users/{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _db.Users.Include(r => r.Reviews).SingleOrDefault(u => u.UserId == id);

        if (user == null)
        {
            return NotFound();
        }
        {
            
        }
        var filteredUser = new UserDto
        {
            UserId = user.UserId,
            DisplayName = user.DisplayName,
            Email = user.Email,
            ImageUrl = user.ImageUrl,
            Reviews = user.Reviews.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                ReviewText = r.ReviewText,
                ReviewRating = r.ReviewRating,
            }).ToList() ?? new List<ReviewDto>()

        };
        return Ok(filteredUser);
    }

    [HttpPost]
    [Route("users")]
    public IActionResult PostUser([FromBody] User user)
    {
        var newUser = new User
        {
            UserId = user.UserId,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Password = user.Password,
            ImageUrl = user.ImageUrl,

        };

        _db.Add(newUser);
        _db.SaveChanges();

        return Ok(newUser);

    }

    [HttpPatch]
    [Route("users/{id}")]
    public IActionResult EditUser(int id, [FromBody] UserDto userDto)
    {
        var user = _db.Users.Find(id);

        if (user == null)
        {
            return NotFound();
        }


        if (userDto.Email != null)
        {
        user.Email = userDto.Email; }

        if (userDto.DisplayName != null)
        {
        user.DisplayName = userDto.DisplayName; }

        if (userDto.ImageUrl != null)
        {
        user.ImageUrl = userDto.ImageUrl; }

        _db.SaveChanges();

        var userToReturn = new UserDto
        {
            UserId = user.UserId,
            Email = user.Email,
            DisplayName = user.DisplayName,
            ImageUrl = user.ImageUrl
        };

        return Ok(userToReturn);
    }

    [HttpDelete]
    [Route("users/{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _db.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }

        _db.Remove(user);
        _db.SaveChanges();

        return Ok("The user has been successfully deleted");
    }
}