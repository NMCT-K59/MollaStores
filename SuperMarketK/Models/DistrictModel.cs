using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class DistrictModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int ProvinceID { set; get; }

        public DistrictModel(int iD, string name, int provinceID)
        {
            ID = iD;
            Name = name;
            ProvinceID = provinceID;
        }
    }
}