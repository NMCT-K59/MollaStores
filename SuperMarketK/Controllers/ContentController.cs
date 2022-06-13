using Model.DAO;
using Model.EntityFramework;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class ContentController : Controller
    {

        SuperMarketKDbContext db = new SuperMarketKDbContext();
        // GET: Content
        public ActionResult Index()
        {
            var model = new ContentDAO();
            ViewBag.ListXemNhieu = model.getXemNhieu();
            return View(model.getList());
        }

        public ActionResult DetailNew(long id)
        {
            var model = new ContentDAO();
            model.tangView(id);
            ViewBag.ListXemNhieu = model.getXemNhieu();
            return View(model.GetByID(id));
        }

        public ActionResult Comments(long? id)
        {
            var comments = db.Comments.Where(x => x.PostID == id).ToArray();
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Comment(Comment data)
        {
            if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Message) || string.IsNullOrEmpty(data.Name))
            {
                return null;
            }
            data.CreatedDate = DateTime.Now;
            db.Comments.Add(data);
            db.SaveChanges();
            //var options = new PusherOptions();
            //options.Cluster = "ap1";
            //var pusher = new Pusher("1228143", "76aed70b9604a2cf8a17", "1f2d8d2b1b34485451e3", options);
            //ITriggerResult result = await pusher.TriggerAsync("asp_channel", "asp_event", data);
            return Content("ok");
        }

    }
}