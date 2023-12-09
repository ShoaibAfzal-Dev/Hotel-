using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api")]
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private MyDBContext _DB;
        //      private readonly UserManager<IdentityUser> userManager;
        //     private readonly RoleManager<IdentityRole> roleManager;

        public NewsController(ILogger<NewsController> logger,
             MyDBContext DB
            //  UserManager<IdentityUser> _userManager,
            //  RoleManager<IdentityRole> _roleManager
            )
        {
            _logger = logger;
            _DB = DB;
            // userManager = _userManager;
            //  roleManager = _roleManager;
        }
        [HttpGet]
        [Route("/AllNews")]
        public IActionResult Index()
        {
            var data = _DB.News.OrderByDescending(s => s.CreatedDate).ToList();
            if (data != null)
            {
                return Ok(data);
            }
            return Ok("Nodata found ");
        }
        [HttpPost]
        [Route("/News")]
        public IActionResult Index([FromBody]News nw)
        {
            if (ModelState.IsValid)
            {
                News news = new News()
                {
                    Id = nw.Id,
                    Name= nw.Name,
                    Description = nw.Description,
                    CreatedDate = DateTime.Now,
                };
                _DB.Add(news);
                _DB.SaveChanges();
                return Ok(news);
            }
            return BadRequest("invalid model state ");
        }
        [HttpGet]
        [Route("/SingleNews")]
        public IActionResult Edit(int? id)
        {
            var da= _DB.News.FirstOrDefault(s=>s.Id==id);
            if (da != null)
            {
                News news = new News()
                {
                   Id=da.Id,
                   Name=da.Name,
                   Description = da.Description,
                   CreatedDate = da.CreatedDate,
                };
                return Ok(news);
            }
            return BadRequest("No data found at this URL");
        }
        [HttpPost]
        [Route("/News/Update")]
        public IActionResult Edit([FromBody]News nw)
        {
            if(ModelState.IsValid) {
             // var news=_DB.News.FirstOrDefault(nw=>nw.Id==nw.Id);
                News news1 = new News()
                {
                    Id=nw.Id,
                    Name=nw.Name,
                    Description = nw.Description,
                    CreatedDate = DateTime.Now,
                };
                _DB.News.Update(news1);
                _DB.SaveChanges();
                return Ok(news1);
            }
            return BadRequest("Invalid model state");
        }
        [HttpDelete]
        [Route("/News/Delete")]
        public IActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                var f= _DB.News.FirstOrDefault(s=>s.Id==id);
                if(f != null)
                {
                    _DB.News.Remove(f);
                    _DB.SaveChanges();
                    return Ok("data removed successfully");
                }
            }
            return BadRequest("No data Found at this location");
        }
    }
}
