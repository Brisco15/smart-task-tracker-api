using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SmartTaskTracker.API.Models
{
    [Table("TimeTracking")]
    public class TimeTracking
    {
        [Column("timeTrackingID")]
        public int TimeTrackingID { get; set; }

        [Required]
        [Column("taskID")]
        public int TaskID { get; set; }
        public TaskItem? Task { get; set; }

        [Required]
        [Column("userID")]
        public int UserID { get; set; }
        public User? User { get; set; }

        [Required]
        [Column("startTime")]
        public DateTime StartTime { get; set; }

        
        [Column("endTime")]
        public DateTime? EndTime { get; set; }

        [Column("duration")]
        public double? Duration { get; set; } 
        
    }
}