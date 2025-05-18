using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Models;

namespace Shoppng_Tutorial.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();

            // Seed Categories, Brands, and Products
            if (!_context.Products.Any())
            {
                // Danh mục
                var aoNam = new CategoryModel
                {
                    Name = "Áo Nam",
                    Slug = "ao-nam",
                    Description = "Các loại áo thời trang cho nam giới",
                    Status = 1
                };

                var aoNu = new CategoryModel
                {
                    Name = "Áo Nữ",
                    Slug = "ao-nu",
                    Description = "Các loại áo thời trang cho nữ giới",
                    Status = 1
                };

                // Thương hiệu
                var zara = new BrandModel
                {
                    Name = "Zara",
                    Slug = "zara",
                    Description = "Zara - Thương hiệu thời trang cao cấp",
                    Status = 1
                };

                var hnm = new BrandModel
                {
                    Name = "H&M",
                    Slug = "h-m",
                    Description = "H&M - Thương hiệu thời trang phổ thông",
                    Status = 1
                };

                // Thêm sản phẩm
                _context.Products.AddRange(
                    new ProductModel
                    {
                        Name = "Áo sơ mi nam Zara",
                        Slug = "ao-so-mi-nam-zara",
                        Description = "Áo sơ mi nam hàng hiệu Zara, chất liệu cotton thoáng mát.",
                        Image = "aosominam.jpg",
                        Category = aoNam,
                        Brand = zara,
                        Price = 850000,
                        CapitalPrice = 500000,
                        Quantity = 50
                    },
                    new ProductModel
                    {
                        Name = "Áo thun nữ H&M",
                        Slug = "ao-thun-nu-hm",
                        Description = "Áo thun nữ phong cách basic, phù hợp mặc hàng ngày.",
                        Image = "aothunnu.jpg",
                        Category = aoNu,
                        Brand = hnm,
                        Price = 450000,
                        CapitalPrice = 200000,
                        Quantity = 120
                    }
                );

                _context.SaveChanges();
            }

        }
    }
}
