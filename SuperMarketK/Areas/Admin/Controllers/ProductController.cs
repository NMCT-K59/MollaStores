using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EntityFramework;
using Common;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();

        // GET: Admin/Product
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDAO();
            var model = dao.getAllPaging(keyword, page, pageSize);
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            ViewBag.keyword = keyword;
            return View(model);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        private string SaveImg(HttpPostedFileBase img)
        {
            string fullName = null;
            if (img != null)
            {
                var InputFileName = Path.GetFileName(img.FileName);
                var ServerSavePath = Path.Combine(Server.MapPath("~/Assets/client/images"), InputFileName);
                //Save file to server folder  
                img.SaveAs(ServerSavePath);
                //assigning file uploaded status to ViewBag for showing message to user.
                fullName = "/Assets/client/images/" + InputFileName;
            }
            return fullName;
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, 
            HttpPostedFileBase ImageFileMain,
            HttpPostedFileBase ImageFileSub1,
            HttpPostedFileBase ImageFileSub2,
            HttpPostedFileBase ImageFileSub3,
            HttpPostedFileBase ImageFileSub4)
        {
            if (ModelState.IsValid)
            {
                // string fileName = null;
                var productId = new ProductDAO().getId();
                productId++;
                var resultMain = SaveImg(ImageFileMain);
                var resultSub1 = SaveImg(ImageFileSub1);
                var resultSub2 = SaveImg(ImageFileSub2);
                var resultSub3 = SaveImg(ImageFileSub3);
                var resultSub4 = SaveImg(ImageFileSub4);
                if (resultMain != null)
                {
                    product.Image = resultMain;
                    new ProductDAO().AddNewSubImages(productId, resultMain);
                }
                if (resultSub1 != null)
                {
                    new ProductDAO().AddNewSubImages(productId, resultSub1);
                }
                if (resultSub2 != null)
                {
                    new ProductDAO().AddNewSubImages(productId, resultSub2);
                }
                if (resultSub3 != null)
                {
                    new ProductDAO().AddNewSubImages(productId, resultSub3);
                }
                if (resultSub4 != null)
                {
                    new ProductDAO().AddNewSubImages(productId, resultSub4);
                }
                product.Rating = 0;
                product.CreatedDate = DateTime.Now;
                var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
                product.CreatedBy = userLogin.UserName;
                product.Code = db.Database.SqlQuery<string>("SELECT [dbo].[AUTO_CODE]()").FirstOrDefault();
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,MetaTitle,Description,Image,MoreImages,Price,PromotionPrice,IncludedVAT,Quantity,CategoryID,Detail,Warranty,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,TopHot,ViewCount")] Product product, 
            HttpPostedFileBase ImageFileMain,
            HttpPostedFileBase ImageFileSub1,
            HttpPostedFileBase ImageFileSub2,
            HttpPostedFileBase ImageFileSub3,
            HttpPostedFileBase ImageFileSub4)
        {
            if (ModelState.IsValid)
            {
                var sp = db.Products.Find(product.ID);
                var listImgFromDb = db.ProductImages
                    .Where(x => x.ProductId == product.ID).ToList();

                if (ImageFileMain != null && listImgFromDb.Count >= 1)
                {
                    var resultSub1 = SaveImg(ImageFileMain);
                    sp.Image = resultSub1;
                    listImgFromDb[0].Image = resultSub1;
                }

                if (ImageFileSub1 != null && listImgFromDb.Count >= 2)
                {
                    var resultSub1 = SaveImg(ImageFileSub1);
                    listImgFromDb[1].Image = resultSub1;
                }

                if (ImageFileSub2 != null && listImgFromDb.Count >= 3)
                {
                    var resultSub1 = SaveImg(ImageFileSub2);
                    listImgFromDb[2].Image = resultSub1;
                }

                if (ImageFileSub3 != null && listImgFromDb.Count >= 4)
                {
                    var resultSub1 = SaveImg(ImageFileSub3);
                    listImgFromDb[3].Image = resultSub1;
                }

                sp.MetaTitle = StringHelper.ToUnsignString(product.Name);
                sp.Price = product.Price;
                sp.PromotionPrice = product.PromotionPrice;
                sp.Detail = product.Detail;
                sp.Description = product.Description;
                sp.Name = product.Name;
                sp.CategoryID = product.CategoryID;
                sp.Quantity = product.Quantity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBag(product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            Product product = db.Products.Find(id);
            var list = db.ProductImages.Where(x => x.ProductId == id).ToList();
            db.ProductImages.RemoveRange(list);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var list = db.ProductCategories.ToList();
            ViewBag.CategoryID = new SelectList(list, "ID", "Name", selectedId);
        }

        [HttpGet]
        public JsonResult LoadImages(long id)
        {
            ProductDAO dao = new ProductDAO();
            return Json(new
            {
                data = dao.loadImgById(id)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveImages(List<HttpPostedFileBase> images)
        {
            //var listImages = serializer.Deserialize<List<string>>(images);
            ProductDAO dao = new ProductDAO();
            var productId = dao.getId();
            productId++;
            try
            {
              //  dao.UpdateImages(productId, listImages);
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}
