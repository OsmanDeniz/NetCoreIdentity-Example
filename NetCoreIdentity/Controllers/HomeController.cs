using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                var identityResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (identityResult.Succeeded) {
                    return RedirectToAction("Index", "Panel");
                }
                ModelState.AddModelError("","Kullanici Adi veya Sifre hatali");
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