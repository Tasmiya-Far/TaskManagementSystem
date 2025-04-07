namespace TaskManagementSystem.DTOs
{
    public class TaskQueryParamsViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool? IsCompleted { get; set; }
        public string SortOrder { get; set; } = "asc";
        public DateTime? DueDate { get; set; }
    }
}
