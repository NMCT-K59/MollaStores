using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Common;
namespace Model.DAO
{
    public class UserDAO
    {
        SuperMarketKDbContext db = null;
        public UserDAO()
        {
            db = new SuperMarketKDbContext();
        }

        public List<User> getList()
        {
            return db.Users.ToList();
        }

        public void changePassword(long ID,string pass)
        {
            var userUpdate = db.Users.Find(ID);
            userUpdate.Password = pass;
            db.SaveChanges();
        }

        public long InsertAccountFB(User entity)
        {
            var user = db.Users.SingleOrDefault(x=>x.UserName == entity.UserName);
            if (user == null)
            {
                entity.Password = "NULL";
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else return user.ID;
        }

        public bool UpdateClient(User user)
        {
            try
            {
                var userUpdate = db.Users.Find(user.ID);
                userUpdate.Name = user.Name;
                userUpdate.Phone = user.Phone;
                userUpdate.Email = user.Email;
                userUpdate.Address = user.Address;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool Update(User user)
        {
            try
            {
                var userUpdate = db.Users.Find(user.ID);
                if (!string.IsNullOrEmpty(user.Password))
                {
                    userUpdate.Password = user.Password;
                }
                userUpdate.UserName = user.UserName;
                userUpdate.Name = user.Name;           
                userUpdate.Phone = user.Phone;
                userUpdate.Email = user.Email;
                userUpdate.Status = user.Status;
                userUpdate.ProvinceID = user.ProvinceID;
                userUpdate.GroupID = user.GroupID;
                userUpdate.Address = user.Address;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public IEnumerable<User> getAllPaging(string searchString,int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return db.Users.Where(x=>x.UserName.Contains(searchString)
                || x.Name.Contains(searchString)).OrderBy(x=>x.ID).ToPagedList(page, pageSize);
            }
            return db.Users.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        public User GetByID(int ID)
        {
            return db.Users.Find(ID);
        }

        public User GetByUsername(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName || x.Phone == userName || x.Email == userName);
        }
        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }


        public bool Delete(int ID)
        {
            try
            {
                var user = db.Users.Find(ID);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
      
        }

        public bool LoginClient(string username, string password)
        {
            return db.Users.Where(x => x.GroupID == Common.Contants.MEMBER_GROUP)
                .Count(x => (x.UserName == username || x.Phone == username || x.Email == username) && x.Password == password) > 0;
        }

        public string getEmailByUserName(string username)
        {
            return db.Users.Where(x => x.UserName == username).Select(x => x.Email).FirstOrDefault();
        }

        public List<string> getCredentialByUserName(string username)
        {
            var user = db.Users.Single(x => x.UserName == username);
            var data = (from a in db.Credentials
                       join b in db.UserGroups
                       on a.UserGroupID equals b.ID
                       join c in db.Roles
                       on a.RoleID equals c.ID
                       where b.ID == user.GroupID
                       select new 
                       {
                           RoleID = a.RoleID,
                           UserGroupID = a.UserGroupID
                       }).AsEnumerable().Select(x=>new Credential() {
                           RoleID = x.RoleID,
                           UserGroupID = x.UserGroupID
                       });
            return data.Select(x=>x.RoleID).ToList();
        }

        public bool Login(string username, string password)
        {
            return db.Users.Where(x => x.GroupID == Common.Contants.ADMIN_GROUP || x.GroupID == Common.Contants.MOD_GROUP)
                .Count(x => x.UserName == username && x.Password ==  password) > 0;
        }

        public bool CheckUserName(string username)
        {
            return db.Users.Count(x => x.UserName == username) > 0;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email || x.Phone == email) > 0;
        }

        public void updateRecoveryCode(string email, string code)
        {
            db.Users.Where(x => x.Email == email || x.Phone == email).FirstOrDefault().CodeRecovery = code;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            
        }
    }
}
