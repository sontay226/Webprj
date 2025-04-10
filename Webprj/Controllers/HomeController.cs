using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Test2WebContext _context;

        public HomeController ( ILogger<HomeController> logger , Test2WebContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index ()
        {
            var products = await _context.Products.ToListAsync();
            return View (products);
        }
        public IActionResult Page2()
        {
            return View ();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [ResponseCache (Duration = 0 , Location = ResponseCacheLocation.None , NoStore = true)]
        public IActionResult Error ()
        {
            return View (new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
