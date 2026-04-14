using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query;

namespace SmartTaskTracker.API.Models
{
    [Table("Status")]
    public class  Status
    {
        [Column("statusID")]
        public int StatusID { get; set; }

        [Required]
        [Column("statusName")]
        [StringLength(255, MinimumLength = 3)]
        public string StatusName { get; set; } = string.Empty;
        // Initialization prevents null errors
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}