using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShipperDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        int Add(Shipper shipper);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        bool Update(Shipper shipper);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        bool Delete(int[] shipperIDs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        Shipper Get(int shipperID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Shipper> List(int page, int pagesize, string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
    }
}
