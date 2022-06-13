using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index(long order_id)
        {
            var dao = new OrderDAO().getListOrderDetail(order_id);
            return View(dao);
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(long order_id)
        {
            string message = "";
            var dao = new OrderDAO();
            if (dao.checkOrderExits(order_id))
            {
               return Redirect("/tracking-order?order_id=" + order_id);
            } else
            {
                message = "Not found this order";
            }
            ViewBag.Message = message;
            return View();
        }
    }
}