namespace class CreateTaskItemRequest
{
    public class CreateTaskItemRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? AssignedUserID { get; set; }
        public int? ProjectID { get; set; }
        public int StatusID { get; set; } = 1;
        public int PriorityID { get; set; }
    }
}