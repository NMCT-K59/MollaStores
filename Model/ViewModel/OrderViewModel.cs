using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class OrderViewModel
    {
        public long ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? PriceOrder { get; set; }
        public int? Status { get; set; }
        public virtual List<ListItems> Items { get; set; }
    }

    public class ListItems
    {
        public long ProductID { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }
    }
}
