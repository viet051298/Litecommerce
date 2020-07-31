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
    public class CategoryDAL : ICategoryDAL
    {
        private string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public CategoryDAL(string connection)
        {
            this.connectionString = connection;
        }

        public int Add(Category category)
        {
            int categoryID = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Categories
                                          (
                                               CategoryName,
                                               Description
                                          )
                                          VALUES
                                          (
                                               @CategoryName,
                                               @Description
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("Description", category.Description);

                categoryID = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return categoryID;
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
                    cmd.CommandText = @"SELECT COUNT(*) FROM dbo.Categories
                                    WHERE (@searchValue = N'') OR (CategoryName LIKE @searchValue)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        public bool Delete(int[] categoryIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM dbo.Categories
                                    WHERE (CategoryID = @categoryID) AND 
                                    CategoryID NOT IN (SELECT CategoryID FROM dbo.Products)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@categoryID", SqlDbType.Int);
                foreach (int categoryID in categoryIDs)
                {
                    cmd.Parameters["@categoryID"].Value = categoryID;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == categoryIDs.Length;
        }

        public Category Get(int categoryID)
        {
            Category data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Categories WHERE CategoryID = @categoryID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@categoryID", categoryID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Category()
                        {
                           CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                           CategoryName = Convert.ToString(dbReader["CategoryName"]),
                           Description = Convert.ToString(dbReader["Description"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Category> List(int page, int pageSize, string searchValue)
        {
            List<Category> data = new List<Category>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT CategoryID, CategoryName, Description 
                                    FROM 
                                    (
                                        SELECT *, ROW_NUMBER() OVER (ORDER BY CategoryID) AS RowNumber 
                                        FROM dbo.Categories
                                        WHERE (@searchValue = N'') OR (CategoryName LIKE @searchValue)
                                    )AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Category() {
                                CategoryID = Convert.ToInt32(dbReader["CategoryID"]),
                                CategoryName = Convert.ToString(dbReader["CategoryName"]),
                                Description = Convert.ToString(dbReader["Description"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Category category)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Categories SET CategoryName = @CategoryName, 
                                Description = @Description WHERE CategoryID = @CategoryID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CategoryID", category.CategoryID);
                cmd.Parameters.AddWithValue("CategoryName", category.CategoryName);
                cmd.Parameters.AddWithValue("Description", category.Description);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
