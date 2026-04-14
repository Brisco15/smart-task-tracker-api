using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace SmartTaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TimeTrackingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TimeTrackingsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeTracking>>> GetTimeTrackings()
        {
            var timeTrackings = await _context.TimeTrackings
                .Include(tt => tt.Task)
                .Include(tt => tt.User)
                .ToListAsync();
            return Ok(timeTrackings);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeTracking>> GetTimeTracking(int id)
        {
            var timeTracking = await _context.TimeTrackings
                .Include(tt => tt.Task)
                .Include(tt => tt.User)
                .FirstOrDefaultAsync(tt => tt.TimeTrackingID == id);
            if (timeTracking == null) { return NotFound(); }
            return Ok(timeTracking);
        }
        [HttpPost]
        public async Task<ActionResult<TimeTracking>> CreateTimeTracking(TimeTracking timeTracking)
        {
            if (timeTracking == null) { return BadRequest(); }
            _context.TimeTrackings.Add(timeTracking);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTimeTracking), new { id = timeTracking.TimeTrackingID }, timeTracking);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeTracking(int id, TimeTracking timeTracking)
        {
            if (id != timeTracking.TimeTrackingID)
            {
                return BadRequest();
            }
            _context.TimeTrackings.Update(timeTracking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}