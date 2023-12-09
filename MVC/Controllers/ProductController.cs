using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebApi.Models;
using MVC.Models;
using WebApi.Controllers;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        // Add your url here
        private string BaseURL = "";
        public ProductController()
        {
             _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseURL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public IActionResult Index()
        {
            return View(); 
        }
        public async Task<IActionResult> GetData()
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                HttpResponseMessage response = await _httpClient.GetAsync("getallData");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var productList = System.Text.Json.JsonSerializer.Deserialize<List<Models.Productdata>>(data, options);
                    return View(productList);
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
        public async Task<IActionResult> Singledata(int id)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/getsingle?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var product = System.Text.Json.JsonSerializer.Deserialize<List<Models.Productdata>>(data, options);
                    return View(product);
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

        [HttpGet]
        public IActionResult Createdata()
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("user", "user");
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> Createdata(MyView productdata)
        {
           
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("CreateProduct);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Createdata");
                }
                return View();
            
        }*/

        [HttpGet]
        public async Task<IActionResult> Editdata(int id)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                
           
            HttpResponseMessage response = await _httpClient.GetAsync($"api/getsingle?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var product = System.Text.Json.JsonSerializer.Deserialize<List<Models.Productdata>>(data, options);
                return View(product);
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


    }
}
