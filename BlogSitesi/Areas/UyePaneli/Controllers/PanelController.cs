using BlogSitesi.DAL;
using BlogSitesi.Models;
using BlogSitesi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSitesi.Areas.UyePaneli.Controllers
{
    [Area("UyePaneli")]
    [Authorize(Roles = "Uye")]
    public class PanelController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        private readonly BlogDBContext _context;
        public PanelController(UserManager<Uye> userManager,BlogDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }



        public IActionResult Index()
        {
            ViewBag.ID = GetUserID();
            return View();

        }

        public async Task<IActionResult> Details()
        {
            int id = GetUserID();
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }




        public int GetUserID()
        {           
            return int.Parse(_userManager.GetUserId(User));
        }


        public async Task<IActionResult> Edit()
        {
            Uye_VM uyeVm =new Uye_VM();
            
            int id = GetUserID();
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Users.FindAsync(id);
            if (uye == null)
            {
                return NotFound();
            }
            uyeVm.Uye = uye;



            return View(uyeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Uye_VM uyeVm)
        {
          
            if (id != uyeVm.Uye.Id)
            {
                return NotFound();
            }

          
                try
                {
                    var uye = await _context.Users.FindAsync(id);


                    uye.Ad = uyeVm.Uye.Ad;
                    uye.Soyad = uyeVm.Uye.Soyad;
                    uye.Adres = uyeVm.Uye.Adres;
                    uye.Acıklama = uyeVm.Uye.Acıklama;

                    Guid guid = Guid.NewGuid();
                    string dosyaAdi = guid.ToString();
                if (uyeVm.ResimYolu!=null)
                {
                    dosyaAdi += uyeVm.ResimYolu.FileName;
                    string dosyaYolu = "wwwroot/img/";
                    uye.Resim = dosyaAdi;
                    FileStream fs = new FileStream(dosyaYolu + dosyaAdi, FileMode.Create);
                    uyeVm.ResimYolu.CopyTo(fs);
                    fs.Close();
                }
                   
                   

                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Details));
            
            return View(uyeVm);
        }
    }
}
