using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContactDAO
    {
        SuperMarketKDbContext db = null;
        public ContactDAO()
        {
            db = new SuperMarketKDbContext();

        }

        public bool Insert(Feedback contact)
        {
            db.Feedbacks.Add(contact);
            db.SaveChanges();
            return contact.ID > 0;
        }
    }
}
