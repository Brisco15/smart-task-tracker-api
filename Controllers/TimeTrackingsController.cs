using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            try
            {
                Console.WriteLine($"🔍 GetTimeTrackingForProject called for projectId: {projectId}");

                // Fetch all time trackings with non-null duration first
                var allTimeTrackings = await _context.TimeTrackings
                    .Where(tt => tt.Duration != null)
                    .ToListAsync();
                
                Console.WriteLine($"📊 Total time trackings in DB: {allTimeTrackings.Count}");

                // Fetch all tasks for the specified project
                var tasks = await _context.Tasks
                    .Where(t => t.ProjectID == projectId)
                    .Select(t => new { t.TaskID, t.Title })
                    .ToListAsync();
                
                Console.WriteLine($"📋 Tasks for project {projectId}: {tasks.Count}");
                // Create a HashSet of TaskIDs for efficient lookup
                var taskIds = tasks.Select(t => t.TaskID).ToHashSet();

                // Filter time trackings to only those that belong to tasks in the specified project
                var projectTimeTrackings = allTimeTrackings
                    .Where(tt => taskIds.Contains(tt.TaskID))
                    .ToList();
                
                Console.WriteLine($"⏱️ Time trackings for this project: {projectTimeTrackings.Count}");

                // Group by TaskID and calculate total duration and count of entries
                var taskBreakdown = projectTimeTrackings
                    .GroupBy(tt => tt.TaskID)
                    .Select(g => new
                    {
                        taskID = g.Key,
                        taskName = tasks.FirstOrDefault(t => t.TaskID == g.Key)?.Title ?? "Unknown Task",
                        totalDuration = (double)g.Sum(tt => tt.Duration ?? 0),
                        entries = g.Count()
                    })
                    .ToList();
                
                Console.WriteLine($"✅ Returning {taskBreakdown.Count} task breakdown entries");
                
                return Ok(taskBreakdown);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetTimeTrackingForProject: {ex.Message}");
                Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Failed to retrieve time tracking data", details = ex.Message });
            }
        }

        // Endpoint to get total time tracked for a specific project
        [HttpGet("project/{projectId}/total")]
        public async Task<IActionResult> GetTotalTimeTrackingForProject(int projectId)
        {
            try
            {
                // Fetch all time trackings with non-null duration first
                var totalDuration = await _context.TimeTrackings
                    .Include(tt => tt.Task)
                    .Where(tt => tt.Task != null && tt.Task.ProjectID == projectId && tt.Duration != null)
                    .SumAsync(tt => tt.Duration ?? 0);
                
                return Ok((double)totalDuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in GetTotalTimeTrackingForProject: {ex.Message}");
                return StatusCode(500, new { error = "Failed to retrieve total time", details = ex.Message });
            }
        }

        // Endpoint to get total time tracked for a specific task
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTimeTrackingForTask(int taskId)
        {
            var timeTrackings = await _context.TimeTrackings
                .Include(tt => tt.Task)
                .Include(tt => tt.User)
                .Where(tt => tt.TaskID == taskId && tt.Duration != null)
                .SumAsync(tt => tt.Duration ?? 0);  
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
            
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskID == taskId);
            if(task == null)
            {
                return NotFound("Task not found.");
            }

            if(task.AssignedTo != userId) 
            { 
                return Forbid("User is not assigned to this task.");
            }
            

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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); 
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            int userId = int.Parse(userIdClaim.Value);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.TaskID == taskId);
            if (task == null) 
            { 
                return NotFound("Task not found."); 
            }

            if (task.AssignedTo != userId)
            {
                return Forbid("User is not assigned to this task.");
            }
            
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
            entry.Duration = (int?)duration;

            _context.TimeTrackings.Update(entry);
            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        
    }
}