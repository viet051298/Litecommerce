using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAccount
    {
        public string UserID { get; set; }

        public string Pass { get; set; }

        public string FullName { get; set; }

        public string Photo { get; set; }

        public string Title { get; set; }

        public string Roles { get; set; }

    }
}
