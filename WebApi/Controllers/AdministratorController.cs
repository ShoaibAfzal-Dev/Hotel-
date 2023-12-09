using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private MyDBContext _dBContext;
        public AdministratorController(MyDBContext dBContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager
            )
        {
            _dBContext = dBContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/CreateRole")]
        public async Task<IActionResult> createrole(Role role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole ir = new IdentityRole
                {
                    Name = role.RoleName
                };
                IdentityResult result = await _roleManager.
                    CreateAsync(ir);
                if (result.Succeeded)
                {
                    return Ok(role);
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            return BadRequest("Error");
        }
        [HttpGet]
        [Route("/getroles")]
        public async Task<IActionResult> Getrole()
        {
            var rs = _roleManager.Roles.ToList();
            return View(rs);
        }
        [HttpPost]
        [Route("/ToggleRole")]
        public async Task<IActionResult> Toggle(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            { 
                return BadRequest("Error");
            }
            var isAdmin = await _userManager.IsInRoleAsync(user, "admin");
            if (isAdmin)
            { 
                await _userManager.RemoveFromRoleAsync(user, "admin");
            }
            else if (await _userManager.IsInRoleAsync(user, "manager")) {
                await _userManager.RemoveFromRoleAsync(user, "manager");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "admin");
            }
            return Ok();
        }
    }
}
