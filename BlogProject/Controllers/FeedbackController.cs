using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Web.Controllers
{
    public class FeedbackController : Controller
    {
        public readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [Route("Feedback/Submit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] FeedbackSubmitViewModel dto)
        {
            Console.WriteLine($"submit e geldi");
            if (string.IsNullOrWhiteSpace(dto.Message))
            {
                return BadRequest("Geri bildirim boş olamaz.");
            }

            var isOk = await _feedbackService.SubmitFeedbackAsync(dto.Message);
            Console.WriteLine("Geri bildirim alındı: " + dto.Message);

            return Ok(new { success = isOk });
        }

    }
}
