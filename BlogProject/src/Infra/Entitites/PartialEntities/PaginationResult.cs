namespace BlogProject.src.Infra.Entitites.PartialEntities
{
    public class PaginationResult<T>
    {
        public ICollection<T> Posts { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
