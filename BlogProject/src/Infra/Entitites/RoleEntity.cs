using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class RoleEntity : BaseEntity
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
