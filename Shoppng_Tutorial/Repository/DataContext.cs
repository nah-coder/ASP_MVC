using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Models;

namespace Shoppng_Tutorial.Repository
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SliderModel> Sliders { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<RatingModel> Rating { get; set; }
        public DbSet<BrandModel> Brands { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
    
}
