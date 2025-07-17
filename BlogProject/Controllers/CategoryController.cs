using BlogProject.Application.Interfaces;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public CategoryController(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }



        //[Route("{category:string}")]
        public IActionResult Index()
        {
            //if(string.IsNullOrEmpty(category))
            //{
            //    TempData["Failed"] = "Category not found.";
            //    return NotFound();
            //}
            //var selectedCategory = _context.Categories.FirstOrDefault(c => c.Name.Equals(category, StringComparison.OrdinalIgnoreCase));
            //if(selectedCategory is null)
            //{
            //    TempData["Failed"] = "Category not found.";
            //    return NotFound();
            //}
            //var posts = _context.Posts
            //    .Where(p => p.CategoryId == selectedCategory.Id && p.IsDeleted!)
            //    .OrderByDescending(p => p.CreatedTime)
            //    .ToList();

            //return View(posts);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Category(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var posts = await _postService.GetByCategoryIdAsync(id);
            var relatedCategories = await _categoryService.GetRelatedCategoriesAsync(id);
            ViewBag.CategoryId = id;

            var viewModel = new CategoryViewModel
            {
                Category = category,
                Posts = posts,
                RelatedCategories = relatedCategories,
                TotalViews = posts.Sum(p => p.ViewCount),
                AuthorCount = posts.Select(p => p.AuthorId).Distinct().Count()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostCountsByDate(string categoryId)
        {
            var chartData = await _categoryService.GetDailyPostCountsAsync(categoryId);
            return Json(chartData);
        }

    }
}
