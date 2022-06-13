using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EntityFramework
{
    [Table("UserVoucher")]
    public class UserVoucher
    {
        [Key]
        [Column(Order = 0)]
        public long UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public long VoucherId { get; set; }
        public int Used { get; set; }
    }
}
