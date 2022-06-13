using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMarketK.Models
{
    public class WardModel
    {
        public int WardCode { get; set; }
        public string WardName { get; set; }
        public int DistrictID { get; set; }

        public WardModel(int wardCode, string wardName, int districtID)
        {
            WardCode = wardCode;
            WardName = wardName;
            DistrictID = districtID;
        }
    }
}