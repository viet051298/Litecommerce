using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    public class OrderDAL : IOrderDAL
    {
        private string connectionString;

        public OrderDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Order order)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT COUNT(*) FROM dbo.Orders
                                       WHERE (@searchValue = N'')";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        public bool Delete(int[] orderIDs)
        {
            throw new NotImplementedException();
        }

        public Order Get(int orderID)
        {
            throw new NotImplementedException();
        }

        public List<Order> List(int page, int pageSize, string searchValue)
        {
            List<Order> data = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT OrderID, CustomerID, EmployeeID, OrderDate, RequiredDate, ShippedDate, 
                                        ShipperID, Freight, ShipAddress, ShipCity, ShipCountry, Status  
                                        FROM 
                                        (
	                                        SELECT *, ROW_NUMBER() OVER(ORDER BY OrderID) AS RowNumber
	                                        FROM dbo.Orders
	                                        WHERE (@searchValue = N'')
                                        )AS t  WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            DateTime? shippedDate = null;
                            try
                            {
                                shippedDate = Convert.ToDateTime(dbReader["ShippedDate"]);
                            }
                            catch (InvalidCastException)
                            {
                                
                            }
                            data.Add(new Order()
                            {
                                OrderID = Convert.ToInt32(dbReader["OrderID"]),
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                OrderDate = Convert.ToDateTime(dbReader["OrderDate"]),
                                RequiredDate = Convert.ToDateTime(dbReader["RequiredDate"]),
                                ShippedDate = shippedDate,
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                Freight = Convert.ToDouble(dbReader["Freight"]),
                                ShipAddress = Convert.ToString(dbReader["ShipAddress"]),
                                ShipCity = Convert.ToString(dbReader["ShipCity"]),
                                ShipCountry = Convert.ToString(dbReader["ShipCountry"]),
                                Status = Convert.ToString(dbReader["Status"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
