using System.ComponentModel.DataAnnotations;

namespace Shoppng_Tutorial.Models.ViewModel
{
    public class ProductDetailsViewModel
    {
        public ProductModel ProductDetails { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập đánh giá sản phẩm")]
        public string Comment { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { get; set; }

        public List<RatingModel> Ratings { get; set; }

    }
}
