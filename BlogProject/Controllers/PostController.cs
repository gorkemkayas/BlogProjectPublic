using BlogProject.Models.ViewModels;
using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly BlogDbContext _context;

        public PostController(IPostService postService, BlogDbContext context)
        {
            _postService = postService;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> NewPost()
        {
            var model = new CreatePostViewModel
            {
                // Kategorileri veritabanından veya servisten doldurun
                AvailableCategories = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "Teknoloji" },
                    new SelectListItem { Value = "2", Text = "Yazılım" },
                    new SelectListItem { Value = "3", Text = "Tasarım" }
                },

                // Etiketleri veritabanından veya servisten doldurun
                AvailableTags = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "C#" },
                    new SelectListItem { Value = "2", Text = "ASP.NET Core" },
                    new SelectListItem { Value = "3", Text = "MVC" },
                    new SelectListItem { Value = "4", Text = "Entity Framework" }
                }
            };

            return View(model);
        }
    }
}
