using Model.DAO;
using Model.EntityFramework;
using SuperMarketK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            var list = new OrderDAO().getList();
            var month = list.Select(x => x.CreatedDate.Value.Month).Distinct();
            List<decimal?> listTongTien = new List<decimal?>();
            SuperMarketKDbContext db = new SuperMarketKDbContext();
            decimal tongTien = 0;
            foreach (var item in month)
            {
                tongTien = db.Database.SqlQuery<decimal>("select sum(Quantity*Price) from [OrderDetail] a, [Order] b where a.OrderID = b.id and MONTH(b.CreatedDate) = " + item)
                    .FirstOrDefault();
                listTongTien.Add(tongTien);
            }
            ViewBag.TongTien = listTongTien;
            ViewBag.Thang = month;
            ViewBag.SoNguoiTruyCap = HttpContext.Application["Online"].ToString();
            int i = 0;
            foreach (var item in month)
            {
                if (item == DateTime.Now.Month)
                {
                    break;
                }
                i++;
            }
            if (i >= listTongTien.Count)
            {
                ViewBag.DoanhThu = 0;
            } else
            {
                ViewBag.DoanhThu = listTongTien[i].GetValueOrDefault().ToString("N0");
            }
            
            ViewBag.KhachHang = db.Users.Where(x => x.GroupID == "MEMBER").Count();
            ViewBag.SanPham = db.Products.Where(x => x.Status == true).Count();
            return View();
        }

        [HttpGet]
        public JsonResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetContacts(notificationRegisterTime);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return Json(new
            {
                data = list
            },JsonRequestBehavior.AllowGet);
        }
    }
}