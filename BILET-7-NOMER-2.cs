using System;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleAppNetFramework
{
    internal class Program
    {
        private static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True;";

        static void Main(string[] args)
        {
            Console.WriteLine("Задание 7.2 — Вставка данных с корректной обработкой NULL\n");

            InsertEmployee("Роман", "Титов", 142000.00m, new DateTime(1989, 6, 17), "+7(977)111-22-33", true, "Финансы", 1, new DateTime(2018, 3, 12));
            InsertEmployee("Юлия", null, null, null, null, false, null, null, null);
            InsertEmployee(null, "Григорьев", 87000m, new DateTime(1995, 10, 5), "8-800-555-35-35", true, "Логистика", 2, new DateTime(2023, 7, 19));
            InsertEmployee("Артём", "Семёнов", 118000m, new DateTime(1992, 2, 28), null, null, null, 3, DateTime.Today);
            InsertEmployee("Наталья", "Орлова", null, new DateTime(1987, 11, 11), "+7(903)999-88-77", true, "Кадры", null, new DateTime(2020, 9, 1));
            InsertEmployee("Игорь", "Белов", 99000m, null, null, true, "IT", 1, null);

            Console.WriteLine("\nВсе сотрудники добавлены успешно!\n");
            Console.WriteLine(new string('═', 100));
            Console.WriteLine("Текущий список всех сотрудников:");
            Console.WriteLine(new string('═', 100));

            ShowAllEmployees();

            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void InsertEmployee(string firstName, string lastName, decimal? salary, DateTime? birthDate,
            string phoneNumber, bool? isActive, string department, int? managerId, DateTime? hireDate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"
                    INSERT INTO Employees 
                    (FirstName, LastName, Salary, BirthDate, PhoneNumber, IsActive, Department, ManagerID, HireDate)
                    VALUES 
                    (@FirstName, @LastName, @Salary, @BirthDate, @PhoneNumber, @IsActive, @Department, @ManagerID, @HireDate)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add(CreateParam("@FirstName", SqlDbType.NVarChar, 50, firstName));
                    cmd.Parameters.Add(CreateParam("@LastName", SqlDbType.NVarChar, 50, lastName));
                    cmd.Parameters.Add(CreateParam("@Salary", SqlDbType.Decimal, salary.HasValue ? (object)salary.Value : DBNull.Value));
                    cmd.Parameters.Add(CreateParam("@BirthDate", SqlDbType.Date, birthDate.HasValue ? (object)birthDate.Value : DBNull.Value));
                    cmd.Parameters.Add(CreateParam("@PhoneNumber", SqlDbType.VarChar, 20, phoneNumber));
                    cmd.Parameters.Add(CreateParam("@IsActive", SqlDbType.Bit, isActive.HasValue ? (object)isActive.Value : DBNull.Value));
                    cmd.Parameters.Add(CreateParam("@Department", SqlDbType.NVarChar, 100, department));
                    cmd.Parameters.Add(CreateParam("@ManagerID", SqlDbType.Int, managerId.HasValue ? (object)managerId.Value : DBNull.Value));
                    cmd.Parameters.Add(CreateParam("@HireDate", SqlDbType.Date, hireDate.HasValue ? (object)hireDate.Value : DBNull.Value));

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"Добавлено строк: {rows} → {firstName ?? "[NULL]"} {lastName ?? "[NULL]"}");
                }
            }
        }

        static SqlParameter CreateParam(string name, SqlDbType type, object value)
        {
            SqlParameter p = new SqlParameter(name, type);
            p.Value = value ?? DBNull.Value;
            return p;
        }

        static SqlParameter CreateParam(string name, SqlDbType type, int size, object value)
        {
            SqlParameter p = new SqlParameter(name, type, size);
            p.Value = value ?? DBNull.Value;
            return p;
        }

        static void ShowAllEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT EmployeeID, FirstName, LastName, Salary, BirthDate, PhoneNumber, IsActive, Department, ManagerID, HireDate FROM Employees ORDER BY EmployeeID DESC", conn))
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Console.WriteLine(new string('─', 100));
                        Console.WriteLine($"ID:             {r["EmployeeID"]}");
                        Console.WriteLine($"Имя:            {GetString(r, 1, "Не указано")}");
                        Console.WriteLine($"Фамилия:        {GetString(r, 2, "Не указано")}");
                        Console.WriteLine($"Зарплата:       {GetDecimal(r, 3):N0} ₽");
                        Console.WriteLine($"Дата рождения:  {GetDate(r, 4)}");
                        Console.WriteLine($"Телефон:        {GetString(r, 5, "не указан")}");
                        Console.WriteLine($"Активен:        {GetBool(r, 6)}");
                        Console.WriteLine($"Отдел:          {GetString(r, 7, "не указан")}");
                        Console.WriteLine($"Начальник ID:   {GetInt(r, 8)}");
                        Console.WriteLine($"Дата приёма:    {GetDate(r, 9)}");
                    }
                }
            }
        }

        static string GetString(SqlDataReader r, int ordinal, string def)
        {
            return r.IsDBNull(ordinal) ? def : r.GetString(ordinal);
        }

        static decimal GetDecimal(SqlDataReader r, int ordinal)
        {
            return r.IsDBNull(ordinal) ? 0m : r.GetDecimal(ordinal);
        }

        static bool GetBool(SqlDataReader r, int ordinal)
        {
            return r.IsDBNull(ordinal) ? false : r.GetBoolean(ordinal);
        }

        static string GetInt(SqlDataReader r, int ordinal)
        {
            return r.IsDBNull(ordinal) ? "нет" : r.GetInt32(ordinal).ToString();
        }

        static string GetDate(SqlDataReader r, int ordinal)
        {
            if (r.IsDBNull(ordinal)) return "не указана";
            DateTime d = r.GetDateTime(ordinal);
            return d.Year < 1754 ? "не указана" : d.ToString("dd.MM.yyyy");
        }
    }
}