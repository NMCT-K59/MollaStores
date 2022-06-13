using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EntityFramework;
using SuperMarketK.Common;
using System.IO;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDAO();
            var model = dao.ListAllPaging(keyword, page, pageSize);
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            ViewBag.keyword = keyword;
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(new ContentDAO().GetByID(id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDAO();
            var content = dao.GetByID(id);

            SetViewBag(content.CategoryID);
            return View(content);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Content model)
        {
            if (ModelState.IsValid)
            {
                SuperMarketKDbContext db = new SuperMarketKDbContext();
                var content = db.Contents.Find(model.ID);
                content.Detail = model.Detail;
                content.Name = model.Name;
                content.Description = model.Description;
                content.CategoryID = model.CategoryID;
                db.SaveChanges();
            }
            SetViewBag(model.ID);
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Content model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = Path.GetFileName(ImageFile.FileName);
                    // store the file inside ~/Assets/client/images folder
                    var path = Path.Combine(Server.MapPath("~/Assets/client/images"), fileName);
                    ImageFile.SaveAs(path);
                }
                var session = (UserLogin)Session[CommonContants.USER_SESSION];
                model.CreatedBy = session.UserName;
                model.Image = "/Assets/client/images/" + fileName;
                new ContentDAO().Create(model);
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View();
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new CategoryDAO();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                new ContentDAO().Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}