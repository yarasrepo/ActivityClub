using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        private readonly UserController _userController;

        public AdminController(DatabaseServerContext context, UserController userController)
        {
            _context = context;
            _userController = userController;
        }

        [HttpGet("GetAdmins")]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins ()
        {
            try
            {
                var admins = await _context.Admins.ToListAsync();
                return admins;
            }
            catch (System.Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAdminById")]
        public async Task<ActionResult<Admin>> GetAdminById(int UserId)
        {
            try
            {
                var admin = await _context.Admins.FindAsync(UserId);
                return admin;
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddAdmin")]
        public async Task<ActionResult> AddAdmin([FromQuery]UserDTO userDto)
        {
            try
            {
                var result = await _userController.AddUser(userDto, "Admin");

                if (result.Result is OkObjectResult okResult)
                {
                    int userId = (int)okResult.Value; // Extract the User ID

                    return Ok("Admin created");
                }
                else
                {
                    return result.Result; // Handle errors from AddUser
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("UpdateAdmin")]
        public async Task<ActionResult> UpdateAdmin(int id, [FromQuery] UserDTO userDto)
        {
            try
            {
                await _userController.UpdateUser(id, userDto);
                return Ok("Admin Updated");
            }
            catch (System.Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAdmin/{id}")]
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            try
            {
                var admin = await _context.Admins.FindAsync(id);

                if (admin == null) { 
                
                    return NotFound("Admin not found");
                }

                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();

                return Ok("Admin deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
