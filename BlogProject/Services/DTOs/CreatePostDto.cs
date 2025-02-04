namespace BlogProject.Services.DTOs
{
    public record CreatePostDto
    {
        string Title;
        string Subtitle;
        string Content;
        string SubContent;
        string CoverImageUrl;
        Guid AuthorId;
        Guid CategoryId;

    }
}
