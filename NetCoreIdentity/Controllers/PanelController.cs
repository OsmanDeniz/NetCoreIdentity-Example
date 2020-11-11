using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Context;
using NetCoreIdentity.Models;

namespace NetCoreIdentity.Controllers
{
    [Authorize] // bir kullanicinin giris yaptigini bu attribute ile anlayabiliriz. Bunu kullanabilmek icinde 
    // startup.cs icerisinde bir middleware kullanilmalidir.  app.UseAuthentication(); app.UseAuthorization();
    public class PanelController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public PanelController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
        public async Task<IActionResult> UpdateUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateViewModel model = new UserUpdateViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                PictureURL = user.PictureURL
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (model.Picture != null)
                {
                    var extension = Path.GetExtension(model.Picture.FileName);
                    var picName = Guid.NewGuid() + extension;
                    string path = Directory.GetCurrentDirectory() + "/wwwroot/img/" + picName;
                    using var stream = new FileStream(path, FileMode.Create);
                    await model.Picture.CopyToAsync(stream);
                    user.PictureURL = picName;
                }
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

            }
            return View(model);
        }
        //Bir action'a herkesin erismesini istiyorsan [AllowAnonymous] yazmamiz yeterli
    }
}