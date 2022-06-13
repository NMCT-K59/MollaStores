using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
using Model.EntityFramework;
using OfficeOpenXml;
using SuperMarketK.Common;

namespace SuperMarketK.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private SuperMarketKDbContext db = new SuperMarketKDbContext();
        private OrderDAO orderDao = new OrderDAO();
        // GET: Admin/Orders
        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Index(string keyword, int page = 1, int pageSize = 10)
        {
            ViewBag.keyword = keyword;
            var userLogin = (SuperMarketK.Common.UserLogin)Session["USER_SESSION"];
            ViewBag.GroupID = userLogin.GroupID;
            return View(orderDao.ListOrder(keyword, page, pageSize));
        }
        [HasCredential(RoleID = "VIEW_ORDER")]
        // GET: Admin/Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = new OrderDAO().getListOrderDetail(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            SetViewBag();
            return View(order);
        }

        [HasCredential(RoleID = "EDIT_ORDER")]
        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            SetViewBag(order.Status.ToString());
            SetStatusDeli(order.StatusDelivery.ToString());
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_ORDER")]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var dao = new OrderDAO();
                dao.Update(order);
                return RedirectToAction("Index");
            }
            SetViewBag(order.Status.ToString());
            SetStatusDeli(order.StatusDelivery.ToString());
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_ORDER")]
        public ActionResult Delete(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SetViewBag(string selectID = null)
        {
            var listStatusSelected = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Đã thanh toán", Value = "1"},
                    new SelectListItem { Text = "Chưa thanh toán", Value = "0" },
                };



            foreach (var item in listStatusSelected)
            {
                if (item.Value.Equals(selectID))
                {
                    item.Selected = true;
                    break;
                }
            }
            ViewBag.listStatusSelected = listStatusSelected;

        }
        public void SetStatusDeli(string selectID = null)
        {
            var listStatusDeliverySelected = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Đặt hàng thành công", Value = "0"},
                    new SelectListItem { Text = "Kiểm hàng và đóng gói", Value = "1" },
                    new SelectListItem { Text = "Chuyển cho shipper", Value = "2" },
                    new SelectListItem { Text = "Đã giao hàng", Value = "3" }
                };

            foreach (var item in listStatusDeliverySelected)
            {
                if (item.Value.Equals(selectID))
                {
                    item.Selected = true;
                    break;
                }
            }
            ViewBag.listStatusDeliverySelected = listStatusDeliverySelected;
        }

        public void Export()
        {
            var listUser = new OrderDAO().getList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A6"].Value = "ID";
            ws.Cells["B6"].Value = "CustomerID";
            ws.Cells["C6"].Value = "Name";
            ws.Cells["D6"].Value = "Phone";
            ws.Cells["E6"].Value = "Address";
            ws.Cells["F6"].Value = "Email";
            ws.Cells["G6"].Value = "Status";
            ws.Cells["H6"].Value = "StatusDelivery";

            int rowStart = 7;
            foreach (var item in listUser)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ID;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.CustomerID;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.ShipName;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ShipMobile;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.ShipAddress;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.ShipEmail;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.Status;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.StatusDelivery;
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
