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
    public async Task<IActionResult> GetUsers()
    {

        try
        {
            var users = await _db.Users.Include(r => r.Reviews).ToListAsync();

            var usersDto = users.Select(u => new UserDto
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                Email = u.Email,
                ImageUrl = u.ImageUrl,
                });

            return Ok(usersDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error has occured while trying to retrieve the users: "+ ex.Message);
        }
       
    }

    [HttpGet]
    [Route("users/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _db.Users.Include(r => r.Reviews).SingleOrDefaultAsync(u => u.UserId == id);

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
                

            };
            return Ok(filteredUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occured while trying to get the user: " + ex.Message);
        }
        
    }

    [HttpPost]
    [Route("users")]
    public async Task<IActionResult> PostUser([FromBody] User user)
    {
        var newUser = new User
        {
            UserId = user.UserId,
            DisplayName = user.DisplayName,
            Email = user.Email,
            Password = user.Password,
            ImageUrl = user.ImageUrl,

        };

        try
        {
            _db.Add(newUser);
            await _db.SaveChangesAsync();

            return Ok(newUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding an user: " + ex.Message);
        }

        

    }

    [HttpPatch]
    [Route("users/{id}")]
    public async Task<IActionResult> EditUser(int id, [FromBody] UserDto userDto)
    {
        var user = await _db.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        if (userDto.Email != null)
        {
            user.Email = userDto.Email;
        }

        if (userDto.DisplayName != null)
        {
            user.DisplayName = userDto.DisplayName;
        }

        if (userDto.ImageUrl != null)
        {
            user.ImageUrl = userDto.ImageUrl;
        }

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, "An error occurred while updating the user: " + ex.Message);
        }

        
        return NoContent();
    }


    [HttpDelete]
    [Route("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            _db.Remove(user);
            await _db.SaveChangesAsync();

            return Ok("The user has been successfully deleted");
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occurer while deleting the user.");
        }

       
    }
}