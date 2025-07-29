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
            try
            {
                Console.WriteLine("Geri bildirim mesajı alındı: " + message);
                Console.WriteLine("sanitizere geldim.");
                var sanitizer = new HtmlSanitizer();
                var sanitizedMessage = sanitizer.Sanitize(message);
                Console.WriteLine($"sanitized message : {sanitizedMessage}");


                if (string.IsNullOrWhiteSpace(sanitizedMessage))
                {
                    throw new ArgumentException("Geri bildirim mesajı boş olamaz.", nameof(sanitizedMessage));
                }

                var newFeedback = new FeedbackEntity
                {
                    Message = sanitizedMessage,
                    CreatedTime = DateTime.Now
                };
                Console.WriteLine($"newFeedback Entitysi oluşuturldu : {newFeedback.Message}, {newFeedback.CreatedTime}");
                Console.WriteLine($"db ye ekleniyor.");
                _blogDbContext.Feedbacks.Add(newFeedback);
                Console.WriteLine("db ye eklendi.");
                var result = await _blogDbContext.SaveChangesAsync();
                Console.WriteLine($"db ye ekleme sonucu : {result}");
                return result > 0;
            } 
            catch (Exception ex)
            {

                throw new Exception("Geri bildirim gönderilirken bir hata oluştu: " + ex.Message, ex);
            }

        }

    }
}
