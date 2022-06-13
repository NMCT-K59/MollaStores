using Model.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class VoucherDAO
    {
        SuperMarketKDbContext db = null;
        public VoucherDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public IEnumerable<Vouchers> getAllPaging(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Vouchers.Where(x => x.description.Contains(searchString)
                || x.code.Contains(searchString)).OrderBy(x => x.id).ToPagedList(page, pageSize);
            }
            return db.Vouchers.OrderBy(x=>x.id).ToPagedList(page, pageSize);
        }
    }
}
