using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UdemyEgitimPlatformu.Models;

namespace UdemyEgitimPlatformu.ViewModel
{
    public class VideoUploadViewModel
    {
        [Required]
        [Display(Name = "Video Adı")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Video Dosyası")]
        public IFormFile VideoFile { get; set; }

        public List<string> VideoContentList { get; set; } = new List<string>();


        [Required]
        public IFormFile CoverImageFile { get; set; }

        [Required]
        [Display(Name = "Kısa Video Dosyası")]

   
        public IFormFile ShortVideoFile { get; set; }

        public IEnumerable<Kategoriler> Categories { get; set; }
    }
}
