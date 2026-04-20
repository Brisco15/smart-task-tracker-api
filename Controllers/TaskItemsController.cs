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
    public class  TaskItemsController : ControllerBase 
    {
        private readonly AppDbContext _context;
        public TaskItemsController(AppDbContext context)
        {
        _context = context;
        }

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

        //Only Managers can create tasks
        [Authorize(Policy = "ManagerOnly")]
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem taskItem)
        {
            if (taskItem == null) { return BadRequest(); }
            _context.Tasks.Add(taskItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaskItem), new { id = taskItem.TaskID }, taskItem);
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