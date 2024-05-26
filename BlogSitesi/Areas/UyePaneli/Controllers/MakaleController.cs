using BlogSitesi.DAL;
using BlogSitesi.Models;
using BlogSitesi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogSitesi.Areas.UyePaneli.Controllers
{
    [Area("UyePaneli")]
    [Authorize(Roles = "Uye")]
    public class MakaleController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        private readonly BlogDBContext _context;
        public MakaleController(UserManager<Uye> userManager, BlogDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
           

            MakaleVM vm = new MakaleVM();    
            vm.mlist=_context.Makale.Include(x=>x.Uye).ToList();



            return View(vm);


        }
        public int GetUserID()
        {
            return int.Parse(_userManager.GetUserId(User));
        }

        public IActionResult Create()
        {
            MakaleVM vm = new MakaleVM();
            vm.Konu = new SelectList(_context.Konu.ToList(), "KonuID", "KonuBaslik");
            
            return View(vm);
        }

        // POST: YonetimPaneli/Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(MakaleEkle makaleEkle)
        {


            if (ModelState.IsValid)
            {
                var id =GetUserID();
                var uye = _userManager.Users.First(x => x.Id == id);


               
                Makale makale = new Makale();
                
                makale.OrtalamaSure = makaleEkle.OrtalamaSure;
                makale.OkunmaSayisi = makaleEkle.OkunmaSayisi;
                makale.Tarih=DateTime.Now;
                makale.Metin= makaleEkle.Metin;
                makale.Baslık = makaleEkle.Baslık;
                makale.UyeID=uye.Id;
              
                _context.Add(makale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                MakaleVM vM = new MakaleVM();
                vM.Konu = new SelectList(_context.Konu.ToList(), "KonuID", "KonuBaslik");

                return View(vM);
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makale = await _context.Makale
               .FirstOrDefaultAsync(m => m.MakaleID == id);
            if (makale == null)
            {
                return NotFound();
            }

            return View(makale);
        }

        //GET: UyePaneli/Makale/IlgiliMakaleler
        public async Task<IActionResult> IlgiliMakaleler()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (user.UyeKonus == null)
            {
                user.UyeKonus = new List<UyeKonu>();
            }

            var userKonuIds = user.UyeKonus.Select(uk => uk.KonuID).ToList();

            var makaleler = await _context.Makale
                .Include(m => m.MakaleKonus)
                    .ThenInclude(mk => mk.Konu)
                .Where(m => m.MakaleKonus.Any(mk => userKonuIds.Contains(mk.KonuID)) || m.UyeID == user.Id)
                .ToListAsync();

            return View(makaleler);
        }
    }
}
