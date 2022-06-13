using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common;
using Model.DAO;
using Model.EntityFramework;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class FeedbacksController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Feedbacks
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new CommentDAO();
            var model = dao.ListAllPagingFeedback(keyword, page, pageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult RepComment(string emailRep, string contentRep)
        {
            new MailHelper().SendMail(emailRep, "Reply contact", contentRep);
            setAlert("Reply successfully " + emailRep, "success");
            return RedirectToAction("Index");
        }

        // GET: Admin/Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Admin/Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Admin/Feedbacks/Delete/5
        [HttpDelete]
        public void Delete(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
