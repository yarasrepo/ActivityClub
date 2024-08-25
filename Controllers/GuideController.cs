using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuideController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        private readonly UserController _userController;

        public GuideController(DatabaseServerContext context, UserController userController)
        {
            _context = context;
            _userController = userController;
        }

        [HttpGet("GetGuides")]
        public async Task<ActionResult<IEnumerable<Guide>>> GetGuides()
        {
            try
            {
                var guides = await _context.Guides.ToListAsync();
                return guides;
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetGuideById")]
        public async Task<ActionResult<Guide>> GetGuideById(int UserId)
        {
            try
            {
                var guide = await _context.Guides.FindAsync(UserId);
                return guide;
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddGuide")]
        public async Task<ActionResult> AddGuide([FromQuery] UserDTO userDto, [FromQuery] GuideDTO? guideDto)
        {
            try
            {
                var result = await _userController.AddUser(userDto, "Guide");

                if (result.Result is OkObjectResult okResult)
                {
                    int UserId = (int)okResult.Value; // Extract the User ID

                    if (guideDto != null){
                        var guide = await _context.Guides.FindAsync(UserId);

                        if (guide != null)
                        {
                            guide.Photo = guideDto.Photo;
                            guide.Profession = guideDto.Profession;

                            _context.Guides.Update(guide);
                            await _context.SaveChangesAsync();
                        }                        
                    }

                    return Ok("Guide created");
                }
                else
                {
                    return BadRequest("Guide not created"); // Handle errors from AddUser
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateGuide")]
        public async Task<ActionResult> UpdateGuide(int UserId, [FromQuery]UserDTO? userDto, [FromQuery] GuideDTO? guideDto)
        {
            try
            {
                if (userDto != null)
                    await _userController.UpdateUser(UserId, userDto);

                var guide = await _context.Guides.FindAsync(UserId);

                if (guide == null)
                {
                    return NotFound("Guide not found");
                }

                if (guideDto != null)
                {
                    if (guideDto.Photo != null)
                        guide.Photo = guideDto.Photo;
                    if (guideDto.Profession != null)
                        guide.Profession = guideDto.Profession;

                    await _context.SaveChangesAsync();
                    return Ok("Guide Updated");
                }
                return Ok("Nothing to update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpDelete("DeleteGuide/{id}")]
        public async Task<ActionResult> DeleteGuide(int id)
        {
            try
            {
                var guide = await _context.Guides.FindAsync(id);

                if (guide == null)
                {
                    return NotFound("Guide not found");
                }

                _context.Guides.Remove(guide);
                await _context.SaveChangesAsync();

                return Ok("Guide deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
