using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Context;
using NetCoreIdentity.Models;

namespace NetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        // Dependency Injection icin 
        // UserManager Identity icerisinde yardimci olacak bir sinif
        private readonly UserManager<AppUser> _userManager;
        //Kullanici girisi icin kullaniliyor.
        private readonly SignInManager<AppUser> _signInManager;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index()
        {
            return View(new UserSignInViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(UserSignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                // en sonda bulunan true 5 kere yanlis giriste hesabi kilitleme islemi icin. Bir onceki cookie ile ilgili durumdur.
                var identityResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);

                if (identityResult.IsLockedOut)
                {
                    var remaindLockOutTime = await _userManager.GetLockoutEndDateAsync(await _userManager.FindByNameAsync(model.Username));
                    var blockedTime = remaindLockOutTime.Value;
                    var remaindTime = blockedTime.Minute - DateTime.Now.Minute;

                    ModelState.AddModelError("", "Hesap kilitlenmistir. Bir yanlislik oldugunu dusunuyorsaniz admin ile iletisime geciniz.");
                    ModelState.AddModelError("", $"Kalan Sure {remaindTime} dakikadir.");
                    return View("Index", model);
                }

                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Index", "Panel");
                }
                var LogInCount = await _userManager.GetAccessFailedCountAsync(await _userManager.FindByNameAsync(model.Username));
                ModelState.AddModelError("", $"Kullanici Adi veya Sifre hatali, kalan giris sayisi {5 - LogInCount}");
            }
            return View("Index", model);
        }



        public IActionResult Register()
        {
            return View(new UserRegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Name = viewModel.Name,
                    Surname = viewModel.Surname,
                    Email = viewModel.Email,
                    UserName = viewModel.Username
                };
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(viewModel);
        }
    }
}