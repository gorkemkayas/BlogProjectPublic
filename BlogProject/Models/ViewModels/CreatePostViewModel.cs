using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models.ViewModels
{
    public class CreatePostViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Title { get; set; }

        [Display(Name = "Alt Başlık")]
        [StringLength(300, ErrorMessage = "Alt başlık en fazla 300 karakter olabilir.")]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; }

        [Display(Name = "Özet İçerik")]
        public string SubContent { get; set; }

        [Display(Name = "Kapak Resmi")]
        public IFormFile CoverImage { get; set; }

        [Display(Name = "Kapak Resmi URL")]
        public string CoverImageUrl { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Etiketler")]
        public List<int> TagIds { get; set; } = new List<int>();

        [Display(Name = "Taslak olarak kaydet")]
        public bool IsDraft { get; set; } = true;

        public string AuthorId { get; set; } = null!;

        // Select list'ler
        public List<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AvailableTags { get; set; } = new List<SelectListItem>();

        // Ek özellikler
        [Display(Name = "Yayın Tarihi")]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Yorumlara izin ver")]
        public bool AllowComments { get; set; } = true;

        //// SEO ile ilgili alanlar
        //[Display(Name = "SEO Başlık")]
        //[StringLength(60, ErrorMessage = "SEO başlığı en fazla 60 karakter olabilir.")]
        //public string SeoTitle { get; set; }

        //[Display(Name = "SEO Açıklama")]
        //[StringLength(160, ErrorMessage = "SEO açıklaması en fazla 160 karakter olabilir.")]
        //public string SeoDescription { get; set; }

        //[Display(Name = "SEO Etiketler")]
        //public string SeoTags { get; set; }
    }
}
