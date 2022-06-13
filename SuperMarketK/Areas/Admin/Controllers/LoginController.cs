using Model.DAO;
using SuperMarketK.Areas.Admin.Models;
using SuperMarketK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var userDAO = new UserDAO();
                var result = userDAO.Login(loginModel.userName, EncyptMD5.getMD5(loginModel.passWord));
                if (result)
                {
                    var user = userDAO.GetByUsername(loginModel.userName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.Name = user.Name;
                    userSession.GroupID = user.GroupID;
                    ViewBag.GroupIDStatus = user.GroupID;
                    var listCredential = userDAO.getCredentialByUserName(loginModel.userName);
                    Session.Add(CommonContants.SESSION_CREDENTIALS, listCredential);
                    Session.Add(CommonContants.USER_SESSION, userSession);
                    return RedirectToAction("Index","Home");
                }
                else ModelState.AddModelError("", "Thông tin đăng nhập không đúng");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonContants.USER_SESSION] = null;
            return View("Index");
        }

    }
}