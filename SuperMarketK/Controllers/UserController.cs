using BotDetect.Web.Mvc;
using Common;
using Facebook;
using Model.DAO;
using Model.EntityFramework;
using SuperMarketK.Common;
using SuperMarketK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vonage.Request;
using Vonage;

namespace SuperMarketK.Controllers
{
    public class UserController : Controller
    {
        public Uri RedirecUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Mã xác nhận không chính xác")]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.Name = model.Name;
                    user.Password = Common.EncyptMD5.getMD5(model.PassWord);
                    user.Phone = model.Phone;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    user.GroupID = "MEMBER";
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng kí thành công";
                        ModelState.AddModelError("", "Đăng kí thành công");
                        model = new Register();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(model);
        }

        public void GoogleLogin(string email, string name)
        {
            //Write your code here to access these paramerters
            User user = new User();
            user.Email = email;
            user.UserName = email;
            user.Status = true;
            user.GroupID = "MEMBER";
            user.Name = name;
            user.CreatedDate = DateTime.Now;
            var resultInsert = new UserDAO().InsertAccountFB(user);
            if (resultInsert > 0)
            {
                var userClientSession = new UserLogin();
                userClientSession.Name = name;
                userClientSession.UserID = resultInsert;
                userClientSession.GroupID = "MEMBER";
                userClientSession.UserName = email;
                Session.Add(CommonContants.USER_SESSION, userClientSession);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginURL = fb.GetLoginUrl(new
            {
                client_id = "1895084803982863",
                client_secret = "ac0c9d1ff89eb16c5d409f942b85a2af",
                redirect_uri = RedirecUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginURL.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "1895084803982863",
                client_secret = "ac0c9d1ff89eb16c5d409f942b85a2af",
                redirect_uri = RedirecUri.AbsoluteUri,
                code = code,
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name, middle_name, last_name,id,email");
                string email = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                string UserName = me.email;
                User user = new User();
                user.Email = email;
                user.UserName = UserName;
                user.Status = true;
                user.GroupID = "MEMBER";
                user.Name = firstName + " " + middleName + " " + lastName;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDAO().InsertAccountFB(user);
                if (resultInsert > 0)
                {
                    var userClientSession = new UserLogin();
                    userClientSession.Name = user.Name;
                    userClientSession.UserID = resultInsert;
                    userClientSession.GroupID = "MEMBER";
                    userClientSession.UserName = user.UserName;
                    Session.Add(CommonContants.USER_SESSION, userClientSession);
                }
            }
            return Redirect("/");
        }
        // /User/Login httpPost
        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                var result = dao.LoginClient(model.UserName, Common.EncyptMD5.getMD5(model.PassWord));
                if (result)
                {
                    var user = dao.GetByUsername(model.UserName);
                    var userClientSession = new UserLogin();
                    userClientSession.Name = user.Name;
                    userClientSession.UserName = user.UserName;
                    userClientSession.UserID = user.ID;
                    userClientSession.GroupID = "MEMBER";
                    Session.Add(CommonContants.USER_SESSION, userClientSession);
                    return Redirect("/");
                }
                else
                {
                    ViewBag.MessageError = "Username or password is incorrect";
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string message = "";
            if (new UserDAO().CheckEmail(EmailID))
            {
                string resetCode;
                if (EmailID.Contains("@"))
                {
                    resetCode = Guid.NewGuid().ToString();
                    var link = "/User/ResetPassword/" + resetCode;
                    var link2 = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, link);
                    string subject = "Reset Password";
                    string body = "Vui lòng click vào link để đặt lại mật khẩu: <a href=" + link2 + ">Reset password</a>";
                    new MailHelper().SendMail(EmailID, subject, body);
                    message = "Please check your email for change password";
                    new UserDAO().updateRecoveryCode(EmailID, resetCode);
                }
                else
                {
                    System.Random random = new Random();
                    resetCode = random.Next(1000, 9999).ToString();
                    new UserDAO().updateRecoveryCode(EmailID, resetCode);
                    var credentials = Credentials.FromApiKeyAndSecret(
                        "63b22531",
                        "VvKIuXeVTySk7QrG"
                    );
                    var VonageClient = new VonageClient(credentials);

                    var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
                    {
                        To = "84367717714",
                        From = "Test API",
                        Text = resetCode
                    });
                    return RedirectToAction("ResetPassword");
                }
            }
            else
            {
                message = "Email hoặc số điện thoại này chưa được đăng kí";
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id = null)
        {
            using (SuperMarketKDbContext db = new SuperMarketKDbContext())
            {
                var user = db.Users.Where(x => x.CodeRecovery == id).FirstOrDefault();
                if (user != null)
                {
                    ForgotPasswordViewModel model = new ForgotPasswordViewModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        public ActionResult ResetPassword(ForgotPasswordViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (SuperMarketKDbContext db = new SuperMarketKDbContext())
                {
                    var user = db.Users.Where(x => x.CodeRecovery == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Common.EncyptMD5.getMD5(model.ConfirmPassword);
                        user.CodeRecovery = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Thay đổi mật khẩu thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mã OTP không đúng");
                        return View(model);
                    }
                }
            }
            else
            {
                message = "Có lỗi xảy ra";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult Logout()
        {
            Session[Common.CommonContants.USER_SESSION] = null;
            return Redirect("/");
        }
    }
}