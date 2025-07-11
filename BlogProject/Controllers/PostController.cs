using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Interfaces;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _env;

        private readonly IMapper _mapper;

        public PostController(IPostService postService, BlogDbContext context, ITagService tagService, ICategoryService categoryService, ICommentService commentService, IWebHostEnvironment env, IMapper mapper)
        {
            _postService = postService;
            _context = context;
            _tagService = tagService;
            _categoryService = categoryService;
            _commentService = commentService;
            _env = env;
            _mapper = mapper;
        }


        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("Post/NewPost")]
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
            // Kategorileri ve etiketleri tekrar doldurun
            model.AvailableCategories = _categoryService.GetAllCategorySelectList().Data!;
            model.AvailableTags = _tagService.GetAllTagSelectList().Data!;
            return View(model);
        }

        [HttpGet("Post/{id}")]
        public async Task<IActionResult> PostDetails(string id)
        {
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
                CurrentUser = User.Identity.IsAuthenticated ? await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name) : null
            };
            return View(model);
        }
    }
}
