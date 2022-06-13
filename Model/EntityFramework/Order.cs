namespace Model.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public long ID { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime? CreatedDate { get; set; }

        public long? CustomerID { get; set; }
        [Display(Name = "Full Name")]
        [StringLength(50)]
        public string ShipName { get; set; }
        [Display(Name = "Phone")]
        [StringLength(50)]
        public string ShipMobile { get; set; }
        [Display(Name = "Address")]
        public string ShipAddress { get; set; }
        [Display(Name = "Email")]
        [StringLength(50)]
        public string ShipEmail { get; set; }
        [Display(Name = "Status")]
        public int? Status { get; set; }
        public int? StatusDelivery { get; set; }

        public int TienShip { get; set; }
        public decimal? TotalDiscount { get; set; }
        public List<OrderDetail> ListOrderDetail { get; set; }
    }


}
