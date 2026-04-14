using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query;

namespace SmartTaskTracker.API.Models
{
    [Table("Tasks")]
    public class TaskItem
    {
        [Column("taskID")]
        public int TaskID { get; set; }
        
        [Required]
        [Column("title")]
        [StringLength(255, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [Column("description")]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        // Foreign Key to User who is assigned to the task, can be null because a task can be created without being assigned
        [Column("assignedTo")]
        public int? AssignedTo { get; set; }
        public User? AssignedUser { get; set; }

        //Foreign Key to Project
        [Column("projectID")]
        public int ProjectID { get; set; }
        public Project? Project { get; set; }

        //Foreign key to status
        [Column("statusID")]
        public int StatusID { get; set; }
        public Status? Status { get; set; }

        //Foreign key to priority
        [Column("priorityID")]
        public int PriorityID { get; set; }
        public Priority? Priority { get; set; }

        //Audit fields
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("modifiedAt")]
        public DateTime? ModifiedAt { get; set; }

        [Column("deletedAt")]
        public DateTime? DeletedAt { get; set; }

        [Column("archivedAt")]
        public DateTime? ArchivedAt { get; set; }

        [Column("archived")]
        public bool Archived { get; set; } = false;

        [Column("createdBy")]
        public int CreatedBy { get; set; }
        public User? CreatedByUser { get; set; }

        [Column("modifiedBy")]
        public int? ModifiedBy { get; set; }
        public User? ModifiedByUser { get; set; }

        [Column("deletedBy")]
        public int? DeletedBy { get; set; }
        public User? DeletedByUser { get; set; }

        [Column("archivedBy")]
        public int? ArchivedBy { get; set; }
        public User? ArchivedByUser { get; set; }

        
    }
}