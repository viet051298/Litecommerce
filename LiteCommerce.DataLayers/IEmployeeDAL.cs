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
    public interface IEmployeeDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        int Add(Employee employee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        bool Update(Employee employee);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        bool Delete(int[] employeeIDs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        Employee Get(int employeeID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        List<Employee> List(int page, int pagesize, string searchValue, string country);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue, string country);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        bool CheckLogin(string email, string pass);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CheckEmail(string email, bool isUpdate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Employee GetEmployeeByEmail(string email);

    }
}
