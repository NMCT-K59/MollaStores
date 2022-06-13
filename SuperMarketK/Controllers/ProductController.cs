using Model.DAO;
using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string keyword)
        {
            ViewBag.ListProductByKeyword = new ProductDAO().getProductByKeyword(keyword);
            ViewBag.Keyword = keyword;
            return View();
        }

        public ActionResult Category(string metatitle)
        {
            var dao = new ProductDAO().getCategory(metatitle);
            ViewBag.Keyword = metatitle;
            ViewBag.ListProductByID = new ProductDAO().getProductByCategoryID(dao.ID);
            return View(dao);
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDAO().getList();
            return PartialView(model);
        }

        // [OutputCache(Duration = int.MaxValue, VaryByParam ="id")]
        public ActionResult Detail(long id)
        {
            ViewBag.ListReview = new ProductDAO().getListReview(id);
            var session = (SuperMarketK.Common.UserLogin)Session[SuperMarketK.Common.CommonContants.USER_SESSION];
            if (session != null)
            {
                ViewBag.IsBuyProduct = new ProductDAO().checkBuyProduct(session.UserID, id);
            }
            ViewBag.ListRecommend = new ProductDAO().getListProductRecommend(id, 5);
            return View(new ProductDAO().getProductImageByID(id));
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDAO().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet); 
        }

        [HttpPost]
        public ActionResult CreateReview(long productID, string Title, string Review, int Rating)
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Review) || Rating == 0)
            {
                return null;
            }
            var session = (SuperMarketK.Common.UserLogin)Session[SuperMarketK.Common.CommonContants.USER_SESSION];
            ReviewProduct model = new ReviewProduct()
            {
                ProductId = productID,
                Title = Title,
                Comment = Review,
                UserId = session.UserID,
                Rating = Rating,
                CreatedTime = DateTime.Now,
            };
            var userName = "User default";
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                db.ReviewProducts.Add(model);
                db.SaveChanges();
                userName = db.Users.Where(x => x.ID == session.UserID).FirstOrDefault().Name;
                // update rating product
                var product = db.Products.Where(x => x.ID == productID).FirstOrDefault();
                product.Rating = db.ReviewProducts.Average(x => x.Rating);
                db.SaveChanges();
            }
            return Json(new { data = model, Name = userName }, JsonRequestBehavior.AllowGet);
        }

    }
}