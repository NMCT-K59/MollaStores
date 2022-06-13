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
    public class CommentController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Comment
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new CommentDAO();
            var model = dao.ListAllPaging(keyword, page, pageSize);
            return View(model);
        }

        // POST: Admin/Comment/Delete/5
        [HttpDelete]
        public void Delete(long id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult RepComment(string emailRep, string contentRep)
        {
            new MailHelper().SendMail(emailRep,"Phản hồi từ SuperMarket K", contentRep);
            setAlert("Đã gửi Email trả lời đến " + emailRep, "success");
            return RedirectToAction("Index");
        }
    }
}
