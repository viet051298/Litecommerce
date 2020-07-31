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
    public interface ISupplierDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        int Add(Supplier supplier);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        bool Update(Supplier supplier);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        bool Delete(int[] supplierIDs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        Supplier Get(int supplierID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Supplier> List(int page, int pageSize, string searchValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue);
    }
}
