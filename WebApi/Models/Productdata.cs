namespace WebApi.Models
{
    public class Productdata
    {
        public Product Product { get; set; }
        public List<Image> Images { get; set; }
        public List<IFormFile> imageFiles { get; set; }
    }
}
