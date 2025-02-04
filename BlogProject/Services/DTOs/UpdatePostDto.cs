namespace BlogProject.Services.DTOs
{
    public class UpdatePostDto
    {
        string Title;
        string Subtitle;
        string Content;
        string SubContent;
        string CoverImageUrl;
        bool isDraft;
        Guid CategoryId;
    }
}
