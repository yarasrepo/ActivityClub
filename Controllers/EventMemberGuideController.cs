using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventGuideController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        private readonly UserController _userController;

        public EventGuideController(DatabaseServerContext context, UserController userController)
        {
            _context = context;
            _userController = userController;
        }

        [HttpGet("EventsByGuide")]
        public async Task<ActionResult<IEnumerable<Event>>> EventsByGuide (int GuideId)
        {
            try
            {
                // Get the list of Event IDs associated with the given guide (UserId)
                var eventIds = await _context.EventGuides
                    .Where(eg => eg.GuideId == GuideId)
                    .Select(eg => eg.EventId)
                    .ToListAsync();

                if (eventIds == null || !eventIds.Any())
                {
                    return NotFound("No events found for the specified guide.");
                }

                // Retrieve the events corresponding to the list of event IDs
                var events = await _context.Events
                    .Where(e => eventIds.Contains(e.Id))
                    .ToListAsync();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GuidesByEvent")]
    }
}