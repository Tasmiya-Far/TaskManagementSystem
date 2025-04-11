namespace TaskManagementSystem.Core.ViewModel
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Tasks { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
