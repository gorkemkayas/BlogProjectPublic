using BlogProject.Application.Interfaces;
using BlogProject.Domain.Entities;
using BlogProject.Infrastructure.Persistence;
using Ganss.Xss;

namespace BlogProject.Infrastructure.Services
{
    public class FeedbackService : IFeedbackService
    {
        public readonly BlogDbContext _blogDbContext;

        public FeedbackService(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<bool> SubmitFeedbackAsync(string message)
        {

            var sanitizer = new HtmlSanitizer();
            var sanitizedMessage = sanitizer.Sanitize(message);
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Geri bildirim mesajı boş olamaz.", nameof(sanitizedMessage));
            }
            var newFeedback = new FeedbackEntity
            {
                Message = sanitizedMessage,
                CreatedTime = DateTime.Now
            };

            _blogDbContext.Feedbacks.Add(newFeedback);
            var result = await _blogDbContext.SaveChangesAsync();

            // Eğer kayıt başarılıysa, 1 döner
            return result > 0;
        }
    }
}
