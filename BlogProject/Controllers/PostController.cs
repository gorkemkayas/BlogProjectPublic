using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}
