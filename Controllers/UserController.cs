using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        public UserController(DatabaseServerContext context) { _context = context; }


        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return Ok(users);
            }
            catch (System.Exception ex)
            {
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<int>> AddUser(
            [FromQuery] UserDTO userDto,  // Use FromBody for complex types
            [FromQuery] string? userType) // Optional query parameter
        {
            try
            {
                var existingUser = await _context.Users
                    .Where(u => u.Email == userDto.Email)
                    .SingleOrDefaultAsync();

                if (existingUser != null)
                {
                    return BadRequest("User already exists.");
                }

                DateOnly? dob = null;
                if (userDto.DateOfBirth != null)
                {
                    if (!DateOnly.TryParse(userDto.DateOfBirth, out var parsedDOB))
                    {
                        return BadRequest("Invalid date format. Format is YYYY-MM-DD");
                    }
                    dob = parsedDOB;

                }
                var user = new User
                {
                    Gender = userDto.Gender,
                    FullName = userDto.FullName,
                    DateOfBirth = dob,
                    Email = userDto.Email,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(userType))
                {
                    switch (userType)
                    {
                        case "Admin":
                            var admin = new Admin
                            {
                                UserId = user.Id,
                                User = user
                            };
                            _context.Admins.Add(admin);
                            break;

                        case "Member":
                            var member = new Member
                            {
                                UserId = user.Id,
                                JoiningDate = DateOnly.FromDateTime(DateTime.Now)
                            };
                            _context.Members.Add(member);
                            break;

                        case "Guide":
                            var guide = new Guide
                            {
                                UserId = user.Id,
                                JoiningDate = DateOnly.FromDateTime(DateTime.Now)
                            };
                            _context.Guides.Add(guide);
                            break;

                        default:
                            return BadRequest("Invalid user type specified");
                    }

                    await _context.SaveChangesAsync(); // Save related entities
                }

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("user is not found");
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<ActionResult> UpdateUser(int id, [FromQuery] UserDTO userDto)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);

                DateOnly? dob = null;
                if (userDto.DateOfBirth != null )
                {
                    if (!DateOnly.TryParse(userDto.DateOfBirth, out var parsedDOB))
                    {
                        return BadRequest("Invalid date format. Format is YYYY-MM-DD");
                    }
                    dob = parsedDOB;

                }

                if (existingUser != null)
                {
                    if (userDto.Gender != null)
                        existingUser.Gender = userDto.Gender;
                    if (userDto.FullName != null)
                        existingUser.FullName = userDto.FullName;
                    if (userDto.Email != null)
                        existingUser.Email = userDto.Email;
                    if (userDto.DateOfBirth != null)
                        existingUser.DateOfBirth = dob;

                    await _context.SaveChangesAsync();

                    return Ok("User Updated");
                }
                else
                {
                    return NotFound("User not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok("User deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
