using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class SystemConfigController : BaseController
    {
        // GET: Admin/SystemConfig
        public ActionResult Index()
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                ViewBag.Logo = db.SystemConfigs.Where(x => x.ID == "LOGO").FirstOrDefault();
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditLogo(HttpPostedFileBase img, string name, string link)
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                var model = db.SystemConfigs.Find("LOGO");
                model.Name = name;
                model.Value = model.Value != link ? link : model.Value;
                if (img != null && img.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(img.FileName);
                    // store the file inside ~/Assets/client/images folder
                    var path = Path.Combine(Server.MapPath("~/Assets/client/images"), fileName);
                    img.SaveAs(path);
                    model.Value = "/Assets/client/images/" + fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}