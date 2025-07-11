using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Enums;
using BlogProject.Application.Interfaces;
using BlogProject.Application.Models;
using BlogProject.Areas.Admin.Models;
using BlogProject.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogProject.Web.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [IgnoreAntiforgeryToken]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TagUpdate(Guid tagId)
        {
            var result = await _tagService.GetTagByIdAsync(tagId);
            if (result == null)
            {
                TempData["Failed"] = "Tag not found.";
                return RedirectToAction(nameof(TagList));
            }
            var model = _mapper.Map<TagUpdateViewModel>(result);
            if (model == null)
            {
                TempData["Failed"] = "Tag could not be mapped.";
                return RedirectToAction(nameof(TagList));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TagUpdate(TagUpdateViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                TempData["Failed"] = "An error ocured while attempting update tag.";
                return View(model);
            }
            var mappedModel = _mapper.Map<TagUpdateDto>(model);
            var result = await _tagService.UpdateTagAsync(mappedModel);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Tag could not be updated. Please try again.";
                foreach (var error in result.Errors!)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["Success"] = "Tag updated successfully!";
            return RedirectToAction(nameof(TagList), "Tag");
        }

        public async Task<IActionResult> TagList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var tags = await _tagService.GetPagedTagsAsync(page, pageSize, includeDeleted);
            if (tags == null)
            {
                TempData["Failed"] = "No tags found.";
                return View(new ItemPagination<TagViewModel>());
            }
            tags.IncludeDeleted = includeDeleted;
            tags.ControllerName = "Tag";
            tags.ActionName = "TagList";

            return View(tags);
        }

        [HttpGet]
        public IActionResult TagAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TagAdd(TagAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid model state
                return View(model);
            }
            var mappedModel = _mapper.Map<TagAddDto>(model);
            var result = await _tagService.AddTagAsync(mappedModel);

            if(!result.IsSuccess)
            {
                TempData["Failed"] = string.Join(", ", result.Errors!.Select(e => e.Description));
                ModelState.AddModelErrorList(result.Errors!);
            }

            TempData["Success"] = "Tag added successfully.";
            return RedirectToAction("TagList", "Tag");
        }

        [HttpPost("Tag/TagDelete/{id}")]

        public async Task<IActionResult> TagDelete(string id)
        {
            var deleterUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(deleterUserId))
            {
                TempData["Failed"] = "Invalid user ID or tag ID!";
                return RedirectToAction(nameof(TagList), "Tag");
            }
            var result = await _tagService.DeleteTagByTypeAsync(id, DeleteType.Soft, deleterUserId);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "Invalid tag ID!";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(TagList)) });
            }
            TempData["Succeed"] = "Tag deleted successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(TagList)) });
        }

        [HttpPost("Tag/TagActivate/{id}")]
        public async Task<IActionResult> TagActivate(string id)
        {
            var activatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(activatorUserId))
            {
                TempData["Failed"] = "Invalid user ID!";
                return RedirectToAction(nameof(TagList), "Tag");
            }
            var result = await _tagService.ActivateTagById(id);

            if (!result.IsSuccess)
            {
                TempData["Failed"] = "An error occured while attemping activate tag.";
                return Json(new { status = false, redirectUrl = Url.Action(nameof(TagList)) });
            }
            TempData["Succeed"] = "Tag activated successfully!";
            return Json(new { status = true, redirectUrl = Url.Action(nameof(TagList)) });
        }
    }
}
