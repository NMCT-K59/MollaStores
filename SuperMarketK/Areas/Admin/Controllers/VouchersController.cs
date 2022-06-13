using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EntityFramework;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class VouchersController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Vouchers
        public ActionResult Index(string keyword, int page = 1, int pageSize = 5)
        {
            ViewBag.keyword = keyword;
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            return View(new VoucherDAO().getAllPaging(keyword, page, pageSize));
        }

        // GET: Admin/Vouchers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vouchers vouchers = db.Vouchers.Find(id);
            if (vouchers == null)
            {
                return HttpNotFound();
            }
            return View(vouchers);
        }

        // GET: Admin/Vouchers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Vouchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,code,name,description,uses,max_uses,max_uses_user,type,discount_amount,is_fixed,starts_at,expires_at,condition")] Vouchers vouchers)
        {
            if (ModelState.IsValid)
            {
                db.Vouchers.Add(vouchers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vouchers);
        }

        // GET: Admin/Vouchers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vouchers vouchers = db.Vouchers.Find(id);
            if (vouchers == null)
            {
                return HttpNotFound();
            }
            return View(vouchers);
        }

        // POST: Admin/Vouchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,code,name,description,uses,max_uses,max_uses_user,type,discount_amount,is_fixed,starts_at,expires_at,condition")] Vouchers vouchers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vouchers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vouchers);
        }

        // GET: Admin/Vouchers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vouchers vouchers = db.Vouchers.Find(id);
            if (vouchers == null)
            {
                return HttpNotFound();
            }
            return View(vouchers);
        }

        // POST: Admin/Vouchers/Delete/5
        [HttpDelete, ActionName("Delete")]
        public ActionResult Delete(long id)
        {
            Vouchers vouchers = db.Vouchers.Find(id);
            db.Vouchers.Remove(vouchers);
            db.SaveChanges();
            return RedirectToAction("Index");
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
