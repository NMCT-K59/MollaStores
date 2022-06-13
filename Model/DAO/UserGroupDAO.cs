using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class UserGroupDAO
    {
        private SuperMarketKDbContext db = null;
        public UserGroupDAO()
        {
            db = new SuperMarketKDbContext();
        }
        public List<UserGroup> getList()
        {
            return db.UserGroups.ToList();
        }
    }
}
