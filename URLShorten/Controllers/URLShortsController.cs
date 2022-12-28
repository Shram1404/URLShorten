using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using URLShorten.Data;
using URLShorten.Models;

namespace URLShorten.Controllers
{
    public class URLShortsController : Controller
    {
        private readonly URLShortenContext _context;

        public URLShortsController(URLShortenContext context)
        {
            _context = context;
        }

        // GET: URLShorts
        public async Task<IActionResult> Index()
        {
              return _context.URLShort != null ? 
                          View(await _context.URLShort.ToListAsync()) :
                          Problem("Entity set 'URLShortenContext.URLShort'  is null.");
        }

        // GET: URLShorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.URLShort == null)
            {
                return NotFound();
            }

            var uRLShort = await _context.URLShort
                .FirstOrDefaultAsync(m => m.URLId == id);
            if (uRLShort == null)
            {
                return NotFound();
            }

            return View(uRLShort);
        }

        // GET: URLShorts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: URLShorts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("URLId,CreatedDate,FullURL,ShortURL,CreatedBy")] URLShort uRLShort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uRLShort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uRLShort);
        }

        // GET: URLShorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.URLShort == null)
            {
                return NotFound();
            }

            var uRLShort = await _context.URLShort.FindAsync(id);
            if (uRLShort == null)
            {
                return NotFound();
            }
            return View(uRLShort);
        }

        // POST: URLShorts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("URLId,CreatedDate,FullURL,ShortURL,CreatedBy")] URLShort uRLShort)
        {
            if (id != uRLShort.URLId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uRLShort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!URLShortExists(uRLShort.URLId))
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
            return View(uRLShort);
        }

        // GET: URLShorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.URLShort == null)
            {
                return NotFound();
            }

            var uRLShort = await _context.URLShort
                .FirstOrDefaultAsync(m => m.URLId == id);
            if (uRLShort == null)
            {
                return NotFound();
            }

            return View(uRLShort);
        }

        // POST: URLShorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.URLShort == null)
            {
                return Problem("Entity set 'URLShortenContext.URLShort'  is null.");
            }
            var uRLShort = await _context.URLShort.FindAsync(id);
            if (uRLShort != null)
            {
                _context.URLShort.Remove(uRLShort);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool URLShortExists(int id)
        {
          return (_context.URLShort?.Any(e => e.URLId == id)).GetValueOrDefault();
        }
    }
}
