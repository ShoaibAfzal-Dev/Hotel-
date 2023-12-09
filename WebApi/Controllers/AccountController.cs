using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{
    public class AccountController : Controller
    {
     /*   private MyDBContext _dbContext;*/
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public AccountController(
            /*MyDBContext dbContext,*/
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager
            )
        {

       /*     _dbContext = dbContext;*/
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("/registration")]
        public async Task<IActionResult> Registration([FromBody] Registeration model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    /*Password=model.Password,*/
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var defaultrole = roleManager.FindByNameAsync("User").Result;
                    if (defaultrole != null)
                    {
                        await userManager.AddToRoleAsync(user, defaultrole.Name);
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(user.Email);
                }
                foreach (var item in result.Errors)
                {
                    return BadRequest(item.Code);
                }

            }
            return BadRequest("Error");
        }

        public async Task<IActionResult> login([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return Ok(model.Email);
                }
            }
            return BadRequest("Invalid Login Attempt");
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {

            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var roles = await userManager.GetRolesAsync(user);
                    string singleRole = roles.FirstOrDefault(); 

                    var response = new
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Roles = singleRole
                    };

                    return Ok(response);
                }
                return BadRequest("Invalid Email or Password");
            }
            catch
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
        /*[HttpPost]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                var user = await userManager.FindByEmailAsync(model.Email);

                if (result.Succeeded)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token, Email = user.Email });
                }

                return Unauthorized("Invalid Login Attempt");
            }

            return BadRequest("Invalid Model");
        }*/
        [HttpGet]
        [Route("/profile")]
        public async Task<IActionResult> profile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest("Error");
            }
            return View(user);
        }

    }
}