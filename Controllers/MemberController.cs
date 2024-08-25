using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        private readonly UserController _userController;

        public MemberController(DatabaseServerContext context, UserController userController)
        {
            _context = context;
            _userController = userController;
        }

        [HttpGet("GetMembers")]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            try
            {
                var members = await _context.Members.ToListAsync();
                return Ok(members);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMemberById")]
        public async Task<ActionResult<Member>> GetMemberById (int UserId)
        {
            try
            {
                var member = await _context.Members.FindAsync(UserId);
                return Ok(member);
            }
            catch (System.Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddMember")]
        public async Task<ActionResult> AddMember([FromQuery] UserDTO userDto, [FromQuery] MemberDTO? memberDto)
        {
            try
            {
                var result = await _userController.AddUser(userDto, "Member");

                if (result.Result is OkObjectResult okResult)
                {
                    int userId = (int)okResult.Value; // Extract the User ID

                    if (memberDto != null)
                        await UpdateMember(userId, memberDto);

                    return Ok("Member created");
                }
                else
                {
                    return result.Result; // Handle errors from AddUser
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateMember")]
        public async Task<ActionResult> UpdateMember(int userId, [FromQuery] MemberDTO memberDto, [FromQuery] UserDTO? userDto = null)
        {
            try
            {
                if (userDto != null)
                {
                    // Assuming UpdateUser returns an ActionResult
                    var updateResult = await _userController.UpdateUser(userId, userDto);
                    //if (updateResult is not OkResult)
                    //{
                    //    return updateResult; // Return if the update failed
                    //}
                }

                var member = await _context.Members.FindAsync(userId);

                if (member != null)
                {
                    // Update only the provided fields in memberDto
                    if (!string.IsNullOrEmpty(memberDto.Profession))
                        member.Profession = memberDto.Profession;
                    if (!string.IsNullOrEmpty(memberDto.EmergencyNumber))
                        member.EmergencyNumber = memberDto.EmergencyNumber;
                    if (!string.IsNullOrEmpty(memberDto.MobileNumber))
                        member.MobileNumber = memberDto.MobileNumber;
                    if (!string.IsNullOrEmpty(memberDto.Photo))
                        member.Photo = memberDto.Photo;

                    await _context.SaveChangesAsync();
                    return Ok("Member Updated");
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

        [HttpDelete("DeleteMember/{id}")]
        public async Task<ActionResult> DeleteMember(int id)
        {
            try
            {
                var member = await _context.Members.FindAsync(id);

                if (member == null)
                {
                    return NotFound("Member not found");
                }

                _context.Members.Remove(member);
                await _context.SaveChangesAsync();

                return Ok("Member deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
