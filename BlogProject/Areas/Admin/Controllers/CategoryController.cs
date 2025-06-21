using AutoMapper;
using BlogProject.Areas.Admin.Models;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using BlogProject.src.Infra.Context;
using BlogProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static BlogProject.Utilities.RoleService;

namespace BlogProject.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [IgnoreAntiforgeryToken]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CategoryList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var categories = await _categoryService.GetPagedCategoriesAsync(page, pageSize, includeDeleted);
            if (categories == null)
            {
                TempData["Failed"] = "No categories found.";
                return View(new ItemPagination<CategoryViewModel>());
            }
            categories.IncludeDeleted = includeDeleted;
            categories.ControllerName = "Category";
            categories.ActionName = "CategoryList";

            return View(categories);
        }

        [HttpGet]
        public IActionResult CategoryAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAdd(CategoryAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Here you would typically save the model to the database
                // For example:
                // _context.Categories.Add(model);
                // _context.SaveChanges();
                // Redirect to a success page or back to the index
                return View(model);
            }
            var result = await _categoryService.AddCategoryAsync(model);
            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Category could not be added. Please try again.";
                foreach (var error in result.Errors!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            TempData["Success"] = "Category added successfully!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CategoryUpdate(Guid categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            if (result == null)
            {
                TempData["Failed"] = "Category not found.";
                return RedirectToAction(nameof(CategoryList));
            }
            var model = _mapper.Map<CategoryUpdateViewModel>(result);
            if (model == null)
            {
                TempData["Failed"] = "Category could not be mapped.";
                return RedirectToAction(nameof(CategoryList));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CategoryUpdate(CategoryUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _categoryService.UpdateCategoryAsync(model);
            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Category could not be updated. Please try again.";
                foreach (var error in result.Errors!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            TempData["Success"] = "Category updated successfully!";
            return RedirectToAction(nameof(CategoryList), "Category");
        }

        [HttpPost("Category/CategoryDelete/{id}")]

        public async Task<IActionResult> CategoryDelete(string id)
        {
            var deleterUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(deleterUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction("CategoryList", "Category");
            }
            var result = await _categoryService.DeleteCategoryByTypeAsync(id, DeleteType.Soft, deleterUserId);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Invalid category ID!";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(CategoryList)) });
            }
            TempData["Succeed"] = "Category deleted successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(CategoryList)) });
        }

        [HttpPost("Category/CategoryActivate/{id}")]
        public async Task<IActionResult> CategoryActivate(string id)
        {
            var activatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(activatorUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction("CategoryList", "Category");
            }
            var result = await _categoryService.ActivateCategoryById(id);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "An error occured while attemping activate category.";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(CategoryList)) });
            }
            TempData["Succeed"] = "Category activated successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(CategoryList)) });
        }


    }
}
