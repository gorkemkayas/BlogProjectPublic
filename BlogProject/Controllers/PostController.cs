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
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly BlogDbContext _context;

        public PostController(IPostService postService, BlogDbContext context, ITagService tagService, ICategoryService categoryService)
        {
            _postService = postService;
            _context = context;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> NewPost()
        {
            var model = new CreatePostViewModel
            {
                // Kategorileri veritabanından veya servisten doldurun
                AvailableCategories = _categoryService.GetAllCategorySelectList().Data!,

                // Etiketleri veritabanından veya servisten doldurun
                AvailableTags = _tagService.GetAllTagSelectList().Data!
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> NewPost(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Post ekleme işlemi
                var result = await _postService.CreatePostAsync(model);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors!)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            // Kategorileri ve etiketleri tekrar doldurun
            model.AvailableCategories = _categoryService.GetAllCategorySelectList().Data!;
            model.AvailableTags = _tagService.GetAllTagSelectList().Data!;
            return View(model);
        }
    }
}
