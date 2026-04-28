using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartTaskTracker.API.Data;
using SmartTaskTracker.API.Models;
using SmartTaskTracker.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace SmartTaskTracker.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        { 
            try
            {     
                var projects = await _context.Projects
                    .Include(p => p.CreatedByUser)
                    .Where(p => p.DeletedAt == null && p.Archived == false)
                    .Select(p => new 
                    {
                        projectID = p.ProjectID,
                        projectName = p.ProjectName,
                        description = p.Description,
                        startDate = p.StartDate,
                        endDate = p.EndDate,
                        createdAt = p.CreatedAt,
                        createdBy = p.CreatedByUser != null ? new { p.CreatedByUser.UserID, p.CreatedByUser.UserName } : null,
                        modifiedAt = p.ModifiedAt,
                        archived = p.Archived,
                    })
                    .ToListAsync();
                
                return Ok(projects);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProjects: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while retrieving projects.", details = ex.Message });
            }
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProject(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(p => p.CreatedByUser)
                    .Where(p => p.ProjectID == id && p.DeletedAt == null && p.Archived == false)
                    .Select(p => new 
                    {
                        projectID = p.ProjectID,
                        projectName = p.ProjectName,
                        description = p.Description,
                        startDate = p.StartDate,
                        endDate = p.EndDate,
                        createdAt = p.CreatedAt,
                        createdBy = p.CreatedByUser != null ? new { p.CreatedByUser.UserID, p.CreatedByUser.UserName } : null,
                        modifiedAt = p.ModifiedAt,
                        archived = p.Archived,
                    })
                    .FirstOrDefaultAsync();
                
                if (project == null)
                { 
                    return NotFound(); 
                }
                
                return Ok(project);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetProject: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while retrieving the project.", details = ex.Message });
            }
        } 

        // Only Managers can create projects
        [Authorize(Policy = "ManagerOnly")]
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (request == null) 
            { 
                return BadRequest("Request body is null."); 
            }

            // Validate project name
            if(string.IsNullOrWhiteSpace(request.ProjectName))
            {
                return BadRequest("Project name is required.");
            }

            // Check if project name already exists
            if (await _context.Projects.AnyAsync(p => p.ProjectName == request.ProjectName))
            {
                return Conflict("A project with the same name already exists.");
            }

            // ✅ Get current user ID from JWT token
            var currentUserClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (currentUserClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int currentUserId = int.Parse(currentUserClaim.Value);

            // Create new project
            var newProject = new Project
            {
                ProjectName = request.ProjectName,
                Description = request.Description ?? string.Empty,
                StartDate = DateOnly.FromDateTime(request.StartDate), // ✅ DateTime → DateOnly
                EndDate = request.EndDate.HasValue 
                    ? DateOnly.FromDateTime(request.EndDate.Value)    // ✅ DateTime? → DateOnly?
                    : null,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow,
                ModifiedBy = null,
                ModifiedAt = DateTime.UtcNow,
                Archived = false
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();
            
           return Ok(new {
            message = "Project created successfully",
            projectID = newProject.ProjectID
            
            });
        }

        // Only managers can update projects
        [Authorize(Policy = "ManagerOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
        {
            try
            {
                Console.WriteLine($"📥 UpdateProject called for ID: {id}");
                
                if (request == null)
                {
                    return BadRequest("Request body is required");
                }

                var project = await _context.Projects.FindAsync(id);
                if (project == null) 
                {
                    return NotFound();
                }

                if (!string.IsNullOrWhiteSpace(request.ProjectName) &&
                    await _context.Projects.AnyAsync(p => p.ProjectName == request.ProjectName && p.ProjectID != id))
                {
                    return Conflict("A project with the same name already exists.");
                }

                var currentUserClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                int? currentUserId = null;
                if (currentUserClaim != null)
                {
                    currentUserId = int.Parse(currentUserClaim.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.ProjectName))
                {
                    project.ProjectName = request.ProjectName;
                }

                if (request.Description != null)
                {
                    project.Description = request.Description;
                }

                if (request.StartDate.HasValue)
                {
                    project.StartDate = DateOnly.FromDateTime(request.StartDate.Value);
                }

                if (request.EndDate.HasValue)
                {
                    project.EndDate = DateOnly.FromDateTime(request.EndDate.Value);
                }

                project.ModifiedAt = DateTime.UtcNow;
                project.ModifiedBy = currentUserId;

                await _context.SaveChangesAsync();
                
                Console.WriteLine($"✅ Project {id} updated successfully");

                // Return a response indicating success
                return Ok(new 
                {
                    message = "Project updated successfully",
                    project = new 
                    {
                        projectID = project.ProjectID,
                        projectName = project.ProjectName,
                        description = project.Description,
                        startDate = project.StartDate,
                        endDate = project.EndDate,
                        modifiedAt = project.ModifiedAt,
                        archived = project.Archived
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in UpdateProject: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id && e.DeletedAt == null);
        }

        // Only Admins can delete projects (soft delete)
        [Authorize(Policy = "AdminOrManager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            // Get current user ID from token
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            int? currentUserId = null;
            if (currentUserIdClaim != null)
            {
                currentUserId = int.Parse(currentUserIdClaim.Value);
            }

            // Soft Delete
            project.DeletedAt = DateTime.UtcNow;
            project.DeletedBy = currentUserId;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // Only Admins can archive/unarchive projects
        [Authorize(Policy = "AdminOrManager")]
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) 
            { 
                return NotFound(); 
            }

            // Get current user ID from JWT token
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null)
            {
                return Unauthorized(new { error = "Current user ID not found in token." });
            }

            int currentUserId = int.Parse(currentUserIdClaim.Value);

            // Toggle archive status
            project.Archived = !project.Archived;
            project.ArchivedAt = project.Archived ? DateTime.UtcNow : null;
            project.ArchivedBy = project.Archived ? currentUserId : null;
            project.ModifiedAt = DateTime.UtcNow;
            project.ModifiedBy = currentUserId;
            
            await _context.SaveChangesAsync();
            
            return Ok(new
            {
                message = project.Archived ? "Project archived successfully." : "Project unarchived successfully.",
                archived = project.Archived
            });
        }
    } 
}