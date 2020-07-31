using LiteCommerce.BussinessLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LiteCommerce.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Profile
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                string cookie = User.Identity.Name;
                ViewBag.Title = "Profile";
                string[] arrInfo = cookie.Split('|');
                Employee employee = CatalogBLL.Employee_Get(Convert.ToInt32(arrInfo[0]));
                if (employee == null)
                    return RedirectToAction("Index");

                return View(employee);
            }
            catch (FormatException)
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult ChangePwd(string id = "")
        {
            string cookie = User.Identity.Name;
            string[] arrInfo = cookie.Split('|');
            Employee employee = CatalogBLL.Employee_Get(Convert.ToInt32(arrInfo[0]));
            UserAccount userAccount = new UserAccount()
            {
                UserID = arrInfo[0],
                FullName = arrInfo[1],
                Pass = employee.Password,
                Photo = arrInfo[6]
            };
            return View(userAccount);
        }

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePwd(UserAccount userAccount)
        {
            try
            {
                string cookie = User.Identity.Name;
                string newPass = Request.Form["newPass"];
                string reNewPass = Request.Form["reNewPass"];

                if (string.IsNullOrEmpty(newPass))
                {
                    ModelState.AddModelError("NewPass", "New pass is required");
                }
                if (string.IsNullOrEmpty(reNewPass))
                {
                    ModelState.AddModelError("ReNewPass", "New pass is required");
                }

                if (!newPass.Equals(reNewPass))
                {
                    ModelState.AddModelError("Pass", "New pass is not the same");
                }

                if (!ModelState.IsValid)
                {
                    return View(userAccount);
                }

                string[] arrInfo = cookie.Split('|');
                Employee employee = CatalogBLL.Employee_Get(Convert.ToInt32(arrInfo[0]));
                userAccount = new UserAccount()
                {
                    UserID = arrInfo[0],
                    FullName = arrInfo[1],
                    Pass = employee.Password,
                    Photo = arrInfo[6]
                };

                UserAccountBLL.Employee_ChangePassword(userAccount, newPass);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " : " + ex.StackTrace);
                return View(userAccount);
            }
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            //nếu đã đăng nhập chuyển về dashboard
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string email = "", string password = "")
        {
            // Kiểm tra thông tin đăng nhập trong CSDL
            var userAccount = UserAccountBLL.Authorize(email, password, UserAccountType.Employee);
            if (userAccount != null)
            {
                WebUserData userData = new WebUserData()
                {
                    UserID = userAccount.UserID,
                    FullName = userAccount.FullName,
                    GroupName = userAccount.Roles, //TODO: Cần thay đổi cho đúng
                    LoginTime = DateTime.Now,
                    SessionID = Session.SessionID,
                    ClientIP = Request.UserHostAddress,
                    Photo = userAccount.Photo
                };
                FormsAuthentication.SetAuthCookie(userData.ToCookieString(), false);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("errLogin", "Đăng nhập thất bại!");
                ViewBag.Email = email;
                return View();
            }
        }

        /// <summary>
        /// Forgot Pass
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPwd()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult EditProfile()
        {
            return View();
        }
    }
}