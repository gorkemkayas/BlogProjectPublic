using BlogProject.Application.Interfaces;

namespace BlogProject.Application.Models
{
    public class ItemPagination<T> : IPaginationInfo
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool IncludeDeleted { get; set; } = false;
        public string ControllerName { get; set ; }
        public string ActionName { get; set; }
    }
}
