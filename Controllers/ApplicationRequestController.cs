using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UdemyEgitimPlatformu.Data;
using UdemyEgitimPlatformu.Services;
using UdemyEgitimPlatformu.ViewModel;

namespace UdemyEgitimPlatformu.Controllers
{
    public class ApplicationRequestController : Controller
    {

        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ApplicationRequestController(IBackgroundTaskQueue taskQueue, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IEmailService emailService)
        {
            _taskQueue = taskQueue;

            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Create()
        {


            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
               
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CompositeViewModel model)
        {
            var applicationRequestErrors = ModelState
             .Where(x => x.Key.StartsWith(nameof(model.RequestType)) || x.Key.StartsWith(nameof(model.Comments)))
             .SelectMany(x => x.Value.Errors)
             .ToList();
            var user = await _userManager.GetUserAsync(User);

            if (!applicationRequestErrors.Any()) {

                var existingRequest = _context.ApplicationRequests.FirstOrDefault(r => r.UserId == user.Id && !r.IsApproved);

                if (existingRequest != null)
                {
                    TempData["ErrorMessage"] = "Zaten başvuruda bulundunuz ve başvurunuz henüz onaylanmadı.";
                    return RedirectToAction("Index", "Home");
                }

                try
                {

                    var request = new ApplicationRequest
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        RequestType = model.RequestType,
                        RequestDate = DateTime.Now,
                        IsApproved = false,
                        Comments = model.Comments
                    };

                    _context.ApplicationRequests.Add(request);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Başvurunuz başarıyla alındı.";
                    return RedirectToAction("Index","Home");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Başvuru sırasında bir hata oluştu: {ex.Message}";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Başvuru formunda hatalar var. Lütfen tüm alanları doğru doldurduğunuzdan emin olun.";
            }




            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                ApplicationRequest = model.ApplicationRequest,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);
           
        }
    

    [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var requests = _context.ApplicationRequests.ToList();

            var KategoriListGenel = _context.Kategoriler.ToList();
            var Settings_Ayarlar = _context.Settings.ToList();

            var BirlestirilmisViewModel = new CompositeViewModel
            {
                ApplicationRequest = requests,
                CategoryViewModel = new CategoryViewModel
                {
                    KategoriListGenel = KategoriListGenel,
                    Ayarlar = Settings_Ayarlar,
                }
            };

            return View(BirlestirilmisViewModel);


        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = _context.ApplicationRequests.FirstOrDefault(r => r.Id == id);
            if (request != null)
            {
                request.IsApproved = true;

                // Kullanıcıyı İçerik Üreticisi Rolüne Ekle
                var user = await _userManager.FindByIdAsync(request.UserId);
                await _userManager.AddToRoleAsync(user, "ContentCreator");

                _context.ApplicationRequests.Update(request);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
