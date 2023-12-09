using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.Json;
using System.Net.Http.Json;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        // Add your url here
        private string BaseURL = "";
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseURL);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Privacy()
        {
            if (HttpContext.Session.GetString("UserRole") != null)
            {
                HttpResponseMessage response = await _httpClient.GetAsync("AllNews");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var newsList = System.Text.Json.JsonSerializer.Deserialize<List<News>>(data, options);
                    return View(newsList);
                }
                else
                {
                    return Ok("No data Exist");
                }
            }
            else
            {
                return RedirectToAction("user","user");
            }
        }

        [HttpGet]
        public IActionResult CreateNews()
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateNews(News news)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {

                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("News", news);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Privacy");
                    }
                    else
                    {
                        return View("Error");
                    }         
                }              
                return View("Error");
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"SingleNews?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var data = await response.Content.ReadAsStringAsync();
                    var news = System.Text.Json.JsonSerializer.Deserialize<News>(data, options);
                    return View(news);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(News nw)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                if (ModelState.IsValid)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("News/Update", nw);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Privacy");
                    }

                }
                return View("Error");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"News/Delete?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Privacy");
                }
                return View("Error");
            }
            else
            {
                return RedirectToAction("index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        // chat code

        [HttpGet]
        public async Task<IActionResult> chat()
        {
            var id = HttpContext.Session.GetString("UserId");
            // AllMessages
            if (HttpContext.Session.GetString("UserRole") == "User")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"AllMessages?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var data = await response.Content.ReadAsStringAsync();
                    var news = System.Text.Json.JsonSerializer.Deserialize<List<ChatModel>>(data, options);
                    ViewBag.Chat = news;
                    return View("chat");
                }
                return View("Error");
            }
            else if (HttpContext.Session.GetString("UserRole") == "admin")
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Getallusers");
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    var data = await response.Content.ReadAsStringAsync();
                    var news = System.Text.Json.JsonSerializer.Deserialize<List<allusersdata>>(data, options);
                    ViewBag.Chat = news;
                    return View("chat");
                }

            }
            return View();
        }
    }
}