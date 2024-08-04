using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using UdemyEgitimPlatformu.Data;

namespace UdemyEgitimPlatformu.Controllers
{
    public class GenelVeriler : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ViewDataDictionary _viewData;


        public GenelVeriler(ApplicationDbContext context, ViewDataDictionary viewData)
        {
            _context = context;
            _viewData = viewData;
        }

        public IActionResult Index()
        {

            return View();
        }


        public void LoadSiteSettings()
        {
            var siteName = _context.Settings.FirstOrDefault(s => s.Name == "sitename");
            var siteNameValue = siteName?.Value;
            _viewData["siteName"] = siteNameValue;

            var siteDescription = _context.Settings.FirstOrDefault(s => s.Name == "sitedescription");
            var siteDescriptionValue = siteDescription?.Value;
            _viewData["siteDescription"] = siteDescriptionValue;

            var logoTextSetting = _context.Settings.FirstOrDefault(s => s.Name == "logo");
            var logoTextValue = logoTextSetting?.Value;
            _viewData["LogoSrc"] = logoTextValue;

            var HeaderLoginButtonsEnableCheck = _context.Settings.FirstOrDefault(s => s.Name == "login_buttons_enable");
            var HeaderLoginButtonsEnable = HeaderLoginButtonsEnableCheck?.Value;
            _viewData["headerLoginButtons"] = HeaderLoginButtonsEnable;

            var header_menu_is_enable = _context.Settings.FirstOrDefault(s => s.Name == "header_menu_enable");
            var header_menu_is_enable_value = header_menu_is_enable?.Value;
            _viewData["headerMenu"] = header_menu_is_enable_value;

            var favicon = _context.Settings.FirstOrDefault(s => s.Name == "favicon");
            var favicon_value = favicon?.Value;
            _viewData["favicon"] = favicon_value;

            var siteurl = _context.Settings.FirstOrDefault(s => s.Name == "siteurl");
            var siteurl1 = siteurl?.Value;
            _viewData["siteurl"] = siteurl1; 


            
            
            
            var yonetimeposta = _context.Settings.FirstOrDefault(s => s.Name == "yonetimeposta");
            var yonetimeposta1 = yonetimeposta?.Value;
            _viewData["yonetimeposta"] = yonetimeposta1;



    

        }

    }
}
