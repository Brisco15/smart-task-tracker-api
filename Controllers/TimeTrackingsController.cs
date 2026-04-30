using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;  // ✅ Füge diesen Import hinzu!

namespace SmartTaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/timetrackings")]
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

        // Endpoint to get total time tracked for a specific project
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTimeTrackingForProject(int projectId)
        {
            var timeTrackings = await _context.TimeTrackings
                .Include(tt => tt.Task)
                .Include(tt => tt.User)
                .Where(tt => tt.Task!.ProjectID == projectId && tt.Duration != null)  // ✅ ! hinzugefügt
                .GroupBy(tt => tt.TaskID)
                .Select(g => new
                {
                    taskID = g.Key,
                    totalDuration = g.Sum(tt => tt.Duration)
                })
                .ToListAsync();
            return Ok(timeTrackings);
        }

        // Endpoint to get total time tracked for a specific task
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTimeTrackingForTask(int taskId)
        {
            var timeTrackings = await _context.TimeTrackings
                .Include(tt => tt.Task)
                .Include(tt => tt.User)
                .Where(tt => tt.TaskID == taskId && tt.Duration != null)
                .SumAsync(tt => tt.Duration ?? 0);  // ✅ ?? 0 hinzugefügt
            return Ok(timeTrackings);
        }

        // Endpoint to start time tracking for a task
        // Only developers can start time tracking
        [Authorize(Policy = "DeveloperOnly")]
        [HttpPost("start")]
        public async Task<IActionResult> StartTimeTracking([FromQuery] int taskId) 
        {
            // Get the user ID from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);

            // Check if there's an active time tracking for the user
            var activeTracking = await _context.TimeTrackings
                .FirstOrDefaultAsync(tt => tt.UserID == userId && tt.EndTime == null);

            // If there's an active tracking, return a bad request response
            if (activeTracking != null)
            {
                return BadRequest("User already has an active time tracking.");
            }

            // Create a new time tracking entry
            var entry = new TimeTracking
            {
                TaskID = taskId,
                UserID = userId,
                StartTime = DateTime.UtcNow
            };
            _context.TimeTrackings.Add(entry);
            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        // Endpoint to stop time tracking for a task
        // Only developers can stop time tracking
        [Authorize(Policy = "DeveloperOnly")]
        [HttpPost("stop")]
        public async Task<IActionResult> StopTimeTracking([FromQuery] int taskId)  
        {
            // Get the user ID from the JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);  // ✅ Variable umbenannt
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);

            // Find the active time tracking entry for the user and task
            var entry = await _context.TimeTrackings
                .FirstOrDefaultAsync(tt => tt.UserID == userId && tt.TaskID == taskId && tt.EndTime == null);

            // If no active entry is found, return a not found response
            if (entry == null)
            {
                return NotFound("No active time tracking found for the specified task and user.");
            }
            
            // Set the end time and calculate the duration
            entry.EndTime = DateTime.UtcNow;
            // Calculate duration in minutes
            var duration = (entry.EndTime.Value - entry.StartTime).TotalMinutes;
            entry.Duration = duration;

            _context.TimeTrackings.Update(entry);
            await _context.SaveChangesAsync();
            return Ok(entry);
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