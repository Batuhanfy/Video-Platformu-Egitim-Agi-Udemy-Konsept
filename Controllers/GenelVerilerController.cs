using Microsoft.AspNetCore.Mvc;
using UdemyEgitimPlatformu.Data;

namespace UdemyEgitimPlatformu.Controllers
{


    public class GenelVerilerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenelVerilerController(ApplicationDbContext context)
        {
            _context = context;
            LoadSiteSettings();
        }


        private void LoadSiteSettings()
        {
            var siteName = _context.Settings.FirstOrDefault(s => s.Name == "sitename");
            var siteNameValue = siteName?.Value;
            ViewData["siteName"] = siteNameValue;

            var siteDescription = _context.Settings.FirstOrDefault(s => s.Name == "sitedescription");
            var siteDescriptionValue = siteDescription?.Value;
            ViewData["siteDescription"] = siteDescriptionValue;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
