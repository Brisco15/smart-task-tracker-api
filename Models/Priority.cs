using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query;

namespace SmartTaskTracker.API.Models
{
    [Table("Priority")]
    public class  Priority
    {
        [Column("priorityID")]
        public int PriorityID { get; set; }

        [Required]
        [Column("priorityName")]
        [StringLength(255, MinimumLength = 3)]
        public string PriorityName { get; set; } = string.Empty;
        // Initialization prevents null errors
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}