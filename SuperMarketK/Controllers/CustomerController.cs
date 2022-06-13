using Model.DAO;
using Model.EntityFramework;
using SuperMarketK.Common;
using SuperMarketK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            var dao = new UserDAO();
            var session = (UserLogin)Session[Common.CommonContants.USER_SESSION];
            if (session == null)
            {
                return HttpNotFound();
            }
            var user = dao.GetByUsername(session.UserName);
            return View(user);
        }

        [HttpPost]
        public ActionResult Save(User model)
        {
            new UserDAO().UpdateClient(model);
            return RedirectToAction("Index");
        }

        public ActionResult ChangePassword()
        {
            var dao = new UserDAO();
            var session = (UserLogin)Session[Common.CommonContants.USER_SESSION];
            var user = dao.GetByUsername(session.UserName);
            return View(user);
        }

        [HttpPost]
        public ActionResult ChangePassword(string pass, string pass1, string pass2)
        {
            ViewBag.Message = "";
            var dao = new UserDAO();
            var session = (UserLogin)Session[Common.CommonContants.USER_SESSION];
            var user = dao.GetByUsername(session.UserName);
            if (!Common.EncyptMD5.getMD5(pass).ToLower().Equals(user.Password))
            {
                ViewBag.Message = "Mật khẩu cũ không đúng";
            }
            else
            {
                if (string.IsNullOrEmpty(pass1))
                {
                    ViewBag.Message = "Hãy nhập mật khẩu mới";
                    return View();
                }
                if (pass1 != pass2)
                {
                    ViewBag.Message = "Mật khẩu xác nhận không đúng";
                    return View();
                }
                dao.changePassword(user.ID, Common.EncyptMD5.getMD5(pass1).ToLower());
                ViewBag.Message = "Đổi mật khẩu thành công";
            }
            return View();
        }

        public ActionResult OrdersHistory()
        {
            var dao = new UserDAO();
            var session = (UserLogin)Session[Common.CommonContants.USER_SESSION];
            var user = dao.GetByUsername(session.UserName);
            SuperMarketKDbContext db = new SuperMarketKDbContext();
            string query = string.Format("exec usp_LichSuMuaHang {0}", user.ID);
            var list = db.Database.SqlQuery<UserOrderModel>(query).ToList();
            return View(list);
        }
    }
}