
using Model.EntityFramework;
using Rotativa;
using SuperMarketK.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        // GET: Admin/Report
        public ActionResult Index(string dateFrom = null, string dateTo = null)
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                if (dateFrom == null || dateTo == null)
                {
                    dateTo = DateTime.Now.ToString("yyyy-MM-dd");
                    dateFrom = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                }
                ViewBag.DateFrom = dateFrom;
                ViewBag.DateTo = dateTo;
                TempData["fromDate"] = dateFrom;
                TempData["toDate"] = dateTo;
                string query = string.Format("exec usp_BaoCaoLoiNhuan '{0}', '{1}'", dateFrom, dateTo);
                var list = db.Database.SqlQuery<Report>(query).ToList();
                return View(list);
            }
        }

        public ActionResult PrintToPdf()
        {
            return new ActionAsPdf("Index") { FileName = "Test.pdf" };
        }

        [HttpGet]
        public ActionResult PrintPartialViewToPdf()
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                string dateTo = TempData["toDate"].ToString();
                string dateFrom = TempData["fromDate"].ToString();

                string query = string.Format("exec usp_BaoCaoLoiNhuan '{0}', '{1}'", dateFrom, dateTo);
                var list = db.Database.SqlQuery<Report>(query).ToList();
                var report = new PartialViewAsPdf("/Areas/Admin/Views/Report/View.cshtml");
                return report;
            }
        }
    }
}