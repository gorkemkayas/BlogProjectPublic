using System.ComponentModel.DataAnnotations.Schema;
using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class UserEntity : BaseUserEntity
    {
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public Guid SettingsId { get; set; }
        public virtual SettingsEntity Settings { get; set; }

        public Guid UserMembershipId { get; set; }
        public virtual UserMemberShipEntity UserMemberShipEntity { get; set; }

        public virtual ICollection<PostEntity>? Posts { get; set; }
        public virtual ICollection<SavedPostEntity>? SavedPosts { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }
        public virtual ICollection<NotificationEntity>? Notifications { get; set; }
        public virtual ICollection<WalletEntity> Wallets { get; set; }
        public virtual ICollection<BadgeUserEntity>? BadgeUsers { get; set; }
        public virtual ICollection<ShareEntity>? Shares { get; set; }
        public virtual ICollection<LikeEntity>? Likes { get; set; }

        public virtual ICollection<FollowEntity>? Followers { get; set; } // Kullanıcıyı takip edenler
        public virtual ICollection<FollowEntity>? Followings { get; set; } // Kullanıcının takip ettikleri

        public virtual ICollection<SubscriptionEntity>? Subscriptions { get; set; }
        public virtual ICollection<SubscriptionEntity>? Subscribers { get; set; }

        public virtual ICollection<DonationEntity>? SendedDonations { get; set; }
        public virtual ICollection<DonationEntity>? ReceivedDonations { get; set; }

        public virtual ICollection<ReportEntity>? CreatedReports { get; set; }
        public virtual ICollection<ReportEntity>? ReceivedReports { get; set; }

        public virtual ICollection<MessageEntity>? SentMessages { get; set; }
        public virtual ICollection<MessageEntity>? ReceivedMessages { get; set; }
    }
}
