using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;
namespace Webprj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public RoleController(RoleManager<IdentityRole<int>> roleManager ) => _roleManager = roleManager;
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        // thêm quyền
        public IActionResult Create() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ( IdentityRole<int> role )
        {
            if ( !ModelState.IsValid ) return View(role);
           var ans = await _roleManager.CreateAsync(role);
            if (ans.Succeeded) return RedirectToAction(nameof(Index));
            foreach ( var err in ans.Errors ) ModelState.AddModelError(string.Empty , err.Description);
            return View(role);
        }
        // sửa quyền
        public async Task<IActionResult> Edit( int id )
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int id , IdentityRole<int> role )
        {
            if (id != role.Id) return BadRequest();
            if (!ModelState.IsValid) return View(role);
            var exist = await _roleManager.FindByIdAsync(id.ToString());
            if (exist== null) return NotFound();
            exist.Name = role.Name;
            var ans = await _roleManager.UpdateAsync(exist);
            if ( ans.Succeeded) return RedirectToAction(nameof(Index));
            foreach ( var err in ans.Errors ) ModelState.AddModelError(string.Empty, err.Description);
            return View(role);
        }
        // xóa quyền
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete( int id )
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
