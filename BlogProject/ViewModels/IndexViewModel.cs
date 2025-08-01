using BlogProject.Application.DTOs;
using BlogProject.Domain.Entities;

namespace BlogProject.Web.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<PostDto> MostViewedPosts { get; set; } = Enumerable.Empty<PostDto>();
        public IEnumerable<PostDto> LatestPosts { get; set; } = Enumerable.Empty<PostDto>();
        public IEnumerable<TagEntity> PopularTags { get; set; } = Enumerable.Empty<TagEntity>();
        public IEnumerable<ContributorDto> MostContributors { get; set; } = Enumerable.Empty<ContributorDto>();
        public IEnumerable<PostDto> MostReadPostsThisWeek { get; set; } = Enumerable.Empty<PostDto>();
    }
}

