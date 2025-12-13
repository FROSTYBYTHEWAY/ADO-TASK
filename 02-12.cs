//1
//using System;
//using System.Data;

//namespace AdoNetTask1_Final
//{
//    class Program
//    {
//        static void Main()
//        {
//            var dt = new DataTable("Students");
//            dt.Columns.Add("ID", typeof(int));
//            dt.Columns.Add("FullName", typeof(string));
//            dt.Columns.Add("Email", typeof(string));
//            dt.Columns.Add("Class", typeof(string));
//            dt.Columns.Add("AverageGrade", typeof(decimal));
//            dt.PrimaryKey = new[] { dt.Columns["ID"] };

//            dt.Rows.Add(1, "Иванов Иван Иванович", "ivanov@mail.ru", "10А", 4.5m);
//            dt.Rows.Add(2, "Петров Пётр Петрович", "petrov@mail.ru", "10А", 4.2m);
//            dt.Rows.Add(3, "Сидорова Анна Сергеевна", "sidorova@mail.ru", "11Б", 4.8m);
//            dt.Rows.Add(4, "Козлова Мария Алексеевна", "kozlova@mail.ru", "11Б", 3.9m);
//            dt.Rows.Add(5, "Новиков Дмитрий Викторович", "novikov@mail.ru", "10В", 4.6m);

//            Console.WriteLine("Исходная таблица:");
//            PrintSimple(dt);
//            Pause();

//            dt.Rows.Add(6, "Смирнова Екатерина Олеговна", "smirnova@mail.ru", "10А", 4.7m);
//            dt.Rows.Add(7, "Фёдоров Алексей Андреевич", "fedorov@mail.ru", "11А", 4.1m);
//            dt.Rows.Add(8, "Морозова Ольга Николаевна", "morozova@mail.ru", "10В", 4.9m);

//            dt.Rows.Find(2)["Email"] = "petrov.new@mail.ru";
//            dt.Rows.Find(2)["AverageGrade"] = 4.4m;
//            dt.Rows.Find(4)["Email"] = "kozlova2025@mail.ru";
//            dt.Rows.Find(4)["AverageGrade"] = 4.3m;

//            dt.Rows.Find(3).Delete();

//            Console.WriteLine("Подробный отчёт после всех операций:");
//            PrintDetailedReport(dt);
//            Pause();

//            Console.WriteLine("Изменённые строки через GetChanges(): ");
//            var changes = dt.GetChanges();
//            if (changes == null) Console.WriteLine("нет");
//            else PrintWithState(changes);

//            Pause();
//            dt.AcceptChanges();

//            Console.WriteLine("После AcceptChanges():");
//            PrintDetailedReport(dt);

//            Console.ReadKey();
//        }

//        static void PrintSimple(DataTable dt)
//        {
//            Console.WriteLine($"{"ID",-4} {"ФИО",-32} {"Email",-28} {"Класс",-8} {"Оценка",8}");
//            Console.WriteLine(new string('-', 85));
//            foreach (DataRow r in dt.Rows)
//                if (r.RowState != DataRowState.Deleted)
//                    Console.WriteLine($"{r["ID"],-4} {r["FullName"],-32} {r["Email"],-28} {r["Class"],-8} {r["AverageGrade"],8}");
//            Console.WriteLine();
//        }

//        static void PrintWithState(DataTable dt)
//        {
//            Console.WriteLine($"\n{"ID",-4} {"ФИО",-32} {"Email",-28} {"Класс",-8} {"Оценка",10} {"Состояние",-12}");
//            Console.WriteLine(new string('-', 95));
//            foreach (DataRow r in dt.Rows)
//            {
//                if (r.RowState == DataRowState.Deleted)
//                    Console.WriteLine($"{r["ID", DataRowVersion.Original],-4} {r["FullName", DataRowVersion.Original],-32} " +
//                                      $"{r["Email", DataRowVersion.Original],-28} {r["Class", DataRowVersion.Original],-8} " +
//                                      $"{r["AverageGrade", DataRowVersion.Original],10} Deleted");
//                else
//                    Console.WriteLine($"{r["ID"],-4} {r["FullName"],-32} {r["Email"],-28} {r["Class"],-8} {r["AverageGrade"],10} {r.RowState,-12}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintDetailedReport(DataTable dt)
//        {
//            Console.WriteLine($"{"ID",-4} {"ФИО",-32} {"Email",-28} {"Класс",-8} {"Оценка",8} {"RowState",-12} Изменённые поля");
//            Console.WriteLine(new string('-', 120));

//            foreach (DataRow row in dt.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["ID", DataRowVersion.Original],-4} {row["FullName", DataRowVersion.Original],-32} " +
//                                      $"{row["Email", DataRowVersion.Original],-28} {row["Class", DataRowVersion.Original],-8} " +
//                                      $"{row["AverageGrade", DataRowVersion.Original],8} {"Deleted",-12} [удалена]");
//                    continue;
//                }

//                string changed = "";
//                if (row.RowState == DataRowState.Modified)
//                {
//                    for (int i = 1; i < dt.Columns.Count; i++)
//                        if (!Equals(row[i, DataRowVersion.Current], row[i, DataRowVersion.Original]))
//                            changed += dt.Columns[i].ColumnName + ", ";
//                    changed = changed.TrimEnd(' ', ',');
//                }
//                if (string.IsNullOrEmpty(changed)) changed = "-";

//                Console.WriteLine($"{row["ID"],-4} {row["FullName"],-32} {row["Email"],-28} {row["Class"],-8} " +
//                                  $"{row["AverageGrade"],8} {row.RowState,-12} {changed}");
//            }
//            Console.WriteLine();
//        }

//        static void Pause()
//        {
//            Console.WriteLine("\nНажмите любую клавишу...\n");
//            Console.ReadKey(true);
//        }
//    }
//} 

//2
//using System;
//using System.Data;

//namespace AdoNetTask2_Correct
//{
//    class Program
//    {
//        static void Main()
//        {
//            DataTable dt = new DataTable("Products");

//            dt.Columns.Add("ID", typeof(int));
//            dt.Columns.Add("Name", typeof(string));
//            dt.Columns.Add("Price", typeof(decimal));
//            dt.Columns.Add("Quantity", typeof(int));
//            dt.Columns.Add("IsAvailable", typeof(bool));

//            dt.PrimaryKey = new DataColumn[] { dt.Columns["ID"] };

//            dt.Rows.Add(1, "Ноутбук Lenovo", 74990.00m, 12, true);
//            dt.Rows.Add(2, "Смартфон Samsung", 45990.00m, 25, true);
//            dt.Rows.Add(3, "Монитор 27\"", 18990.00m, 8, true);
//            dt.Rows.Add(4, "Клавиатура механическая", 8490.00m, 41, true);
//            dt.Rows.Add(5, "Мышь беспроводная", 2490.00m, 0, false);
//            dt.Rows.Add(6, "Наушники Sony", 12990.00m, 15, true);

//            Console.WriteLine("Исходные данные товаров:");
//            PrintTable(dt);
//            Pause();


//            dt.Rows.Find(1)["Price"] = 69990.00m;     
//            dt.Rows.Find(1)["Quantity"] = 10;

//            dt.Rows.Find(3)["Price"] = 20990.00m;  

//            dt.Rows.Find(5)["Price"] = 2290.00m;     
//            dt.Rows.Find(5)["Quantity"] = 5;
//            dt.Rows.Find(5)["IsAvailable"] = true;

//            dt.Rows.Find(4)["Price"] = 7990.00m;      

//            Console.WriteLine("После внесения изменений:");
//            PrintTable(dt);
//            Pause();

//            Console.WriteLine("Отчёт: изменение цен (старая → новая)");
//            PrintPriceChangesReport(dt);
//            Pause();

//            Console.WriteLine("Только товары с изменённой ценой:");
//            PrintOnlyPriceChangedProducts(dt);
//            Pause();

//            Console.WriteLine("Все доступные версии строки — товар ID = 1");
//            PrintAllRowVersions(dt.Rows.Find(1));

//            Console.WriteLine("Все доступные версии строки — товар ID = 5");
//            PrintAllRowVersions(dt.Rows.Find(5));

//            Console.WriteLine("\nГотово. Нажмите любую клавишу для выхода...");
//            Console.ReadKey(true);
//        }

//        static void PrintTable(DataTable dt)
//        {
//            Console.WriteLine($"{"ID",-4} {"Название",-35} {"Цена",14} {"Кол-во",8} {"В наличии",10}");
//            Console.WriteLine(new string('-', 85));
//            foreach (DataRow row in dt.Rows)
//            {
//                Console.WriteLine($"{row["ID"],-4} {row["Name"],-35} {row["Price"],14:C} {row["Quantity"],8} {(Convert.ToBoolean(row["IsAvailable"]) ? "Да" : "Нет"),10}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintPriceChangesReport(DataTable dt)
//        {
//            Console.WriteLine($"{"ID",-4} {"Название",-35} {"Старая цена",14} {"Новая цена",14} {"Разница",12} {"Процент",10}");
//            Console.WriteLine(new string('-', 105));

//            foreach (DataRow row in dt.Rows)
//            {
//                if (!row.HasVersion(DataRowVersion.Original)) continue;

//                decimal oldPrice = Convert.ToDecimal(row["Price", DataRowVersion.Original]);
//                decimal newPrice = Convert.ToDecimal(row["Price"]);

//                if (Math.Abs(newPrice - oldPrice) < 0.01m) continue;

//                decimal diff = newPrice - oldPrice;
//                decimal percent = oldPrice != 0 ? Math.Round(diff / oldPrice * 100, 2) : 0;

//                Console.WriteLine($"{row["ID"],-4} {row["Name"],-35} {oldPrice,14:C} {newPrice,14:C} {diff,12:C} {percent,9:F2}%");
//            }
//            Console.WriteLine();
//        }

//        static void PrintOnlyPriceChangedProducts(DataTable dt)
//        {
//            Console.WriteLine($"{"ID",-4} {"Название",-35} {"Текущая цена",14} {"Было",14} {"Изменение",12}");
//            Console.WriteLine(new string('-', 90));

//            foreach (DataRow row in dt.Rows)
//            {
//                if (!row.HasVersion(DataRowVersion.Original)) continue;

//                decimal curr = Convert.ToDecimal(row["Price"]);
//                decimal orig = Convert.ToDecimal(row["Price", DataRowVersion.Original]);

//                if (Math.Abs(curr - orig) > 0.01m)
//                {
//                    Console.WriteLine($"{row["ID"],-4} {row["Name"],-35} {curr,14:C} {orig,14:C} {(curr - orig),12:C}");
//                }
//            }
//            Console.WriteLine();
//        }

//        static void PrintAllRowVersions(DataRow row)
//        {
//            if (row == null) return;

//            Console.WriteLine($"Версии данных для: {row["Name"]} (ID = {row["ID"]})");
//            Console.WriteLine(new string('-', 75));

//            DataRowVersion[] versions = {
//                DataRowVersion.Current,
//                DataRowVersion.Original,
//                DataRowVersion.Default,
//                DataRowVersion.Proposed
//            };

//            foreach (DataRowVersion v in versions)
//            {
//                if (row.HasVersion(v))
//                {
//                    decimal price = Convert.ToDecimal(row["Price", v]);
//                    int qty = Convert.ToInt32(row["Quantity", v]);
//                    bool avail = Convert.ToBoolean(row["IsAvailable", v]);

//                    Console.WriteLine($"{v,-10}: Цена = {price,12:C} | Кол-во = {qty,3} | В наличии = {(avail ? "Да" : "Нет")}");
//                }
//                else
//                {
//                    Console.WriteLine($"{v,-10}: недоступна");
//                }
//            }
//            Console.WriteLine($"RowState   : {row.RowState}");
//            Console.WriteLine();
//        }

//        static void Pause()
//        {
//            Console.WriteLine("Нажмите любую клавишу для продолжения...");
//            Console.ReadKey(true);
//            Console.WriteLine();
//        }
//    }
//}

//3
//using System;
//using System.Data;
//using System.Linq;

//namespace AdoNetTask3_Final
//{
//    class Program
//    {
//        private static readonly DataSet ds = new DataSet();

//        static void Main()
//        {
//            CreateTablesAndData();

//            while (true)
//            {
//                Console.Clear();
//                Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ СОТРУДНИКАМИ ===");
//                Console.WriteLine("1. Поиск по фамилии");
//                Console.WriteLine("2. Фильтр по отделу");
//                Console.WriteLine("3. Зарплата выше...");
//                Console.WriteLine("4. Сортировка по дате найма");
//                Console.WriteLine("5. Комбинированный фильтр (IT + ЗП > 50000)");
//                Console.WriteLine("6. Статистика");
//                Console.WriteLine("7. Показать все таблицы");
//                Console.WriteLine("0. Выход");
//                Console.Write("\nВыбор: ");

//                string? choice = Console.ReadLine();

//                switch (choice)
//                {
//                    case "1": SearchByLastName(); break;
//                    case "2": FilterByDepartment(); break;
//                    case "3": FilterBySalary(); break;
//                    case "4": SortByHireDate(); break;
//                    case "5": CombinedFilter(); break;
//                    case "6": ShowStatistics(); break;
//                    case "7": ShowAllData(); break;
//                    case "0": return;
//                    default:
//                        Console.WriteLine("Неверный выбор.");
//                        Pause();
//                        break;
//                }
//            }
//        }

//        static void CreateTablesAndData()
//        {
//            DataTable emp = new DataTable("Сотрудники");
//            emp.Columns.Add("ID", typeof(int));
//            emp.Columns.Add("ФИО", typeof(string));
//            emp.Columns.Add("Отдел", typeof(string));
//            emp.Columns.Add("Зарплата", typeof(decimal));
//            emp.Columns.Add("ДатаНайма", typeof(DateTime));
//            emp.PrimaryKey = new[] { emp.Columns["ID"] };

//            emp.Rows.Add(1, "Иванов Иван Иванович", "IT", 85000m, new DateTime(2020, 5, 15));
//            emp.Rows.Add(2, "Петров Пётр Петрович", "Бухгалтерия", 60000m, new DateTime(2019, 3, 22));
//            emp.Rows.Add(3, "Сидорова Анна Сергеевна", "IT", 95000m, new DateTime(2021, 8, 10));
//            emp.Rows.Add(4, "Козлов Дмитрий Алексеевич", "Маркетинг", 70000m, new DateTime(2022, 1, 5));
//            emp.Rows.Add(5, "Новикова Ольга Викторовна", "IT", 120000m, new DateTime(2018, 11, 30));
//            emp.Rows.Add(6, "Морозов Алексей Николаевич", "HR", 55000m, new DateTime(2023, 2, 18));
//            emp.Rows.Add(7, "Волкова Екатерина Андреевна", "IT", 88000m, new DateTime(2020, 9, 1));
//            emp.Rows.Add(8, "Лебедев Сергей Михайлович", "Бухгалтерия", 62000m, new DateTime(2021, 6, 20));

//            DataTable proj = new DataTable("Проекты");
//            proj.Columns.Add("ID", typeof(int));
//            proj.Columns.Add("Название", typeof(string));
//            proj.Columns.Add("Отдел", typeof(string));
//            proj.Columns.Add("БюджетПроекта", typeof(decimal));
//            proj.Columns.Add("ДатаНачала", typeof(DateTime));
//            proj.PrimaryKey = new[] { proj.Columns["ID"] };

//            proj.Rows.Add(101, "CRM-система", "IT", 5000000m, new DateTime(2024, 1, 10));
//            proj.Rows.Add(102, "Рекламная кампания 2025", "Маркетинг", 1200000m, new DateTime(2024, 9, 1));
//            proj.Rows.Add(103, "Автоматизация бухгалтерии", "Бухгалтерия", 800000m, new DateTime(2024, 3, 15));
//            proj.Rows.Add(104, "Корпоративный портал", "IT", 3500000m, new DateTime(2023, 12, 1));

//            ds.Tables.Add(emp);
//            ds.Tables.Add(proj);
//        }

//        static void SearchByLastName()
//        {
//            Console.Write("Часть фамилии: ");
//            string? part = Console.ReadLine();
//            if (string.IsNullOrWhiteSpace(part)) return;

//            DataRow[] found = ds.Tables["Сотрудники"]!.Select($"ФИО LIKE '%{part}%'");
//            Console.WriteLine($"\nНайдено: {found.Length} чел.");
//            PrintRows(found);
//            Pause();
//        }

//        static void FilterByDepartment()
//        {
//            Console.Write("Отдел: ");
//            string? dept = Console.ReadLine();
//            if (string.IsNullOrWhiteSpace(dept)) return;

//            DataView dv = new DataView(ds.Tables["Сотрудники"]!) { RowFilter = $"Отдел = '{dept}'", Sort = "ФИО" };
//            PrintView(dv);
//            Pause();
//        }

//        static void FilterBySalary()
//        {
//            Console.Write("Минимальная ЗП: ");
//            if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
//            {
//                Console.WriteLine("Ошибка ввода.");
//                Pause();
//                return;
//            }

//            DataView dv = new DataView(ds.Tables["Сотрудники"]!) { RowFilter = $"Зарплата > {salary}", Sort = "Зарплата DESC" };
//            PrintView(dv);
//            Pause();
//        }

//        static void SortByHireDate()
//        {
//            Console.WriteLine("1 – по возрастанию, 2 – по убыванию");
//            string? ch = Console.ReadLine();
//            string sort = ch == "2" ? "ДатаНайма DESC" : "ДатаНайма ASC";

//            DataView dv = new DataView(ds.Tables["Сотрудники"]!) { Sort = sort };
//            PrintView(dv);
//            Pause();
//        }

//        static void CombinedFilter()
//        {
//            DataView dv = new DataView(ds.Tables["Сотрудники"]!)
//            {
//                RowFilter = "Отдел = 'IT' AND Зарплата > 50000",
//                Sort = "ФИО ASC"
//            };

//            Console.WriteLine("\nIT-отдел, ЗП > 50 000 ₽, по алфавиту:");
//            PrintView(dv);
//            Pause();
//        }

//        static void ShowStatistics()
//        {
//            Console.Write("Фильтр (Enter = все): ");
//            string? filter = Console.ReadLine();

//            DataView dv = new DataView(ds.Tables["Сотрудники"]!);
//            if (!string.IsNullOrWhiteSpace(filter))
//            {
//                try { dv.RowFilter = filter; }
//                catch (EvaluateException ex)
//                {
//                    Console.WriteLine($"Ошибка фильтра: {ex.Message}");
//                    Pause();
//                    return;
//                }
//            }

//            DataRow[] rows = dv.ToTable().Select();

//            if (rows.Length == 0)
//            {
//                Console.WriteLine("Нет данных.");
//                Pause();
//                return;
//            }

//            decimal sum = rows.Sum(r => Convert.ToDecimal(r["Зарплата"]));
//            decimal avg = sum / rows.Length;
//            decimal max = rows.Max(r => Convert.ToDecimal(r["Зарплата"]));
//            decimal min = rows.Min(r => Convert.ToDecimal(r["Зарплата"]));

//            Console.WriteLine("\n=== СТАТИСТИКА ===");
//            Console.WriteLine($"Сотрудников: {rows.Length}");
//            Console.WriteLine($"Средняя ЗП: {avg:C}");
//            Console.WriteLine($"Максимум: {max:C}");
//            Console.WriteLine($"Минимум: {min:C}");
//            Console.WriteLine($"Сумма ЗП: {sum:C}");
//            Pause();
//        }

//        static void ShowAllData()
//        {
//            Console.WriteLine("СОТРУДНИКИ:");
//            PrintTable(ds.Tables["Сотрудники"]!);

//            Console.WriteLine("\nПРОЕКТЫ:");
//            PrintTable(ds.Tables["Проекты"]!);
//            Pause();
//        }

//        static void PrintRows(DataRow[] rows)
//        {
//            Console.WriteLine($"{"ID",-4} {"ФИО",-32} {"Отдел",-12} {"Зарплата",12} Дата найма");
//            Console.WriteLine(new string('-', 80));
//            foreach (DataRow r in rows)
//            {
//                Console.WriteLine($"{r["ID"],-4} {r["ФИО"],-32} {r["Отдел"],-12} {r["Зарплата"],12:C} {((DateTime)r["ДатаНайма"]):yyyy-MM-dd}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintView(DataView dv)
//        {
//            Console.WriteLine($"{"ID",-4} {"ФИО",-32} {"Отдел",-12} {"Зарплата",12} Дата найма");
//            Console.WriteLine(new string('-', 80));
//            foreach (DataRowView rv in dv)
//            {
//                Console.WriteLine($"{rv["ID"],-4} {rv["ФИО"],-32} {rv["Отдел"],-12} {rv["Зарплата"],12:C} {((DateTime)rv["ДатаНайма"]):yyyy-MM-dd}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintTable(DataTable dt)
//        {
//            var columns = dt.Columns.Cast<DataColumn>().ToArray();
//            Console.WriteLine(string.Join("  ", columns.Select(c => c.ColumnName.PadRight(20))));
//            Console.WriteLine(new string('-', columns.Length * 22));

//            foreach (DataRow row in dt.Rows)
//            {
//                Console.WriteLine(string.Join("  ", row.ItemArray.Select(x => x?.ToString()?.PadRight(20) ?? string.Empty)));
//            }
//            Console.WriteLine();
//        }

//        static void Pause()
//        {
//            Console.WriteLine("\nНажмите любую клавишу...");
//            Console.ReadKey(true);
//        }
//    }
//}

