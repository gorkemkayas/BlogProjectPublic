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
            ViewBag.AuthorName = User.Identity?.Name;
            ViewBag.AuthorBio = "Yazar Hakkında Kısa Bilgi";

            var availableCategories = await _context.Categories.ToListAsync();
            var availableTags = await _context.Tags.ToListAsync();
            var viewModel = new CreatePostViewModel
            {
                AvailableCategories = availableCategories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                AvailableTags = availableTags.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList()
            };
            return View(viewModel);
        }
    }
}
