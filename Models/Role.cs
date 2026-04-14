using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTaskTracker.API.Models
{
    [Table("roles")]
    public class Role
    {
        [Column("roleID")]
        public int RoleID { get; set; }
        
        [Required]
        [Column("roleName")]
        [StringLength(255)]
        public string RoleName { get; set; } = string.Empty;
        
        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}