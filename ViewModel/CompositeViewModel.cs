using System.Collections.Generic;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.ViewModels;

namespace UdemyEgitimPlatformu.ViewModel
{
    public class CompositeViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public List<AlinanVideolar> AlinanVideolarList { get; set; } 

        public ApplicationUser ApplicationUser {  get; set; }

        public CategoryViewModel CategoryViewModel { get; set; }
        public List<Videolar> Videolar { get; set; }
        public List<SmtpSettings> SmtpSettings { get; set; }

        public string Username { get; set; }
        public string UsernameID { get; set; }


    }
}
