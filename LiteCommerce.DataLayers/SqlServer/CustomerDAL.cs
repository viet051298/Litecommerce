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
    /// <summary>
    /// 
    /// </summary>
    public class CustomerDAL : ICustomerDAL
    {
        private string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public CustomerDAL(string connection)
        {
            this.connectionString = connection;
        }

        public string Add(Customer customer)
        {
            string customerID = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Customers
                                          (
	                                            CustomerID,
                                                CompanyName,
                                                ContactName,
                                                ContactTitle,
                                                Address,
                                                City,
                                                Country,
                                                Phone,
                                                Fax
                                          )
                                          VALUES
                                          (
	                                          @CustomerID,
	                                          @CompanyName,
	                                          @ContactName,
	                                          @ContactTitle,
	                                          @Address,
	                                          @City,
	                                          @Country,
	                                          @Phone,
                                              @Fax
                                          );
                                           SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("CompanyName", customer.CompanyName);
                cmd.Parameters.AddWithValue("ContactName", customer.ContactName);
                cmd.Parameters.AddWithValue("ContactTitle", customer.ContactTitle);
                cmd.Parameters.AddWithValue("Address", customer.Address);
                cmd.Parameters.AddWithValue("City", customer.City);
                cmd.Parameters.AddWithValue("Country", customer.Country);
                cmd.Parameters.AddWithValue("Phone", customer.Phone);
                cmd.Parameters.AddWithValue("Fax", customer.Fax);

                customerID = Convert.ToString(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return customerID;
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
                    cmd.CommandText = @"SELECT COUNT(*) FROM dbo.Customers
                                        WHERE (@searchValue = N'') OR (CompanyName LIKE @searchValue)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        public bool Delete(string[] customerIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Customers
                                            WHERE(CustomerID = @customerID)
                                              AND(CustomerID NOT IN(SELECT CustomerID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@customerID", SqlDbType.NChar);
                foreach (string customerID in customerIDs)
                {
                    cmd.Parameters["@customerID"].Value = customerID;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == customerIDs.Length;
        }

        public Customer Get(string customerID)
        {
            Customer data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Customers WHERE CustomerID = @customerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@customerID", customerID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Customer()
                        {
                            CustomerID = Convert.ToString(dbReader["CustomerID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Customer> List(int page, int pageSize, string searchValue)
        {
            List<Customer> data = new List<Customer>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM (
	                                    SELECT *, ROW_NUMBER() OVER (ORDER BY CustomerID) AS RowNumber
	                                    FROM dbo.Customers
	                                    WHERE (@searchValue = N'') OR (CompanyName LIKE @searchValue)
                                    )AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Customer() {
                                CustomerID = Convert.ToString(dbReader["CustomerID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                ContactName = Convert.ToString(dbReader["ContactName"]),
                                ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                Phone = Convert.ToString(dbReader["Phone"]),
                                Fax = Convert.ToString(dbReader["Fax"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Customer customer)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Customers SET CompanyName = @CompanyName, ContactName = @ContactName, 
                            ContactTitle = @ContactTitle, Address = @Address, City = @City, Country = @Country,
                            Phone = @Phone, Fax = @Fax WHERE CustomerID = @CustomerID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                
                cmd.Parameters.AddWithValue("CompanyName", customer.CompanyName);
                cmd.Parameters.AddWithValue("ContactName", customer.ContactName);
                cmd.Parameters.AddWithValue("ContactTitle", customer.ContactTitle);
                cmd.Parameters.AddWithValue("Address", customer.Address);
                cmd.Parameters.AddWithValue("Country", customer.Country);
                cmd.Parameters.AddWithValue("City", customer.City);
                cmd.Parameters.AddWithValue("Phone", customer.Phone);
                cmd.Parameters.AddWithValue("Fax", customer.Fax);
                cmd.Parameters.AddWithValue("CustomerID", customer.CustomerID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
