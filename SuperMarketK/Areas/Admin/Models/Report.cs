using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Areas.Admin.Models
{
    public class Report
    {
        [Key]
        public long ProductID { get; set; }
        public string Name { get; set; }
        public int SL { get; set; }
        public int Quantity { get; set; }
        public decimal? DoanhThu { get; set; }
    }
}