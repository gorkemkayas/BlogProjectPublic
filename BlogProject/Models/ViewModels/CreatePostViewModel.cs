using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(150, ErrorMessage = "Başlık en fazla 150 karakter olabilir")]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "Alt başlık en fazla 200 karakter olabilir")]
        public string? Subtitle { get; set; }

        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Content { get; set; }

        public string? SubContent { get; set; }

        [Display(Name = "Kapak Resmi")]
        public IFormFile? CoverImage { get; set; }
        public string? CoverImageUrl { get; set; }

        [Display(Name = "Taslak olarak kaydet")]
        public bool IsDraft { get; set; }

        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Kategori seçimi zorunludur")]
        public Guid CategoryId { get; set; }

        [Display(Name = "Etiketler")]
        public List<Guid>? TagIds { get; set; }

        public List<SelectListItem>? AvailableCategories { get; set; }
        public List<SelectListItem>? AvailableTags { get; set; }
    }
}
