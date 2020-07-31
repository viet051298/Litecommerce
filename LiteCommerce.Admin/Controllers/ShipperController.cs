using LiteCommerce.BussinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ADMINISTRATOR)]
    public class ShipperController : Controller
    {
        /// <summary>
        /// Quản lý Shipper
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1,string searchValue = "")
        {
            var model = new Models.ShipperPaginationResult()
            {
                Page = page,
                RowCount = CatalogBLL.Shippers_Count(searchValue),
                PageSize = AppSettings.DefaultPageSize,
                Data = CatalogBLL.Shipper_List(page, AppSettings.DefaultPageSize, searchValue)
            };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Shipper";
                Shipper newShipper = new Shipper();
                newShipper.ShipperID = 0;
                return View(newShipper);
            }
            else
            {
                try
                {
                    ViewBag.Title = "Edit Shipper";
                    Shipper editShipper = CatalogBLL.Shipper_Get(Convert.ToInt32(id));
                    if (editShipper == null)
                        return RedirectToAction("Index");
                    return View(editShipper);
                }
                catch (FormatException)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Input(Shipper model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.CompanyName))
                {
                    ModelState.AddModelError("CompanyName", "Company Name is required");
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    model.Phone = "";
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.ShipperID == 0 ? "Add New Shipper" : "Edit Shipper";
                    return View(model);
                }

                if (model.ShipperID == 0)
                {
                    int supplierID = CatalogBLL.Shipper_Add(model);
                }
                else
                {
                    bool updateResult = CatalogBLL.Shipper_Update(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " : " + ex.StackTrace);
                return View(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string methods = "", int[] shipperIDs = null)
        {
            if(shipperIDs != null)
            {
                CatalogBLL.Shipper_Delete(shipperIDs);
            }
            return RedirectToAction("Index");
        }
    }
}