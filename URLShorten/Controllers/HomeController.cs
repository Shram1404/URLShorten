using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using URLShorten.Data;
using URLShorten.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace URLShorter.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly URLShortenContext _context;

        public HomeController(ILogger<HomeController> logger, URLShortenContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                string fullURL = _context.URLShort.FirstOrDefault(x => x.ShortURL.Substring(x.ShortURL.Length - 6) == s)?.FullURL;

                if (!String.IsNullOrEmpty(fullURL))
                    return RedirectPermanent(fullURL);

                return View();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}