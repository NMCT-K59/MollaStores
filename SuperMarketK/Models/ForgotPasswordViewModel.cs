using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class ForgotPasswordViewModel
    {
        [Key]
        [Required(ErrorMessage ="Hãy nhập mật khẩu", AllowEmptyStrings =false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage ="Mật khẩu xác nhận không đúng")]
        public string ConfirmPassword { get; set; }

        public string ResetCode { get; set; }
    }
}