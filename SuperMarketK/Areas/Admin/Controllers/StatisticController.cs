using Model.DAO;
using Model.EntityFramework;
using SuperMarketK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Admin/Statistic
        public ActionResult Index(string dateFrom = null, string dateTo = null)
        {
            if (dateFrom == null || dateTo == null)
            {
                dateTo = DateTime.Now.ToString("yyyy-MM-dd");
                dateFrom = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            }
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            SuperMarketKDbContext db = new SuperMarketKDbContext();
            string sqlSTR = string.Format("select CAST(a.CreatedDate AS DATE) as 'DateTime', sum(b.Price*b.Quantity) as TongTien,sum(b.Price*b.Quantity)*0.3 as LoiNhuan from [Order] a,[OrderDetail] b where a.ID = b.OrderID and a.CreatedDate >= '{0}' and a.CreatedDate <= '{1}' group by CAST(a.CreatedDate AS DATE)", dateFrom, dateTo);
            List<Statistic> list = db.Database.SqlQuery<Statistic>(sqlSTR)
                    .ToList();
            var days = new OrderDAO().getList().Where(x=>x.CreatedDate >= DateTime.Parse(dateFrom) && x.CreatedDate <= DateTime.Parse(dateTo)).Select(x =>x.CreatedDate.Value.Day).Distinct();
            List<decimal?> listTongTien = new List<decimal?>();
            decimal tongTien = 0;
            foreach (var item in days)
            {
                tongTien = db.Database.SqlQuery<decimal>("select sum(Quantity*Price) from [OrderDetail] a, [Order] b where a.OrderID = b.id and DAY(b.CreatedDate) = " + item)
                    .FirstOrDefault();
                listTongTien.Add(tongTien);
            }
            ViewBag.TongTienTheoNgay = listTongTien;
            ViewBag.Ngay = days;
            return View(list);
        }
    }
}