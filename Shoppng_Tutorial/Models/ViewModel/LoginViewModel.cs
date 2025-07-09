using System.ComponentModel.DataAnnotations;

namespace Shoppng_Tutorial.Models.ViewModel
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập Tên Đăng Nhập")]
        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Nhập Mật Khẩu")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}
