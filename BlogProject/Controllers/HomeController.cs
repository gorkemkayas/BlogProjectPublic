using System.Diagnostics;
using BlogProject.Models;
using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;


        public HomeController(ILogger<HomeController> logger, IPostService postService, ITagService tagService)
        {
            _logger = logger;
            _postService = postService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var mostViewedPosts = await _postService.GetMostViewedPostsWithCount();
            var LeastPosts = await _postService.GetLatestPostsWithCount();
            var popularTags = await _tagService.GetPopularTags(15);

            var model = new IndexViewModel
            {
                MostViewedPosts = mostViewedPosts,
                LatestPosts = LeastPosts,
                PopularTags = popularTags
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult TestViewComponent()
        {
            return ViewComponent("SuggestedUsers");
        }

    }
}
