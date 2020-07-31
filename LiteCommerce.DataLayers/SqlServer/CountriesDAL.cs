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
    public class CountriesDAL : ICountriesDAL
    {
        private string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public CountriesDAL(string connection)
        {
            this.connectionString = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Countries> List()
        {
            List<Countries> data = new List<Countries>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT * FROM dbo.Countries";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Countries() {
                                Country = Convert.ToString(dbReader["Country"])
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }
    }
}
