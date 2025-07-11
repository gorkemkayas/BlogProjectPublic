using BlogProject.Services.Abstract;
using BlogProject.src.Infra.Context;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BlogDbContext _context;

        public CategoryController(BlogDbContext context)
        {
            _context = context;
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
    }
}
