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
    public class ShipperDAL : IShipperDAL
    {
        private string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public ShipperDAL(string connection)
        {
            this.connectionString = connection;
        }

        public int Add(Shipper shipper)
        {
            int shipperID = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Shippers
                                          (
                                            CompanyName,
                                            Phone
                                          )
                                          VALUES
                                          (
	                                          @CompanyName,
	                                          @Phone
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CompanyName", shipper.CompanyName);
                cmd.Parameters.AddWithValue("Phone", shipper.Phone);

                shipperID = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return shipperID;
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
                    cmd.CommandText = @"SELECT COUNT(*) FROM dbo.Shippers
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

        public bool Delete(int[] shipperIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE FROM Shippers
                                            WHERE(ShipperID = @ShipperID)
                                              AND(ShipperID NOT IN(SELECT ShipperID FROM Orders))";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@ShipperID", SqlDbType.Int);
                foreach (int shipperID in shipperIDs)
                {
                    cmd.Parameters["@ShipperID"].Value = shipperID;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == shipperIDs.Length;
        }

        public Shipper Get(int shipperID)
        {
            Shipper data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM Shippers WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@ShipperID", shipperID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Shipper()
                        {
                            ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                            CompanyName = Convert.ToString(dbReader["CompanyName"]),
                            Phone = Convert.ToString(dbReader["Phone"])
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Shipper> List(int page, int pageSize, string searchValue)
        {
            List<Shipper> data = new List<Shipper>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT ShipperID, CompanyName, Phone
                                        FROM
                                        (
                                            SELECT *, ROW_NUMBER() OVER (ORDER BY ShipperID) AS RowNumber 
                                            FROM dbo.Shippers
                                            WHERE (@searchValue = N'') OR (CompanyName LIKE @searchValue)
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
                            data.Add(new Shipper() {
                                ShipperID = Convert.ToInt32(dbReader["ShipperID"]),
                                CompanyName = Convert.ToString(dbReader["CompanyName"]),
                                Phone = Convert.ToString(dbReader["Phone"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Shipper shipper)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Shippers SET CompanyName = @CompanyName, Phone = @Phone
                                    WHERE ShipperID = @ShipperID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("CompanyName", shipper.CompanyName);
                cmd.Parameters.AddWithValue("Phone", shipper.Phone);
                cmd.Parameters.AddWithValue("ShipperID", shipper.ShipperID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());

                connection.Close();
            }

            return rowsAffected > 0;
        }
    }
}
