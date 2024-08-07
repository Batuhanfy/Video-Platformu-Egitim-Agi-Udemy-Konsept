using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UdemyEgitimPlatformu.ViewModels;
using Newtonsoft.Json;

namespace UdemyEgitimPlatformu.Controllers
{
    [Authorize(Roles = "Admin,ContentCreator")]
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VideoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Create()
        {

            var categories = _context.Kategoriler.Where(c => !c.IsDeleted).ToList();


            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();


            var BirlestirilmisViewModel = new CompositeViewModel
            {
                VideoUploadViewModel = new VideoUploadViewModel
                {
                    Categories = categories
                },
                RegisterViewModel = new RegisterViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompositeViewModel model)
        {
            var videoUploadViewModelErrors = ModelState
                .Where(x => x.Key.StartsWith("VideoUploadViewModel.") && x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .ToList();

        

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var video = new Videolar
                {
                    Name = model.VideoUploadViewModel.Name,
                    Description = model.VideoUploadViewModel.Description,
                    Author = user.UserName,
                    CategoryId = model.VideoUploadViewModel.CategoryId,
                    yildizortalamasi = 5,
                    songuncelleme = DateTime.Now,
                    VideoContentList = JsonConvert.SerializeObject(model.VideoUploadViewModel.VideoContentList) // JSON formatında sakla
                };

                if (model.VideoUploadViewModel.VideoFile != null)
                {
                    string videoPath = Path.Combine(_hostEnvironment.WebRootPath, "videos");
                    if (!Directory.Exists(videoPath))
                    {
                        Directory.CreateDirectory(videoPath);
                    }
                    string videoFileName = Path.GetFileNameWithoutExtension(model.VideoUploadViewModel.VideoFile.FileName);
                    string videoFileExtension = Path.GetExtension(model.VideoUploadViewModel.VideoFile.FileName);
                    string newVideoFileName = $"{videoFileName}_{DateTime.Now:yyyyMMddHHmmss}{videoFileExtension}";
                    string fullVideoPath = Path.Combine(videoPath, newVideoFileName);

                    using (var stream = new FileStream(fullVideoPath, FileMode.Create))
                    {
                        await model.VideoUploadViewModel.VideoFile.CopyToAsync(stream);
                    }
                    video.videoaddress = $"{newVideoFileName}";
                }

                if (model.VideoUploadViewModel.ShortVideoFile != null)
                {
                    string shortVideoPath = Path.Combine(_hostEnvironment.WebRootPath, "videos-short");
                    if (!Directory.Exists(shortVideoPath))
                    {
                        Directory.CreateDirectory(shortVideoPath);
                    }
                    string shortVideoFileName = Path.GetFileNameWithoutExtension(model.VideoUploadViewModel.ShortVideoFile.FileName);
                    string shortVideoFileExtension = Path.GetExtension(model.VideoUploadViewModel.ShortVideoFile.FileName);
                    string newShortVideoFileName = $"{shortVideoFileName}_{DateTime.Now:yyyyMMddHHmmss}{shortVideoFileExtension}";
                    string fullShortVideoPath = Path.Combine(shortVideoPath, newShortVideoFileName);

                    using (var stream = new FileStream(fullShortVideoPath, FileMode.Create))
                    {
                        await model.VideoUploadViewModel.ShortVideoFile.CopyToAsync(stream);
                    }
                    video.videoshortadress = $"{newShortVideoFileName}";
                }

                if (model.VideoUploadViewModel.CoverImageFile != null)
                {
                    string coverImagePath = Path.Combine(_hostEnvironment.WebRootPath, "VideosImage");
                    if (!Directory.Exists(coverImagePath))
                    {
                        Directory.CreateDirectory(coverImagePath);
                    }
                    string coverImageFileName = Path.GetFileNameWithoutExtension(model.VideoUploadViewModel.CoverImageFile.FileName);
                    string coverImageFileExtension = Path.GetExtension(model.VideoUploadViewModel.CoverImageFile.FileName);
                    string newCoverImageFileName = $"{coverImageFileName}_{DateTime.Now:yyyyMMddHHmmss}{coverImageFileExtension}";
                    string fullCoverImagePath = Path.Combine(coverImagePath, newCoverImageFileName);

                    using (var stream = new FileStream(fullCoverImagePath, FileMode.Create))
                    {
                        await model.VideoUploadViewModel.CoverImageFile.CopyToAsync(stream);
                    }
                    video.img = $"{newCoverImageFileName}";
                }

                _context.Videolar.Add(video);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Video başarıyla yüklendi.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Video yüklenirken bir hata oluştu: {ex.Message}";
            }

            model.VideoUploadViewModel.Categories = _context.Kategoriler.Where(c => !c.IsDeleted).ToList();
            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                VideoUploadViewModel = model.VideoUploadViewModel,
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
