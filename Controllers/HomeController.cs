using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using UdemyEgitimPlatformu.Controllers;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BidemyLearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;

        }
        public IActionResult Video(int id)
        {

            var video_kategori_id = _context.Videolar
                    .Where(v => v.Id == id)
                    .Select(v => new { v.CategoryId })
                    .FirstOrDefault();

            if (video_kategori_id == null)
            {
                return NotFound();
            }

            var kategori = _context.Kategoriler
                                   .Where(k => k.Id == video_kategori_id.CategoryId)
                                   .Select(k => new { k.Name })
                                   .FirstOrDefault();

            var video = _context.Videolar
                                   .Where(k => k.Id == id)
                                   .Select(k => new { k.Author,k.VideoContentList,k.videoaddress,k.videoshortadress,k.Id,k.Name ,k.Description,k.img,k.yildizortalamasi})
                                   .FirstOrDefault();

            if (kategori == null)
            {
                return NotFound();
            }




            var VideoBilgisi = _context.Videolar
                               .Include(v=>v.Category)
                               .Where(k => k.Id == id)
                               .ToList();

            var KategoriListGenel = _context.Kategoriler.ToList();

            if (KategoriListGenel == null || !KategoriListGenel.Any() && VideoBilgisi == null || !VideoBilgisi.Any())
            {
                return NotFound();
            }
            var Settings_Ayarlar = _context.Settings.ToList();

            var IsVideoInUserList = false;

            if(User.Identity.IsAuthenticated){
                var user = _userManager.GetUserAsync(User).Result;

                var userVideos = _context.AlinanVideolar
                                         .Where(uv => uv.Username == user.UserName)
                                         .Select(uv => uv.VideoId)
                                         .ToList();

               
                if (userVideos.Contains(id))
                {
                    ViewData["AlreadyInList"] = "Bu video zaten listenizde var.";
                    IsVideoInUserList = true;
                }
            }

            var BirlestirilmisViewModel1 = new CompositeViewModel
            {
                CategoryViewModel = new CategoryViewModel
                {
                    Videolar = VideoBilgisi,
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                    KategoriAdi = kategori.Name,
                    VideoBaslik = video.Name,
                    VideoAciklama = video.Description,
                    VideoImgSrc = video.img,
                    YildizSayisi = video.yildizortalamasi,
                    videoIde = video.Id,
                    videovideoaddress = video.videoaddress,
                    videoshortadress = video.videoshortadress,
                    videoauthor = video.Author,
                    VideoContentList = JsonConvert.DeserializeObject<List<string>>(video.VideoContentList) ,
                    IsVideoInUserList = IsVideoInUserList
                }
            };

            return View(BirlestirilmisViewModel1);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ara(string aramaifadesi)
        {
            var aramaifadesiLower = aramaifadesi.ToLower();

            var videolar = await _context.Videolar
                .Where(f => f.Name.ToLower().Contains(aramaifadesiLower))
                .ToListAsync();

            var Settings_Ayarlar = _context.Settings.ToList();

            var KategoriListGenel = _context.Kategoriler.ToList();
            var BirlestirilmisViewModel1 = new CompositeViewModel
            {
                Videolar = videolar,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel1);
        }
        public IActionResult Category(int id)
        {
           


            var KategoriListID = _context.Kategoriler
                               .Include(k => k.Videolar)
                               .Where(k => k.Id == id)
                               .ToList();

            var VideolarKategorisi = KategoriListID[0];

            var videolar = _context.Videolar.Where(f => f.CategoryId == id).ToList();

            var KategoriListGenel = _context.Kategoriler.ToList();

            if ((KategoriListID == null || !KategoriListID.Any()) && (KategoriListGenel == null || !KategoriListGenel.Any()))
            {
                return NotFound();
            }
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel1 = new CompositeViewModel
            {
                Videolar = videolar,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListID = KategoriListID,
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel1);



        }



            public async Task<IActionResult> SSS()
        {
            var Settings_Ayarlar = _context.Settings.ToList();
            var KategoriListGenel = _context.Kategoriler.ToList();

            var BirlestirilmisViewModel1 = new CompositeViewModel
            {
               
                CategoryViewModel = new CategoryViewModel
                {
            
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel1);

        }
        public async Task<IActionResult> Destek()
        {
            var Settings_Ayarlar = _context.Settings.ToList();
            var KategoriListGenel = _context.Kategoriler.ToList();

            var BirlestirilmisViewModel1 = new CompositeViewModel
            {

                CategoryViewModel = new CategoryViewModel
                {

                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel1);

        }
        public async Task<IActionResult> Help()
        {
            var Settings_Ayarlar = _context.Settings.ToList();
            var KategoriListGenel = _context.Kategoriler.ToList();

            var BirlestirilmisViewModel1 = new CompositeViewModel
            {

                CategoryViewModel = new CategoryViewModel
                {

                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel1);

        }

        public async Task<IActionResult> Index()
        {

            var kategoriler = _context.Kategoriler.ToList();


            var KategoriListGenel = _context.Kategoriler.ToList();


            var Settings_Ayarlar = _context.Settings.ToList();

            var Videolar = _context.Videolar.ToList();

            var menuler = _context.Menuler.ToList();

            var user = await _userManager.GetUserAsync(User);

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                Username = user?.UserName ?? "Anonim",
                Menuler = menuler,
                ApplicationUser = user != null ? new ApplicationUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = user.DateOfBirth,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    Address = user.Address,
                    City = user.City,
                    Country = user.Country,
                    PostalCode = user.PostalCode
                } : new ApplicationUser(),
                Videolar = Videolar,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar
                }
            };

            return View(BirlestirilmisViewModel);


        }
    }
}
