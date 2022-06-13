using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class FooterDAO
    {
        SuperMarketKDbContext db = null;
        public FooterDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public Footer getFooter()
        {
            return db.Footers.FirstOrDefault(x => x.Status == true);
        }
    }
}
