namespace BlogProject.Application.DTOs 
{
    public class UserRoleDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool Exists { get; set; }
    }
}
