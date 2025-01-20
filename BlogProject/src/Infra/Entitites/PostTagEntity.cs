namespace BlogProject.src.Infra.Entitites
{
    public class PostTagEntity
    {
        public DateTime AssignedDate { get; set; }
        
        public Guid TagId { get; set; }
        public TagEntity Tag { get; set; }

        public Guid PostId { get; set; }
        public PostEntity Post { get; set; }

        public bool IsDeleted { get; set; }
    }
}
