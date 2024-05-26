using BlogSitesi.Models.ViewModels;
using BlogSitesi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BlogSitesi.Controllers
{
    public class LoginController : Controller
    {


        private readonly SignInManager<Uye> _signInManager;
        private readonly UserManager<Uye> _userManager;

        public LoginController(SignInManager<Uye> signInManager, UserManager<Uye> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login_VM login)
        {
            var uye = _userManager.FindByEmailAsync(login.EMail).Result;

            if (uye == null)
            {
                //eposta kontrolu...
                ModelState.AddModelError("Hata", "Kullanıcı adı veya şifre yanlış...");
                return View();
            }

            if (!_userManager.CheckPasswordAsync(uye, login.Password).Result)
            {

                //sifre kontrolu...
                ModelState.AddModelError("Hata", "Kullanıcı adı veya şifre yanlış...");
                return View();
            }
            if (uye.EmailConfirmed!=true)
            {
                ModelState.AddModelError("Hata", "Uyelik onaylı degil");
                return RedirectToAction("Index", "ConfirmMail");
            }

            await _signInManager.SignInAsync(uye, false);

            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            //return Redirect("~/Home/Index");
            //return LocalRedirect("~/localhost:5168/Home/Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register_VM register)
        {
            
            Random random= new Random();
            int code;
            code = random.Next(100000, 1000000);

            Uye uye = new Uye();
            uye.Adres = register.Address;
            uye.UserName = register.EMail;
            uye.Email = register.EMail;
            uye.Ad = register.Ad;
            uye.Soyad = register.Soyad;
            uye.ConfirmCode = code;

            uye.PasswordHash = _userManager.PasswordHasher.HashPassword(uye, register.Password);
            var result = await _userManager.CreateAsync(uye);

            if (result.Succeeded)
            {
                MimeMessage mineMessage = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin","borayildirim222@gmail.com");
                MailboxAddress mailboxAddressTo = new MailboxAddress("Uye", uye.Email);

                mineMessage.From.Add(mailboxAddressFrom);
                mineMessage.To.Add(mailboxAddressTo);

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Kayıt işlemini gerçekleştirmek için onay kodunuz : " + code;
                mineMessage.Body=bodyBuilder.ToMessageBody();
                mineMessage.Subject = "Onay Kodu";

                 SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("borayildirim222@gmail.com", "ntxl drqm fizs rxsp");
                client.Send(mineMessage);
                client.Disconnect(true);

                TempData["Mail"] = register.EMail;

                await _userManager.AddToRoleAsync(uye, "Uye");
                return RedirectToAction("Index", "ConfirmMail");
            }

            return View();


        }
    }
}
