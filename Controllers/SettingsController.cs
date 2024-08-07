using Microsoft.AspNetCore.Mvc;
using UdemyEgitimPlatformu.Data;

namespace UdemyEgitimPlatformu.Controllers
{

    public class SettingsController : Controller

    {
        private readonly ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

 

        public IActionResult Create()
        {
            return View();
        }



        public IActionResult Index()
        {

            return View();
        }
    }
}
