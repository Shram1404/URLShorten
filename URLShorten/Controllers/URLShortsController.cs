using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(URLShort uRL)
        {

            if (IsValidUrl(uRL.FullURL))
            {
                bool isDuplicate = await _context.URLShort.AnyAsync(x => x.FullURL == uRL.FullURL);
                if (!isDuplicate)
                {

                    string tempShortURL = CreateShortURL();
                    while (_context.URLShort.Any(x => x.ShortURL == tempShortURL))
                    {
                        tempShortURL = CreateShortURL();
                    }

                    uRL.ShortURL = tempShortURL;
                    uRL.CreatedBy = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                    if (ModelState.IsValid)
                    {
                        if (uRL.CreatedDate.Equals(DateTime.MinValue))
                            uRL.CreatedDate = DateTime.Now;

                        _context.URLShort.Add(uRL);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return View(uRL);
                    }
                }
                else
                {
                    ModelState.AddModelError("FullURL", "This URL is already in the database");
                    return View(uRL);
                }
            }
            else
            {
                ModelState.AddModelError("FullURL", "Incorrect URL");
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewShortLink(string tempShortURL)
        {
            var shortLink = _context.URLShort.FirstOrDefault(s => s.ShortURL == tempShortURL);

            if (shortLink == null)
            {
                return NotFound();
            }
            return Redirect(shortLink.FullURL);
        }

        private bool IsValidUrl(string url)
        {
            var regex = new Regex(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$");
            return regex.IsMatch(url);
        }

        private string CreateShortURL()
        {
            Random rnd = new Random();
            int j;
            string tempUrl = "";

            for (int i = 0; i < 6; i++)
            {
                j = rnd.Next(0, 35);

                if (j < 10)
                    j += 48;
                else
                    j += 87;
                tempUrl = tempUrl + char.ConvertFromUtf32(j);
            }
            string tempFullURL = Url.Action("", "", new { s = tempUrl }, Request.Scheme);
            return tempFullURL;
        }

        // GET: URLs
        public async Task<IActionResult> Table()
        {
            return _context.URLShort != null ?
                        View(await _context.URLShort.ToListAsync()) :
                        Problem("Entity set 'URLShorterContext.URL'  is null.");
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
