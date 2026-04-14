using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTaskTracker.API.Models
{
    [Table("users")]
    public class User 
    {
        [Column("userID")]
        public int UserID { get; set; }

        [Required]
        [Column("userName")]
        [StringLength(255, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Column("email")]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("passwordHash")]
        [StringLength(500)] // ? Erhöht für BCrypt-Hash (kein MinimumLength!)
        public string PasswordHash { get; set; } = string.Empty;

        // Foreign Key to Role
        [Column("roleID")]
        public int RoleID { get; set; }
        public Role? Role { get; set; }

        // Audit fields
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }
        
        [Column("modifiedBy")]
        public int? ModifiedBy { get; set; }
        public User? ModifiedByUser { get; set; }

        [Column("deletedAt")]
        public DateTime? DeletedAt { get; set; }
        
        [Column("deletedBy")]
        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }

        [Column("archived")]
        public bool Archived { get; set; } = false;
        
        [Column("archivedBy")]
        public int? ArchivedBy { get; set; }
        public User? ArchivedByUser { get; set; }
        
        [Column("archivedAt")]
        public DateTime? ArchivedAt { get; set; }

        // Navigation properties
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}