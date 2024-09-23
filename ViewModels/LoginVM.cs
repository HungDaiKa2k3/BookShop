using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels
{
    public class LoginVM
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage ="Phải nhập Email")]
        [EmailAddress(ErrorMessage ="Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Phải nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
