using Model.DAO;
using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        SuperMarketKDbContext db = new SuperMarketKDbContext();
        // GET: Admin/Slide
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new SlideDAO();
            var model = dao.ListAllPaging(keyword, page, pageSize);
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            ViewBag.keyword = keyword;
            return View(model);
        }
        

        // GET: Admin/Slide/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            return View(slide);
        }
        
        // GET: Admin/Slide/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Slide/Create
        [HttpPost]
        public ActionResult Create(Slide slide, HttpPostedFileBase ImageFile)
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
                slide.CreatedDate = DateTime.Now;
                var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
                slide.CreatedBy = userLogin.UserName;
                slide.Status = true;
                slide.Image = "/Assets/client/images/" + fileName;
                db.Slides.Add(slide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slide);
        }

        // GET: Admin/Slide/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slide slide = db.Slides.Find(id);
            if (slide == null)
            {
                return HttpNotFound();
            }
            SetViewBag();
            return View(slide);
        }

        // POST: Admin/Slide/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Image,DisplayOrder,Link,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,Status")] Slide slide, HttpPostedFileBase ImageFile = null)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                var sp = db.Slides.Find(slide.ID);
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    // extract only the fielname
                    fileName = Path.GetFileName(ImageFile.FileName);
                    // store the file inside ~/Assets/client/images folder
                    var path = Path.Combine(Server.MapPath("~/Assets/client/images"), fileName);
                    ImageFile.SaveAs(path);
                    sp.Image = "/Assets/client/images/" + fileName;
                }
                sp.DisplayOrder = slide.DisplayOrder;
                sp.Link = slide.Link;
                sp.Description = slide.Description;
                sp.Status = slide.Status;
                if (ImageFile == null)
                {
                    slide.Image = sp.Image;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slide);
        }

        // POST: Admin/Product/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            Slide slide = db.Slides.Find(id);
            db.Slides.Remove(slide);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetViewBag(string selectID = null)
        {
            var listStatusSelected = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Kích hoạt", Value = "true", Selected = true },
                    new SelectListItem { Text = "Vô hiệu hóa", Value = "false" },
                };
            ViewBag.listStatusSelected = listStatusSelected;
        }
    }
}
