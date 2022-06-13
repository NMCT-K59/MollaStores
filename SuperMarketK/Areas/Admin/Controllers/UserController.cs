using Model.DAO;
using Model.EntityFramework;
using SuperMarketK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using OfficeOpenXml;
using System.Drawing;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string keyword, int page = 1, int pageSize = 5)
        {
            ViewBag.keyword = keyword;
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            return View(new UserDAO().getAllPaging(keyword, page, pageSize));
        }

        // GET: Admin/User/Details/5
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Details(int id)
        {
            return View(new UserDAO().GetByID(id));
        }

        // GET: Admin/User/Create
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDAO();
                    user.Password = EncyptMD5.getMD5(user.Password);
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Thêm không thành công.");
                    }
                }
                SetViewBag(user.GroupID);
                return View(user);
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Edit/5
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var user = new UserDAO().GetByID(id);
            SetViewBag(user.GroupID);
            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDAO();
                    if (user.Password.Equals("MacDinh") && !string.IsNullOrEmpty(user.Password))
                    {
                        user.Password = EncyptMD5.getMD5(user.Password);
                    }
                    var result = dao.Update(user);
                    if (result)
                    {
                        setAlert("Sửa thành công.", "success");
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sửa không thành công.");
                    }
                }
                SetViewBag(user.GroupID);
                return View(user);
            }
            catch
            {
                return View();
            }
        }

        // POST: Admin/User/Delete/5
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            try
            {
                new UserDAO().Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public void SetViewBag(string selectID = null)
        {
            var dao = new UserGroupDAO();
            ViewBag.GroupID = new SelectList(dao.getList(), "ID", "Name", selectID);
            var listStatusSelected = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Kích hoạt", Value = "true", Selected = true },
                    new SelectListItem { Text = "Vô hiệu hóa", Value = "false" },
                };
            ViewBag.listStatusSelected = listStatusSelected;
        }

        [HttpGet]
        public void Export()
        {
            var listUser = new UserDAO().getList();
            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
 
            ws.Cells["A6"].Value = "ID";
            ws.Cells["B6"].Value = "UserName";
            ws.Cells["C6"].Value = "GroupID";
            ws.Cells["D6"].Value = "Name";
            ws.Cells["E6"].Value = "Address";
            ws.Cells["F6"].Value = "Email";
            ws.Cells["G6"].Value = "Phone";
            ws.Cells["H6"].Value = "CreatedDate";

            int rowStart = 7;
            foreach (var item in listUser)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.UserName;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.GroupID;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.Name;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.Address;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Email;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Phone;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.CreatedDate;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();


        }
    }
}
