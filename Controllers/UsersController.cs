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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                // ✅ DTO verwenden statt direkte User-Objekte
                var users = await _context.Users
                    .Include(u => u.Role)
                    .Where(u => u.DeletedAt == null && u.Archived == false)
                    .Select(u => new 
                    {
                        userID = u.UserID,
                        userName = u.UserName,
                        email = u.Email,
                        createdAt = u.CreatedAt,
                        archived = u.Archived,
                        role = u.Role != null ? new 
                        {
                            roleID = u.Role.RoleID,
                            roleName = u.Role.RoleName
                        } : null,
                    })
                    .ToListAsync();
                
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsers: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Role)
                    .Where(u => u.DeletedAt == null && u.UserID == id)
                    .Select(u => new 
                    {
                        userID = u.UserID,
                        userName = u.UserName,
                        email = u.Email,
                        createdAt = u.CreatedAt,
                        archived = u.Archived,
                        role = u.Role != null ? new 
                        {
                            roleID = u.Role.RoleID,
                            roleName = u.Role.RoleName
                        } : null
                    })
                    .FirstOrDefaultAsync();
                    
                if (user == null) 
                { 
                    return NotFound(); 
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUser: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        
        
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (user == null) { return BadRequest(); }
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        
        // Update: Only Admins can update user details
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (request == null) 
            { 
                return BadRequest("Request body is required"); 
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null) 
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

            // Update user properties
            user.UserName = request.UserName ?? user.UserName;
            user.Email = request.Email ?? user.Email;
            user.RoleID = request.RoleID > 0 ? request.RoleID : user.RoleID;
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = currentUserId;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id)) 
                { 
                    return NotFound(); 
                }
                else 
                { 
                    throw; 
                }
            }
            
            return Ok(new { message = "User updated successfully" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        // Soft Delete: Set DeletedAt instead of removing the record
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //Get current user ID from JWT token
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null)
            {
                return Unauthorized(new { error = "Current user ID not found in token." });
            }

            int currentUserId = int.Parse(currentUserIdClaim.Value);

            // Soft Delete
            // Set DeletedAt and DeletedBy instead of removing the record
            // This allows us to keep a record of deleted users and who deleted them
            
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = currentUserId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("{id}/archive")]
        public async Task<IActionResult> ArchiveUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //Get current user ID from JWT token
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (currentUserIdClaim == null) {
                return Unauthorized(new { error = "Current user ID not found in token." });
            }

            int currentUserId = int.Parse(currentUserIdClaim.Value);

            // Archive user
            user.Archived = !user.Archived;
            user.ArchivedAt = user.Archived ? DateTime.UtcNow : (DateTime?)null;
            user.ArchivedBy = user.Archived ? currentUserId : (int?)null;
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = currentUserId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = user.Archived ? "User archived successfully." : "User unarchived successfully.",
                archived = user.Archived,
            });
        }
    }

    // ⭐ DTO für Update Request
    public class UpdateUserRequest
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int RoleID { get; set; }
    }
}