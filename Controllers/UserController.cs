using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prac.Data;
using prac.Models;
using prac.DTOs;

namespace prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Select(user => new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                })
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Role = userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.UserId = user.UserId;

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.UserId) return BadRequest();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Role = userDto.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
