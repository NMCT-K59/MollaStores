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
using Common;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class ProductCategoriesController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/ProductCategories
        public ActionResult Index(string keyword, int page = 1, int pageSize = 5)
        {
            ViewBag.keyword = keyword;
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            return View(new ProductCategoryDAO().getAllPaging(keyword, page, pageSize));
        }

        // GET: Admin/ProductCategories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,ShowOnHome")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.MetaTitle = StringHelper.ToUnsignString(productCategory.Name);
                productCategory.CreatedDate = DateTime.Now;
                var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
                productCategory.CreatedBy = userLogin.UserName;
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,ShowOnHome")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.MetaTitle = StringHelper.ToUnsignString(productCategory.Name);
                productCategory.CreatedDate = DateTime.Now;
                var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
                productCategory.CreatedBy = userLogin.UserName;
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        // POST: Admin/ProductCategories/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
