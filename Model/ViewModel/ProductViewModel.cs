using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductViewModel
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string MetaTitle { get; set; }
        public long CategoryID { get;set; }

        public string Description { get; set; }
        public string Detail { get; set; }

        public string Image { get; set; }

        public decimal? Price { get; set; }

        public decimal? PromotionPrice { get; set; }
        public string MetaDescriptions { get; set; }

        public int Quantity { get; set; }
        public string CategoryName { get; set; }
        public double? Rating { get; set; }
        public int? CountRating { get; set; } = 0;
        public List<ProductImage> ListImages { get; set; } = new List<ProductImage>();
    }
}
