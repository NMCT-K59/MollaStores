using Model.DAO;
using Model.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketK.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult InsertMail(string email)
        {
            var model = new Email();
            model.Email_ID = email;
            if (new EmailDAO().Insert(model) > 0)
            {
                return Json(new
                {
                    status = true
                });
            } else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpPost]
        public JsonResult Insert(string name, string email, string sdt, string diachi, string tinnhan)
        {
            var model = new Feedback();
            model.Name = name;
            model.Phone = sdt;
            model.Email = email;
            model.Content = tinnhan;
            model.Address = diachi;
            model.CreatedDate = DateTime.Now;
            if (new ContactDAO().Insert(model))
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
            
        }
    }
}