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
    public class CustomerController : Controller
    {
        /// <summary>
        /// Quản lý 
        /// </summary>
        /// <returns></returns>
        // GET: Customer
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.CustomerPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Customer_Count(searchValue),
                Data = CatalogBLL.Customer_List(page, AppSettings.DefaultPageSize, searchValue)
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
            List<Countries> listCountry = CatalogBLL.Country_List();
            ViewBag.ListCountry = listCountry;
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Add New Customer";
                Customer newCustomer = new Customer();
                return View(newCustomer);
            }
            else
            {
                try
                {
                    ViewBag.Title = "Edit Customer";
                    Customer editCustomer = CatalogBLL.Customer_Get(id);
                    if (editCustomer == null)
                        return RedirectToAction("Index");
                    return View(editCustomer);
                }
                catch(FormatException)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult Input(Customer model)
        {
            try
            {
                // Check Value
                if (string.IsNullOrEmpty(model.CustomerID) || model.CustomerID.Length > 5)
                {
                    ModelState.AddModelError("CustomerID", "CustomerID is required");
                }
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
                if (string.IsNullOrEmpty(model.Country) || model.Country == "NoSelect")
                {
                    ModelState.AddModelError("Country", "Country is required");
                }
                if (string.IsNullOrEmpty(model.Phone))
                {
                    ModelState.AddModelError("Phone", "Phone is required");
                }
                if (string.IsNullOrEmpty(model.Address))
                {
                    model.Address = "";
                }
                if (string.IsNullOrEmpty(model.City))
                {
                    model.City = "";
                }
                if (string.IsNullOrEmpty(model.Fax))
                {
                    model.Fax = "";
                }


                if (!ModelState.IsValid)
                {
                    // Return True : Valid
                    // Return False : No valid
                    ViewBag.Title = model.CustomerID == "" ? "Add New Customer" : "Edit Customer";
                    return View(model);
                }

                //  DB
                // TODO 
                if (Request.Form["typeInput"] == "Add New Customer") 
                {
                    string customerID = CatalogBLL.Customer_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateResult = CatalogBLL.Customer_Update(model);
                    return RedirectToAction("Index");
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " : " + ex.StackTrace);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Delete(string methods = "", string[] customerIDs = null)
        {
            if(customerIDs != null)
            {
                CatalogBLL.Customer_Delete(customerIDs);
            }
            return RedirectToAction("Index");
        }
    }
}