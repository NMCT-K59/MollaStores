using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class MenuDAO
    {
        SuperMarketKDbContext db = null;
        public MenuDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<Menu> getList(int typeID)
        {
            return db.Menus.Where(x=>x.TypeID == typeID).ToList();
        }

        public IEnumerable<Menu> getAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Menus.Where(x => x.Text.Contains(searchString)
                || x.Link.Contains(searchString)).OrderBy(x => x.ID).ToPagedList(page, pageSize);
            }
            return db.Menus.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
