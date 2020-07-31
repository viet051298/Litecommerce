using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class AttributeDAL : IAttributeDAL
    {
        private string connectionString;

        /// <summary>
        /// construct
        /// </summary>
        /// <param name="connectionString"></param>
        public AttributeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Attributes> List()
        {
            return null;
        }

    }
}
