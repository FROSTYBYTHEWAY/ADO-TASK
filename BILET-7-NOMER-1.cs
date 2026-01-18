using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppNetFramework
{
    class Program
    {
        static void Main(string[] args)
        {
             string connectionString =
    @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    DataTable resultTable = ReadEmployeesWithNullHandling(connection);

                    foreach (DataRow row in resultTable.Rows)
                    {
                        Console.WriteLine($"ID: {row["EmployeeID"]}");
                        Console.WriteLine($"FirstName: {row["FirstName"]}");
                        Console.WriteLine($"LastName: {row["LastName"]}");
                        Console.WriteLine($"Salary: {row["Salary"]}");
                        Console.WriteLine($"BirthDate: {row["BirthDate"]}");
                        Console.WriteLine($"PhoneNumber: {row["PhoneNumber"]}");
                        Console.WriteLine($"IsActive: {row["IsActive"]}");
                        Console.WriteLine($"Department: {row["Department"]}");
                        Console.WriteLine($"ManagerID: {row["ManagerID"]}");
                        Console.WriteLine($"HireDate: {row["HireDate"]}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static DataTable ReadEmployeesWithNullHandling(SqlConnection connection)
        {
            DataTable table = new DataTable("Employees");
            table.Columns.Add("EmployeeID", typeof(int));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Salary", typeof(decimal));
            table.Columns.Add("BirthDate", typeof(DateTime));
            table.Columns.Add("PhoneNumber", typeof(string));
            table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("Department", typeof(string));
            table.Columns.Add("ManagerID", typeof(int));
            table.Columns.Add("HireDate", typeof(DateTime));

            string query = @"
                SELECT 
                    EmployeeID,
                    FirstName,
                    LastName,
                    Salary,
                    BirthDate,
                    PhoneNumber,
                    IsActive,
                    Department,
                    ManagerID,
                    HireDate
                FROM Employees";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = table.NewRow();

                    row["EmployeeID"] = reader["EmployeeID"] != DBNull.Value ? (int)reader["EmployeeID"] : 0;
                    row["FirstName"] = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : "Не указано";
                    row["LastName"] = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "Не указано";

                    row["Salary"] = reader["Salary"] != DBNull.Value ?
                        (decimal)reader["Salary"] :
                        0m;

                    row["BirthDate"] = reader["BirthDate"] != DBNull.Value ?
                        reader["BirthDate"] is DateTime dt ? dt : DateTime.MinValue :
                        (object)DBNull.Value;

                    row["PhoneNumber"] = reader["PhoneNumber"] != DBNull.Value ?
                        reader["PhoneNumber"].ToString() :
                        "Нет номера";

                    row["IsActive"] = reader["IsActive"] != DBNull.Value ?
                        (bool)reader["IsActive"] :
                        false;

                    row["Department"] = reader["Department"] != DBNull.Value ?
                        reader["Department"].ToString() :
                        "Без отдела";

                    row["ManagerID"] = reader["ManagerID"] != DBNull.Value ?
                        reader["ManagerID"] is int mgrId ? mgrId : (object)DBNull.Value :
                        DBNull.Value;

                    row["HireDate"] = reader["HireDate"] != DBNull.Value ?
                        reader["HireDate"] is DateTime hire ? hire : DateTime.Today :
                        DateTime.Today;

                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}