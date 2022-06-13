using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Hãy nhập username")]
        public string userName { get; set; }
        [Required(ErrorMessage = "Hãy nhập password")]
        public string passWord { get; set; }
        public bool rememberMe { get; set; }
    }
}