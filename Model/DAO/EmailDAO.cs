using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class EmailDAO
    {
        SuperMarketKDbContext db = null;
        public EmailDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public int Insert(Email email)
        {
            if (db.Emails.Count(x=>x.Email_ID == email.Email_ID) > 0)
            {
                return 1;
            }
            db.Emails.Add(email);
            db.SaveChanges();
            return 1;
        }
    }
}
