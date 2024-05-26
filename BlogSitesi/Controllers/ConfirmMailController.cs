using BlogSitesi.Models;
using BlogSitesi.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSitesi.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        public ConfirmMailController(UserManager<Uye> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var value = TempData["Mail"];
            ViewBag.v = value;
            //  confirmMailViewModel.Mail = value.ToString();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailWM confirmMailViewModel)
        {
            var user = await _userManager.FindByEmailAsync(confirmMailViewModel.Mail);
            if (user.ConfirmCode == confirmMailViewModel.ConfirmCode)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
