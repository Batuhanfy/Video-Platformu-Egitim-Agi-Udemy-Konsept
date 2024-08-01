using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BidemyLearning.Models;
using UdemyEgitimPlatformu.Data;
using Microsoft.EntityFrameworkCore;

namespace BidemyLearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _context.Settings.ToListAsync();
            ViewData["Settings"] = settings;
            return View();
        }
    }
}
