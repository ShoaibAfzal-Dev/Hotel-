using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Route("api")]
    public class ProductsController : Controller
    {
        private readonly MyDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(MyDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [Route("/getallData")]
        [HttpGet]
        public ActionResult Index()
        {
            var data = _context.Product
     .GroupJoin(
         _context.Image,
         first => first.Id,
         second => second.ProductId,
         (first, second) => new
         {
             Product = first,
             Images = second.Where(s => s.Product.Id == first.Id)
         }
     )
     .SelectMany(result => result.Images.DefaultIfEmpty(), (first, second) => new
     {
         Product = first.Product,
         Image = second
     });

            /* var dta = _context.Image.Join(_context.Image,
                 first => first.ProductId,
                 second => second.Id,
                 (first, second) =>new
                 {
                     first,second
                 });*/
            var result = data
            .GroupBy(item => item.Product, item => new
           {
                 item.Image.Id,
                 item.Image.ImageURL,
                 item.Image.ProductId
            })
             .Select(group => new
            {
           product = group.Key,
             images = group.ToList()
             })
              .ToList();
            
            return Ok(result);
            
        }
        [Route("getsingle")]
        [HttpGet]
        public IActionResult Single(int id)
        {
            var sing = _context.Product.FirstOrDefault(s => s.Id == id);
            if (sing != null)
            {
                var data = _context.Product.Where(s=>s.Id==id).
                    GroupJoin(_context.Image,
                    firt => firt.Id,
                    send => send.ProductId,
                    (firt, send) => new
                    {
                        Product= firt,
                        Images= send.Select(s => new
                        {
                            s.Id,
                            s.ImageURL,
                            s.ProductId
                        })
                    }
                    );

              
                return Ok(data);
            }
            return BadRequest("No Data exist at this ID");
        }
        [Route("/CreateProduct")]
        [HttpPost]
        public async Task<IActionResult> CreateProductWithImages(MyView  product)
        {
            
           if (product == null)
            {
                return BadRequest("Invalid product data.");
            }
            if (product.imageFiles == null || product.imageFiles.Count == 0)
           {
               return BadRequest("Please provide at least one image.");
           }
           var pd = new Product()
           {
               Id = product.Id,
               Name = product.Name,
               Description = product.Description,
               Category = product.Category,
               Price = product.Price,
               RoomNo = product.RoomNo,
           };
           foreach (var imageFile in product.imageFiles)
           {
               if (imageFile != null)
               {
                   var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                   var filePath = Path.Combine("wwwroot/images", fileName);
                   using (var stream = new FileStream(filePath, FileMode.Create))
                   {
                       await imageFile.CopyToAsync(stream);
                   }
                   var image = new Image()
                   {
                       ImageURL = Path.Combine("images", fileName),
                       Product = pd
                   };
                   _context.Image.Add(image);
               }
           }
             _context.Product.Add(pd);
            await _context.SaveChangesAsync();
            return Ok("done");
         }
        [Route("delete")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sing = _context.Product.FirstOrDefault(s => s.Id == id);
           /* var img = _context.Image.Select(s => s.ProductId == id);*/
            if (sing == null)
            {
                return NotFound("No product exists at this ID.");
            }
          /*  var images = _context.Image.Where(i => i.ProductId == id).ToList();*/
            _context.Product.Remove(sing);
           /* _context.Image.RemoveRange(images);*/
            _context.SaveChanges();
            return Ok("Product and associated images have been deleted.");
        }
        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> UpdateProductWithImages(int id, [FromForm] Product product, List<IFormFile> imageFiles)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }
            if (imageFiles == null || imageFiles.Count == 0)
            {
                return BadRequest("Please provide at least one image.");
            }
            var exp = _context.Product.FirstOrDefault(p => p.Id == id);
            if (exp == null)
            {
                return NotFound("No product exists at this ID.");
            }
            exp.Id= id;
            exp.Name = product.Name;
            exp.Description = product.Description;
            exp.Category = product.Category;
            exp.Price = product.Price;
            exp.RoomNo = product.RoomNo;

            var imag = _context.Image.Where(s => s.ProductId == id).ToList();
            if (imag != null)
            {
                _context.Image.RemoveRange(imag);
            }




            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    var image = new Image
                    {
                        ImageURL = Path.Combine("images", fileName),
                        ProductId = exp.Id 
                    };
                    _context.Image.Update(image);
                }
            }
            _context.Product.Update(exp);
            _context.SaveChanges();
            return Ok("Product and associated images have been updated.");
        }

        [Route("add-images")]
        [HttpPost]
        public async Task<IActionResult> AddImages(int productId, List<IFormFile> imageFiles)
        {
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return NotFound("No product exists at this ID.");
            }

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    var image = new Image
                    {
                        ImageURL = Path.Combine("images", fileName),
                      /*  ProductId = productId*/
                    };
                    _context.Image.Add(image);
                }
            }
            _context.SaveChanges();
            return Ok("Images have been added to the product.");
        }

        [Route("delete-images")]
        [HttpDelete]
        public IActionResult DeleteImages(int productId)
        {
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {  return NotFound("No product exists at this ID.");
            }
            /*var images = _context.Image.Where(i => i.ProductId == productId).ToList();*/
           /* _context.Image.RemoveRange(images);*/
            _context.SaveChanges();
            return Ok("Images for the product have been deleted.");
        }
        [Route("delete-image")]
        [HttpDelete]
        public IActionResult DeleteImage(int imageId)
        {
            var image = _context.Image.FirstOrDefault(i => i.Id == imageId);
            if (image == null)
            {
                return NotFound("No image exists at this ID.");
            }
            _context.Image.Remove(image);
            _context.SaveChanges();
            return Ok("Image has been deleted.");
        }
    }
}
