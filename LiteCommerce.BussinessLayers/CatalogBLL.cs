using LiteCommerce.DataLayers;
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
    public static class CatalogBLL
    {
        /// <summary>
        /// 
        /// </summary>
        private static ISupplierDAL SupplierDB { get; set; }
        private static ICustomerDAL CustomerDB { get; set; }
        private static ICategoryDAL CategoryDB { get; set; }
        private static IProductDAL ProductDB { get; set; }
        private static IShipperDAL ShipperDB { get; set; }
        private static IEmployeeDAL EmployeeDB { get; set; }
        private static ICountriesDAL CountryDB { get; set; }
        private static IProductAttributeDAL ProductAttributeDB { get; set; }
        private static IAttributeDAL AttributeDB { get; set; }
        private static IOrderDAL OrderDB { get; set; }
        /// <summary>
        /// Hàm này được gọi để khởi tạo các chức năng tác nghiệp
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            SupplierDB = new DataLayers.SqlServer.SupplierDAL(connectionString);
            CustomerDB = new DataLayers.SqlServer.CustomerDAL(connectionString);
            ShipperDB = new DataLayers.SqlServer.ShipperDAL(connectionString);
            CategoryDB = new DataLayers.SqlServer.CategoryDAL(connectionString);
            EmployeeDB = new DataLayers.SqlServer.EmployeeDAL(connectionString);
            ProductDB = new DataLayers.SqlServer.ProductDAL(connectionString);
            CountryDB = new DataLayers.SqlServer.CountriesDAL(connectionString);
            OrderDB = new DataLayers.SqlServer.OrderDAL(connectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> Supplier_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 30;
                    
            return SupplierDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Supplier_Count(string searchValue)
        {
            return SupplierDB.Count(searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static int Supplier_Add(Supplier supplier)
        {
            return SupplierDB.Add(supplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public static bool Supplier_Delete(int[] supplierIDs)
        {
            return SupplierDB.Delete(supplierIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier Supplier_Get(int supplierID)
        {
            return SupplierDB.Get(supplierID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static bool Supplier_Update(Supplier supplier)
        {
            return SupplierDB.Update(supplier);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> Customer_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 30;
            return CustomerDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Customer_Count(string searchValue)
        {
            return CustomerDB.Count(searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static string Customer_Add(Customer customer)
        {
            return CustomerDB.Add(customer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer Customer_Get(string customerID)
        {
            return CustomerDB.Get(customerID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerIDs"></param>
        /// <returns></returns>
        public static bool Customer_Delete(string[] customerIDs)
        {
            return CustomerDB.Delete(customerIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool Customer_Update(Customer customer)
        {
            return CustomerDB.Update(customer);
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static List<Shipper> Shipper_List(int page, int pageSize, string searchValue)
        {
            return ShipperDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Shippers_Count(string searchValue)
        {
            return ShipperDB.Count(searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static int Shipper_Add(Shipper shipper)
        {
            return ShipperDB.Add(shipper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper Shipper_Get(int shipperID)
        {
            return ShipperDB.Get(shipperID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipperIDs"></param>
        /// <returns></returns>
        public static bool Shipper_Delete(int[] shipperIDs)
        {
            return ShipperDB.Delete(shipperIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static bool Shipper_Update(Shipper shipper)
        {
            return ShipperDB.Update(shipper);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> Category_List(int page, int pageSize, string searchValue)
        {
            return CategoryDB.List(page, pageSize, searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Category_Count(string searchValue)
        {
            return CategoryDB.Count(searchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int Category_Add(Category category)
        {
            return CategoryDB.Add(category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        public static bool Category_Delete(int[] categoryIDs)
        {
            return CategoryDB.Delete(categoryIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category Category_Get(int categoryID)
        {
            return CategoryDB.Get(categoryID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool Category_Update(Category category)
        {
            return CategoryDB.Update(category);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static int Employee_Add(Employee employee)
        {
            return EmployeeDB.Add(employee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Employee_Count(string searchValue, string country)
        {
            return EmployeeDB.Count(searchValue, country);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeIDs"></param>
        /// <returns></returns>
        public static bool Employee_Delete(int[] employeeIDs)
        {
            return EmployeeDB.Delete(employeeIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool Employee_Update(Employee employee)
        {
            return EmployeeDB.Update(employee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static Employee Employee_Get(int employeeID)
        {
            return EmployeeDB.Get(employeeID);
        }
        
        public static Employee Employee_GetByEmail(string email)
        {
            return EmployeeDB.GetEmployeeByEmail(email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> Employee_List(int page, int pageSize,string searchValue, string country)
        {
            return EmployeeDB.List(page, pageSize, searchValue, country);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product> Product_List(int page, int pageSize, string searchValue, string suppliedID, string categoryID)
        {
            return ProductDB.List(page, pageSize, searchValue, suppliedID, categoryID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        public static Product Product_Get(int productID)
        {
            return ProductDB.Get(productID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Product_Add(Product data)
        {
            return ProductDB.Add(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Product_Update(Product data)
        {
            return ProductDB.Update(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productIDs"></param>
        /// <returns></returns>
        public static bool Product_Delete(int[] productIDs)
        {
            return ProductDB.Delete(productIDs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static int Product_Count(string searchValue, string categoryID, string supplierID)
        {
            return ProductDB.Count(searchValue, categoryID, supplierID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Countries> Country_List()
        {
            return CountryDB.List();
        }


        public static List<Order> Order_List(int page, int pageSize, string searchValue)
        {
            if (page < 1)
                page = 1;
            if (pageSize <= 0)
                pageSize = 30;
            return OrderDB.List(page, pageSize, searchValue);
        }

        public static int Order_Count(string searchValue)
        {
            return OrderDB.Count(searchValue);
        }
    }
}
