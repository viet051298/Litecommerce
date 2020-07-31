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
    public class SupplierController : Controller
    {
        /// <summary>
        /// Trang hiển thị danh sách các Supplier, các "liên kết" đến chức năng liên quan
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.SupplierPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Supplier_Count(searchValue),
                SearchValue = searchValue,
                Data = CatalogBLL.Supplier_List(page, AppSettings.DefaultPageSize, searchValue)
            };
            return View(model);
            /*
            var listOfSuppliers = CatalogBLL.Supplier_List(page, 10, searchValue);
            int rowCount = CatalogBLL.Supplier_Count(searchValue);
            ViewBag.RowCount = rowCount;
            */
        }

        /// <summary>
        /// Add or Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add new Supplier";
                Supplier newSupplier = new Supplier();
                newSupplier.SupplierID = 0;
                return View(newSupplier);
            }
            else
            {
                try
                {
                    ViewBag.Title = "Edit Supplier";
                    Supplier editSupplier = CatalogBLL.Supplier_Get(Convert.ToInt32(id));
                    if (editSupplier == null)
                        return RedirectToAction("Index");
                    return View(editSupplier);
                }
                catch(FormatException)
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
        public ActionResult Input(Supplier model)
        {
            try
            {
                // Check Value
                if (string.IsNullOrEmpty(model.CompanyName))
                {
                    ModelState.AddModelError("CompanyName", "Company Name is required");
                }
                if (string.IsNullOrEmpty(model.ContactName))
                {
                    ModelState.AddModelError("ContactName", "Contact Name is required");
                }
                if (string.IsNullOrEmpty(model.ContactTitle))
                {
                    ModelState.AddModelError("ContactTitle", "Contact Title is required");
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    model.Address = "";
                }
                if (string.IsNullOrEmpty(model.City))
                {
                    model.City = "";
                }
                if (string.IsNullOrEmpty(model.Country) || model.Country == "NoSelect")
                {
                    ModelState.AddModelError("Country", "Country is required");
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    ModelState.AddModelError("Phone", "Phone is required");
                }
                if (string.IsNullOrEmpty(model.Fax))
                {
                    model.Fax = "";
                }
                if (string.IsNullOrEmpty(model.HomePage))
                {
                    model.HomePage = "";
                }

                if (!ModelState.IsValid)
                {
                    // Return True : Valid
                    // Return False : No valid
                    ViewBag.Title = model.SupplierID == 0 ? "Add New Supplier" : "Edit Supplier";
                    return View(model);
                }

                //  DB
                if (model.SupplierID == 0)
                {
                    int supplierID = CatalogBLL.Supplier_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Supplier_Update(model);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " : " + ex.StackTrace);
                return View(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methods"></param>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string methods = "", int[] supplierIDs = null)
        {
            if(supplierIDs != null)
            {
                CatalogBLL.Supplier_Delete(supplierIDs);
            }
            return RedirectToAction("Index");
        }
    }
}