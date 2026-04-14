namespace SmartTaskTracker.API.Models
{
    public class UpdateProjectRequest
    {
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}