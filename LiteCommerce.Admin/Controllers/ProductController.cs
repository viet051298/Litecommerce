using LiteCommerce.BussinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.ADMINISTRATOR)]
    public class ProductController : Controller
    {
        /// <summary>
        /// Quản lý sản phẩm
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "", string Category = "", string Supplier = "")
        {
            var model = new Models.ProductPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Product_Count(searchValue, Category, Supplier),
                SearchValue = searchValue,
                Category = Category,
                Supplier = Supplier,
                Data = CatalogBLL.Product_List(page, AppSettings.DefaultPageSize, searchValue, Supplier, Category)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Input(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.Title = "Create New Product";
                Product newProduct = new Product()
                {
                    ProductID = 0,
                    PhotoPath = "upload.jpg"
                };
                return View(newProduct);
            }
            else
            {
                ViewBag.Title = "Edit a Product";
                Product editProduct = CatalogBLL.Product_Get(Convert.ToInt32(id));
                if (editProduct == null)
                    return RedirectToAction("Index");
                return View(editProduct);
            }
        }

        [HttpPost]
        public ActionResult Input(Product model, HttpPostedFileBase uploadFile)
        {
            if (string.IsNullOrEmpty(model.ProductName))
                ModelState.AddModelError("ProductName", "ProductName Expected");
            if (model.CategoryID.ToString() == "0")
                ModelState.AddModelError("CategoryID", "CategoryName Expected");
            if (model.SupplierID.ToString() == "0")
                ModelState.AddModelError("SupplierID", "SupplierName Expected");
            if (model.UnitPrice == 0)
                ModelState.AddModelError("UnitPrice", "UnitPrice Expected");
            if (string.IsNullOrEmpty(model.QuantityPerUnit))
                model.QuantityPerUnit = "";
            if (string.IsNullOrEmpty(model.Descriptions))
                model.Descriptions = "";

            if (uploadFile != null)
            {
                string filePath = Path.Combine(Server.MapPath("~/Images/Products"), Guid.NewGuid() + uploadFile.FileName);
                // Upload file lên server
                uploadFile.SaveAs(filePath);
                // Save to DB
                model.PhotoPath = filePath.Split('\\')[filePath.Split('\\').Length - 1];
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Create new Product" : "Edit a Product";
                return View(model);
            }

            if (model.ProductID == 0)
            {
                CatalogBLL.Product_Add(model);
            }
            else
            {
                CatalogBLL.Product_Update(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int[] productIDs = null)
        {
            if (productIDs != null)
            {
                CatalogBLL.Product_Delete(productIDs);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddAttribute(string id = "")
        {
            ViewBag.Title = "Create New ProductAttribute";
            ProductAttribute newproductAttribute = new ProductAttribute()
            {
                AttributeID = 0,
                ProductID = Convert.ToInt32(id),
            };
            return View(newproductAttribute);
        }

        


        public ActionResult Detailt(string id = "")
        {
            ViewBag.Title = "Detailt Product";
            return View();
        }
    }
}