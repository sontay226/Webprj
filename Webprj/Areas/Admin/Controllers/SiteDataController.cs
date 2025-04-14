using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;
namespace Webprj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SiteDataController : Controller 
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public SiteDataController( RoleManager<IdentityRole<int>> roleManager ) => _roleManager = roleManager;
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
    }
}
