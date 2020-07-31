using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DataLayers;

namespace LiteCommerce.BussinessLayers
{
    /// <summary>
    /// 
    /// </summary>
    public static class HumanResourceBLL
    {
        private static IEmployeeDAL EmployeeDB { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool Employee_CheckLogin(string email, string pass)
        {
            return EmployeeDB.CheckLogin(email, pass);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool Employee_CheckEmail(string email, bool isUpdate)
        {
            return EmployeeDB.CheckEmail(email, isUpdate);
        }
    }
}
