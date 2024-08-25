using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventMemberController : ControllerBase
    {
        private readonly DatabaseServerContext _context;
        private readonly UserController _userController;

        public EventMemberController(DatabaseServerContext context, UserController userController)
        {
            _context = context;
            _userController = userController;
        }

        [HttpPost("AddMemberToEvent")]
        public async Task<ActionResult> AddMemberToEvent(int eventId, int memberId)
        {
            try
            {
                var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
                var memberExists = await _context.Members.AnyAsync(m => m.UserId == memberId);

                if (!eventExists)
                {
                    return NotFound("Event not found.");
                }

                if (!memberExists)
                {
                    return NotFound("Member not found.");
                }

                var existingAssociation = await _context.EventMembers
                    .AnyAsync(em => em.EventId == eventId && em.MemberId == memberId);

                if (existingAssociation)
                {
                    return BadRequest("Member is already associated with this event.");
                }

                var eventMember = new EventMember
                {
                    EventId = eventId,
                    MemberId = memberId
                };

                _context.EventMembers.Add(eventMember);

                await _context.SaveChangesAsync();

                return Ok("Member successfully added to the event.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoveMemberFromEvent")]
        public async Task<ActionResult> RemoveMemberFromEvent(int eventId, int memberId)
        {
            try
            {
                var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
                var memberExists = await _context.Members.AnyAsync(m => m.UserId == memberId);

                if (!eventExists)
                {
                    return NotFound("Event not found.");
                }

                if (!memberExists)
                {
                    return NotFound("Member not found.");
                }

                var eventMember = await _context.EventMembers
                    .FirstOrDefaultAsync(em => em.EventId == eventId && em.MemberId == memberId);

                if (eventMember == null)
                {
                    return NotFound("The member is not associated with this event.");
                }

                _context.EventMembers.Remove(eventMember);

                await _context.SaveChangesAsync();

                return Ok("Member successfully removed from the event.");
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("2627") == true)
            {
                return BadRequest("Unique constraint violation occurred.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EventsByMember")]
        public async Task<ActionResult<IEnumerable<Event>>> EventsByMember(int memberId)
        {
            try
            {
                var memberExists = await _context.Members.AnyAsync(m => m.UserId == memberId);

                if (!memberExists)
                {
                    return NotFound("Member not found.");
                }

                var events = await _context.EventMembers
                    .Where(em => em.MemberId == memberId)
                    .Select(em => em.Event)
                    .ToListAsync();

                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("MembersByEvent")]
        public async Task<ActionResult<IEnumerable<Member>>> MembersByEvent(int eventId)
        {
            try
            {
                var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);

                if (!eventExists)
                {
                    return NotFound("Event not found.");
                }

                var members = await _context.EventMembers
                    .Where(em => em.EventId == eventId)
                    .Select(em => em.Member)
                    .ToListAsync();

                return Ok(members);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}