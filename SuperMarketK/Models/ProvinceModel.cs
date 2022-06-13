using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class ProvinceModel
    {
        public int ID { set; get; }
        public string Name { set; get; }

        public ProvinceModel(int iD, string name)
        {
            ID = iD;
            Name = name;
        }
    }
}