using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using SmartTaskTracker.API.Models.DTOs;

namespace SmartTaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class  TaskItemsController : ControllerBase 
    {
        private readonly AppDbContext _context;
        public TaskItemsController(AppDbContext context)
        {
        _context = context;
        }

        // Get all tasks, including related entities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTaskItems()
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.ArchivedByUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.DeletedByUser)
                .Where(t => t.DeletedAt == null)
                .ToListAsync();
            return Ok(tasks);
        }

        // Get tasks for a specific project, including related entities
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByProject(int projectId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.ArchivedByUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.DeletedByUser)
                .Where(t => t.ProjectID == projectId && t.DeletedAt == null)
                .ToListAsync();
            return Ok(tasks);
        }

        // Get a specific task by ID, including related entities
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItem(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.AssignedUser)
                .Include(t => t.Project)
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.ArchivedByUser)
                .Include(t => t.CreatedByUser)
                .Include(t => t.ModifiedByUser)
                .Include(t => t.DeletedByUser)
                .Where(t => t.DeletedAt == null)
                .FirstOrDefaultAsync(t => t.TaskID == id);
            if (task == null) { return NotFound(); }
            return Ok(task);    
        }


        // Get tasks for a specific project
        //Only Managers can create tasks
        [Authorize(Policy = "ManagerOnly")]
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask([FromBody] CreateTaskItemRequest request)
        
        {
            // Map the request to a TaskItem entity
            if (request == null) 
            { 
                return BadRequest("Request body is null.");
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("Title is required.");
            }

            // check if task with same title already exists in the same project
            var existingTask = await _context.Tasks
                .Where(t => t.ProjectID == request.ProjectID && t.Title == request.Title && t.DeletedAt == null && t.Archived == false)
                .FirstOrDefaultAsync();

            // If a task with the same title exists, return a conflict response
            if (existingTask != null)
            {
                return Conflict("A task with the same title already exists in this project.");
            }

            // Get the current user ID from the JWT token
            var currentUserClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (currentUserClaim == null)
            {
                return Unauthorized("User ID claim not found in token.");

            }
            // Parse the user ID claim to an integer
            int currentUserId = int.Parse(currentUserClaim.Value);
            
            // Create a new TaskItem entity
            var newTaskItem = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                ProjectID = request.ProjectID,
                AssignedTo = request.AssignedTo,
                StatusID = request.StatusID,
                PriorityID = request.PriorityID,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow,
                Archived = false,
                ModifiedAt = null,
                ModifiedBy = null,
            };

            // Add the new task to the database
            _context.Tasks.Add(newTaskItem);
            await _context.SaveChangesAsync();

            // Return a response with the created task's ID
            return Ok(new
            {
                message = "Task created successfully",
                taskID = newTaskItem.TaskID,
            });
        }

        // Managers and Developers can update tasks
        [Authorize(Policy = "ManagerOrDeveloper")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
        {
            if(id != taskItem.TaskID)
            {
                return BadRequest();
            }

            _context.Tasks.Update(taskItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Soft delete a task by setting the DeletedAt timestamp
        //Only Managers can delete tasks
        [Authorize(Policy = "ManagerOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null) { return NotFound(); }
            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();
            return NoContent();     
        }
    }
}