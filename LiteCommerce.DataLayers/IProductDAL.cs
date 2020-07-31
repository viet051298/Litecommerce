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
    public interface IProductDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        int Add(Product product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        bool Update(Product product);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        bool Delete(int[] productIDs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product Get(int productID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <param name="searchValue"></param>
        /// <param name="supplier"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        List<Product> List(int page, int pagesize, string searchValue, string supplier, string category);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, string categoryID, string supplierID);
    }
}
