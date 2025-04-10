using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Webprj.Models;
using Microsoft.AspNetCore.Authorization;

namespace Webprj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<Customer> _userManager;

        public UserController( UserManager<Customer> userManager )
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<(Customer, IList<string>)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add((user, roles));
            }

            return View(userRoles);
        }

        public async Task<IActionResult> Promote( string id )
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!await _userManager.IsInRoleAsync(user , "Admin"))
                await _userManager.AddToRoleAsync(user , "Admin");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Demote( string id )
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (await _userManager.IsInRoleAsync(user , "Admin"))
                await _userManager.RemoveFromRoleAsync(user , "Admin");

            return RedirectToAction("Index");
        }
    }
}
