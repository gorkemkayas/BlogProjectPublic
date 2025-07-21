using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;

        public TagController(IPostService postService, ITagService tagService)
        {
            _postService = postService;
            _tagService = tagService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Tag(string id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            var posts = await _postService.GetPostByTagIdAsync(id);
            var relatedTags = await _tagService.GetRelatedTagsAsync(id);
            ViewBag.TagId = id;

            if(posts is null)
                posts = new List<PostEntity>();

            var viewModel = new TagViewModel
            {
                Tag = tag,
                Posts = posts,
                RelatedTags = relatedTags,
                TotalViews = posts.Sum(p => p.ViewCount),
                AuthorCount = posts.Select(p => p.AuthorId).Distinct().Count()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostCountsByDate(string tagId)
        {
            var chartData = await _tagService.GetDailyPostCountsAsync(tagId);
            return Json(chartData);
        }
    }
}
