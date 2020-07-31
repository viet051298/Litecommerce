using LiteCommerce.BussinessLayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize(Roles = WebUserRoles.STAFF)]
    public class OrderController : Controller
    {
        /// <summary>
        /// Quản lý Order
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int page = 1, string searchValue = "")
        {
            var model = new Models.OrderPaginationResult()
            {
                Page = page,
                PageSize = AppSettings.DefaultPageSize,
                RowCount = CatalogBLL.Order_Count(searchValue),
                SearchValue = searchValue,
                Data = CatalogBLL.Order_List(page, AppSettings.DefaultPageSize, searchValue)
            };
            return View(model);
        }

        public ActionResult Detailt(string id = "")
        {
            return View();
        }

        /// <summary>
        /// Tạo mới Order
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
    }
}