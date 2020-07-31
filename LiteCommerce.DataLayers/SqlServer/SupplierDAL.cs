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
    public class SupplierDAL : ISupplierDAL
    {
        private string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SupplierDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public int Add(Supplier supplier)
        {
            int supplierId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Suppliers
                                          (
	                                          CompanyName,
	                                          ContactName,
	                                          ContactTitle,
	                                          Address,
	                                          City,
	                                          Country,
	                                          Phone,
	                                          Fax,
	                                          HomePage
                                          )
                                          VALUES
                                          (
	                                          @companyName,
	                                          @contactName,
	                                          @contactTitle,
	                                          @address,
	                                          @city,
	                                          @country,
	                                          @phone,
	                                          @fax,
	                                          @homePage
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("companyName", supplier.CompanyName);
                cmd.Parameters.AddWithValue("contactName", supplier.ContactName);
                cmd.Parameters.AddWithValue("contactTitle", supplier.ContactTitle);
                cmd.Parameters.AddWithValue("address", supplier.Address);
                cmd.Parameters.AddWithValue("city", supplier.City);
                cmd.Parameters.AddWithValue("country", supplier.Country);
                cmd.Parameters.AddWithValue("phone", supplier.Phone);
                cmd.Parameters.AddWithValue("fax", supplier.Fax);
                cmd.Parameters.AddWithValue("homePage", supplier.HomePage);

                supplierId = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return supplierId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
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
                    cmd.CommandText = @"SELECT COUNT(*) FROM dbo.Suppliers
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierIDs"></param>
        /// <returns></returns>
        public bool Delete(int[] supplierIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Suppliers
                                            WHERE(SupplierID = @supplierId)
                                              AND(SupplierID NOT IN(SELECT SupplierID FROM Products))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@supplierId", SqlDbType.Int);
                foreach (int supplierId in supplierIDs)
                {
                    cmd.Parameters["@supplierId"].Value = supplierId;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == supplierIDs.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public Supplier Get(int supplierID)
        {
            Supplier data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Suppliers WHERE SupplierID = @supplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@supplierID", supplierID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Supplier()
                        {
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            ContactName = Convert.ToString(dbReader["ContactName"]),
                            ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            Phone = Convert.ToString(dbReader["Phone"]),
                            Fax = Convert.ToString(dbReader["Fax"]),
                            HomePage = Convert.ToString(dbReader["HomePage"]),
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public List<Supplier> List(int page, int pageSize, string searchValue)
        {
            List<Supplier> data = new List<Supplier>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM 
                                        (
	                                        SELECT *, ROW_NUMBER() OVER(ORDER BY SupplierID) AS RowNumber
	                                        FROM dbo.Suppliers
	                                        WHERE (@searchValue = N'') OR (CompanyName LIKE @searchValue)
                                        )AS t  WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)
                                        ORDER BY t.RowNumber";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    
                    using(SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Supplier()
                            {
                                SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                ContactName = Convert.ToString(dbReader["ContactName"]),
                                ContactTitle = Convert.ToString(dbReader["ContactTitle"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                Phone = Convert.ToString(dbReader["Phone"]),
                                Fax = Convert.ToString(dbReader["Fax"]),
                                HomePage = Convert.ToString(dbReader["HomePage"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public bool Update(Supplier supplier)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Suppliers SET CompanyName = @CompanyName, ContactName = @ContactName, 
                                    ContactTitle = @ContactTitle, Address = @Address, City = @City, Country = @Country, 
                                    Phone = @Phone, Fax = @Fax, HomePage = @HomePage WHERE SupplierID = @SupplierID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("SupplierID", supplier.SupplierID);
                cmd.Parameters.AddWithValue("CompanyName", supplier.CompanyName);
                cmd.Parameters.AddWithValue("ContactName", supplier.ContactName);
                cmd.Parameters.AddWithValue("ContactTitle", supplier.ContactTitle);
                cmd.Parameters.AddWithValue("Address", supplier.Address);
                cmd.Parameters.AddWithValue("City", supplier.City);
                cmd.Parameters.AddWithValue("Country", supplier.Country);
                cmd.Parameters.AddWithValue("Phone", supplier.Phone);
                cmd.Parameters.AddWithValue("Fax", supplier.Fax);
                cmd.Parameters.AddWithValue("HomePage", supplier.HomePage);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
