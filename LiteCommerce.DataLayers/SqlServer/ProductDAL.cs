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
    public class ProductDAL : IProductDAL
    {
        private string connectionString;

        public ProductDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Add(Product data)
        {
            int productId = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Products
                                          (
	                                          ProductName,
	                                          SupplierID,
                                              CategoryID,
	                                          QuantityPerUnit,
                                              UnitPrice,
                                              Descriptions,
                                              PhotoPath
                                          )
                                          VALUES
                                          (
	                                          @ProductName,
	                                          @SupplierID,
                                              @CategoryID,
	                                          @QuantityPerUnit,
                                              @UnitPrice,
                                              @Descriptions,
                                              @PhotoPath
	                                          
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);

                productId = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return productId;
        }

        public int Count(string searchValue, string categoryID, string supplierID)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Count(*) 
                                    FROM Products 
                                    WHERE ((@searchValue = N'') OR (ProductName LIKE @searchValue))
                                            AND ((@SupplierID= N'') OR (SupplierID = @supplierID)) 
                                            AND ((@CategoryID= N'') OR (CategoryID = @categoryID))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@searchValue", searchValue);
                cmd.Parameters.AddWithValue("@supplierID", supplierID);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
                count = Convert.ToInt32(cmd.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public bool Delete(int[] productIDs)
        {
            int countDeleted = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Products
                                            WHERE(ProductID = @productId)
                                              AND(ProductID NOT IN(SELECT ProductID FROM OrderDetails))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@productId", SqlDbType.Int);
                foreach (int productId in productIDs)
                {
                    cmd.Parameters["@productId"].Value = productId;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        countDeleted += 1;
                }
                connection.Close();
            }
            return countDeleted > 0;
        }

        public Product Get(int productID)
        {
            Product data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Products WHERE ProductID = @productID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@productID", productID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Product()
                        {
                            ProductID = Convert.ToInt32(dbReader["ProductID"]),
                            ProductName = Convert.ToString(dbReader["ProductName"]),
                            SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                            CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                            QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                            UnitPrice = Convert.ToDouble(dbReader["UnitPrice"]),
                            Descriptions = Convert.ToString(dbReader["Descriptions"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Product> List(int page, int pagesize, string searchValue, string supplierID, string categoryID)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
            }
         
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit,
                                           UnitPrice, Descriptions, PhotoPath                
                                    FROM 
                                    (
                                        SELECT *, ROW_NUMBER() OVER (ORDER BY ProductID) AS RowNumber 
                                        FROM dbo.Products
                                        WHERE ((@searchValue = N'') OR (ProductName LIKE @searchValue))
                                            AND ((@SupplierID= N'') OR (SupplierID = @supplierID)) 
                                            AND ((@CategoryID= N'') OR (CategoryID = @categoryID)) 
                                    )AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pagesize);
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@supplierID", supplierID);
                    cmd.Parameters.AddWithValue("@categoryID", categoryID);
                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Product()
                            {
                                ProductID = Convert.ToInt32(dbReader["ProductID"]),
                                ProductName = Convert.ToString(dbReader["ProductName"]),
                                SupplierID = Convert.ToInt32(dbReader["SupplierID"]),
                                CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                                QuantityPerUnit = Convert.ToString(dbReader["QuantityPerUnit"]),
                                UnitPrice = Convert.ToInt32(dbReader["UnitPrice"]),
                                Descriptions = Convert.ToString(dbReader["Descriptions"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Product data)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Products
                                           SET  ProductName = @ProductName,
	                                          SupplierID = @SupplierID,
                                              CategoryID = @CategoryID,
	                                          QuantityPerUnit = @QuantityPerUnit,
                                              UnitPrice = @UnitPrice,
                                              Descriptions = @Descriptions,
                                              PhotoPath = @PhotoPath
                                              
                                          WHERE ProductID = @ProductID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@ProductName", data.ProductName);
                cmd.Parameters.AddWithValue("@SupplierID", data.SupplierID);
                cmd.Parameters.AddWithValue("@CategoryID", data.CategoryID);
                cmd.Parameters.AddWithValue("@QuantityPerUnit", data.QuantityPerUnit);
                cmd.Parameters.AddWithValue("@UnitPrice", data.UnitPrice);
                cmd.Parameters.AddWithValue("@Descriptions", data.Descriptions);
                cmd.Parameters.AddWithValue("@PhotoPath", data.PhotoPath);
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
