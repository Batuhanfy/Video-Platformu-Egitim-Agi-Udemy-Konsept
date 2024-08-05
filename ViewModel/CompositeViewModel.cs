using System.Collections.Generic;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.ViewModels;

namespace UdemyEgitimPlatformu.ViewModel
{
    public class CompositeViewModel
    {
        public string? RequestType { get; set; }

        public List<Kategoriler>? KategorilerList { get; set; }
        public List<Menuler>? Menuler { get; set; }

        public string? Comments { get; set; }
        public LoginViewModel? LoginViewModel { get; set; }
        public RegisterViewModel? RegisterViewModel { get; set; }
        public List<AlinanVideolar>? AlinanVideolarList { get; set; }
        public List<ApplicationRequest>? ApplicationRequest { get; set; }

        public VideoUploadViewModel? VideoUploadViewModel { get; set; }
        public ApplicationUser? ApplicationUser {  get; set; }

        public CategoryViewModel? CategoryViewModel { get; set; }
        public List<Videolar>? Videolar { get; set; }


        public List<SmtpSettings>? SmtpSettings { get; set; }

        public bool? IsVideoInUserList { get; set; }
        public string? Username { get; set; }
        public string? UsernameID { get; set; }


    }
}
