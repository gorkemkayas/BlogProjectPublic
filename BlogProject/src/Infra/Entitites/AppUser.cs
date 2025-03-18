using System.ComponentModel.DataAnnotations.Schema;
using BlogProject.src.Infra.Entitites.Base;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.src.Infra.Entitites
{
    public class AppUser : IdentityUser<Guid>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;

        public string FullName { get { return Name + " " + Surname; } }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? WorkingAt { get; set; }
        public string? WorkingAtLogo { get; set; }

        //public string? LivesIn { get; set; }

        public string? Country { get; set; }

        // yeni eklediklerim
        public string? CurrentPosition { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }

        public string? XAddress { get; set; }
        public string? LinkedinAddress { get; set; }
        public string? GithubAddress { get; set; }
        public string? MediumAddress { get; set; }
        public string? YoutubeAddress { get; set; }
        public string? PersonalWebAddress { get; set; }

        public string? HighSchoolName { get; set; }
        public string? HighSchoolStartYear { get; set; }
        public string? HighSchoolGraduationYear { get; set; }

        public string? UniversityName { get; set; }
        public string? UniversityStartYear { get; set; }
        public string? UniversityGraduationYear { get; set; }

        public ICollection<PostEntity> FeaturedPosts { get; set; } = new List<PostEntity>();



        // sonu
        public DateTime BirthDate { get; set; }

        public DateTime RegisteredDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ProfilePicture { get; set; }
        public string? CoverImagePicture { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int NotificationCount { get; set; } = 0;

        public Guid SettingsId { get; set; }
        public virtual SettingsEntity? Settings { get; set; }

        public Guid UserMembershipId { get; set; }
        public virtual UserMemberShipEntity? UserMemberShipEntity { get; set; }

        public virtual ICollection<PostEntity>? Posts { get; set; }
        public virtual ICollection<SavedPostEntity>? SavedPosts { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }
        public virtual ICollection<NotificationEntity>? Notifications { get; set; }
        public virtual ICollection<WalletEntity> Wallets { get; set; } = new List<WalletEntity>();
        public virtual ICollection<BadgeUserEntity>? BadgeUsers { get; set; }
        public virtual ICollection<ShareEntity>? Shares { get; set; }
        public virtual ICollection<LikeEntity>? Likes { get; set; }

        public virtual ICollection<FollowEntity>? Followers { get; set; } // Kullanıcıyı takip edenler
        public virtual ICollection<FollowEntity>? Followings { get; set; } // Kullanıcının takip ettikleri


        [InverseProperty("Following")]
        public virtual ICollection<SubscriptionEntity>? Subscriptions { get; set; }

        [InverseProperty("Follower")]
        public virtual ICollection<SubscriptionEntity>? Subscribers { get; set; }

        public virtual ICollection<DonationEntity>? SendedDonations { get; set; }
        public virtual ICollection<DonationEntity>? ReceivedDonations { get; set; }

        public virtual ICollection<ReportEntity>? CreatedReports { get; set; }
        public virtual ICollection<ReportEntity>? ReceivedReports { get; set; }

        public virtual ICollection<MessageEntity>? SentMessages { get; set; }
        public virtual ICollection<MessageEntity>? ReceivedMessages { get; set; }
    }
}
