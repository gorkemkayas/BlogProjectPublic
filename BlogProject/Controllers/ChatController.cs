using BlogProject.Application.DTOs;
using BlogProject.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BlogProject.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly BlogDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _deepSeekApiKey = "sk-bd9f85f7e442499dad3c2d85737291fa";

        public ChatController(BlogDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("summarize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Summarize([FromBody] SummarizeDto dto)
        {
            var article = await _dbContext.Posts
                .Where(p => p.Id.ToString() == dto.PostId)
                .Select(p => new { p.Title, p.Content })
                .FirstOrDefaultAsync();

            if (article == null)
                return NotFound("Makale bulunamadı");

            var title = article.Title ?? "";
            var content = article.Content ?? "";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _deepSeekApiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
                {
            new { role = "system", content = "Sen yardımcı bir asistansın." },
            new { role = "user", content = $"Aşağıdaki makaleyi kısa ve anlaşılır şekilde özetle:\nBaşlık: {title}\nİçerik: {content}\nÖzet:" }
        }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var contentString = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://api.deepseek.com/v1/chat/completions", contentString);
                var responseString = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<JsonElement>(responseString);
                var summary = result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                return Ok(new { Summary = summary });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, response = ex.InnerException?.Message });
            }
        }

    }
}
