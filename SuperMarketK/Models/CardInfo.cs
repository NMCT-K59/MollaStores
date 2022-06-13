using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class CardInfo
    {
        [Required(ErrorMessage ="Enter your fullname")]
        public string name { get; set; }
        [Required(ErrorMessage = "Enter your phone")]
        public string mobile { get; set; }
        [Required(ErrorMessage = "Enter your address")]
        public string address { get; set; }
    }
}