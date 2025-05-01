namespace BlogProject.Utilities
{
    public interface IPaginationInfo
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; }
        public bool IncludeDeleted { get; set; }

        string ControllerName { get; set; }
        string ActionName { get; set; }
    }
}
