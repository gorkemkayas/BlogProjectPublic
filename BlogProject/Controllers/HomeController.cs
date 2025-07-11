using System.Diagnostics;
using BlogProject.Application.Interfaces;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IUserService _userService;


        public HomeController(ILogger<HomeController> logger, IPostService postService, ITagService tagService, IUserService userService)
        {
            _logger = logger;
            _postService = postService;
            _tagService = tagService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var mostViewedPosts = await _postService.GetMostViewedPostsWithCount();
            var LatestPosts = await _postService.GetLatestPostsWithCount();
            var popularTags = await _tagService.GetPopularTags(15);
            var mostContributors = await _userService.MostContributors(3);
            var mostReadPostsThisWeek = await _postService.GetMostViewedPostsWithCount(5, true);

            var model = new IndexViewModel
            {
                MostViewedPosts = mostViewedPosts,
                LatestPosts = LatestPosts,
                PopularTags = popularTags,
                MostContributors = mostContributors,
                MostReadPostsThisWeek = mostReadPostsThisWeek

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
