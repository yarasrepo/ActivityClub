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
        public async Task<ActionResult<IEnumerable<Event>>> EventsByGuide(int GuideId)
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
        public async Task<ActionResult<IEnumerable<Guide>>> GuidesByEvent(int eventId)
        {
            try
            {
                // Get the list of Guide IDs associated with the given event (eventId)
                var guideIds = await _context.EventGuides
                    .Where(eg => eg.EventId == eventId)
                    .Select(eg => eg.GuideId)
                    .ToListAsync();

                if (guideIds == null || !guideIds.Any())
                {
                    return NotFound("No guides found for the specified event.");
                }

                // Retrieve the guides corresponding to the list of guide IDs
                var guides = await _context.Guides
                    .Where(g => guideIds.Contains(g.UserId))
                    .ToListAsync();

                return Ok(guides);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddGuideToEvent")]
        public async Task<ActionResult> AddGuideToEvent(int eventId, int guideId)
        {
            try
            {
                // Check if the event and guide exist
                var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
                var guideExists = await _context.Guides.AnyAsync(g => g.UserId == guideId);

                if (!eventExists)
                {
                    return NotFound("Event not found.");
                }

                if (!guideExists)
                {
                    return NotFound("Guide not found.");
                }

                // Check if the association already exists
                var existingAssociation = await _context.EventGuides
                    .AnyAsync(eg => eg.EventId == eventId && eg.GuideId == guideId);

                if (existingAssociation)
                {
                    return BadRequest("The guide is already associated with this event.");
                }

                // Create the association
                var eventGuide = new EventGuide
                {
                    EventId = eventId,
                    GuideId = guideId
                };

                _context.EventGuides.Add(eventGuide);
                await _context.SaveChangesAsync();

                return Ok("Guide successfully added to the event.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoveGuideFromEvent")]
        public async Task<ActionResult> RemoveGuideFromEvent(int eventId, int guideId)
        {
            try
            {
                // Check if the event and guide exist
                var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
                var guideExists = await _context.Guides.AnyAsync(g => g.UserId == guideId);

                if (!eventExists)
                {
                    return NotFound("Event not found.");
                }

                if (!guideExists)
                {
                    return NotFound("Guide not found.");
                }

                // Find the association
                var eventGuide = await _context.EventGuides
                    .FirstOrDefaultAsync(eg => eg.EventId == eventId && eg.GuideId == guideId);

                if (eventGuide == null)
                {
                    return NotFound("The guide is not associated with this event.");
                }

                // Remove the association
                _context.EventGuides.Remove(eventGuide);
                await _context.SaveChangesAsync();

                return Ok("Guide successfully removed from the event.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}