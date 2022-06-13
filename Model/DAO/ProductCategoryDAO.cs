using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductCategoryDAO
    {
        SuperMarketKDbContext db = null;
        public ProductCategoryDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<ProductCategory> getList()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<ProductCategory> getAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.ProductCategories.Where(x => x.Name.Contains(searchString)
                ).OrderBy(x => x.ID).ToPagedList(page, pageSize);
            }
            return db.ProductCategories.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
