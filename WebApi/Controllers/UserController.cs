using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class UserController : Controller
    {
        private MyDBContext _DB;
        public UserController(MyDBContext DB)
        {
            _DB = DB;
        }
        [HttpGet]
        [Route("/getwish")]
        public IActionResult getwish(string userid)
        {
            var djh=new List<object>();

            var dsf=_DB.localdata.Where(s=>s.UserId == userid);

            foreach (var item in dsf)
            {
                if (item.Status == null)
                {
                   var sds=_DB.Product.Where(s=>s.Id==item.productwishid).ToList();
                   djh.Add(sds);
                }
                else
                {
                    var sds = _DB.localdata.Where(s => s.productwishid == item.productwishid).ToList();
                    djh.Add(sds);
                }
            }
            return Ok(djh);
        }
        [HttpPost]
        [Route("Wishdata")]
        public IActionResult addwish(Localdata localdata)
        {
             var chk=_DB.localdata.Where(s=>s.productwishid == localdata.productwishid
             && s.UserId==localdata.UserId
             ).ToList();
             if (chk.Count()>0)
             {
                 return Ok("data already exist");
             }
             Localdata ld= new Localdata()
             {  
                 productwishid=localdata.productwishid,
                 UserId=localdata.UserId,
             };
             _DB.localdata.Add(ld);
             _DB.SaveChanges();
            return Ok("done");

        }
        [HttpDelete]
        [Route("delwish")]
        public IActionResult delwish(RemoveWish fam)
        {
            var sing = _DB.localdata.Where(s => s.productwishid == fam.productwishid
            && s.UserId== fam.UserId);
            if (!sing.Any())
            {
                return NotFound("No product exists at this ID.");
            }
            _DB.localdata.RemoveRange(sing);
            _DB.SaveChanges();
            return Ok("data removed");
        }
        [HttpPost]
        [Route("orderdata")]
        public IActionResult putwish(Localdata localdata)
        {
           var s =_DB.localdata.FirstOrDefault(s=>s.productwishid==localdata.productwishid);

            if (s != null)
            {
                s.Name = localdata.Name;
                s.Category = localdata.Category;
                s.Price = localdata.Price;
                s.RoomNo = localdata.RoomNo;
                s.StartingDate = localdata.StartingDate;
                s.EndingDate = localdata.EndingDate;
                s.Status = false;
                s.UserId= localdata.UserId;


                _DB.localdata.Update(s);
                _DB.SaveChanges();
            
            return Ok("dff");
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpGet]
        [Route("getorders")]
        public IActionResult allorders()
        {
            var s = _DB.localdata.Where(s => s.EndingDate != null && s.StartingDate != null).ToList();
            if (s.Count > 0)
            {
                return Ok(s);
            }
            else
            {
                return Ok("No data exist");
            }
        }
        [HttpPost]
        [Route("verifyorder")]
        public IActionResult verifyorder(int id)
        {
            var s = _DB.localdata.FirstOrDefault(s => s.Id==id
            );

            if (s != null)
            {
                s.Status= true;
                s.OrderVarification = "Verified";
                _DB.localdata.Update(s);
                _DB.SaveChanges();

                return Ok("Update ");
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpPost]
        [Route("cancelorder")]
        public IActionResult cancelorder(int id)
        {
            var s = _DB.localdata.FirstOrDefault(s => s.Id == id
            );

            if (s != null)
            {
                s.OrderVarification = "Cancel";
                _DB.localdata.Update(s);
                _DB.SaveChanges();

                return Ok("Update ");
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpGet]
        [Route("AllMessages")]
        public IActionResult allmesaages(string id) {
            if (id != null)
            {
               var d= _DB.Chat.Where(s => s.SenderID==id || s.ReceiverID == id).ToList();
                return Ok(d);
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpGet]
        [Route("Getallusers")]
        public IActionResult GetAllUsers()
        {
            var allusers = _DB.Users
           .Where(u => u.UserName != "admin")
           .Select(u => new
            {
               UserName = u.UserName,
               Email = u.Email,
               Id = u.Id
              });
            return Ok(allusers);
        }




        [HttpPost]
        [Route("SendToAdmin")]
        public IActionResult sendtoadmin([FromBody]ChatModel mdl)
        {
            if (mdl.ReceiverID == null)
            {
                var admnId = _DB.Users
                .Where(u => u.UserName == "admin")
                .FirstOrDefault();
                if (admnId != null)
                {
                    ChatModel ds = new ChatModel()
                    {
                        SenderID = mdl.SenderID,
                        ReceiverID = admnId.Id,
                        message = mdl.message,
                    };

                    _DB.Chat.Add(ds);
                    _DB.SaveChanges();
                }
            }
            else
            {
                _DB.Chat.Add(mdl);
                _DB.SaveChanges();
            }
            return Ok();
        }
        [HttpPost]
        [Route("AdmintoUser")]
        public IActionResult admintouser(ChatModel mdl)
        {
           
                    ChatModel ds = new ChatModel()
                    {
                        SenderID = mdl.SenderID,
                        ReceiverID = mdl.ReceiverID,
                        message = mdl.message,
                    };

                    _DB.Chat.Add(ds);
                    _DB.SaveChanges();
                return Ok();
                
            
        }
    }
}
