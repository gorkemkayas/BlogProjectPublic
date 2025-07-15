using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Interfaces;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        public PostController(IPostService postService, ITagService tagService, ICategoryService categoryService, ICommentService commentService, IWebHostEnvironment env, IMapper mapper, IUserService userService)
        {
            _postService = postService;
            _tagService = tagService;
            _categoryService = categoryService;
            _commentService = commentService;
            _env = env;
            _mapper = mapper;
            _userService = userService;
        }


        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("Post/NewPost")]
        public async Task<IActionResult> NewPost()
        {
            var availableCategories = _categoryService.GetAllCategorySelectList();
            var availableTags = _tagService.GetAllTagSelectList();
            List<SelectListItem>? selectCategoriesList = availableCategories.Data?.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value,
                Selected = x.Selected
            }).ToList();
            List<SelectListItem>? selectTagsList = availableTags.Data?.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value,
                Selected = x.Selected
            }).ToList();
            var model = new CreatePostViewModel
            {
                // Kategorileri veritabanından veya servisten doldurun
                AvailableCategories = (selectCategoriesList is null) ? new List<SelectListItem>() : selectCategoriesList,

                // Etiketleri veritabanından veya servisten doldurun
                AvailableTags = (selectTagsList is null) ? new List<SelectListItem>() : selectTagsList
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> NewPost(CreatePostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedResult = _mapper.Map<CreatePostDto>(model);
                // Post ekleme işlemi
                var result = await _postService.CreatePostAsync(mappedResult);
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


            var availableCategories = _categoryService.GetAllCategorySelectList();
            var availableTags = _tagService.GetAllTagSelectList();
            List<SelectListItem>? selectCategoriesList = availableCategories.Data?.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value,
                Selected = x.Selected
            }).ToList();
            List<SelectListItem>? selectTagsList = availableTags.Data?.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value,
                Selected = x.Selected
            }).ToList();

            // Kategorileri ve etiketleri tekrar doldurun
            model.AvailableCategories = (selectCategoriesList is null) ? new List<SelectListItem>() : selectCategoriesList;
            model.AvailableTags = (selectTagsList is null) ? new List<SelectListItem>() : selectTagsList;
            return View(model);
        }

        [HttpGet("Post/{id}")]
        public async Task<IActionResult> PostDetails(string id)
        {
            var profilePicClaim = HttpContext.User.FindFirst("ProfilePictureUrl")?.Value;
            Console.WriteLine("Controller claim: " + (profilePicClaim ?? "Claim yok"));

            var post = await _postService.GetPostByIdAsync(Guid.Parse(id),true);
            var recommendedPost = await _postService.GetLatestPostsWithCount(3);
            if (post == null)
            {
                return NotFound();
            }
            var comments = await _commentService.GetCommentsByPostIdAsync(id);
            var model = new PostDetailsViewModel
            {
                Post = post,
                RecommendedPosts = recommendedPost,
                Comments = comments,
                CurrentUser = await _userService.FindByUsername(User.Identity.Name)
            };
            return View(model);
        }

        [HttpGet("Post/ListByCategory")]
        public IActionResult ListByCategory()
        {
            return View();
        }
    }
}
