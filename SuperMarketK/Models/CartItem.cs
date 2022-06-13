using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    [Serializable]
    public class CartItem
    {
        public Product product { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
}