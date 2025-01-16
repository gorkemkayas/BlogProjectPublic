using System.ComponentModel.DataAnnotations.Schema;
using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class UserEntity : BaseUserEntity
    {
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; }


        public virtual ICollection<CommentEntity> Comments { get; set; }
        public virtual ICollection<PostEntity> Posts { get; set; }

        public virtual ICollection<NotificationEntity> Notifications { get; set; }

        public Guid SettingsId { get; set; }
        public virtual SettingsEntity Settings { get; set; }

        public Guid UserMembershipId { get; set; }
        public virtual UserMemberShipEntity UserMemberShipEntity { get; set; }
        
        public virtual ICollection<WalletEntity> Wallets { get; set; }

    }
}
