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
    public class EmployeeDAL : IEmployeeDAL
    {
        private string connectionString;
        private string key = "123";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public EmployeeDAL(string connection)
        {
            this.connectionString = connection;
        }

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
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public int Add(Employee employee)
        {
            int employeeID = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Ma Hoa MD5
                employee.Password = MD5Hash(employee.Password);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO Employees
                                          (
                                            LastName,
                                            FirstName,
                                            Title,
                                            BirthDate,
                                            HireDate,
                                            Email,
                                            Address,
                                            City,
                                            Country,
                                            HomePhone,
                                            Notes,
                                            PhotoPath,
                                            Password,
                                            Roles
                                          )
                                          VALUES
                                          (
	                                        @lastName,
                                            @firstName,
                                            @title,
                                            @birthDate,
                                            @hireDate,
                                            @email,
                                            @address,
                                            @city,
                                            @country,
                                            @homePhone,
                                            @notes,
                                            @photoPath,
                                            @password,
                                            @roles
                                          );
                                          SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("lastName", employee.LastName);
                cmd.Parameters.AddWithValue("firstName", employee.FirstName);
                cmd.Parameters.AddWithValue("title", employee.Title);
                cmd.Parameters.AddWithValue("birthDate", employee.BirthDate);
                cmd.Parameters.AddWithValue("hireDate", employee.HireDate);
                cmd.Parameters.AddWithValue("email", employee.Email);
                cmd.Parameters.AddWithValue("address", employee.Address);
                cmd.Parameters.AddWithValue("city", employee.City);
                cmd.Parameters.AddWithValue("country", employee.Country);
                cmd.Parameters.AddWithValue("homePhone", employee.HomePhone);
                cmd.Parameters.AddWithValue("notes", employee.Notes);
                cmd.Parameters.AddWithValue("photoPath", employee.PhotoPath);
                cmd.Parameters.AddWithValue("password", employee.Password);
                cmd.Parameters.AddWithValue("@roles", employee.Roles);
                cmd.Parameters.AddWithValue("employeeID", employee.EmployeeID);

                employeeID = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            return employeeID;
        }

        /// <summary>
        /// Kiểm tra Email, trả về True nếu email chưa tồn tại, False nếu đã tồn tại
        /// </summary>
        /// <param name="email"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        public bool CheckEmail(string email, bool isUpdate)
        {
            int cout = 0;
            List<string> listEmail = new List<string>();
            List<Employee> employees = List(1,100,"", "");
            employees.ForEach(employee => listEmail.Add(employee.Email)); 

            foreach(string subEmail in listEmail)
            {
                if (email == subEmail)
                    cout++;
            }
            if (isUpdate)
            {
                return cout == 1 ? true : false;
            }
            else
            {
                return cout == 0 ? true : false;
            }
        }

        public bool CheckLogin(string email, string pass)
        {
            int count = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT COUNT(*)
                                        FROM Employees
                                        WHERE Email = @email AND Password = @pass";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@pass", MD5Hash(pass));
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count > 0;
        }

        public int Count(string searchValue, string country)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT Count(*)
                                        FROM dbo.Employees
                                        WHERE ((@searchValue = N'') OR (LastName LIKE @searchValue)
                                                                    OR (FirstName LIKE @searchValue)) 
                                                                    AND ((@country=N'') OR (Country = @country))";

                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
                connection.Close();
            }
            return count;
        }

        public bool Delete(int[] employeeIDs)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"DELETE dbo.Employees WHERE EmployeeID = @employeeID AND 
                                    EmployeeID NOT IN (SELECT EmployeeID FROM dbo.Orders)";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.Add("@employeeID", SqlDbType.Int);
                foreach (int employeeID in employeeIDs)
                {
                    cmd.Parameters["@employeeID"].Value = employeeID;
                    rowsAffected += Convert.ToInt32(cmd.ExecuteNonQuery());
                }

                connection.Close();
            }
            return rowsAffected == employeeIDs.Length;
        }

        public Employee Get(int employeeID)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT EmployeeID, LastName, FirstName, Title, BirthDate, HireDate,
                                    Email, Address, City, Country, HomePhone, Notes, PhotoPath, Password, Roles
                                    FROM Employees WHERE EmployeeID = @employeeID";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@employeeID", employeeID);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = decodeMD5(Convert.ToString(dbReader["Password"])),
                            Roles = Convert.ToString(dbReader["Roles"]),
                        };
                    }
                }
                connection.Close();
            }
            return data;
        }

        public List<Employee> List(int page, int pageSize, string searchValue, string country)
        {
            List<Employee> data = new List<Employee>();

            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT EmployeeID, LastName, FirstName, Title, BirthDate, HireDate,
                                        Email, Address, City, Country, HomePhone, Notes, PhotoPath, Password                  
                                    FROM 
                                    (
                                        SELECT *, ROW_NUMBER() OVER (ORDER BY EmployeeID) AS RowNumber 
                                        FROM dbo.Employees
                                        WHERE ((@searchValue = N'') OR (LastName LIKE @searchValue)
                                                                    OR (FirstName LIKE @searchValue)) 
                                                                    AND ((@country=N'') OR (Country = @country)) 
                                    )AS t WHERE t.RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND (@page * @pageSize)";
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Connection = connection;
                    cmd.Parameters.AddWithValue("@searchValue", searchValue);
                    cmd.Parameters.AddWithValue("@country", country);
                    cmd.Parameters.AddWithValue("@page", page);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);

                    using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (dbReader.Read())
                        {
                            data.Add(new Employee() {
                                EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                                LastName = Convert.ToString(dbReader["LastName"]),
                                FirstName = Convert.ToString(dbReader["FirstName"]),
                                Title = Convert.ToString(dbReader["Title"]),
                                BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                                HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                                Email = Convert.ToString(dbReader["Email"]),
                                Address = Convert.ToString(dbReader["Address"]),
                                City = Convert.ToString(dbReader["City"]),
                                Country = Convert.ToString(dbReader["Country"]),
                                HomePhone = Convert.ToString(dbReader["HomePhone"]),
                                Notes = Convert.ToString(dbReader["Notes"]),
                                PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                                Password = Convert.ToString(dbReader["Password"]),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return data;
        }

        public bool Update(Employee employee)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE dbo.Employees 
                                    SET LastName = @LastName,
	                                    FirstName = @FirstName,
	                                    Title = @Title,
                                        Roles = @Roles,
                                        BirthDate = @BirthDate,
	                                    HireDate = @HireDate,
                                        Email = @Email,
                                        Address = @Address, 
                                        City = @City,
	                                    Country = @Country,
                                        HomePhone = @HomePhone,
                                        Notes = @Notes,
                                        PhotoPath = @PhotoPath,
	                                    Password = @Password
                                    WHERE EmployeeID = @EmployeeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("LastName", employee.LastName);
                cmd.Parameters.AddWithValue("FirstName", employee.FirstName);
                cmd.Parameters.AddWithValue("Title", employee.Title);
                cmd.Parameters.AddWithValue("@Roles", employee.Roles);
                cmd.Parameters.AddWithValue("BirthDate", employee.BirthDate);
                cmd.Parameters.AddWithValue("HireDate", employee.HireDate);
                cmd.Parameters.AddWithValue("Email", employee.Email);
                cmd.Parameters.AddWithValue("Address", employee.Address);
                cmd.Parameters.AddWithValue("City", employee.City);
                cmd.Parameters.AddWithValue("Country", employee.Country);
                cmd.Parameters.AddWithValue("HomePhone", employee.HomePhone);
                cmd.Parameters.AddWithValue("Notes", employee.Notes);
                cmd.Parameters.AddWithValue("PhotoPath", employee.PhotoPath);
                cmd.Parameters.AddWithValue("Password", MD5Hash(employee.Password));
                cmd.Parameters.AddWithValue("EmployeeID", employee.EmployeeID);

                rowsAffected = Convert.ToInt32(cmd.ExecuteNonQuery());
                connection.Close();
            }
            return rowsAffected > 0;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee data = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT EmployeeID, LastName, FirstName, Title, BirthDate, HireDate,
                                    Email, Address, City, Country, HomePhone, Notes, PhotoPath, Password, Roles
                                    FROM Employees WHERE Email = @email";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@email", email);

                using (SqlDataReader dbReader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (dbReader.Read())
                    {
                        data = new Employee()
                        {
                            EmployeeID = Convert.ToInt32(dbReader["EmployeeID"]),
                            LastName = Convert.ToString(dbReader["LastName"]),
                            FirstName = Convert.ToString(dbReader["FirstName"]),
                            Title = Convert.ToString(dbReader["Title"]),
                            BirthDate = Convert.ToDateTime(dbReader["BirthDate"]),
                            HireDate = Convert.ToDateTime(dbReader["HireDate"]),
                            Email = Convert.ToString(dbReader["Email"]),
                            Address = Convert.ToString(dbReader["Address"]),
                            City = Convert.ToString(dbReader["City"]),
                            Country = Convert.ToString(dbReader["Country"]),
                            HomePhone = Convert.ToString(dbReader["HomePhone"]),
                            Notes = Convert.ToString(dbReader["Notes"]),
                            PhotoPath = Convert.ToString(dbReader["PhotoPath"]),
                            Password = decodeMD5(Convert.ToString(dbReader["Password"])),
                            Roles = Convert.ToString(dbReader["Roles"]),
                        };
                        Console.WriteLine(data.Password);
                    }
                }
                connection.Close();
            }
            return data;
        }
    }
}
