using System.ComponentModel.DataAnnotations.Schema;
using BlogProject.src.Infra.Entitites.Base;

namespace BlogProject.src.Infra.Entitites
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }

        [NotMapped]
        public string FullName 
        { 
            get
            { 
                return FirstName + SurName; 
            } 
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Bio {  get; set; }
        public string WorkingAt {  get; set; }

        public string Country { get; set; }
        public DateTime BirthDate { get; set; }

        public string ProfilePicture { get; set; }
        public string CoverImagePicture { get; set; }

        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public int NotificationCount { get; set; } = 0;

        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; }

        public ICollection<PostEntity> Posts { get; set; }

    }
}
