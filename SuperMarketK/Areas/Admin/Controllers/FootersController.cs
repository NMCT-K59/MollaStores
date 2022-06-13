using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EntityFramework;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class FootersController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Footers
        public ActionResult Index()
        {
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            return View(db.Footers.ToList());
        }

        // GET: Admin/Footers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            SetViewBag(footer.Status);
            return View(footer);
        }

        // GET: Admin/Footers/Create
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: Admin/Footers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,Status")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Footers.Add(footer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag();
            return View(footer);
        }

        // GET: Admin/Footers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footer footer = db.Footers.Find(id);
            if (footer == null)
            {
                return HttpNotFound();
            }
            SetViewBag(footer.Status);
            return View(footer);
        }

        // POST: Admin/Footers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,Status")] Footer footer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(footer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag(footer.Status);
            return View(footer);
        }

        // POST: Admin/Footers/Delete/5
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            Footer footer = db.Footers.Find(id);
            db.Footers.Remove(footer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetViewBag(Boolean? selectedId = null)
        {
            var listStatus = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Hiển thị", Value = "true"},
                    new SelectListItem { Text = "Khóa", Value = "false" },
                };
            ViewBag.StatusList = new SelectList(listStatus, "Value", "Text");
        }

    }
}
