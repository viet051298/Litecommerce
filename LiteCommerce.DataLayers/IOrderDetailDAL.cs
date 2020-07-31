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
    public interface IOrderDetailDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        int Add(OrderDetail orderDetail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        bool Update(OrderDetail orderDetail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderIDs"></param>
        /// <returns></returns>
        bool Delete(int[] orderIDs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        OrderDetail Get(int orderID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<OrderDetail> List(int page, int pageSize, string searchValue);
    }
}
