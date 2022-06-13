using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class Register
    {
        [Key]
        public long ID { get; set; }
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu phải lớn hơn 6 kí tự")]
        [Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string PassWord { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PassWord", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassWord { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Hãy nhập họ tên")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
    }
}