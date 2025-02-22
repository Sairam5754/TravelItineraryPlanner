using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelItineraryPlanner.Controllers.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TravelItineraryPlanner.Controllers
{
    [Route("api/users")]
   
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;

        public UserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDTO request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email))
                return BadRequest("Username or Email already exists.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(new { message = "Login successful", UserId = user.Id, Role = user.Role });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserRegistrationDTO request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            user.Username = request.Username ?? user.Username;
            user.Email = request.Email ?? user.Email;
            user.Role = request.Role ?? user.Role;

           
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("User updated successfully.");
        }


       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(new { user.Id, user.Username, user.Email, user.Role });
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully.");
        }

        
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
