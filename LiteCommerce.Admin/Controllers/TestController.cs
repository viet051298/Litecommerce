using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = WebUserRoles.STAFF)]
        public ActionResult CheckAuth()
        {
            return Json(User.GetUserData(), JsonRequestBehavior.AllowGet);
        }
    }
}