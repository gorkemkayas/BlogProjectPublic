using BlogProject.src.Infra.Entitites.Base;
using BlogProject.src.Infra.Entitites.PartialEntities;

namespace BlogProject.src.Infra.Entitites
{
    public class ShareEntity : BaseEntity
    {
        public string ShareLink { get; set; }
        public Platform Platform { get; set; }
        
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }

        public Guid? PostId { get; set; }
        public PostEntity? Post { get; set; }
    }
}
