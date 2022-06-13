using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    class OrderDetailViewModel
    {
        public long ID { get; set; }

        public DateTime? CreatedDate { get; set; }


        public string ShipName { get; set; }


        public string ShipMobile { get; set; }


        public string ShipAddress { get; set; }

  
        public string ShipEmail { get; set; }

        public int? Status { get; set; }


    }
}
