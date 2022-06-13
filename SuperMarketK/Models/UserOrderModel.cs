using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class UserOrderModel
    {
        [Key]
        [Display(Name ="Mã đơn hàng")]
        public long ID { get; set; }
        [Display(Name = "Ngày mua")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Người nhận hàng")]
        public string ShipName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string ShipMobile { get; set; }
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }
        [Display(Name = "Tình trạng")]
        public int? StatusDelivery { get; set; }
    }
}