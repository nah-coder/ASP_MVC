using System.ComponentModel.DataAnnotations;

namespace Shoppng_Tutorial.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập Tên Đăng Nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Nhập Mật Khẩu")]
        public string Password { get; set; }

    }
}
