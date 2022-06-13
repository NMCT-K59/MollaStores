using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDetailDAO
    {
        SuperMarketKDbContext db = null;
        public OrderDetailDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public bool Insert(OrderDetail orderDetail)
        {
            try
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
                throw ex;
            }
        }
    }
}
