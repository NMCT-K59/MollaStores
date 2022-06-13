using Model.EntityFramework;
using Model.ViewModel;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xNet;
using System.IO;

namespace Model.DAO
{
    public class ResultResponse<T>
    {
        public bool status { get; set; } = false;
        public string message { get; set; }
        public T ResultObj { get; set; }
    }
    public class ProductDAO
    {
        SuperMarketKDbContext db = null;
        public ProductDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<ProductImage> loadImgById(long idProduct)
        {
            return db.ProductImages.Where(x => x.ProductId == idProduct).OrderBy(x=>x.Id).Skip(1).ToList();
        }

        public void DeleteImgExitst(long idProduct)
        {
            var list = db.ProductImages.Where(x=>x.ProductId == idProduct).ToList();
            db.ProductImages.RemoveRange(list);
            db.SaveChanges();
        }

        public void AddNewSubImages(long idProduct, string img)
        {
            var model = new ProductImage()
            {
                ProductId = idProduct,
                Description = "img",
                Image = img
            };
            db.ProductImages.Add(model);
            db.SaveChanges();
        }

        public List<ProductViewModel> getListProductRecommend(long productId, int top)
        {
            HttpRequest http = new HttpRequest();
            var data = new
            {
                id = productId,
                top = top,
            };
            var dataString = JsonConvert.SerializeObject(data);
            dynamic json = JsonConvert.DeserializeObject(http.Post("https://utc2api.azurewebsites.net/api/Recommenders", dataString, "application/json; charset=UTF-8").ToString());
            var listResult = new List<ProductViewModel>();
            foreach (var item in json)
            {
                long id = item.Value;
                var product = db.Products.Where(x => x.ID == id).FirstOrDefault();
                if (product != null)
                {
                    var productRecommend = new ProductViewModel()
                    {
                        ID = product.ID,
                        Name = product.Name,
                        Image = product.Image,
                        Detail = product.Detail,
                        Price = product.Price,
                        PromotionPrice = product.PromotionPrice,
                        CategoryName = db.ProductCategories
                        .Where(x => x.ID == product.CategoryID)
                        .FirstOrDefault().Name,
                        Rating = product.Rating,
                        CountRating = db.ReviewProducts
                        .Where(x => x.ProductId == product.ID).Count()
                    };
                    listResult.Add(productRecommend);
                }
            }
            return listResult;
        }

        public void updateUses(string voucher)
        {
            var voucherModel = db.Vouchers.Where(x => x.code == voucher).FirstOrDefault();
            voucherModel.uses++;
            db.SaveChanges();
        }

        public ResultResponse<Vouchers> checkVoucher(string voucher, decimal? totalSum, long userId)
        {
            var voucherModel = db.Vouchers.Where(x => x.code == voucher).FirstOrDefault();
            var query = from v in db.Vouchers
                        join uv in db.UserVouchers
                        on v.id equals uv.VoucherId
                        where uv.UserId == userId
                        select new { v, uv };
            if (voucherModel == null)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "This voucher does not exists!"
                };
            }
            if (query.Count() > 0 && query.FirstOrDefault().uv.Used > query.FirstOrDefault().v.max_uses_user)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "This voucher does not exists!"
                };
            }
            if (totalSum < voucherModel.condition)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "Minimum order value has not been reached"
                };
            }
            if (voucherModel.uses >= voucherModel.max_uses)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "you have exceeded the number of uses"
                };
            }
            if (DateTime.Compare(voucherModel.expires_at, DateTime.Now) < 0)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "This voucher out of date"
                };
            }
            if (DateTime.Compare(voucherModel.starts_at, DateTime.Now) > 0)
            {
                return new ResultResponse<Vouchers>()
                {
                    message = "This voucher not yet used date"
                };
            }
            return new ResultResponse<Vouchers>()
            {
                status = true,
                ResultObj = voucherModel,
            };
        }

        public bool checkBuyProduct(long userId, long productId)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails
                        on o.ID equals od.OrderID
                        where o.CustomerID == userId && od.ProductID == productId
                        select o;
            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<ReviewProductViewModel> getListReview(long productId)
        {
            var query = from user in db.Users
                        join review in db.ReviewProducts
                        on user.ID equals review.UserId
                        where review.ProductId == productId
                        select new ReviewProductViewModel()
                        {
                            Id = review.Id,
                            UserId = review.UserId,
                            UserName = user.Name,
                            Title = review.Title,
                            Comment = review.Comment,
                            Rating = review.Rating,
                            CreatedTime = review.CreatedTime,
                        };
            return query.OrderBy(x => x.CreatedTime).ToList();
        }

        public long getId()
        {
            return db.Products.Max(x => x.ID);
        }

        public List<Product> getList(int top)
        {
            return db.Products.Where(x => x.Quantity > 0).OrderBy(x => x.CreatedDate).Take(top).ToList();
        }

        public List<ProductViewModel> getListProductCategory()
        {
            return db.Database.SqlQuery<ProductViewModel>("select p.ID, p.Name, p.Rating, pc.Name as 'CategoryName', p.MetaTitle, Description, Image, Price, PromotionPrice, Quantity, T.CountRating, p.CategoryID, H.TotalBuy from [Product] p left join ProductCategory pc on p.CategoryID = pc.ID left join (select ProductId, count(ProductId) as CountRating from ReviewProduct RP group by ProductId) T on T.ProductId = p.ID left join (select od.ProductId, sum(quantity) as 'TotalBuy' from [order] o join [OrderDetail] od on o.Id = od.OrderId group by od.ProductId) H on H.ProductID = p.ID order by H.TotalBuy desc").ToList();
        }

        public List<ProductViewModel> getListOnSale()
        {
            return db.Database.SqlQuery<ProductViewModel>("select p.ID, p.Name, p.Rating, pc.Name as 'CategoryName', p.MetaTitle, Description, Image, Price, PromotionPrice, Quantity, T.CountRating from [Product] p left join ProductCategory pc on p.CategoryID = pc.ID left join (select ProductId, count(ProductId) as CountRating from ReviewProduct RP group by ProductId) T on T.ProductId = p.ID where p.PromotionPrice > 0").Take(10).ToList();
        }

        public List<ProductViewModel> getListTopRating()
        {
            string query = "select p.ID, p.Name, p.Rating, pc.Name as 'CategoryName', p.MetaTitle, Description, Image, Price, PromotionPrice, Quantity, T.CountRating from [Product] p left join ProductCategory pc on p.CategoryID = pc.ID left join (select ProductId, count(ProductId) as CountRating from ReviewProduct RP group by ProductId) T on T.ProductId = p.ID order by Rating desc";
            return db.Database.SqlQuery<ProductViewModel>(query).Take(10).ToList();
        }

        public List<Product> getListHot()
        {
            List<long> list = db.Database.SqlQuery<long>("select top 4 b.ProductID from [Order] a,[OrderDetail] b,[Product] c where a.ID = b.OrderID and b.ProductID = c.id group by b.ProductID  order by count(*) desc").ToList();
            return db.Products.Where(x => list.Contains(x.ID)).ToList();
        }

        public List<Product> getRecommend()
        {
            return db.Products.Where(x => x.Quantity > 0).OrderBy(x => Guid.NewGuid()).Take(4).ToList();
        }

        public ProductCategory getCategory(string metatile)
        {
            return db.ProductCategories.Single(x => x.MetaTitle == metatile);
        }

        public List<ProductViewModel> getProductByKeyword(string keyword)
        {
            return db.Database.SqlQuery<ProductViewModel>($"select p.ID, p.Name, p.Rating, pc.Name as 'CategoryName', p.MetaTitle, Description, Image, Price, PromotionPrice, Quantity from [Product] p left join ProductCategory pc on p.CategoryID = pc.ID where p.Name like N'%{keyword}%'").ToList();
        }

        public List<Product> getProductByCategoryID(long id)
        {
            return db.Products.Where(x => x.CategoryID == id).ToList();
        }

        public List<Product> getProductByCateID(long idCurrent)
        {
            var idGroup = getProductByID(idCurrent);
            return db.Products.Where(x => x.CategoryID == idGroup.CategoryID && x.ID != idCurrent).Take(4).ToList();
        }

        public ProductViewModel getProductImageByID(long id)
        {
            var product = db.Products.Find(id);
            var model = new ProductViewModel()
            {
                ID = product.ID,
                Name = product.Name,
                Description = product.Description,
                MetaTitle = product.MetaTitle,
                Image = product.Image,
                Price = product.Price,
                PromotionPrice = product.PromotionPrice,
                Detail = product.Detail,
                Quantity = product.Quantity,
                Rating = product.Rating,
                MetaDescriptions = product.MetaDescriptions,
                ListImages = db.ProductImages.Where(x => x.ProductId == id).ToList(),
                CategoryName = db.ProductCategories.Where(x => x.ID == product.CategoryID).FirstOrDefault().Name
            };
            return model;
        }

        public Product getProductByID(long id)
        {
            return db.Products.Find(id);
        }

        public List<string> ListName(string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public IEnumerable<Product> getAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Products.Where(x => x.Name.Contains(searchString)
                || x.Code.Contains(searchString)).OrderBy(x => x.ID).ToPagedList(page, pageSize);
            }
            return db.Products.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        public int descQuantity(long idProduct, int quanTity)
        {
            var product = db.Products.Find(idProduct);
            product.Quantity = product.Quantity - quanTity;
            return db.SaveChanges();
        }
    }
}
