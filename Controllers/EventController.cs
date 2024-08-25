using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly DatabaseServerContext _context;

        public EventController(DatabaseServerContext context)
        {
            _context = context;
        }

        [HttpGet("GetEvents")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            try
            {
                var events = await _context.Events.ToListAsync();
                return events;
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEventById")]
        public async Task<ActionResult<Event>> GetEventById(int Id)
        {
            try
            {
                var eventt = await _context.Events.FindAsync(Id);
                if (eventt != null)
                    return eventt;
                else
                    return NotFound("Event Not Found");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddEvent")]
        public async Task<ActionResult> AddEvent([FromQuery] EventDTO eventDto)
        {
            try
            {
                DateOnly? dateFrom = null;
                DateOnly? dateTo = null;

                if (eventDto.DateFrom != null)
                {
                    if (!DateOnly.TryParse(eventDto.DateFrom, out var parsedDateFrom))
                    {
                        return BadRequest("Invalid date format. Format is YYYY-MM-DD");
                    }
                    dateFrom = parsedDateFrom;
                }

                if (eventDto.DateTo != null)
                {
                    if (!DateOnly.TryParse(eventDto.DateTo, out var parsedDateTo))
                    {
                        return BadRequest("Invalid date format. Format is YYYY-MM-DD");
                    }
                    dateTo = parsedDateTo;
                }

                var eventt = new Event
                {
                    Name = eventDto.Name,
                    Description = eventDto.Description,
                    Cost = eventDto.Cost,
                    Destination = eventDto.Destination,
                    Status = eventDto.Status,
                    CategoryId = eventDto.CategoryId,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };

                _context.Events.Add(eventt);
                await _context.SaveChangesAsync();

                return Ok("Event created");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateEvent")]
        public async Task<ActionResult> UpdateEvent(int Id, [FromQuery] EventDTO eventDto)
        {
            try
            {
                var eventt = await _context.Events.FindAsync(Id);

                if (eventt != null)
                {
                    if (eventDto.Name != null)
                        eventt.Name = eventDto.Name;
                    if (eventDto.Description != null)
                        eventt.Description = eventDto.Description;
                    if (eventDto.Cost != null)
                        eventt.Cost = eventDto.Cost;
                    if (eventDto.Destination != null)
                        eventt.Destination = eventDto.Destination;
                    if (eventDto.Status != null)
                        eventt.Status = eventDto.Status;
                    if (eventDto.CategoryId != null)
                        eventt.CategoryId = eventDto.CategoryId;

                    if (eventDto.DateFrom != null)
                    {
                        if (DateOnly.TryParse(eventDto.DateFrom, out var dateFrom))
                        {
                            eventt.DateFrom = dateFrom;
                        }
                        else
                        {
                            return BadRequest("Invalid DateFrom format. Format should be YYYY-MM-DD");
                        }
                    }

                    if (eventDto.DateTo != null)
                    {
                        if (DateOnly.TryParse(eventDto.DateTo, out var dateTo))
                        {
                            eventt.DateTo = dateTo;
                        }
                        else
                        {
                            return BadRequest("Invalid DateTo format. Format should be YYYY-MM-DD");
                        }
                    }

                    _context.Events.Update(eventt);
                    await _context.SaveChangesAsync();

                    return Ok("Event Updated");
                }
                else
                {
                    return NotFound("Event not found");
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeleteEvent")]
        public async Task<ActionResult> DeleteEvent (int Id)
        {
            try
            {
                var eventt = await _context.Events.FindAsync(Id);
                if (eventt != null)
                {
                    _context.Events.Remove(eventt);
                    await _context.SaveChangesAsync();

                    return Ok("Event deleted");
                }
                else
                {
                    return NotFound("Event not found");
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
        

    }
}