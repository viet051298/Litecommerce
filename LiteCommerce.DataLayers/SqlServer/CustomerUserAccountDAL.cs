using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class CustomerUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;

        public CustomerUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public UserAccount Authorize(string userName, string password)
        {
            return new UserAccount
            {
                UserID = userName,
                FullName = "abc",
                Photo = "abc.jpg"
            };
        }

        public bool ChangePassword(UserAccount userAccount, string newPass)
        {
            throw new NotImplementedException();
        }
    }
}
