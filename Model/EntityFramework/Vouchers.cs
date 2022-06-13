using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("Vouchers")]
    public class Vouchers
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int uses { get; set; }
        public int max_uses { get; set; }
        public int max_uses_user { get; set; }
        public int type { get; set; }
        public decimal? discount_amount { get; set; }
        public bool is_fixed { get; set; }
        public DateTime starts_at { get; set; }
        public DateTime expires_at { get; set; }
        public decimal? condition { get; set; }
    }
}
