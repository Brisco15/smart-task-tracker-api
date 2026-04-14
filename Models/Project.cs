using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query;

namespace SmartTaskTracker.API.Models
{
    [Table("Projects")]
    public class  Project
    {
        [Column("projectID")]
        public int ProjectID { get; set; }

        [Required]
        [Column("projectName")]
        [StringLength(255, MinimumLength = 5)]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        [Column("description")]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("startDate")]
        public DateOnly StartDate { get; set; }

        
        [Column("endDate")]
        public DateOnly? EndDate { get; set; }

        [Column("createdBy")]
        public int CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

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

        [Column("archivedAt")]
        public DateTime? ArchivedAt { get; set; }

        [Column("archivedBy")]
        public int? ArchivedBy { get; set; }
        public User? ArchivedByUser { get; set; }

        [Column("archived")]
        public bool Archived { get; set; } = false;

        // Navigation property for related tasks
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        

    }
}