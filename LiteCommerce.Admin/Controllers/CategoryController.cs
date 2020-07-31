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
    public class CategoryController : Controller
    {
        /// <summary>
        /// Quản lý danh mục
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.CategoryPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Category_Count(searchValue),
                Data = CatalogBLL.Category_List(page, AppSettings.DefaultPageSize, searchValue)
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
                ViewBag.Title = "Add new Category";
                Category newCategory = new Category();
                newCategory.CategoryID = 0;
                return View(newCategory);
            }
            else
            {
                try
                {
                    ViewBag.Title = "Edit Category";
                    Category editCategory = CatalogBLL.Category_Get(Convert.ToInt32(id));
                    if (editCategory == null)
                        return RedirectToAction("Index");
                    return View(editCategory);
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
        public ActionResult Input(Category model)
        {
            try
            {
                // Check Validate
                if (string.IsNullOrEmpty(model.CategoryName))
                {
                    ModelState.AddModelError("CategoryName", "CategoryName is required");
                }
                if (string.IsNullOrEmpty(model.Description))
                {
                    model.Description = "";
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Title = model.CategoryID == 0 ? "Add New Category" : "Edit Category";
                    return View(model);
                }

                // Connect to DB and Insert or Update
                if(model.CategoryID == 0)
                {
                    int categoryID = CatalogBLL.Category_Add(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    bool updateCategory = CatalogBLL.Category_Update(model);
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
        public ActionResult Delete(string methods = "",  int[] categoryIDs = null)
        {
            if(categoryIDs != null)
            {
                CatalogBLL.Category_Delete(categoryIDs);
            }
            return RedirectToAction("Index");
        }
    }
}