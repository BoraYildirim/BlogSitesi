using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogSitesi.DAL;
using BlogSitesi.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogSitesi.Areas.UyePaneli.Controllers
{
    [Area("UyePaneli")]
    public class KonusController : Controller
    {
        private readonly BlogDBContext _context;
        private readonly UserManager<Uye> _userManager;
        public KonusController(UserManager<Uye> userManager, BlogDBContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UyePaneli/Konus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Konu.ToListAsync());
        }

        // GET: UyePaneli/Konus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konu
                .FirstOrDefaultAsync(m => m.KonuID == id);
            if (konu == null)
            {
                return NotFound();
            }

            return View(konu);
        }

        // GET: UyePaneli/Konus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UyePaneli/Konus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KonuID,KonuBaslik")] Konu konu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(konu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(konu);
        }

        // GET: UyePaneli/Konus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konu.FindAsync(id);
            if (konu == null)
            {
                return NotFound();
            }
            return View(konu);
        }

        // POST: UyePaneli/Konus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KonuID,KonuBaslik")] Konu konu)
        {
            if (id != konu.KonuID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(konu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KonuExists(konu.KonuID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(konu);
        }

        // GET: UyePaneli/Konus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var konu = await _context.Konu
                .FirstOrDefaultAsync(m => m.KonuID == id);
            if (konu == null)
            {
                return NotFound();
            }

            return View(konu);
        }

        // POST: UyePaneli/Konus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var konu = await _context.Konu.FindAsync(id);
            if (konu != null)
            {
                _context.Konu.Remove(konu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KonuExists(int id)
        {
            return _context.Konu.Any(e => e.KonuID == id);
        }

        // GET: UyePaneli/Konus/AddUserTopics
        public async Task<IActionResult> AddUserTopics()
        {
            var topics = await _context.Konu.ToListAsync();
            return View(topics);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserTopics(int[] selectedTopics)
        {
            if (selectedTopics == null || selectedTopics.Length == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // UyeKonus koleksiyonunu başlat
            if (user.UyeKonus == null)
            {
                user.UyeKonus = new List<UyeKonu>();
            }

            foreach (var topicId in selectedTopics)
            {
                var topic = await _context.Konu.FindAsync(topicId);
                if (topic != null && !user.UyeKonus.Any(ut => ut.KonuID == topicId))
                {
                    user.UyeKonus.Add(new UyeKonu { KonuID = topicId ,UyeID=user.Id});
                }
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
