namespace SmartTaskTracker.API.Models.DTOs
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public int StatusID { get; set; }
        public int PriorityID { get; set; }
        
    }
}