using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace MVC.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly HttpClient _httpClient;
        // Add your url here
        private string BaseURL = "";

        public AccountController1()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseURL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Registeration model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/registration", model);
                var d=response.Content.ReadAsStringAsync();
                ViewBag.user=response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserSession", d.Result);
                    ViewBag.email = d.Result;
                    return RedirectToAction("user","user");
                }
                else
                {
                    ViewBag.error = d.Result;
                    return View();
                }
            }
            return View("Error");
        }
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login(Login model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/login", model);
                var d = response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var ussd =await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var userSession = System.Text.Json.JsonSerializer.Deserialize<userdata>(ussd,options);
                    HttpContext.Session.SetString("UserRole", userSession.Roles);
                    HttpContext.Session.SetString("UserSession", userSession.Email);
                    HttpContext.Session.SetString("UserId",userSession.UserId);
                    return RedirectToAction("user", "user");
                }
                else
                {
                    ViewBag.error = d.Result;
                    return View();
                }
            }
            return View("Error");
        }
        public async Task<IActionResult> logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null) {
                HttpContext.Session.Remove("UserSession");
                HttpContext.Session.Remove("UserRole");

                return RedirectToAction("login");
            }
            return View();
        }
    }
}
