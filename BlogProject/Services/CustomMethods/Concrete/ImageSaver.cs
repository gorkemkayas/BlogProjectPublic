using System.Text.RegularExpressions;

namespace BlogProject.Services.CustomMethods.Concrete
{
    public static class ImageSaver
    {
        public static async Task<string> SaveUserImageAsync(IFormFile formFile, string postTitle)
        {
            string? safeTitle = null;
            if(postTitle.Length > 50)
            {
                var slug = Slugify(postTitle);
                safeTitle = slug.Length > 50 ? slug.Substring(0, 50) : slug;
            }
            else
            {
                safeTitle = postTitle;
            }
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Posts", safeTitle);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Path.GetFileNameWithoutExtension(formFile.FileName)
                .Replace(" ", "_") + Path.GetExtension(formFile.FileName);
            string filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // wwwroot'tan sonraki yolu döndürmek genellikle yeterlidir
            return $"/img/Posts/{safeTitle}/{fileName}";
        }
        public static string Slugify(string input)
        {
            input = input.ToLowerInvariant();
            input = Regex.Replace(input, @"\s+", "-");               // boşlukları tire ile değiştir
            input = Regex.Replace(input, @"[^a-z0-9\-]", "");         // geçersiz karakterleri sil
            input = Regex.Replace(input, @"-+", "-");                 // ardışık tireleri tek tire yap
            return input.Trim('-');
        }
    }
}
