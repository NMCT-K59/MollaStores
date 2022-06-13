using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class Statistic
    {
        public DateTime DateTime { get; set; }
        public decimal? TongTien { get; set; }
        public decimal? LoiNhuan { get; set; }
    }
}