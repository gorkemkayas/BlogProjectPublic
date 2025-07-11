namespace BlogProject.Domain.Entities
{
    public class SavedPostEntity
    {
        public Guid Id { get; set; }
        public DateTime SavedDate { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }

        public bool IsDeleted { get; set; }
    }
}
