using Model.EntityFramework;
using Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDAO
    {
        SuperMarketKDbContext db = null;
        public OrderDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<Order> getList()
        {
            return db.Orders.ToList();
        }

        public long Insert(Order order)
        {
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return order.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool Update(Order user)
        {
            try
            {
                var userUpdate = db.Orders.Find(user.ID);
                userUpdate.CreatedDate = user.CreatedDate;
                userUpdate.ShipName = user.ShipName;
                userUpdate.ShipAddress = user.ShipAddress;
                userUpdate.ShipMobile = user.ShipMobile;
                userUpdate.Status = user.Status;
                userUpdate.StatusDelivery = user.StatusDelivery;
                if (user.StatusDelivery == 3)
                {
                    UpdateStatus(1, user.ID);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
                throw ex;
            }
        }

        public void UpdateStatus(int status, long id)
        {
            db.Orders.Find(id).Status = status;
            db.SaveChanges();
            return;
        }

        public IEnumerable<OrderViewModel> ListOrder(string searchString, int page, int pageSize)
        {
            var model = from a in db.Orders
                        join b in db.Users on a.CustomerID equals b.ID
                        select new OrderViewModel()
                        {
                            ID = a.ID,
                            CustomerName = a.ShipName,
                            CustomerPhone = a.ShipMobile,
                            Address = a.ShipAddress,
                            CreatedDate = a.CreatedDate,
                            PriceOrder = (decimal)db.OrderDetails.Where(x => x.OrderID == a.ID).Sum(x => x.Quantity * x.Price),
                            Status = a.Status
                        };
            if (!string.IsNullOrEmpty(searchString))
            {
                return model.Where(x => x.CustomerName.Contains(searchString)
                || x.CustomerPhone.Contains(searchString)).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Order getListOrderDetail(long? ID)
        {
            db.Orders.Find(ID).ListOrderDetail = db.OrderDetails.Where(x => x.OrderID == ID).ToList();
            foreach (var item in db.Orders.Find(ID).ListOrderDetail)
            {
                item.Product = db.Products.Find(item.ProductID);
            }
            db.SaveChanges();
            return db.Orders.Find(ID);
        }

        public bool checkOrderExits(long? ID)
        {
            return db.Orders.Where(x => x.ID == ID).Count() > 0;
        }
    }
}
