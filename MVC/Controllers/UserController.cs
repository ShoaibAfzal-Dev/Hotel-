using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using MVC.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly object window;
        // Add your url here
        private string BaseURL = "";
        public UserController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseURL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> User()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("getallData");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var productList = System.Text.Json.JsonSerializer.Deserialize<List<Productdata>>(data, options);
                return View(productList);
            }
            else
            {
                return Ok("No data Exist");
            }
        }
        public async Task<IActionResult> Singledata(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/getsingle?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var product = System.Text.Json.JsonSerializer.Deserialize<List<Productdata>>(data, options);
                return View(product);
            }
            else
            {
                return Ok("No data Exist");
            }
        }
        [HttpGet]
        public async Task<IActionResult> getwish()
        {
            if (HttpContext.Session.GetString("UserRole") !=null)
            {
                var id = HttpContext.Session.GetString("UserId");
                HttpResponseMessage response = await _httpClient.GetAsync($"getwish?UserId={id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var nestedArray = JsonConvert.DeserializeObject<List<List<mylocalWish>>>(data);
                    ViewBag.frsd = nestedArray;
                    return View();
                }
                else
                {
                    return Ok("No data Exist");
                }
            }
            else
            {
                return RedirectToAction("user", "user");
            }   
        }
        [HttpPost]
        [Route("WishList/{id}")]
        public async Task<IActionResult> WishList(int id)
        {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("WishList", id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Privacy");
                }
                else
                {
                    return View("Error");
                }
        }

        [HttpGet]
        public async Task<IActionResult> AllOrders()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("getorders");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var productList = System.Text.Json.JsonSerializer.Deserialize<List<AllOrders>>(data, options);
                ViewBag.data= productList;
                return View();
            }
            else
            {
                return Ok("No data Exist");
            }
        } 
    }
}
