using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using LiteCommerce.DomainModels;

namespace LiteCommerce.DataLayers.SqlServer
{
    /// <summary>
    /// Kiểm tra thông tin đăng nhập của nhân viên
    /// </summary>
    public class EmployeeUserAccountDAL : IUserAccountDAL
    {
        private string connectionString;
        private string key = "123";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EmployeeUserAccountDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public string MD5Hash(string toEncrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public string decodeMD5(string toDecrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Authorize Employee
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserAccount Authorize(string userName, string password)
        {
            // TODO: Kiểm tra thông tin đăng nhập từ DB
            UserAccount data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT EmployeeID, LastName, PhotoPath, Title, Roles FROM Employees WHERE Email = @Email and Password = @Password";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@Email", userName);
                cmd.Parameters.AddWithValue("@Password", MD5Hash(password));

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new UserAccount()
                        {
                            UserID = Convert.ToString(dbReader["EmployeeID"]),
                            FullName = Convert.ToString(dbReader["LastName"]),
                            Photo = Convert.ToString(dbReader["PhotoPath"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            Roles = Convert.ToString(dbReader["Roles"])
                        };
                    }
                }

                connection.Close();
            }
            return data;
        }

        public bool ChangePassword(UserAccount userAccount, string pass)
        {
            string oldPass = MD5Hash(userAccount.Pass);
            string newPass = MD5Hash(pass);

            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Employees SET Password = @newPass WHERE Password = @oldPass AND EmployeeID = @employeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("newPass", newPass);
                cmd.Parameters.AddWithValue("oldPass",oldPass);
                cmd.Parameters.AddWithValue("employeeID", userAccount.UserID);
                
                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}
