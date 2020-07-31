using LiteCommerce.DataLayers;
using LiteCommerce.DataLayers.SqlServer;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BussinessLayers
{
    /// <summary>
    /// 
    /// </summary>
    public static class UserAccountBLL
    {
        public static string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initializer(string connectionString)
        {
            UserAccountBLL.connectionString = connectionString; 
        }

        public static UserAccount Authorize(string userName, string password, UserAccountType userType)
        {
            IUserAccountDAL userAccountDB = null;
            switch (userType)
            {
                case UserAccountType.Employee:
                    userAccountDB = new EmployeeUserAccountDAL(connectionString);
                    break;
                case UserAccountType.Customer:
                    userAccountDB = new CustomerUserAccountDAL(connectionString);
                    break;
                default:
                    throw new Exception("User type not valid");
            }
            return userAccountDB.Authorize(userName, password);
        }

        public static bool Employee_ChangePassword(UserAccount userAccount, string newPass)
        {
            IUserAccountDAL userAccountDB = new EmployeeUserAccountDAL(connectionString);
            return userAccountDB.ChangePassword(userAccount, newPass);
        }
    }
}
