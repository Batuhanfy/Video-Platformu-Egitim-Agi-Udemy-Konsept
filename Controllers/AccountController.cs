using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Models;
using UdemyEgitimPlatformu.ViewModel;
using UdemyEgitimPlatformu.ViewModels;
using UdemyEgitimPlatformu.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UdemyEgitimPlatformu.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(IBackgroundTaskQueue taskQueue,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IEmailService emailService)
        {
            _taskQueue = taskQueue;

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CompositeViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RegisterViewModel.Password) || string.IsNullOrEmpty(model.RegisterViewModel.Email))
            {
              
                ModelState.AddModelError(string.Empty, "Tüm alanları doldurunuz.");
                TempData["ErrorMessage"] = "Tüm alanları doldurunuz.";

                var KategoriListGenel1 = _context.Kategoriler.ToList();
                var Settings_Ayarlar1 = _context.Settings.ToList();

                var BirlestirilmisViewModel1 = new CompositeViewModel
                {
                    RegisterViewModel = model.RegisterViewModel,
                    CategoryViewModel = new CategoryViewModel
                    {
                        KategoriListGenel = KategoriListGenel1,
                        Ayarlar = Settings_Ayarlar1,
                    }
                };

                return View(BirlestirilmisViewModel1);
            }

            var user = new ApplicationUser { UserName = model.RegisterViewModel.Email, Email = model.RegisterViewModel.Email };
            var result = await _userManager.CreateAsync(user, model.RegisterViewModel.Password);

            if (result.Succeeded)
            {

                _taskQueue.QueueBackgroundWorkItem(async token =>
                {
                    var emailSubject = "Hoşgeldiniz!";
                    var emailBody = "Kayıt olduğunuz için teşekkürler!";
                    await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
                });




                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["SuccessMessage"] = "Kayıt Başarılı.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["ErrorMessage"] = "Hata! Kayıt işlemi sırasında bir problemle karşılaşıldı.";
            }

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                RegisterViewModel = model.RegisterViewModel,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Profil()
        {
          
            TempData["ErrorMessage"] = null;

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var user = await _userManager.GetUserAsync(User);
          


            var BirlestirilmisViewModel = new CompositeViewModel
            {
                Username = user.UserName,
                ApplicationUser = new ApplicationUser
                {
                    FirstName=user.FirstName,
                    LastName=user.LastName, 
                    DateOfBirth=user.DateOfBirth,
                    ProfilePictureUrl=user.ProfilePictureUrl,
                    Address=user.Address,
                    City=user.City,
                    Country=user.Country,
                    PostalCode=user.PostalCode 
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
        public async Task<IActionResult> Profil(CompositeViewModel model, IFormFile ProfilePicture)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            if (ProfilePicture != null && ProfilePicture.Length > 0)
            {
                var fileName = Path.GetFileName(ProfilePicture.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile_pictures", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePictureUrl = $"/images/profile_pictures/{fileName}";
            }else
            {
                user.ProfilePictureUrl = "";
            }


            var settings = await _context.Settings.ToListAsync();
            var kategoriListGenel = await _context.Kategoriler.ToListAsync();

            user.FirstName = model.ApplicationUser.FirstName ?? user.FirstName;
            user.LastName = model.ApplicationUser.LastName ?? user.LastName;
            user.DateOfBirth = model.ApplicationUser.DateOfBirth ?? user.DateOfBirth;
            user.Address = model.ApplicationUser.Address ?? user.Address;
            user.City = model.ApplicationUser.City ?? user.City;
            user.Country = model.ApplicationUser.Country ?? user.Country;
            user.PostalCode = model.ApplicationUser.PostalCode ?? user.PostalCode;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Başarıyla Güncellendi.";

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                TempData["ErrorMessage"] = "Güncellenemedi.";
            }

            var updatedUser = await _userManager.FindByNameAsync(model.Username);

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                Username = updatedUser.UserName,
                ApplicationUser = new ApplicationUser
                {
                    FirstName = updatedUser.FirstName,
                    LastName = updatedUser.LastName,
                    DateOfBirth = updatedUser.DateOfBirth,
                    ProfilePictureUrl = updatedUser.ProfilePictureUrl,
                    Address = updatedUser.Address,
                    City = updatedUser.City,
                    Country = updatedUser.Country,
                    PostalCode = updatedUser.PostalCode
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



        [HttpGet]
        public IActionResult Register()
        {
            TempData["SuccessMessage"] = null;
            TempData["ErrorMessage"] = null;

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                RegisterViewModel = new RegisterViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            TempData["SuccessMessage"] = null;
            TempData["ErrorMessage"] = null;

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                LoginViewModel = new LoginViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Videolarim()
        {
            TempData["SuccessMessage"] = null;
            TempData["ErrorMessage"] = null;

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            var username = user?.UserName;
            var alinanVideolarList = _context.AlinanVideolar.Where(av => av.Username == username).ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                LoginViewModel = new LoginViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                },
                AlinanVideolarList = alinanVideolarList
            };

            return View(BirlestirilmisViewModel);
        }

        public async Task<IActionResult> VideoPlay(int id)
        {
         
            var video = _context.Videolar.FirstOrDefault(v => v.Id == id);

            if (video == null)
            {
                return NotFound("Video bulunamadı.");
            }


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            var username = user?.UserName;

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var alinanVideolarList = _context.AlinanVideolar.Where(av => av.Username == username).ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                Videolar = new List<Videolar> { video },
                LoginViewModel = new LoginViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                },
                AlinanVideolarList = alinanVideolarList
            };

            return View(BirlestirilmisViewModel);


        }

        [HttpPost]
        public async Task<IActionResult> VideoyuEkle(int videoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            var username = user?.UserName;


            var video_var_mi = _context.AlinanVideolar.FirstOrDefault(g=> g.VideoId == videoId && g.Username == username);
        


            var video = _context.Videolar.FirstOrDefault(v => v.Id == videoId);
            var videoName = video?.Name;


            var alinanVideo = new AlinanVideolar
            {
                Username = username,
                VideoId = videoId,

                VideoName = videoName,
                Tarih = DateTime.Now,
                SonTarih = DateTime.Now.AddMonths(1) // Default olarak 1 aylık süre belirledim

            };

            if (video_var_mi != null)
            {
                TempData["ErrorMessage"] = "Bu video zaten sizde var.";
            }
            else
            {
                try
                {
                    _context.AlinanVideolar.Add(alinanVideo);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Video başarıyla eklendi.";
                    TempData["VideoName"] = videoName;

                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                }
            }

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var alinanVideolarList = _context.AlinanVideolar.Where(av => av.Username == username).ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                LoginViewModel = new LoginViewModel(),
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                },
                AlinanVideolarList = alinanVideolarList 
            };

            return View(BirlestirilmisViewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(CompositeViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.LoginViewModel.Password) || string.IsNullOrEmpty(model.LoginViewModel.Email))
            {

                ModelState.AddModelError(string.Empty, "Tüm alanları doldurunuz.");
                TempData["ErrorMessage"] = "Tüm alanları doldurunuz.";

                var KategoriListGenel1 = _context.Kategoriler.ToList();
                var Settings_Ayarlar1 = _context.Settings.ToList();

                var BirlestirilmisViewModel1 = new CompositeViewModel
                {
                    LoginViewModel = model.LoginViewModel,
                    CategoryViewModel = new CategoryViewModel
                    {
                        KategoriListGenel = KategoriListGenel1,
                        Ayarlar = Settings_Ayarlar1,
                    }
                };

                return View(BirlestirilmisViewModel1);
            }



            var result = await _signInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var subject = "Giriş Başarılı";
                var message = $"Merhaba {model.LoginViewModel.Email}, giriş işleminiz başarıyla gerçekleşti.";

                return RedirectToAction("Index", "Home");
                TempData["SuccessMessage"] = "Kayıt Başarılı.";
            }
            else
            {
              
                TempData["ErrorMessage"] = "Hata! Tekrar Deneyiniz.";

            }



            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                LoginViewModel = model.LoginViewModel,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }
    }
}
