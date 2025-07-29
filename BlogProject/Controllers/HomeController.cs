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
            if (!Request.Cookies.ContainsKey("FirstVisit"))
            {
                ViewBag.ShowWelcomeMessage = true;
                Response.Cookies.Append("FirstVisit", "true", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });
            }
            else
            {
                ViewBag.ShowWelcomeMessage = false;
            }


            var mostViewedTask = _postService.GetMostViewedPostsWithCount();
            var latestPostsTask = _postService.GetLatestPostsWithCount();
            var popularTagsTask = _tagService.GetPopularTags(15);
            var mostContributorsTask = _userService.MostContributors(3);
            var mostReadThisWeekTask = _postService.GetMostViewedPostsWithCount(5, true);

            try
            {
            await Task.WhenAll(mostViewedTask, latestPostsTask, popularTagsTask, mostContributorsTask, mostReadThisWeekTask);
            }
            catch (Exception)
            {
                if (mostViewedTask.IsFaulted)
                    Console.WriteLine($"mostViewedTask failed: {mostViewedTask.Exception?.GetBaseException().Message}");
                if (latestPostsTask.IsFaulted)
                    Console.WriteLine($"latestPostsTask failed: {latestPostsTask.Exception?.GetBaseException().Message}");
                if (popularTagsTask.IsFaulted)
                    Console.WriteLine($"mostViewedTask failed: {popularTagsTask.Exception?.GetBaseException().Message}");
                if (mostContributorsTask.IsFaulted)
                    Console.WriteLine($"latestPostsTask failed: {mostContributorsTask.Exception?.GetBaseException().Message}");
                if (mostReadThisWeekTask.IsFaulted)
                    Console.WriteLine($"latestPostsTask failed: {mostReadThisWeekTask.Exception?.GetBaseException().Message}");
            }

            var model = new IndexViewModel
            {
                MostViewedPosts = mostViewedTask.Result,
                LatestPosts = latestPostsTask.Result,
                PopularTags = popularTagsTask.Result,
                MostContributors = mostContributorsTask.Result,
                MostReadPostsThisWeek = mostReadThisWeekTask.Result
            };

            return View(model);
        }


        [Route("/development-updates")]
        public async Task<IActionResult> DevelopmentUpdates()
        {
            return View();
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
