namespace BlogProject.src.Infra.Entitites.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
    }
}
