using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class MyDBContext : IdentityDbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options):base(options)
        { }
        public DbSet<Product> Product { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Localdata> localdata { get; set; }
        public DbSet<ChatModel> Chat { get; set; }

    }
}
