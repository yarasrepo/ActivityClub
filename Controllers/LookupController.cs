using ActivityClubAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActivityClubAPIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly DatabaseServerContext _context;

        public LookupController(DatabaseServerContext context)
        {
            _context = context;
        }

        [HttpGet("GetLookups")]
        public async Task<ActionResult<IEnumerable<Lookup>>> GetLookups()
        {
            try
            {
                var lookups = await _context.Lookups.ToListAsync();

                return lookups;
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLookupById/{id}")]
        public async Task<ActionResult<Lookup>> GetLookupById(int id)
        {
            try
            {
                var lookup = await _context.Lookups.FindAsync(id);
                if (lookup != null)
                    return lookup;
                else
                    return NotFound("Lookup not found");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddLookup")]
        public async Task<ActionResult> AddLookup(string? code, string? name, string? lookupOrder)
        {
            try
            {
                var lookup = new Lookup
                {
                    Code = code,
                    Name = name,
                    LookupOrder = lookupOrder
                };

                _context.Lookups.Add(lookup);

                await _context.SaveChangesAsync();
                return Ok("Lookup created");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("UpdateLookup")]
        public async Task<ActionResult> UpdateLookup (int id, string? code, string? name, string? lookupOrder)
        {
            try
            {
                var lookup = await _context.Lookups.FindAsync(id);

                if (lookup == null)
                    return Ok("Lookup does not exist. No action taken.");

                if (code != null)
                    lookup.Code = code;
                if (name != null)
                    lookup.Name = name;
                if (lookupOrder != null)
                    lookup.LookupOrder = lookupOrder;

                await _context.SaveChangesAsync();
                return Ok("Lookup updated");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteLookup/{id}")]
        public async Task<ActionResult> DeleteLookup(int id)
        {
            try
            {
                var lookup = await _context.Lookups.FindAsync(id);

                if (lookup == null)
                {
                    return NotFound("Lookup not found");
                }

                _context.Lookups.Remove(lookup);
                await _context.SaveChangesAsync();

                return Ok("Lookup deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}