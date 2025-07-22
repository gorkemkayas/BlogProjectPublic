using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
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
        public async Task<IActionResult> Submit([FromBody] string message)
        {
            Console.WriteLine("submit e geldi.");
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Geri bildirim boş olamaz.");
            }

            var isOk = await _feedbackService.SubmitFeedbackAsync(message);
            // TODO: Geri bildirimi kaydet, e-posta gönder vs.
            Console.WriteLine("Geri bildirim alındı: " + message);

            return Ok(new { success = true });
        }
    }
}
