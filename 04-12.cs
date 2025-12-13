//1
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Text;

//namespace DataViewDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;

//            DataTable originalTable = CreateProductsTable();
//            FillData(originalTable);

//            Console.WriteLine("Исходная таблица (первые 10 строк):");
//            PrintTable(originalTable, 10);
//            Console.WriteLine(new string('=', 80));

//            DemonstrateFiltering(originalTable);
//            DemonstrateSorting(originalTable);
//            DemonstrateDynamicChanges(originalTable);
//            DemonstrateMultipleViews(originalTable);
//            DemonstrateLiveUpdates(originalTable);
//            PerformanceComparison(originalTable);
//            GenerateReport();

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static DataTable CreateProductsTable()
//        {
//            DataTable dt = new DataTable("Products");

//            dt.Columns.Add("ProductID", typeof(int));
//            dt.Columns.Add("Name", typeof(string));
//            dt.Columns.Add("Price", typeof(decimal));
//            dt.Columns.Add("Quantity", typeof(int));
//            dt.Columns.Add("Category", typeof(string));

//            dt.PrimaryKey = new DataColumn[] { dt.Columns["ProductID"] };

//            return dt;
//        }

//        static void FillData(DataTable dt)
//        {
//            string[] categories = { "Электроника", "Одежда", "Продукты", "Книги", "Игрушки", "Бытовая техника", "Спорт", "Косметика" };
//            string[] names = { "Смартфон", "Ноутбук", "Футболка", "Кроссовки", "Молоко", "Хлеб", "Роман", "Детектив", "Кукла", "Конструктор",
//                              "Телевизор", "Холодильник", "Велосипед", "Мяч", "Крем", "Шампунь", "Наушники", "Планшет", "Джинсы", "Куртка" };

//            Random rnd = new Random(42);
//            int id = 1;

//            for (int i = 0; i < 20; i++)
//            {
//                string category = categories[rnd.Next(categories.Length)];
//                string name = names[rnd.Next(names.Length)] + " " + (i + 1);
//                decimal price = rnd.Next(500, 50000) + rnd.NextDecimal(0, 999) / 1000m;
//                int quantity = rnd.Next(0, 200);

//                dt.Rows.Add(id++, name, price, quantity, category);
//            }
//        }

//        static void DemonstrateFiltering(DataTable table)
//        {
//            Console.WriteLine("1. ФИЛЬТРАЦИЯ: DataView vs DataTable.Select()");

//            DataView expensiveView = new DataView(table)
//            {
//                RowFilter = "Price > 30000"
//            };

//            Console.WriteLine($"\nТовары дороже 30 000 (через DataView, найдено: {expensiveView.Count}):");
//            PrintView(expensiveView, 8);

//            DataRow[] selectResult = table.Select("Price > 30000");
//            Console.WriteLine($"\nТе же товары через DataTable.Select() (найдено: {selectResult.Length}):");
//            foreach (DataRow row in selectResult)
//            {
//                Console.WriteLine($"{row["ProductID"],3} | {row["Name"],-25} | {row["Price"],10:C} | {row["Category"],-15}");
//            }

//            Console.WriteLine("\nИсходная таблица НЕ изменилась (первые 5 строк):");
//            PrintTable(table, 5);
//            Console.WriteLine(new string('-', 80));
//        }

//        static void DemonstrateSorting(DataTable table)
//        {
//            Console.WriteLine("2. СОРТИРОВКА БЕЗ изменения исходной таблицы");

//            DataView sortedByPriceDesc = new DataView(table)
//            {
//                Sort = "Price DESC"
//            };

//            Console.WriteLine("\nТоп-5 самых дорогих товаров (DataView с сортировкой):");
//            PrintView(sortedByPriceDesc, 5);

//            DataView sortedByQuantity = new DataView(table)
//            {
//                Sort = "Quantity DESC, Name ASC"
//            };

//            Console.WriteLine("\nТоп-5 товаров по остатку на складе:");
//            PrintView(sortedByQuantity, 5);

//            Console.WriteLine("\nИсходная таблица остаётся в первоначальном порядке:");
//            PrintTable(table, 6);
//            Console.WriteLine(new string('-', 80));
//        }

//        static void DemonstrateDynamicChanges(DataTable table)
//        {
//            Console.WriteLine("3. ДИНАМИЧЕСКОЕ ИЗМЕНЕНИЕ фильтров и сортировки");

//            DataView dynamicView = new DataView(table);

//            Console.WriteLine("Фильтр: Электроника");
//            dynamicView.RowFilter = "Category = 'Электроника'";
//            dynamicView.Sort = "Price DESC";
//            PrintView(dynamicView, 10);

//            Console.WriteLine("\nМеняем фильтр на: Продукты + Количество > 50");
//            dynamicView.RowFilter = "Category = 'Продукты' AND Quantity > 50";
//            dynamicView.Sort = "Quantity DESC";
//            PrintView(dynamicView, 10);

//            Console.WriteLine("\nМеняем сортировку на: по названию A-Z");
//            dynamicView.Sort = "Name ASC";
//            PrintView(dynamicView, 10);

//            Console.WriteLine(new string('-', 80));
//        }

//        static void DemonstrateMultipleViews(DataTable table)
//        {
//            Console.WriteLine("4. НЕСКОЛЬКО ПРЕДСТАВЛЕНИЙ ОДНОВРЕМЕННО");

//            DataView electronicsView = new DataView(table)
//            {
//                RowFilter = "Category = 'Электроника'",
//                Sort = "Price DESC"
//            };

//            DataView cheapProductsView = new DataView(table)
//            {
//                RowFilter = "Price < 2000",
//                Sort = "Quantity DESC"
//            };

//            DataView booksView = new DataView(table)
//            {
//                RowFilter = "Category = 'Книги'",
//                Sort = "Name ASC"
//            };

//            DataView lowStockView = new DataView(table)
//            {
//                RowFilter = "Quantity < 20",
//                Sort = "Quantity ASC, Category"
//            };

//            Console.WriteLine($"Электроника (дорогие первыми): {electronicsView.Count} шт.");
//            PrintView(electronicsView, 6);

//            Console.WriteLine($"\nДешёвые товары (<2000 руб): {cheapProductsView.Count} шт.");
//            PrintView(cheapProductsView, 6);

//            Console.WriteLine($"\nКниги (по алфавиту): {booksView.Count} шт.");
//            PrintView(booksView, 6);

//            Console.WriteLine($"\nТовары с низким остатком (<20): {lowStockView.Count} шт.");
//            PrintView(lowStockView, 6);

//            Console.WriteLine(new string('-', 80));
//        }

//        static void DemonstrateLiveUpdates(DataTable table)
//        {
//            Console.WriteLine("5. АВТОМАТИЧЕСКОЕ ОБНОВЛЕНИЕ DataView при изменении DataTable");

//            DataView liveView = new DataView(table)
//            {
//                RowFilter = "Price > 40000",
//                Sort = "Price DESC"
//            };

//            Console.WriteLine("Текущие дорогие товары:");
//            PrintView(liveView, 10);

//            Console.WriteLine("\nДобавляем новый дорогой товар в таблицу...");
//            DataRow newRow = table.NewRow();
//            newRow["ProductID"] = 999;
//            newRow["Name"] = "Премиум Ноутбук Pro X";
//            newRow["Price"] = 149990.00m;
//            newRow["Quantity"] = 5;
//            newRow["Category"] = "Электроника";
//            table.Rows.Add(newRow);

//            Console.WriteLine("DataView автоматически обновился!");
//            PrintView(liveView, 10);

//            Console.WriteLine("\nМеняем цену существующего товара...");
//            table.Rows[0]["Price"] = 55000.00m;

//            Console.WriteLine("Если он теперь >40000 — появится в представлении:");
//            PrintView(liveView, 10);

//            Console.WriteLine(new string('-', 80));
//        }

//        static void PerformanceComparison(DataTable table)
//        {
//            Console.WriteLine("6. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ");

//            Stopwatch sw = new Stopwatch();
//            long createViewTime = 0, applyFilterTime = 0;
//            long selectTime1 = 0, selectTime2 = 0;

//            for (int i = 0; i < 10000; i++)
//            {
//                sw.Restart();
//                DataView dv = new DataView(table);
//                createViewTime += sw.ElapsedTicks;

//                sw.Restart();
//                dv.RowFilter = "Category = 'Электроника' AND Price > 10000";
//                applyFilterTime += sw.ElapsedTicks;

//                sw.Restart();
//                DataRow[] rows1 = table.Select("Category = 'Электроника' AND Price > 10000");
//                selectTime1 += sw.ElapsedTicks;

//                sw.Restart();
//                DataRow[] rows2 = table.Select("Quantity < 50", "Price DESC");
//                selectTime2 += sw.ElapsedTicks;
//            }

//            double ticksPerMs = Stopwatch.Frequency / 1000.0;

//            Console.WriteLine($"Создание DataView (10 000 раз):           {(createViewTime / ticksPerMs):F3} мс");
//            Console.WriteLine($"Применение фильтра к DataView:            {(applyFilterTime / ticksPerMs):F3} мс");
//            Console.WriteLine($"DataTable.Select() (простой фильтр):      {(selectTime1 / ticksPerMs):F3} мс");
//            Console.WriteLine($"DataTable.Select() (фильтр + сортировка): {(selectTime2 / ticksPerMs):F3} мс");

//            Console.WriteLine("\nВывод: DataView быстрее при многократном изменении фильтров!");
//            Console.WriteLine(new string('-', 80));
//        }

//        static void GenerateReport()
//        {
//            Console.WriteLine("7. ОТЧЁТ: ПРЕИМУЩЕСТВА DataView В РАЗНЫХ СЦЕНАРИЯХ");

//            string[] report = {
//                "ПРЕИМУЩЕСТВА DataView ПО СРАВНЕНИЮ С ПРЯМОЙ РАБОТОЙ С DataTable:",
//                "",
//                "1. НЕИЗМЕНЯЕМОСТЬ ИСХОДНЫХ ДАННЫХ",
//                "   • Фильтры и сортировки не влияют на порядок строк в таблице",
//                "   • Безопасно для многопоточных приложений и кэширования",
//                "",
//                "2. МНОГОКРАТНОЕ ИСПОЛЬЗОВАНИЕ ОДНОГО ИСТОЧНИКА",
//                "   • Десятки представлений с разными фильтрами одновременно",
//                "   • Идеально для дашбордов, отчётов, гридов с разными вкладками",
//                "",
//                "3. ДИНАМИЧЕСКИЕ ФИЛЬТРЫ БЕЗ ПЕРЕСОЗДАНИЯ",
//                "   • Изменение RowFilter/Sort мгновенно обновляет представление",
//                "   • Значительно быстрее, чем многократные вызовы Select()",
//                "",
//                "4. АВТОМАТИЧЕСКАЯ СИНХРОНИЗАЦИЯ",
//                "   • Изменения в DataTable сразу видны во всех DataView",
//                "   • Поддержка событий RowChanged, ListChanged",
//                "",
//                "5. ЛУЧШАЯ ПРОИЗВОДИТЕЛЬНОСТЬ ПРИ ЧАСТЫХ ФИЛЬТРАХ",
//                "   • Кэширование индексов, оптимизированные алгоритмы фильтрации",
//                "   • Особенно заметно при сложных фильтрах и больших таблицах",
//                "",
//                "РЕКОМЕНДАЦИИ ПО ИСПОЛЬЗОВАНИЮ:",
//                "   • Всегда используйте DataView для отображения в UI (WinForms, WPF при привязке)",
//                "   • Для сложных отчётов — создавайте отдельные DataView",
//                "   • Для поиска по разным критериям — один DataView с динамическим фильтром",
//                "   • Для экспорта/печати — отдельный DataView с нужной сортировкой",
//                "",
//                "DataView — мощный и недооценённый инструмент ADO.NET для гибкой работы с данными в памяти."
//            };

//            Console.ForegroundColor = ConsoleColor.Cyan;
//            foreach (string line in report)
//            {
//                Console.WriteLine(line);
//            }
//            Console.ResetColor();
//        }

//        static void PrintTable(DataTable dt, int maxRows = 20)
//        {
//            Console.WriteLine($"{dt.Rows.Count} строк в таблице:");
//            Console.WriteLine("ID  | Название                    |      Цена | Кол-во | Категория      ");
//            Console.WriteLine(new string('-', 75));

//            int count = 0;
//            foreach (DataRow row in dt.Rows)
//            {
//                if (count++ >= maxRows) break;
//                Console.WriteLine($"{row["ProductID"],3} | {row["Name"],-25} | {row["Price"],10:C} | {row["Quantity"],6} | {row["Category"],-14}");
//            }
//            if (dt.Rows.Count > maxRows) Console.WriteLine("   ... и ещё строк: " + (dt.Rows.Count - maxRows));
//        }

//        static void PrintView(DataView dv, int maxRows = 20)
//        {
//            Console.WriteLine($"Представление: {dv.Count} строк (фильтр: {dv.RowFilter ?? "нет"}, сортировка: {dv.Sort ?? "нет"})");
//            Console.WriteLine("ID  | Название                    |      Цена | Кол-во | Категория      ");
//            Console.WriteLine(new string('-', 75));

//            int count = 0;
//            foreach (DataRowView drv in dv)
//            {
//                if (count++ >= maxRows) break;
//                DataRow row = drv.Row;
//                Console.WriteLine($"{row["ProductID"],3} | {row["Name"],-25} | {row["Price"],10:C} | {row["Quantity"],6} | {row["Category"],-14}");
//            }
//            if (dv.Count > maxRows) Console.WriteLine("   ... и ещё строк: " + (dv.Count - maxRows));
//        }
//    }

//    public static class RandomExtensions
//    {
//        public static decimal NextDecimal(this Random rnd, decimal min, decimal max)
//        {
//            return (decimal)rnd.NextDouble() * (max - min) + min;
//        }
//    }
//}


//2
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Text;

//namespace DataTableFindDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(120, 40);

//            DataTable employees = CreateEmployeesTable();
//            FillEmployeesData(employees);

//            Console.WriteLine("Таблица \"Сотрудники\" успешно создана и заполнена 50 записями.\n");
//            PrintSampleEmployees(employees, 8);

//            DemonstrateFindMethod(employees);
//            DemonstrateNonExistingId(employees);
//            PerformanceComparison_Find_vs_Select(employees);
//            DemonstrateDataViewBinarySearch(employees);
//            DemonstrateRangeSearch(employees, 15, 25);
//            DemonstrateRangeSearch(employees, 40, 55);
//            DemonstrateExceptionHandling(employees);

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static DataTable CreateEmployeesTable()
//        {
//            DataTable dt = new DataTable("Employees");

//            dt.Columns.Add("EmployeeID", typeof(int));
//            dt.Columns.Add("FullName", typeof(string));
//            dt.Columns.Add("Email", typeof(string));
//            dt.Columns.Add("Department", typeof(string));
//            dt.Columns.Add("Salary", typeof(decimal));

//            dt.PrimaryKey = new DataColumn[] { dt.Columns["EmployeeID"] };

//            return dt;
//        }

//        static void FillEmployeesData(DataTable dt)
//        {
//            string[] firstNames = { "Александр", "Дмитрий", "Сергей", "Михаил", "Андрей", "Иван", "Елена", "Ольга", "Анна", "Мария", "Наталья", "Татьяна" };
//            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов", "Попов", "Васильев", "Морозов", "Новиков", "Фёдоров", "Волков", "Зайцев" };
//            string[] departments = { "IT-отдел", "Бухгалтерия", "Продажи", "Маркетинг", "HR", "Логистика", "Производство", "Юридический отдел" };
//            string[] domains = { "company.ru", "corp.mail", "work.net", "office.org" };

//            Random rnd = new Random(123);

//            for (int i = 1; i <= 50; i++)
//            {
//                string firstName = firstNames[rnd.Next(firstNames.Length)];
//                string lastName = lastNames[rnd.Next(lastNames.Length)];
//                string fullName = $"{firstName} {lastName}";

//                string department = departments[rnd.Next(departments.Length)];
//                string email = $"{firstName.ToLower()}.{lastName.ToLower()}@{domains[rnd.Next(domains.Length)]}";
//                decimal salary = rnd.Next(40_000, 180_000) + (decimal)rnd.NextDouble() * 20_000m;

//                dt.Rows.Add(i, fullName, email, department, Math.Round(salary, 2));
//            }
//        }

//        static void DemonstrateFindMethod(DataTable dt)
//        {
//            Console.WriteLine(new string('=', 90));
//            Console.WriteLine("1. ПОИСК ПО ПЕРВИЧНОМУ КЛЮЧУ С ИСПОЛЬЗОВАНИЕМ Find()");
//            Console.WriteLine(new string('=', 90));

//            int searchId = 23;
//            DataRow foundRow = dt.Rows.Find(searchId);

//            if (foundRow != null)
//            {
//                PrintEmployee(foundRow, $"Сотрудник с ID = {searchId} найден через Find():");
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с ID = {searchId} не найден.");
//            }
//        }

//        static void DemonstrateNonExistingId(DataTable dt)
//        {
//            Console.WriteLine("\n2. ПОИСК НЕСУЩЕСТВУЮЩЕГО ID");
//            int searchId = 999;
//            DataRow foundRow = dt.Rows.Find(searchId);

//            if (foundRow == null)
//            {
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine($"Ожидаемо: сотрудник с ID = {searchId} НЕ найден (Find() вернул null)");
//                Console.ResetColor();
//            }
//        }

//        static void PerformanceComparison_Find_vs_Select(DataTable dt)
//        {
//            Console.WriteLine("\n" + new string('=', 90));
//            Console.WriteLine("3. СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ: Find() vs Select()");
//            Console.WriteLine(new string('=', 90));

//            int testId = 42;
//            Stopwatch sw = new Stopwatch();

//            long findTime = 0, selectTime = 0;
//            int iterations = 100_000;

//            for (int i = 0; i < iterations; i++)
//            {
//                sw.Restart();
//                DataRow row = dt.Rows.Find(testId);
//                findTime += sw.ElapsedTicks;

//                sw.Restart();
//                DataRow[] rows = dt.Select($"EmployeeID = {testId}");
//                selectTime += sw.ElapsedTicks;
//            }

//            double ticksPerMs = Stopwatch.Frequency / 1000.0;

//            Console.WriteLine($"Итераций: {iterations:N0}");
//            Console.WriteLine($"Find()          : {findTime / ticksPerMs,12:F3} мс");
//            Console.WriteLine($"Select()        : {selectTime / ticksPerMs,12:F3} мс");
//            Console.ForegroundColor = ConsoleColor.Green;
//            Console.WriteLine($"\nFind() быстрее Select() в {(selectTime / (double)findTime):F1} раз!");
//            Console.ResetColor();
//        }

//        static void DemonstrateDataViewBinarySearch(DataTable dt)
//        {
//            Console.WriteLine("\n" + new string('=', 90));
//            Console.WriteLine("4. ПОИСК ЧЕРЕЗ DataView + BinarySearch()");
//            Console.WriteLine(new string('=', 90));

//            DataView dv = new DataView(dt);
//            dv.Sort = "EmployeeID";

//            int searchId = 31;
//            int index = dv.Find(searchId);

//            if (index >= 0)
//            {
//                DataRowView drv = dv[index];
//                PrintEmployee(drv.Row, $"Сотрудник найден через DataView.BinarySearch (индекс {index}):");
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с ID = {searchId} не найден (BinarySearch вернул {index})");
//            }
//        }

//        static void DemonstrateRangeSearch(DataTable dt, int fromId, int toId)
//        {
//            Console.WriteLine($"\n5. ПОИСК ДИАПАЗОНА ID от {fromId} до {toId}");
//            Console.WriteLine(new string('-', 90));

//            DataView dv = new DataView(dt);
//            dv.Sort = "EmployeeID";
//            dv.RowFilter = $"EmployeeID >= {fromId} AND EmployeeID <= {toId}";

//            Console.WriteLine($"Найдено сотрудников в диапазоне: {dv.Count}");
//            Console.WriteLine("ID  | ФИО                        | Отдел               | Зарплата    | Email");
//            Console.WriteLine(new string('-', 90));

//            foreach (DataRowView drv in dv)
//            {
//                DataRow r = drv.Row;
//                Console.WriteLine($"{r["EmployeeID"],3} | {r["FullName"],-26} | {r["Department"],-18} | {r["Salary"],10:C} | {r["Email"]}");
//            }
//        }

//        static void DemonstrateExceptionHandling(DataTable dt)
//        {
//            Console.WriteLine("\n" + new string('=', 90));
//            Console.WriteLine("6. ОБРАБОТКА ИСКЛЮЧЕНИЙ ПРИ ПОИСКЕ");
//            Console.WriteLine(new string('=', 90));

//            object[] badKeys = { "abc", null, 1.5, DBNull.Value, -10 };

//            foreach (var key in badKeys)
//            {
//                try
//                {
//                    Console.WriteLine($"Попытка Find({key ?? "null"}) → ");
//                    DataRow row = dt.Rows.Find(key);
//                    Console.WriteLine(row != null ? "Успешно (неожиданно)" : "Вернул null");
//                }
//                catch (Exception ex)
//                {
//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine($"Исключение: {ex.GetType().Name}: {ex.Message}");
//                    Console.ResetColor();
//                }
//            }

//            Console.WriteLine("\nВывод: Find() требует значение точного типа первичного ключа (int). Любое несоответствие — исключение.");
//        }

//        static void PrintSampleEmployees(DataTable dt, int count)
//        {
//            Console.WriteLine("Пример записей из таблицы (первые {0}):", count);
//            Console.WriteLine("ID  | ФИО                        | Отдел               | Зарплата    | Email");
//            Console.WriteLine(new string('-', 90));

//            for (int i = 0; i < Math.Min(count, dt.Rows.Count); i++)
//            {
//                DataRow r = dt.Rows[i];
//                Console.WriteLine($"{r["EmployeeID"],3} | {r["FullName"],-26} | {r["Department"],-18} | {r["Salary"],10:C} | {r["Email"]}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintEmployee(DataRow row, string title)
//        {
//            Console.ForegroundColor = ConsoleColor.Cyan;
//            Console.WriteLine(title);
//            Console.ResetColor();
//            Console.WriteLine(new string('-', 90));
//            Console.WriteLine($"ID         : {row["EmployeeID"]}");
//            Console.WriteLine($"ФИО        : {row["FullName"]}");
//            Console.WriteLine($"Email      : {row["Email"]}");
//            Console.WriteLine($"Отдел      : {row["Department"]}");
//            Console.WriteLine($"Зарплата   : {row["Salary"],0:C}");
//            Console.WriteLine(new string('-', 90));
//        }
//    }
//}

//3
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Globalization;
//using System.Text;

//namespace DataTableSelectDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(130, 40);

//            DataTable orders = CreateOrdersTable();
//            FillOrdersData(orders, 120);

//            Console.WriteLine($"Таблица \"Заказы\" заполнена: {orders.Rows.Count} записей.\n");

//            DemonstrateFilters(orders);
//            DemonstrateSorting(orders);
//            CompareWithLinq(orders);
//            DynamicFilterDemo(orders);
//            PerformanceTest(orders);

//            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }

//        static DataTable CreateOrdersTable()
//        {
//            var dt = new DataTable("Orders");
//            dt.Columns.Add("OrderID", typeof(int));
//            dt.Columns.Add("CustomerID", typeof(int));
//            dt.Columns.Add("OrderDate", typeof(DateTime));
//            dt.Columns.Add("Amount", typeof(decimal));
//            dt.Columns.Add("Status", typeof(string));
//            dt.Columns.Add("ShippingAddress", typeof(string));
//            return dt;
//        }

//        static void FillOrdersData(DataTable dt, int count)
//        {
//            string[] statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
//            string[] cities = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань", "Омск", "Челябинск", "Красноярск" };
//            Random r = new Random(42);

//            for (int i = 1; i <= count; i++)
//            {
//                dt.Rows.Add(
//                    i,
//                    r.Next(1, 30),
//                    DateTime.Today.AddDays(-r.Next(0, 180)).AddHours(r.Next(0, 24)),
//                    Math.Round((decimal)r.Next(1000, 50000) + (decimal)r.NextDouble() * 10000, 2),
//                    statuses[r.Next(statuses.Length)],
//                    $"{cities[r.Next(cities.Length)]}, ул. {RandomStreet(r)}, д. {r.Next(1, 200)}"
//                );
//            }
//        }

//        static string RandomStreet(Random r)
//        {
//            string[] streets = { "Ленина", "Мира", "Советская", "Пушкина", "Гагарина", "Кирова", "Садовая", "Центральная" };
//            return streets[r.Next(streets.Length)];
//        }

//        static void DemonstrateFilters(DataTable dt)
//        {
//            Console.WriteLine("ДЕМОНСТРАЦИЯ ФИЛЬТРОВ DataTable.Select()\n");

//            FilterAndPrint(dt, "Amount > 35000", "Сумма > 35 000");
//            FilterAndPrint(dt, "Status = 'Delivered'", "Статус: Delivered");
//            FilterAndPrint(dt, "OrderDate >= #2025-08-01# AND OrderDate < #2025-09-01#", "Заказы за август 2025");
//            FilterAndPrint(dt, "Status = 'Processing' AND Amount > 40000", "Processing И сумма > 40 000");
//            FilterAndPrint(dt, "CustomerID = 15", "Клиент ID = 15");
//            FilterAndPrint(dt, "ShippingAddress LIKE '%Москва%'", "Адрес содержит \"Москва\"");
//            FilterAndPrint(dt, "Status IN ('Pending', 'Processing') OR Amount > 45000", "Pending/Processing ИЛИ сумма > 45 000");
//            Console.WriteLine();
//        }

//        static void DemonstrateSorting(DataTable dt)
//        {
//            Console.WriteLine("СОРТИРОВКА ЧЕРЕЗ ПАРАМЕТР Select()\n");

//            FilterAndPrint(dt, "Amount > 30000", "Топ дорогих заказов", "Amount DESC");
//            FilterAndPrint(dt, "Status = 'Shipped'", "Отгруженные заказы", "OrderDate DESC, Amount DESC");
//            Console.WriteLine();
//        }

//        static void CompareWithLinq(DataTable dt)
//        {
//            Console.WriteLine("СРАВНЕНИЕ С LINQ\n");

//            var sw = Stopwatch.StartNew();
//            var selectResult = dt.Select("Amount > 40000 AND Status = 'Delivered'", "Amount DESC");
//            sw.Stop();
//            Console.WriteLine($"Select(): {selectResult.Length} записей, {sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency:F3} мс");

//            sw.Restart();
//            var linqResult = dt.AsEnumerable()
//                .Where(r => r.Field<decimal>("Amount") > 40000 && r.Field<string>("Status") == "Delivered")
//                .OrderByDescending(r => r.Field<decimal>("Amount"))
//                .ToArray();
//            sw.Stop();
//            Console.WriteLine($"LINQ    : {linqResult.Length} записей, {sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency:F3} мс");
//            Console.WriteLine($"→ Select() обычно быстрее LINQ на DataTable\n");
//        }

//        static void DynamicFilterDemo(DataTable dt)
//        {
//            Console.WriteLine("ДИНАМИЧЕСКОЕ ФОРМИРОВАНИЕ ФИЛЬТРА\n");

//            var filter = BuildFilter(
//                minAmount: 25000,
//                status: "Shipped",
//                city: "Москва",
//                fromDate: new DateTime(2025, 7, 1),
//                toDate: new DateTime(2025, 10, 1)
//            );

//            FilterAndPrint(dt, filter, "Динамический фильтр (25к+, Shipped, Москва, июль-сентябрь 2025)", "OrderDate DESC");
//            Console.WriteLine();
//        }

//        static string BuildFilter(decimal? minAmount = null, string status = null, string city = null, DateTime? fromDate = null, DateTime? toDate = null)
//        {
//            var parts = new System.Collections.Generic.List<string>();

//            if (minAmount.HasValue) parts.Add($"Amount >= {minAmount.Value}");
//            if (!string.IsNullOrEmpty(status)) parts.Add($"Status = '{status.Replace("'", "''")}'");
//            if (!string.IsNullOrEmpty(city)) parts.Add($"ShippingAddress LIKE '%{city.Replace("'", "''")}%'");
//            if (fromDate.HasValue) parts.Add($"OrderDate >= #{fromDate.Value:yyyy-MM-dd}#");
//            if (toDate.HasValue) parts.Add($"OrderDate < #{toDate.Value.AddDays(1):yyyy-MM-dd}#");

//            return parts.Count > 0 ? string.Join(" AND ", parts) : "";
//        }

//        static void PerformanceTest(DataTable dt)
//        {
//            Console.WriteLine("ПРОИЗВОДИТЕЛЬНОСТЬ НА 120 ЗАПИСЯХ (100 000 итераций)\n");

//            string filter = "Amount > 20000 AND Status = 'Delivered'";
//            var sw = new Stopwatch();
//            long ticks = 0;
//            int iterations = 100_000;

//            for (int i = 0; i < iterations; i++)
//            {
//                sw.Restart();
//                dt.Select(filter);
//                ticks += sw.ElapsedTicks;
//            }

//            Console.WriteLine($"Select(\"{filter}\")");
//            Console.WriteLine($"Среднее время: {ticks * 1000.0 / Stopwatch.Frequency / iterations:F4} мс на вызов");
//            Console.WriteLine($"Всего: {ticks * 1000.0 / Stopwatch.Frequency:F2} мс за {iterations:N0} итераций\n");
//        }

//        static void FilterAndPrint(DataTable dt, string filter, string title, string sort = "")
//        {
//            try
//            {
//                DataRow[] rows = string.IsNullOrEmpty(filter) ? dt.Select() : dt.Select(filter, sort);
//                Console.WriteLine($"{title}: {rows.Length} записей");
//                if (rows.Length == 0) return;

//                Console.WriteLine("ID   | Клиент | Дата         | Сумма        | Статус       | Адрес");
//                Console.WriteLine(new string('-', 90));

//                int max = Math.Min(5, rows.Length);
//                for (int i = 0; i < max; i++)
//                {
//                    var r = rows[i];
//                    Console.WriteLine($"{r["OrderID"],4} | {r["CustomerID"],6} | {((DateTime)r["OrderDate"]):yyyy-MM-dd} | {r["Amount"],11:C} | {r["Status"],-11} | {r["ShippingAddress"]}");
//                }
//                if (rows.Length > max) Console.WriteLine($"... и ещё {rows.Length - max} записей");
//                Console.WriteLine();
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"Ошибка фильтра \"{filter}\": {ex.Message}");
//                Console.ResetColor();
//                Console.WriteLine();
//            }
//        }
//    }
//}


////4
//using System;
//using System.Data;
//using System.Text;

//namespace DataViewCreationDemo
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(130, 40);

//            DataTable students = CreateStudentsTable();
//            FillStudentsData(students, 100);

//            Console.WriteLine($"Таблица \"Студенты\" создана: {students.Rows.Count} записей.\n");
//            PrintSample(students, 5);

//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ДЕМОНСТРАЦИЯ РАЗНЫХ СПОСОБОВ СОЗДАНИЯ DataView");
//            Console.WriteLine(new string('=', 100));

//            DataView view1 = new DataView(students);
//            ShowView("1. new DataView(table)", view1);

//            DataView view2 = new DataView(students, "GPA > 4.0", "Name ASC", DataViewRowState.CurrentRows);
//            ShowView("2. new DataView(table, filter, sort, rowState)", view2);

//            DataView view3 = students.DefaultView;
//            view3.RowFilter = "Speciality = 'Информатика'";
//            view3.Sort = "EnrollmentYear DESC, GPA DESC";
//            ShowView("3. table.DefaultView (с последующим изменением)", view3);

//            DataView view4 = new DataView(students);
//            view4.RowFilter = "EnrollmentYear >= 2022";
//            view4.Sort = "GPA DESC";
//            view4.RowStateFilter = DataViewRowState.ModifiedCurrent | DataViewRowState.Added;
//            ShowView("4. new DataView() + установка свойств после", view4);

//            DemonstrateDynamicFilter(view1);

//            DemonstrateLiveUpdates(students, view1);

//            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }

//        static DataTable CreateStudentsTable()
//        {
//            var dt = new DataTable("Students");
//            dt.Columns.Add("StudentID", typeof(int));
//            dt.Columns.Add("Name", typeof(string));
//            dt.Columns.Add("GPA", typeof(decimal));
//            dt.Columns.Add("Speciality", typeof(string));
//            dt.Columns.Add("EnrollmentYear", typeof(int));
//            return dt;
//        }

//        static void FillStudentsData(DataTable dt, int count)
//        {
//            string[] names = { "Александр", "Дмитрий", "Сергей", "Михаил", "Андрей", "Иван", "Елена", "Ольга", "Анна", "Мария", "Наталья", "Татьяна" };
//            string[] surnames = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов", "Попов", "Васильев", "Морозов", "Новиков", "Фёдоров" };
//            string[] specialities = { "Информатика", "Математика", "Физика", "Экономика", "Юриспруденция", "Медицина", "Филология", "Психология" };
//            Random r = new Random(42);

//            for (int i = 1; i <= count; i++)
//            {
//                string name = names[r.Next(names.Length)];
//                string surname = surnames[r.Next(surnames.Length)];
//                decimal gpa = 3.0m + (decimal)r.NextDouble() * 2.0m;
//                string spec = specialities[r.Next(specialities.Length)];
//                int year = r.Next(2019, 2026);

//                dt.Rows.Add(i, $"{name} {surname}", Math.Round(gpa, 2), spec, year);
//            }
//        }

//        static void ShowView(string title, DataView view)
//        {
//            Console.WriteLine($"\n{title}");
//            Console.WriteLine(new string('-', 90));
//            Console.WriteLine($"Количество строк: {view.Count}");
//            Console.WriteLine($"Фильтр: \"{view.RowFilter ?? "(нет)"}\"");
//            Console.WriteLine($"Сортировка: \"{view.Sort ?? "(нет)"}\"");
//            Console.WriteLine($"RowState: {view.RowStateFilter}");

//            if (view.Count > 0)
//            {
//                Console.WriteLine("Первые 5 строк:");
//                Console.WriteLine("ID   | ФИО                     | GPA  | Специальность       | Год поступления");
//                Console.WriteLine(new string('-', 80));

//                for (int i = 0; i < Math.Min(5, view.Count); i++)
//                {
//                    DataRowView drv = view[i];
//                    DataRow row = drv.Row;
//                    Console.WriteLine($"{row["StudentID"],4} | {row["Name"],-23} | {row["GPA"],4} | {row["Speciality"],-18} | {row["EnrollmentYear"],5}");
//                }
//            }
//            else
//            {
//                Console.WriteLine("→ Нет строк, удовлетворяющих фильтру.");
//            }
//            Console.WriteLine();
//        }

//        static void DemonstrateDynamicFilter(DataView view)
//        {
//            Console.WriteLine("ДИНАМИЧЕСКОЕ ИЗМЕНЕНИЕ ФИЛЬТРА В СУЩЕСТВУЮЩЕМ DataView");
//            Console.WriteLine(new string('=', 90));

//            view.RowFilter = "GPA > 4.5";
//            view.Sort = "GPA DESC";
//            ShowView("После изменения: GPA > 4.5, сортировка по GPA DESC", view);

//            view.RowFilter = "Speciality LIKE '%Информатика%' AND EnrollmentYear >= 2023";
//            view.Sort = "Name ASC";
//            ShowView("После изменения: Информатика + с 2023 года, по имени", view);

//            view.RowFilter = null; // сброс фильтра
//            view.Sort = "EnrollmentYear DESC, GPA DESC";
//            ShowView("Фильтр сброшен, сортировка по году и оценке", view);
//        }

//        static void DemonstrateLiveUpdates(DataTable table, DataView view)
//        {
//            Console.WriteLine("ЖИВОЕ ОБНОВЛЕНИЕ DataView ПРИ ИЗМЕНЕНИИ ТАБЛИЦЫ");
//            Console.WriteLine(new string('=', 90));

//            view.RowFilter = "GPA > 4.8";
//            ShowView("До изменений (GPA > 4.8)", view);

//            Console.WriteLine("Добавляем отличника...");
//            table.Rows.Add(101, "Анастасия Соколова", 4.95m, "Информатика", 2024);

//            Console.WriteLine("Изменяем GPA у существующего студента...");
//            table.Rows[0]["GPA"] = 4.92m;

//            ShowView("После добавления и изменения (DataView обновился автоматически!)", view);
//        }

//        static void PrintSample(DataTable dt, int count)
//        {
//            Console.WriteLine($"Пример данных (первые {count} строк):");
//            Console.WriteLine("ID   | ФИО                     | GPA  | Специальность       | Год");
//            Console.WriteLine(new string('-', 80));
//            for (int i = 0; i < Math.Min(count, dt.Rows.Count); i++)
//            {
//                var r = dt.Rows[i];
//                Console.WriteLine($"{r["StudentID"],4} | {r["Name"],-23} | {r["GPA"],4} | {r["Speciality"],-18} | {r["EnrollmentYear"]}");
//            }
//            Console.WriteLine();
//        }
//    }
//}

////5
//using System;
//using System.Data;
//using System.Text;

//namespace DataViewSortingFilteringDemo
//{
//    class Program
//    {
//        static DataTable sales;
//        static DataView view;

//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(140, 45);

//            sales = CreateSalesTable();
//            FillSalesData(sales, 250);

//            view = new DataView(sales);

//            Console.WriteLine($"Таблица \"Продажи\" создана: {sales.Rows.Count} записей.\n");

//            ShowCurrentState("Изначальное состояние (без фильтров и сортировки)");

//            ApplyFilter("Region = 'Север'", "Фильтр: Регион = Север");
//            ApplyFilter("SalesDate >= #2025-06-01# AND SalesDate < #2025-09-01#", "Фильтр: Лето 2025");
//            ApplyFilter("Quantity * Price >= 50000", "Фильтр: Сумма продажи ≥ 50 000 ₽");

//            ApplyCombinedFilter();
//            ApplySortingExamples();
//            DynamicSortingMenu();
//            ResetAll();

//            Console.WriteLine("\nГотово. Нажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static DataTable CreateSalesTable()
//        {
//            var dt = new DataTable("Sales");
//            dt.Columns.Add("SalesID", typeof(int));
//            dt.Columns.Add("ProductName", typeof(string));
//            dt.Columns.Add("SalesDate", typeof(DateTime));
//            dt.Columns.Add("Quantity", typeof(int));
//            dt.Columns.Add("Price", typeof(decimal));
//            dt.Columns.Add("Salesperson", typeof(string));
//            dt.Columns.Add("Region", typeof(string));
//            return dt;
//        }

//        static void FillSalesData(DataTable dt, int count)
//        {
//            string[] products = { "Ноутбук Pro", "Смартфон X", "Монитор 27\"", "Клавиатура", "Мышь", "Принтер", "Планшет", "Наушники", "Колонки", "Веб-камера" };
//            string[] salespeople = { "Иван Петров", "Анна Сидорова", "Дмитрий Кузнецов", "Елена Морозова", "Сергей Волков", "Ольга Зайцева" };
//            string[] regions = { "Север", "Юг", "Центр", "Восток", "Запад" };
//            Random r = new Random(123);

//            for (int i = 1; i <= count; i++)
//            {
//                var date = DateTime.Today.AddDays(-r.Next(0, 365));
//                int qty = r.Next(1, 50);
//                decimal price = r.Next(5000, 150000);
//                dt.Rows.Add(
//                    i,
//                    products[r.Next(products.Length)],
//                    date,
//                    qty,
//                    price,
//                    salespeople[r.Next(salespeople.Length)],
//                    regions[r.Next(regions.Length)]
//                );
//            }
//        }

//        static void ShowCurrentState(string title)
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine(title);
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine($"Записей в представлении: {view.Count} из {sales.Rows.Count}");
//            Console.WriteLine($"Текущий фильтр: \"{view.RowFilter ?? "(нет)"}\"");
//            Console.WriteLine($"Текущая сортировка: \"{view.Sort ?? "(нет)"}\"");
//            Console.WriteLine();

//            if (view.Count > 0)
//            {
//                Console.WriteLine("ID   | Продукт            | Дата       | Кол-во | Цена       | Сумма        | Продавец           | Регион");
//                Console.WriteLine(new string('-', 130));
//                int max = Math.Min(6, view.Count);
//                for (int i = 0; i < max; i++)
//                {
//                    var r = view[i].Row;
//                    decimal total = (int)r["Quantity"] * (decimal)r["Price"];
//                    Console.WriteLine($"{r["SalesID"],4} | {r["ProductName"],-18} | {((DateTime)r["SalesDate"]):yyyy-MM-dd} | {r["Quantity"],6} | {r["Price"],9:C} | {total,11:C} | {r["Salesperson"],-18} | {r["Region"]}");
//                }
//                if (view.Count > max) Console.WriteLine($"... и ещё {view.Count - max} записей");
//            }
//            Console.WriteLine();
//        }

//        static void ApplyFilter(string filter, string description)
//        {
//            try
//            {
//                view.RowFilter = filter;
//                ShowCurrentState(description);
//            }
//            catch (Exception ex)
//            {
//                Console.ForegroundColor = ConsoleColor.Red;
//                Console.WriteLine($"ОШИБКА ФИЛЬТРА: {ex.Message}");
//                Console.ResetColor();
//                view.RowFilter = null;
//            }
//        }

//        static void ApplyCombinedFilter()
//        {
//            Console.WriteLine("КОМБИНИРОВАННЫЙ ФИЛЬТР");
//            view.RowFilter = "Region IN ('Север', 'Центр') AND SalesDate >= #2025-01-01# AND Quantity * Price >= 75000";
//            view.Sort = "Region ASC, SalesDate DESC";
//            ShowCurrentState("Фильтр: Север/Центр + 2025 год + сумма ≥ 75 000 ₽ | Сорт: Регион ↑, Дата ↓");
//        }

//        static void ApplySortingExamples()
//        {
//            Console.WriteLine("ПРИМЕРЫ СОРТИРОВОК");
//            view.RowFilter = null;

//            view.Sort = "SalesDate DESC";
//            ShowCurrentState("Сортировка: по дате убывания");

//            view.Sort = "Salesperson ASC, Quantity * Price DESC";
//            ShowCurrentState("Сортировка: по продавцу ↑, затем по сумме ↓");

//            view.Sort = "Region ASC, Salesperson ASC, SalesDate DESC";
//            ShowCurrentState("Многоуровневая: Регион ↑ → Продавец ↑ → Дата ↓");
//        }

//        static void DynamicSortingMenu()
//        {
//            Console.WriteLine("ДИНАМИЧЕСКАЯ СОРТИРОВКА — выберите вариант:");
//            string[] options = {
//                "1. По дате (новые сверху)",
//                "2. По продавцу (А → Я)",
//                "3. По региону и сумме (регион ↑, сумма ↓)",
//                "4. По количеству проданных единиц ↓",
//                "5. Сброс сортировки"
//            };

//            foreach (var opt in options) Console.WriteLine("   " + opt);

//            Console.Write("\nВведите номер (1-5): ");
//            string input = Console.ReadLine();

//            switch (input)
//            {
//                case "1": view.Sort = "SalesDate DESC"; break;
//                case "2": view.Sort = "Salesperson ASC"; break;
//                case "3": view.Sort = "Region ASC, Quantity * Price DESC"; break;
//                case "4": view.Sort = "Quantity DESC"; break;
//                case "5": view.Sort = ""; break;
//                default: Console.WriteLine("Неверный выбор — сортировка не изменена."); return;
//            }

//            ShowCurrentState($"Динамическая сортировка применена: \"{view.Sort}\"");
//        }

//        static void ResetAll()
//        {
//            Console.WriteLine("СБРОС ФИЛЬТРОВ И СОРТИРОВКИ");
//            view.RowFilter = null;
//            view.Sort = null;
//            ShowCurrentState("Все фильтры и сортировка сброшены");
//        }
//    }
//}

//6
//using System;
//using System.Data;
//using System.Text;

//namespace DataViewRowStateDemo
//{
//    class Program
//    {
//        static DataTable invoices;

//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(130, 45);

//            invoices = CreateInvoicesTable();
//            FillInitialData(invoices);

//            Console.WriteLine($"Исходная таблица \"Счета\" — {invoices.Rows.Count} записей:\n");
//            PrintTable(invoices, "Исходное состояние");

//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ВНОСИМ ИЗМЕНЕНИЯ В ТАБЛИЦУ");
//            Console.WriteLine(new string('=', 100));

//            AddInvoice(1001, "ООО Ромашка", 125000.00m, "Не оплачен", new DateTime(2025, 12, 20));
//            AddInvoice(1002, "ИП Иванов", 89000.50m, "Не оплачен", new DateTime(2025, 12, 25));
//            AddInvoice(1003, "ЗАО Прогресс", 210000.00m, "Не оплачен", new DateTime(2026, 1, 10));
//            AddInvoice(1004, "ФЛ Сидоров А.В.", 45000.00m, "Не оплачен", new DateTime(2025, 12, 18));
//            AddInvoice(1005, "ООО Технологии", 178500.00m, "Не оплачен", new DateTime(2026, 1, 5));

//            ModifyInvoice(3, amount: 95000.00m, status: "Частично оплачен");
//            ModifyInvoice(5, customer: "ООО Новый Клиент", dueDate: new DateTime(2025, 12, 31));
//            ModifyInvoice(7, amount: 135000.00m);

//            DeleteInvoice(2);
//            DeleteInvoice(8);

//            Console.WriteLine("Изменения внесены (AcceptChanges НЕ вызывался).\n");

//            DemonstrateRowStates();
//            GenerateChangeReport();
//            RejectChangesDemo();

//            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }

//        static DataTable CreateInvoicesTable()
//        {
//            var dt = new DataTable("Invoices");
//            dt.Columns.Add("InvoiceID", typeof(int));
//            dt.Columns.Add("CustomerName", typeof(string));
//            dt.Columns.Add("Amount", typeof(decimal));
//            dt.Columns.Add("Status", typeof(string));
//            dt.Columns.Add("DueDate", typeof(DateTime));
//            dt.PrimaryKey = new DataColumn[] { dt.Columns["InvoiceID"] };
//            return dt;
//        }

//        static void FillInitialData(DataTable dt)
//        {
//            var data = new (int id, string customer, decimal amount, string status, DateTime due)[]
//            {
//                (1, "ООО Альфа", 85000.00m, "Оплачен", new DateTime(2025, 11, 15)),
//                (2, "ИП Петров", 62000.00m, "Не оплачен", new DateTime(2025, 12, 10)),
//                (3, "ЗАО Бета", 78000.00m, "Не оплачен", new DateTime(2025, 12, 20)),
//                (4, "ООО Гамма", 112000.00m, "Оплачен", new DateTime(2025, 10, 30)),
//                (5, "ФЛ Смирнова", 39000.00m, "Не оплачен", new DateTime(2025, 12, 25)),
//                (6, "ООО Дельта", 156000.00m, "Оплачен", new DateTime(2025, 11, 20)),
//                (7, "ИП Козлов", 98000.00m, "Не оплачен", new DateTime(2026, 1, 5)),
//                (8, "ЗАО Эпсилон", 72000.00m, "Не оплачен", new DateTime(2025, 12, 15)),
//                (9, "ООО Зета", 105000.00m, "Оплачен", new DateTime(2025, 11, 10)),
//                (10, "ФЛ Орлова", 48000.00m, "Не оплачен", new DateTime(2025, 12, 28))
//            };

//            foreach (var item in data)
//                dt.Rows.Add(item.id, item.customer, item.amount, item.status, item.due);
//        }

//        static void AddInvoice(int id, string customer, decimal amount, string status, DateTime due)
//        {
//            invoices.Rows.Add(id, customer, amount, status, due);
//            Console.WriteLine($"Добавлен счёт #{id} — {customer}");
//        }

//        static void ModifyInvoice(int id, string? customer = null, decimal? amount = null, string? status = null, DateTime? dueDate = null)
//        {
//            var row = invoices.Rows.Find(id);
//            if (row != null)
//            {
//                if (customer != null) row["CustomerName"] = customer;
//                if (amount.HasValue) row["Amount"] = amount.Value;
//                if (status != null) row["Status"] = status;
//                if (dueDate.HasValue) row["DueDate"] = dueDate.Value;
//                Console.WriteLine($"Изменён счёт #{id}");
//            }
//        }

//        static void DeleteInvoice(int id)
//        {
//            var row = invoices.Rows.Find(id);
//            if (row != null)
//            {
//                row.Delete();
//                Console.WriteLine($"Удалён счёт #{id}");
//            }
//        }

//        static void DemonstrateRowStates()
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ДЕМОНСТРАЦИЯ DataViewRowState — ВСЕ СОСТОЯНИЯ");
//            Console.WriteLine(new string('=', 100));

//            var states = new (DataViewRowState state, string title)[]
//            {
//                (DataViewRowState.CurrentRows, "CurrentRows — текущие (не удалённые)"),
//                (DataViewRowState.OriginalRows, "OriginalRows — оригинальные версии"),
//                (DataViewRowState.Added, "Added — только добавленные"),
//                (DataViewRowState.Deleted, "Deleted — только удалённые"),
//                (DataViewRowState.ModifiedCurrent, "ModifiedCurrent — текущие версии изменённых"),
//                (DataViewRowState.ModifiedOriginal, "ModifiedOriginal — оригинальные версии изменённых"),
//                (DataViewRowState.Added | DataViewRowState.ModifiedCurrent, "Added | ModifiedCurrent — новые + изменённые"),
//                (DataViewRowState.Unchanged, "Unchanged — без изменений")
//            };

//            foreach (var s in states)
//            {
//                var view = new DataView(invoices) { RowStateFilter = s.state };
//                PrintView(view, s.title);
//            }
//        }

//        static void GenerateChangeReport()
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ОТЧЁТ О ВСЕХ ИЗМЕНЕНИЯХ ПЕРЕД AcceptChanges()");
//            Console.WriteLine(new string('=', 100));

//            var changes = invoices.GetChanges();
//            if (changes == null)
//            {
//                Console.WriteLine("Изменений нет.\n");
//                return;
//            }

//            Console.WriteLine($"Всего изменённых строк: {changes.Rows.Count}\n");
//            PrintTable(changes, "Таблица изменений (GetChanges())");
//        }

//        static void RejectChangesDemo()
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ОТКАТ ВСЕХ ИЗМЕНЕНИЙ — RejectChanges()");
//            Console.WriteLine(new string('=', 100));

//            Console.WriteLine($"До отката — строк в таблице: {invoices.Rows.Count}");
//            PrintTable(invoices, "До RejectChanges()");

//            invoices.RejectChanges();

//            Console.WriteLine($"\nПосле RejectChanges() — строк: {invoices.Rows.Count}");
//            PrintTable(invoices, "После отката — исходное состояние восстановлено!");
//        }

//        static void PrintTable(DataTable dt, string title)
//        {
//            Console.WriteLine($"\n{title}");
//            Console.WriteLine("ID     | Клиент                     | Сумма         | Статус           | Срок оплаты");
//            Console.WriteLine(new string('-', 100));

//            foreach (DataRow row in dt.Rows)
//            {
//                var state = row.RowState;
//                string marker = state switch
//                {
//                    DataRowState.Added => "[ДОБАВЛЕН]",
//                    DataRowState.Deleted => "[УДАЛЁН]",
//                    DataRowState.Modified => "[ИЗМЕНЁН]",
//                    _ => "         "
//                };

//                string amount = row.RowState == DataRowState.Deleted
//                    ? row["Amount", DataRowVersion.Original].ToString()
//                    : row["Amount"].ToString();

//                string customer = row.RowState == DataRowState.Deleted
//                    ? row["CustomerName", DataRowVersion.Original].ToString()
//                    : row["CustomerName"].ToString();

//                string status = row.RowState == DataRowState.Deleted
//                    ? row["Status", DataRowVersion.Original].ToString()
//                    : row["Status"].ToString();

//                var due = row.RowState == DataRowState.Deleted
//                    ? (DateTime)row["DueDate", DataRowVersion.Original]
//                    : (DateTime)row["DueDate"];

//                Console.WriteLine($"{row["InvoiceID"],6} | {customer,-26} | {decimal.Parse(amount):N0} ₽ | {status,-16} | {due:yyyy-MM-dd}  {marker}");
//            }
//            Console.WriteLine();
//        }

//        static void PrintView(DataView view, string title)
//        {
//            Console.WriteLine($"\n{title}");
//            Console.WriteLine($"Количество строк: {view.Count} | RowStateFilter: {view.RowStateFilter}");
//            if (view.Count == 0)
//            {
//                Console.WriteLine("→ Нет строк с таким состоянием.\n");
//                return;
//            }

//            Console.WriteLine("ID     | Клиент                     | Сумма         | Статус           | Срок оплаты");
//            Console.WriteLine(new string('-', 100));

//            foreach (DataRowView drv in view)
//            {
//                DataRow row = drv.Row;
//                DataRowVersion version = row.RowState == DataRowState.Deleted
//                    ? DataRowVersion.Original
//                    : DataRowVersion.Current;

//                Console.WriteLine($"{row["InvoiceID"],6} | {row["CustomerName", version],-26} | {row["Amount", version],11:N0} ₽ | {row["Status", version],-16} | {((DateTime)row["DueDate", version]):yyyy-MM-dd}");
//            }
//            Console.WriteLine();
//        }
//    }
//}


////7
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Text;

//namespace DataViewFindDemo
//{
//    class Program
//    {
//        static DataTable books;
//        static DataView viewFiction;

//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(140, 45);

//            books = CreateBooksTable();
//            FillBooksData(books);

//            viewFiction = new DataView(books)
//            {
//                RowFilter = "Category = 'Художественная литература'",
//                Sort = "ISBN"
//            };

//            Console.WriteLine($"Таблица \"Книги\" заполнена: {books.Rows.Count} записей");
//            Console.WriteLine($"DataView (Художественная литература): {viewFiction.Count} книг\n");

//            DemonstrateFindOnDataView();
//            DemonstrateFindOnDataTable();
//            PerformanceComparison();
//            DemonstrateBinarySearch();
//            SearchByMultipleCriteria();
//            SearchNonExistingBook();

//            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }

//        static DataTable CreateBooksTable()
//        {
//            var dt = new DataTable("Books");
//            dt.Columns.Add("BookID", typeof(int));
//            dt.Columns.Add("Title", typeof(string));
//            dt.Columns.Add("Author", typeof(string));
//            dt.Columns.Add("ISBN", typeof(string));
//            dt.Columns.Add("Year", typeof(int));
//            dt.Columns.Add("Price", typeof(decimal));
//            dt.Columns.Add("Category", typeof(string));
//            dt.PrimaryKey = new DataColumn[] { dt.Columns["BookID"] };
//            return dt;
//        }

//        static void FillBooksData(DataTable dt)
//        {
//            var data = new (int id, string title, string author, string isbn, int year, decimal price, string cat)[]
//            {
//                (1, "Война и мир", "Лев Толстой", "978-5-699-23456-7", 1869, 890.00m, "Художественная литература"),
//                (2, "Преступление и наказание", "Фёдор Достоевский", "978-5-389-12345-6", 1866, 720.00m, "Художественная литература"),
//                (3, "1984", "Джордж Оруэлл", "978-5-17-098765-4", 1949, 680.00m, "Художественная литература"),
//                (4, "Мастер и Маргарита", "Михаил Булгаков", "978-5-389-07412-5", 1967, 850.00m, "Художественная литература"),
//                (5, "Гарри Поттер и философский камень", "Дж. К. Роулинг", "978-5-353-07654-2", 1997, 990.00m, "Фэнтези"),
//                (6, "Алгоритмы: построение и анализ", "Томас Кормен", "978-5-907144-89-7", 2020, 3200.00m, "Научная литература"),
//                (7, "Чистый код", "Роберт Мартин", "978-5-496-00467-1", 2019, 2100.00m, "Научная литература"),
//                (8, "Тихий Дон", "Михаил Шолохов", "978-5-389-05678-8", 1928, 950.00m, "Художественная литература"),
//                (9, "Двенадцать стульев", "Ильф и Петров", "978-5-389-08765-4", 1928, 690.00m, "Художественная литература"),
//                (10, "Анна Каренина", "Лев Толстой", "978-5-389-12356-9", 1877, 820.00m, "Художественная литература")
//            };

//            foreach (var b in data)
//                dt.Rows.Add(b.id, b.title, b.author, b.isbn, b.year, b.price, b.cat);
//        }

//        static void DemonstrateFindOnDataView()
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ПОИСК В DataView ЧЕРЕЗ Find() ПО ISBN");
//            Console.WriteLine(new string('=', 100));

//            string searchIsbn = "978-5-389-07412-5";
//            int index = viewFiction.Find(searchIsbn);

//            if (index >= 0)
//            {
//                DataRowView found = viewFiction[index];
//                PrintBook(found.Row, $"Найдено в DataView (индекс {index}) по ISBN: {searchIsbn}");
//            }
//            else
//            {
//                Console.WriteLine($"Книга с ISBN {searchIsbn} не найдена в DataView (возможно, не в категории)");
//            }
//        }

//        static void DemonstrateFindOnDataTable()
//        {
//            Console.WriteLine("\nПОИСК В DataTable ЧЕРЕЗ Rows.Find() ПО BookID");
//            string isbnTarget = "978-5-389-12356-9";
//            DataRow row = books.Rows.Find(10);

//            if (row != null)
//            {
//                PrintBook(row, $"Найдено в DataTable по BookID → ISBN: {isbnTarget}");
//            }
//        }

//        static void PerformanceComparison()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ Find() на DataTable vs DataView");
//            Console.WriteLine(new string('=', 100));

//            int iterations = 100000;
//            string testIsbn = "978-5-389-07412-5";
//            int testId = 4;

//            Stopwatch sw = new Stopwatch();

//            sw.Start();
//            for (int i = 0; i < iterations; i++)
//            {
//                viewFiction.Find(testIsbn);
//            }
//            sw.Stop();
//            double viewTime = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

//            sw.Restart();
//            for (int i = 0; i < iterations; i++)
//            {
//                books.Rows.Find(testId);
//            }
//            sw.Stop();
//            double tableTime = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

//            Console.WriteLine($"DataView.Find()  : {viewTime,8:F3} мс");
//            Console.WriteLine($"DataTable.Find() : {tableTime,8:F3} мс");
//            Console.WriteLine($"→ Find() на DataTable обычно быстрее (по первичному ключу), на DataView — по отсортированному полю");
//        }

//        static void DemonstrateBinarySearch()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК ЧЕРЕЗ BinarySearch() (DataView должен быть отсортирован)");
//            Console.WriteLine(new string('=', 100));

//            string searchIsbn = "978-5-389-05678-8";
//            int index = viewFiction.Find(searchIsbn);

//            Console.WriteLine($"BinarySearch по ISBN: {searchIsbn}");
//            if (index >= 0)
//            {
//                PrintBook(viewFiction[index].Row, $"Найдено на позиции {index}");
//            }
//            else
//            {
//                Console.WriteLine($"Не найдено (индекс: {index})");
//            }
//        }

//        static void SearchByMultipleCriteria()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК ПО НЕСКОЛЬКИМ КРИТЕРИЯМ (Автор + Год)");
//            Console.WriteLine(new string('=', 100));

//            DataView tempView = new DataView(books)
//            {
//                RowFilter = "Author = 'Лев Толстой' AND Year = 1869"
//            };

//            Console.WriteLine($"Найдено книг: {tempView.Count}");
//            foreach (DataRowView drv in tempView)
//            {
//                PrintBook(drv.Row, "Результат комбинированного поиска");
//            }
//        }

//        static void SearchNonExistingBook()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК НЕСУЩЕСТВУЮЩЕЙ КНИГИ");
//            Console.WriteLine(new string('=', 100));

//            int index = viewFiction.Find("978-1-234567-89-0");
//            if (index == -1)
//            {
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("Ожидаемо: книга с ISBN 978-1-234567-89-0 НЕ найдена → Find() вернул -1");
//                Console.ResetColor();
//            }

//            DataRow row = books.Rows.Find(999);
//            if (row == null)
//            {
//                Console.WriteLine("DataTable.Rows.Find(999) → вернул null (книга не существует)");
//            }
//        }

//        static void PrintBook(DataRow row, string title)
//        {
//            Console.WriteLine($"\n{title}");
//            Console.WriteLine(new string('-', 90));
//            Console.WriteLine($"ID      : {row["BookID"]}");
//            Console.WriteLine($"Название: {row["Title"]}");
//            Console.WriteLine($"Автор   : {row["Author"]}");
//            Console.WriteLine($"ISBN    : {row["ISBN"]}");
//            Console.WriteLine($"Год     : {row["Year"]}");
//            Console.WriteLine($"Цена    : {row["Price"]:C}");
//            Console.WriteLine($"Категория: {row["Category"]}");
//            Console.WriteLine(new string('-', 90));
//        }
//    }
//



////8
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Text;

//namespace DataViewFindDemo
//{
//    class Program
//    {
//        static DataTable books;
//        static DataView viewFiction;

//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.UTF8;
//            Console.SetWindowSize(140, 45);

//            books = CreateBooksTable();
//            FillBooksData(books);

//            viewFiction = new DataView(books)
//            {
//                RowFilter = "Category = 'Художественная литература'",
//                Sort = "ISBN"
//            };

//            Console.WriteLine($"Таблица \"Книги\" заполнена: {books.Rows.Count} записей");
//            Console.WriteLine($"DataView (Художественная литература): {viewFiction.Count} книг\n");

//            DemonstrateFindOnDataView();
//            DemonstrateFindOnDataTable();
//            PerformanceComparison();
//            DemonstrateBinarySearch();
//            SearchByMultipleCriteria();
//            SearchNonExistingBook();

//            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }

//        static DataTable CreateBooksTable()
//        {
//            var dt = new DataTable("Books");
//            dt.Columns.Add("BookID", typeof(int));
//            dt.Columns.Add("Title", typeof(string));
//            dt.Columns.Add("Author", typeof(string));
//            dt.Columns.Add("ISBN", typeof(string));
//            dt.Columns.Add("Year", typeof(int));
//            dt.Columns.Add("Price", typeof(decimal));
//            dt.Columns.Add("Category", typeof(string));
//            dt.PrimaryKey = new DataColumn[] { dt.Columns["BookID"] };
//            return dt;
//        }

//        static void FillBooksData(DataTable dt)
//        {
//            var data = new (int id, string title, string author, string isbn, int year, decimal price, string cat)[]
//            {
//                (1, "Война и мир", "Лев Толстой", "978-5-699-23456-7", 1869, 890.00m, "Художественная литература"),
//                (2, "Преступление и наказание", "Фёдор Достоевский", "978-5-389-12345-6", 1866, 720.00m, "Художественная литература"),
//                (3, "1984", "Джордж Оруэлл", "978-5-17-098765-4", 1949, 680.00m, "Художественная литература"),
//                (4, "Мастер и Маргарита", "Михаил Булгаков", "978-5-389-07412-5", 1967, 850.00m, "Художественная литература"),
//                (5, "Гарри Поттер и философский камень", "Дж. К. Роулинг", "978-5-353-07654-2", 1997, 990.00m, "Фэнтези"),
//                (6, "Алгоритмы: построение и анализ", "Томас Кормен", "978-5-907144-89-7", 2020, 3200.00m, "Научная литература"),
//                (7, "Чистый код", "Роберт Мартин", "978-5-496-00467-1", 2019, 2100.00m, "Научная литература"),
//                (8, "Тихий Дон", "Михаил Шолохов", "978-5-389-05678-8", 1928, 950.00m, "Художественная литература"),
//                (9, "Двенадцать стульев", "Ильф и Петров", "978-5-389-08765-4", 1928, 690.00m, "Художественная литература"),
//                (10, "Анна Каренина", "Лев Толстой", "978-5-389-12356-9", 1877, 820.00m, "Художественная литература")
//            };

//            foreach (var b in data)
//                dt.Rows.Add(b.id, b.title, b.author, b.isbn, b.year, b.price, b.cat);
//        }

//        static void DemonstrateFindOnDataView()
//        {
//            Console.WriteLine(new string('=', 100));
//            Console.WriteLine("ПОИСК В DataView ЧЕРЕЗ Find() ПО ISBN");
//            Console.WriteLine(new string('=', 100));

//            string searchIsbn = "978-5-389-07412-5";
//            int index = viewFiction.Find(searchIsbn);

//            if (index >= 0)
//            {
//                DataRowView found = viewFiction[index];
//                PrintBook(found.Row, $"Найдено в DataView (индекс {index}) по ISBN: {searchIsbn}");
//            }
//            else
//            {
//                Console.WriteLine($"Книга с ISBN {searchIsbn} не найдена в DataView (возможно, не в категории)");
//            }
//        }

//        static void DemonstrateFindOnDataTable()
//        {
//            Console.WriteLine("\nПОИСК В DataTable ЧЕРЕЗ Rows.Find() ПО BookID");
//            string isbnTarget = "978-5-389-12356-9";
//            DataRow row = books.Rows.Find(10);

//            if (row != null)
//            {
//                PrintBook(row, $"Найдено в DataTable по BookID → ISBN: {isbnTarget}");
//            }
//        }

//        static void PerformanceComparison()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ Find() на DataTable vs DataView");
//            Console.WriteLine(new string('=', 100));

//            int iterations = 100000;
//            string testIsbn = "978-5-389-07412-5";
//            int testId = 4;

//            Stopwatch sw = new Stopwatch();

//            sw.Start();
//            for (int i = 0; i < iterations; i++)
//            {
//                viewFiction.Find(testIsbn);
//            }
//            sw.Stop();
//            double viewTime = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

//            sw.Restart();
//            for (int i = 0; i < iterations; i++)
//            {
//                books.Rows.Find(testId);
//            }
//            sw.Stop();
//            double tableTime = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

//            Console.WriteLine($"DataView.Find()  : {viewTime,8:F3} мс");
//            Console.WriteLine($"DataTable.Find() : {tableTime,8:F3} мс");
//            Console.WriteLine($"→ Find() на DataTable обычно быстрее (по первичному ключу), на DataView — по отсортированному полю");
//        }

//        static void DemonstrateBinarySearch()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК ЧЕРЕЗ BinarySearch() (DataView должен быть отсортирован)");
//            Console.WriteLine(new string('=', 100));

//            string searchIsbn = "978-5-389-05678-8";
//            int index = viewFiction.Find(searchIsbn);

//            Console.WriteLine($"BinarySearch по ISBN: {searchIsbn}");
//            if (index >= 0)
//            {
//                PrintBook(viewFiction[index].Row, $"Найдено на позиции {index}");
//            }
//            else
//            {
//                Console.WriteLine($"Не найдено (индекс: {index})");
//            }
//        }

//        static void SearchByMultipleCriteria()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК ПО НЕСКОЛЬКИМ КРИТЕРИЯМ (Автор + Год)");
//            Console.WriteLine(new string('=', 100));

//            DataView tempView = new DataView(books)
//            {
//                RowFilter = "Author = 'Лев Толстой' AND Year = 1869"
//            };

//            Console.WriteLine($"Найдено книг: {tempView.Count}");
//            foreach (DataRowView drv in tempView)
//            {
//                PrintBook(drv.Row, "Результат комбинированного поиска");
//            }
//        }

//        static void SearchNonExistingBook()
//        {
//            Console.WriteLine("\n" + new string('=', 100));
//            Console.WriteLine("ПОИСК НЕСУЩЕСТВУЮЩЕЙ КНИГИ");
//            Console.WriteLine(new string('=', 100));

//            int index = viewFiction.Find("978-1-234567-89-0");
//            if (index == -1)
//            {
//                Console.ForegroundColor = ConsoleColor.Yellow;
//                Console.WriteLine("Ожидаемо: книга с ISBN 978-1-234567-89-0 НЕ найдена → Find() вернул -1");
//                Console.ResetColor();
//            }

//            DataRow row = books.Rows.Find(999);
//            if (row == null)
//            {
//                Console.WriteLine("DataTable.Rows.Find(999) → вернул null (книга не существует)");
//            }
//        }

//        static void PrintBook(DataRow row, string title)
//        {
//            Console.WriteLine($"\n{title}");
//            Console.WriteLine(new string('-', 90));
//            Console.WriteLine($"ID      : {row["BookID"]}");
//            Console.WriteLine($"Название: {row["Title"]}");
//            Console.WriteLine($"Автор   : {row["Author"]}");
//            Console.WriteLine($"ISBN    : {row["ISBN"]}");
//            Console.WriteLine($"Год     : {row["Year"]}");
//            Console.WriteLine($"Цена    : {row["Price"]:C}");
//            Console.WriteLine($"Категория: {row["Category"]}");
//            Console.WriteLine(new string('-', 90));
//        }
//    }
//}

//9
//using System;
//using System.Data;
//using System.Collections.Generic;

//class EmployeeDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Сотрудники"
//        DataTable employeesTable = CreateEmployeesTable();
//        FillTestData(employeesTable);

//        // Создаём DataView с фильтром по отделу "IT"
//        DataView itEmployeesView = new DataView(employeesTable, "[Отдел] = 'IT'", "", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== РЕДАКТИРОВАНИЕ ДАННЫХ ЧЕРЕЗ DataView ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (IT отдел):");
//        PrintDataView(itEmployeesView);

//        // Поиск сотрудника через Find()
//        Console.WriteLine("\nПоиск сотрудника с EmployeeID = 1:");
//        FindAndEditEmployee(itEmployeesView, 1, 50000, "Активен");

//        // Выводим данные после редактирования
//        Console.WriteLine("\nДанные после редактирования:");
//        PrintDataView(itEmployeesView);

//        // Массовое редактирование (повышение зарплат на 10% для всех в отделе)
//        Console.WriteLine("\nМассовое редактирование (повышение зарплат на 10% для всех в IT отделе):");
//        MassEditSalaries(itEmployeesView, 10);

//        // Выводим данные после массового редактирования
//        Console.WriteLine("\nДанные после массового редактирования:");
//        PrintDataView(itEmployeesView);

//        // Редактирование с валидацией
//        Console.WriteLine("\nРедактирование с валидацией:");
//        EditEmployeeWithValidation(itEmployeesView, 2, -1000, "Неактивен");

//        // Выводим данные после редактирования с валидацией
//        Console.WriteLine("\nДанные после редактирования с валидацией:");
//        PrintDataView(itEmployeesView);

//        // Проверка отражения изменений в исходной таблице
//        Console.WriteLine("\nПроверка отражения изменений в исходной таблице:");
//        PrintDataTable(employeesTable);

//        // Создаём ещё один DataView с другим фильтром
//        DataView activeEmployeesView = new DataView(employeesTable, "[Статус] = 'Активен'", "", DataViewRowState.CurrentRows);

//        // Выводим данные из другого DataView
//        Console.WriteLine("\nДанные из другого DataView (все активные сотрудники):");
//        PrintDataView(activeEmployeesView);

//        // Отчёт обо всех изменениях
//        Console.WriteLine("\nОтчёт обо всех изменениях:");
//        ReportChanges(employeesTable);
//    }

//    static DataTable CreateEmployeesTable()
//    {
//        DataTable table = new DataTable("Сотрудники");

//        table.Columns.Add("EmployeeID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("Зарплата", typeof(decimal));
//        table.Columns.Add("Отдел", typeof(string));
//        table.Columns.Add("Дата Найма", typeof(DateTime));
//        table.Columns.Add("Статус", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["EmployeeID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] departments = { "IT", "HR", "Finance", "Marketing", "Sales" };
//        string[] statuses = { "Активен", "Неактивен", "В отпуске" };

//        for (int i = 1; i <= 50; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                30000 + random.Next(10) * 10000,
//                departments[random.Next(departments.Length)],
//                DateTime.Now.AddDays(-random.Next(365 * 5)),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void FindAndEditEmployee(DataView view, int employeeID, decimal newSalary, string newStatus)
//    {
//        try
//        {
//            // Сортируем DataView по EmployeeID для использования Find()
//            view.Sort = "EmployeeID ASC";

//            // Ищем сотрудника
//            int index = view.Find(employeeID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден сотрудник с EmployeeID = {employeeID}.");

//                // Редактируем данные сотрудника
//                DataRowView rowView = view[index];
//                Console.WriteLine("До редактирования:");
//                PrintEmployeeInfo(rowView.Row);

//                rowView["Зарплата"] = newSalary;
//                rowView["Статус"] = newStatus;

//                Console.WriteLine("После редактирования:");
//                PrintEmployeeInfo(rowView.Row);
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с EmployeeID = {employeeID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при поиске или редактировании сотрудника: {ex.Message}");
//        }
//    }

//    static void MassEditSalaries(DataView view, int percentage)
//    {
//        try
//        {
//            Console.WriteLine($"Повышение зарплат на {percentage}% для всех сотрудников в отделе.");

//            foreach (DataRowView rowView in view)
//            {
//                decimal currentSalary = (decimal)rowView["Зарплата"];
//                decimal newSalary = currentSalary * (1 + percentage / 100.0m);
//                rowView["Зарплата"] = Math.Round(newSalary, 2);
//            }

//            Console.WriteLine("Зарплаты успешно повышены.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при массовом редактировании зарплат: {ex.Message}");
//        }
//    }

//    static void EditEmployeeWithValidation(DataView view, int employeeID, decimal newSalary, string newStatus)
//    {
//        try
//        {
//            // Сортируем DataView по EmployeeID для использования Find()
//            view.Sort = "EmployeeID ASC";

//            // Ищем сотрудника
//            int index = view.Find(employeeID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден сотрудник с EmployeeID = {employeeID}.");

//                // Проверяем валидацию
//                if (newSalary <= 0)
//                {
//                    throw new ArgumentException("Зарплата должна быть больше 0.");
//                }

//                List<string> validStatuses = new List<string> { "Активен", "Неактивен", "В отпуске" };
//                if (!validStatuses.Contains(newStatus))
//                {
//                    throw new ArgumentException("Некорректный статус.");
//                }

//                // Редактируем данные сотрудника
//                DataRowView rowView = view[index];
//                Console.WriteLine("До редактирования:");
//                PrintEmployeeInfo(rowView.Row);

//                rowView["Зарплата"] = newSalary;
//                rowView["Статус"] = newStatus;

//                Console.WriteLine("После редактирования:");
//                PrintEmployeeInfo(rowView.Row);
//            }
//            else
//            {
//                Console.WriteLine($"Сотрудник с EmployeeID = {employeeID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при редактировании сотрудника: {ex.Message}");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintEmployeeInfo(rowView.Row);
//        }
//    }

//    static void PrintDataTable(DataTable table)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            if (row.RowState != DataRowState.Deleted)
//            {
//                PrintEmployeeInfo(row);
//            }
//        }
//    }

//    static void PrintEmployeeInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"ID: {row["EmployeeID"]}, " +
//            $"Имя: {row["Имя"]}, " +
//            $"Зарплата: {row["Зарплата"]:C}, " +
//            $"Отдел: {row["Отдел"]}, " +
//            $"Дата Найма: {((DateTime)row["Дата Найма"]).ToShortDateString()}, " +
//            $"Статус: {row["Статус"]}"
//        );
//    }

//    static void ReportChanges(DataTable table)
//    {
//        DataRow[] addedRows = table.Select("", "", DataViewRowState.Added);
//        DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);
//        DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//        Console.WriteLine($"Добавлено сотрудников: {addedRows.Length}");
//        Console.WriteLine($"Изменено сотрудников: {modifiedRows.Length}");
//        Console.WriteLine($"Удалено сотрудников: {deletedRows.Length}");
//    }
//}


//10
//using System;
//using System.Data;
//using System.Collections.Generic;

//class OrderDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Заказы"
//        DataTable ordersTable = CreateOrdersTable();
//        FillTestData(ordersTable);

//        // Создаём DataView с фильтром по статусу "Отменено"
//        DataView cancelledOrdersView = new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== УДАЛЕНИЕ ДАННЫХ ЧЕРЕЗ DataView ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (заказы со статусом 'Отменено'):");
//        PrintDataView(cancelledOrdersView);

//        // Удаление заказа через Delete()
//        Console.WriteLine("\nУдаление заказа с OrderID = 1:");
//        DeleteOrderByID(cancelledOrdersView, 1);

//        // Выводим данные после удаления
//        Console.WriteLine("\nДанные после удаления заказа с OrderID = 1:");
//        PrintDataView(cancelledOrdersView);

//        // Массовое удаление (все заказы со статусом «Отменено»)
//        Console.WriteLine("\nМассовое удаление всех заказов со статусом 'Отменено':");
//        MassDeleteOrders(cancelledOrdersView);

//        // Выводим данные после массового удаления
//        Console.WriteLine("\nДанные после массового удаления:");
//        PrintDataView(cancelledOrdersView);

//        // Удаление с подтверждением
//        Console.WriteLine("\nУдаление с подтверждением заказа с OrderID = 2:");
//        DeleteOrderWithConfirmation(ordersTable, 2);

//        // Выводим данные после удаления с подтверждением
//        Console.WriteLine("\nДанные после удаления с подтверждением:");
//        PrintDataView(new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows));

//        // Откат удаления
//        Console.WriteLine("\nОткат удаления:");
//        RollbackDeletion(ordersTable);

//        // Выводим данные после отката удаления
//        Console.WriteLine("\nДанные после отката удаления:");
//        PrintDataView(new DataView(ordersTable, "[Статус] = 'Отменено'", "", DataViewRowState.CurrentRows));

//        // Отчёт об удалённых заказах
//        Console.WriteLine("\nОтчёт об удалённых заказах:");
//        ReportDeletedOrders(ordersTable);

//        // Статистика перед AcceptChanges
//        Console.WriteLine("\nСтатистика перед AcceptChanges:");
//        StatisticsBeforeAcceptChanges(ordersTable);
//    }

//    static DataTable CreateOrdersTable()
//    {
//        DataTable table = new DataTable("Заказы");

//        table.Columns.Add("OrderID", typeof(int));
//        table.Columns.Add("CustomerID", typeof(int));
//        table.Columns.Add("Сумма", typeof(decimal));
//        table.Columns.Add("Статус", typeof(string));
//        table.Columns.Add("Дата создания", typeof(DateTime));

//        table.PrimaryKey = new DataColumn[] { table.Columns["OrderID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] statuses = { "Новый", "В обработке", "Отменено", "Выполнено", "Оплачено" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                1000 + i,
//                Math.Round(100 + random.NextDouble() * 9900, 2),
//                statuses[random.Next(statuses.Length)],
//                DateTime.Now.AddDays(-random.Next(365))
//            );
//        }
//    }

//    static void DeleteOrderByID(DataView view, int orderID)
//    {
//        try
//        {
//            // Сортируем DataView по OrderID для использования Find()
//            view.Sort = "OrderID ASC";

//            // Ищем заказ
//            int index = view.Find(orderID);

//            if (index != -1)
//            {
//                Console.WriteLine($"Найден заказ с OrderID = {orderID}.");

//                // Удаляем заказ
//                view[index].Delete();

//                Console.WriteLine($"Заказ с OrderID = {orderID} удалён.");
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с OrderID = {orderID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при удалении заказа: {ex.Message}");
//        }
//    }

//    static void MassDeleteOrders(DataView view)
//    {
//        try
//        {
//            Console.WriteLine("Удаление всех заказов со статусом 'Отменено'...");

//            int deletedCount = 0;
//            foreach (DataRowView rowView in view)
//            {
//                rowView.Delete();
//                deletedCount++;
//            }

//            Console.WriteLine($"Удалено {deletedCount} заказов.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при массовом удалении заказов: {ex.Message}");
//        }
//    }

//    static void DeleteOrderWithConfirmation(DataTable table, int orderID)
//    {
//        try
//        {
//            DataRow orderRow = table.Rows.Find(orderID);

//            if (orderRow != null)
//            {
//                Console.WriteLine($"Найден заказ с OrderID = {orderID}.");
//                PrintOrderInfo(orderRow);

//                Console.Write("Подтвердите удаление (y/n): ");
//                string confirmation = Console.ReadLine();

//                if (confirmation.ToLower() == "y")
//                {
//                    orderRow.Delete();
//                    Console.WriteLine($"Заказ с OrderID = {orderID} удалён.");
//                }
//                else
//                {
//                    Console.WriteLine("Удаление отменено.");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с OrderID = {orderID} не найден.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при удалении заказа: {ex.Message}");
//        }
//    }

//    static void RollbackDeletion(DataTable table)
//    {
//        try
//        {
//            // Находим все удалённые строки
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            if (deletedRows.Length > 0)
//            {
//                Console.WriteLine($"Найдено {deletedRows.Length} удалённых заказов. Откат изменений...");

//                // Откат удаления для каждой строки
//                foreach (DataRow row in deletedRows)
//                {
//                    row.RejectChanges();
//                }

//                Console.WriteLine("Откат удаления выполнен.");
//            }
//            else
//            {
//                Console.WriteLine("Нет удалённых заказов для отката.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при откате удаления: {ex.Message}");
//        }
//    }

//    static void ReportDeletedOrders(DataTable table)
//    {
//        try
//        {
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            if (deletedRows.Length > 0)
//            {
//                Console.WriteLine($"Найдено {deletedRows.Length} удалённых заказов:");

//                foreach (DataRow row in deletedRows)
//                {
//                    PrintOrderInfo(row);
//                }
//            }
//            else
//            {
//                Console.WriteLine("Нет удалённых заказов.");
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при составлении отчёта об удалённых заказах: {ex.Message}");
//        }
//    }

//    static void StatisticsBeforeAcceptChanges(DataTable table)
//    {
//        try
//        {
//            DataRow[] addedRows = table.Select("", "", DataViewRowState.Added);
//            DataRow[] modifiedRows = table.Select("", "", DataViewRowState.ModifiedCurrent);
//            DataRow[] deletedRows = table.Select("", "", DataViewRowState.Deleted);

//            Console.WriteLine($"Добавлено заказов: {addedRows.Length}");
//            Console.WriteLine($"Изменено заказов: {modifiedRows.Length}");
//            Console.WriteLine($"Удалено заказов: {deletedRows.Length}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка при получении статистики: {ex.Message}");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintOrderInfo(rowView.Row);
//        }
//    }

//    static void PrintOrderInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"OrderID: {row["OrderID"]}, " +
//            $"CustomerID: {row["CustomerID"]}, " +
//            $"Сумма: {row["Сумма"]:C}, " +
//            $"Статус: {row["Статус"]}, " +
//            $"Дата создания: {((DateTime)row["Дата создания"]).ToShortDateString()}, " +
//            $"Состояние: {row.RowState}"
//        );
//    }
//}


//11
//using System;
//using System.Data;

//class CreateDataTableFromDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Продукты"
//        DataTable productsTable = CreateProductsTable();
//        FillTestData(productsTable);

//        // Создаём DataView с фильтром и сортировкой
//        DataView expensiveProductsView = new DataView(productsTable, "[Цена] > 1000", "[Цена] DESC", DataViewRowState.CurrentRows);

//        Console.WriteLine("=== СОЗДАНИЕ НОВОЙ DATATABLE ИЗ DATAVIEW ===\n");

//        // Выводим исходные данные
//        Console.WriteLine("Исходные данные (продукты дороже 1000):");
//        PrintDataView(expensiveProductsView);

//        // Способ 1: Использование DataView.ToTable()
//        Console.WriteLine("\nСпособ 1: Использование DataView.ToTable()");
//        DataTable tableFromToTable = CreateTableFromToTable(expensiveProductsView);
//        Console.WriteLine("Новая таблица из DataView.ToTable():");
//        PrintDataTable(tableFromToTable);

//        // Способ 2: Ручное копирование данных через цикл
//        Console.WriteLine("\nСпособ 2: Ручное копирование данных через цикл");
//        DataTable tableFromManualCopy = CreateTableFromManualCopy(expensiveProductsView);
//        Console.WriteLine("Новая таблица из ручного копирования:");
//        PrintDataTable(tableFromManualCopy);

//        // Способ 3: Копирование только нескольких колонок
//        Console.WriteLine("\nСпособ 3: Копирование только нескольких колонок");
//        DataTable tableFromSelectedColumns = CreateTableFromSelectedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица из выбранных колонок:");
//        PrintDataTable(tableFromSelectedColumns);

//        // Способ 4: Копирование с изменением имён колонок
//        Console.WriteLine("\nСпособ 4: Копирование с изменением имён колонок");
//        DataTable tableWithRenamedColumns = CreateTableWithRenamedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица с переименованными колонками:");
//        PrintDataTable(tableWithRenamedColumns);

//        // Создание таблиц с преобразованием данных (цена в другой валюте)
//        Console.WriteLine("\nСоздание таблиц с преобразованием данных (цена в другой валюте)");
//        DataTable tableWithConvertedPrice = CreateTableWithConvertedPrice(expensiveProductsView);
//        Console.WriteLine("Новая таблица с конвертированной ценой:");
//        PrintDataTable(tableWithConvertedPrice);

//        // Создание с добавлением новых расчетных колонок
//        Console.WriteLine("\nСоздание с добавлением новых расчетных колонок");
//        DataTable tableWithCalculatedColumns = CreateTableWithCalculatedColumns(expensiveProductsView);
//        Console.WriteLine("Новая таблица с расчетными колонками:");
//        PrintDataTable(tableWithCalculatedColumns);

//        // Сравнение исходной таблицы и созданной
//        Console.WriteLine("\nСравнение исходной таблицы и созданной:");
//        CompareTables(productsTable, tableWithCalculatedColumns);
//    }

//    static DataTable CreateProductsTable()
//    {
//        DataTable table = new DataTable("Продукты");

//        table.Columns.Add("ProductID", typeof(int));
//        table.Columns.Add("Название", typeof(string));
//        table.Columns.Add("Категория", typeof(string));
//        table.Columns.Add("Цена", typeof(decimal));
//        table.Columns.Add("В наличии", typeof(int));
//        table.Columns.Add("Поставщик", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["ProductID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Ноутбук", "Смартфон", "Планшет", "Монитор", "Клавиатура", "Мышь", "Наушники", "Колонка", "Микрофон", "Веб-камера" };
//        string[] categories = { "Электроника", "Аксессуары", "Периферия" };
//        string[] suppliers = { "Поставщик1", "Поставщик2", "Поставщик3" };

//        for (int i = 1; i <= 100; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {i}",
//                categories[random.Next(categories.Length)],
//                Math.Round(500 + random.NextDouble() * 9500, 2),
//                random.Next(100),
//                suppliers[random.Next(suppliers.Length)]
//            );
//        }
//    }

//    static DataTable CreateTableFromToTable(DataView view)
//    {
//        return view.ToTable();
//    }

//    static DataTable CreateTableFromManualCopy(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableFromSelectedColumns(DataView view)
//    {
//        DataTable newTable = new DataTable("SelectedColumns");

//        // Добавляем только нужные колонки
//        newTable.Columns.Add("ProductID", typeof(int));
//        newTable.Columns.Add("Название", typeof(string));
//        newTable.Columns.Add("Цена", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow["ProductID"] = rowView["ProductID"];
//            newRow["Название"] = rowView["Название"];
//            newRow["Цена"] = rowView["Цена"];
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithRenamedColumns(DataView view)
//    {
//        DataTable newTable = new DataTable("RenamedColumns");

//        // Добавляем колонки с новыми именами
//        newTable.Columns.Add("ID", typeof(int));
//        newTable.Columns.Add("ProductName", typeof(string));
//        newTable.Columns.Add("Category", typeof(string));
//        newTable.Columns.Add("Price", typeof(decimal));
//        newTable.Columns.Add("InStock", typeof(int));
//        newTable.Columns.Add("Supplier", typeof(string));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow["ID"] = rowView["ProductID"];
//            newRow["ProductName"] = rowView["Название"];
//            newRow["Category"] = rowView["Категория"];
//            newRow["Price"] = rowView["Цена"];
//            newRow["InStock"] = rowView["В наличии"];
//            newRow["Supplier"] = rowView["Поставщик"];
//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithConvertedPrice(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        // Добавляем колонку для цены в другой валюте
//        newTable.Columns.Add("ЦенаUSD", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;

//            // Конвертация цены в доллары (например, курс 0.01)
//            decimal priceInRubles = (decimal)rowView["Цена"];
//            newRow["ЦенаUSD"] = Math.Round(priceInRubles * 0.01m, 2);

//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static DataTable CreateTableWithCalculatedColumns(DataView view)
//    {
//        DataTable newTable = view.Table.Clone();

//        // Добавляем расчетную колонку для общей стоимости (Цена * В наличии)
//        newTable.Columns.Add("ОбщаяСтоимость", typeof(decimal));

//        foreach (DataRowView rowView in view)
//        {
//            DataRow newRow = newTable.NewRow();
//            newRow.ItemArray = rowView.Row.ItemArray;

//            // Рассчитываем общую стоимость
//            decimal price = (decimal)rowView["Цена"];
//            int inStock = (int)rowView["В наличии"];
//            newRow["ОбщаяСтоимость"] = price * inStock;

//            newTable.Rows.Add(newRow);
//        }

//        return newTable;
//    }

//    static void CompareTables(DataTable originalTable, DataTable newTable)
//    {
//        Console.WriteLine($"Исходная таблица содержит {originalTable.Rows.Count} строк.");
//        Console.WriteLine($"Новая таблица содержит {newTable.Rows.Count} строк.");

//        Console.WriteLine("\nСтруктура исходной таблицы:");
//        foreach (DataColumn column in originalTable.Columns)
//        {
//            Console.WriteLine($"- {column.ColumnName} ({column.DataType.Name})");
//        }

//        Console.WriteLine("\nСтруктура новой таблицы:");
//        foreach (DataColumn column in newTable.Columns)
//        {
//            Console.WriteLine($"- {column.ColumnName} ({column.DataType.Name})");
//        }
//    }

//    static void PrintDataView(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRowView rowView in view)
//        {
//            PrintProductInfo(rowView.Row);
//        }
//    }

//    static void PrintDataTable(DataTable table)
//    {
//        if (table.Rows.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        foreach (DataRow row in table.Rows)
//        {
//            PrintProductInfo(row);
//        }
//    }

//    static void PrintProductInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"ProductID: {GetValueOrDefault(row, "ProductID", "N/A")}, " +
//            $"Название: {GetValueOrDefault(row, "Название", "N/A")}, " +
//            $"Категория: {GetValueOrDefault(row, "Категория", "N/A")}, " +
//            $"Цена: {GetValueOrDefault(row, "Цена", "N/A")}, " +
//            $"В наличии: {GetValueOrDefault(row, "В наличии", "N/A")}, " +
//            $"Поставщик: {GetValueOrDefault(row, "Поставщик", "N/A")}, " +
//            $"ЦенаUSD: {GetValueOrDefault(row, "ЦенаUSD", "N/A")}, " +
//            $"ОбщаяСтоимость: {GetValueOrDefault(row, "ОбщаяСтоимость", "N/A")}, " +
//            $"ID: {GetValueOrDefault(row, "ID", "N/A")}, " +
//            $"ProductName: {GetValueOrDefault(row, "ProductName", "N/A")}"
//        );
//    }

//    static string GetValueOrDefault(DataRow row, string columnName, string defaultValue)
//    {
//        if (row.Table.Columns.Contains(columnName))
//        {
//            return row[columnName].ToString();
//        }
//        else
//        {
//            return defaultValue;
//        }
//    }
//}


//12
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//class BankAccountSearchDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Счета в банке"
//        DataTable accountsTable = CreateAccountsTable();
//        FillTestData(accountsTable);

//        Console.WriteLine("=== КОМПЛЕКСНЫЙ ПОИСК В DATATABLE ===\n");

//        // Поиск по PK с использованием Find()
//        Console.WriteLine("Поиск по PK (AccountID = 1001):");
//        SearchByPrimaryKey(accountsTable, 1001);

//        // Поиск с использованием Select()
//        Console.WriteLine("\nПоиск с использованием Select() (Баланс > 50000, Статус = Активный):");
//        SearchByCriteria(accountsTable, "Balance > 50000 AND Status = 'Активный'");

//        // Поиск с использованием DataView
//        Console.WriteLine("\nПоиск с использованием DataView (Баланс > 50000, Статус = Активный, Тип счёта = Сберегательный):");
//        SearchByDataView(accountsTable, "Balance > 50000 AND Status = 'Активный' AND AccountType = 'Сберегательный'");

//        // Умный поиск
//        Console.WriteLine("\nУмный поиск:");
//        SmartSearch(accountsTable, "AccountID", "1001");
//        SmartSearch(accountsTable, "Balance", "50000");
//        SmartSearch(accountsTable, "Status,Balance,AccountType", "Активный,50000,Сберегательный");

//        // Поиск с автодополнением
//        Console.WriteLine("\nПоиск с автодополнением (по первым буквам имени):");
//        AutoCompleteSearch(accountsTable, "Иван");

//        // Отчёт о найденных счётах
//        Console.WriteLine("\nОтчёт о найденных счётах:");
//        ReportFoundAccounts(accountsTable, "Balance > 50000 AND Status = 'Активный'");

//        // Измерение производительности каждого метода
//        Console.WriteLine("\nИзмерение производительности:");
//        MeasurePerformance(accountsTable);
//    }

//    static DataTable CreateAccountsTable()
//    {
//        DataTable table = new DataTable("Счета в банке");

//        table.Columns.Add("AccountID", typeof(int));
//        table.Columns.Add("CustomerName", typeof(string));
//        table.Columns.Add("Balance", typeof(decimal));
//        table.Columns.Add("AccountType", typeof(string));
//        table.Columns.Add("OpenDate", typeof(DateTime));
//        table.Columns.Add("Status", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["AccountID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] customerNames = { "Иванов Иван", "Петров Петр", "Сидоров Сидор", "Кузнецов Алексей", "Васильев Василий" };
//        string[] accountTypes = { "Сберегательный", "Текущий", "Зарплатный" };
//        string[] statuses = { "Активный", "Замороженный", "Закрытый" };

//        for (int i = 1; i <= 200; i++)
//        {
//            table.Rows.Add(
//                1000 + i,
//                customerNames[random.Next(customerNames.Length)] + " " + i,
//                Math.Round(1000 + random.NextDouble() * 99000, 2),
//                accountTypes[random.Next(accountTypes.Length)],
//                DateTime.Now.AddDays(-random.Next(365 * 5)),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void SearchByPrimaryKey(DataTable table, int accountID)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow row = table.Rows.Find(accountID);
//        stopwatch.Stop();

//        if (row != null)
//        {
//            Console.WriteLine($"Найден счёт с AccountID = {accountID}:");
//            PrintAccountInfo(row);
//        }
//        else
//        {
//            Console.WriteLine($"Счёт с AccountID = {accountID} не найден.");
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SearchByCriteria(DataTable table, string criteria)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow[] rows = table.Select(criteria);
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {rows.Length} счётов по критерию '{criteria}':");
//        foreach (DataRow row in rows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SearchByDataView(DataTable table, string criteria)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataView view = new DataView(table, criteria, "", DataViewRowState.CurrentRows);
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {view.Count} счётов по критерию '{criteria}':");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            PrintAccountInfo(view[i].Row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void SmartSearch(DataTable table, string fields, string values)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        string[] fieldList = fields.Split(',');
//        string[] valueList = values.Split(',');

//        DataRow[] resultRows = null;
//        string methodUsed = "";

//        if (fieldList.Length == 1 && fieldList[0] == "AccountID")
//        {
//            int accountID = int.Parse(valueList[0]);
//            DataRow row = table.Rows.Find(accountID);
//            resultRows = row != null ? new DataRow[] { row } : new DataRow[0];
//            methodUsed = "Find()";
//        }
//        else if (fieldList.Length == 1)
//        {
//            string criteria = $"{fieldList[0]} = '{valueList[0]}'";
//            if (fieldList[0] == "Balance")
//            {
//                criteria = $"{fieldList[0]} > {valueList[0]}";
//            }
//            resultRows = table.Select(criteria);
//            methodUsed = "Select()";
//        }
//        else
//        {
//            string criteria = "";
//            for (int i = 0; i < fieldList.Length; i++)
//            {
//                if (i > 0) criteria += " AND ";
//                if (fieldList[i] == "Balance")
//                {
//                    criteria += $"{fieldList[i]} > {valueList[i]}";
//                }
//                else
//                {
//                    criteria += $"{fieldList[i]} = '{valueList[i]}'";
//                }
//            }
//            DataView view = new DataView(table, criteria, "", DataViewRowState.CurrentRows);
//            resultRows = view.Table.Select(criteria);
//            methodUsed = "DataView";
//        }

//        stopwatch.Stop();

//        Console.WriteLine($"Умный поиск с использованием {methodUsed}:");
//        Console.WriteLine($"Найдено {resultRows.Length} счётов:");
//        foreach (DataRow row in resultRows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void AutoCompleteSearch(DataTable table, string namePrefix)
//    {
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        DataRow[] rows = table.Select($"CustomerName LIKE '{namePrefix}%'");
//        stopwatch.Stop();

//        Console.WriteLine($"Найдено {rows.Length} счётов с именем, начинающимся на '{namePrefix}':");
//        foreach (DataRow row in rows.Take(5))
//        {
//            PrintAccountInfo(row);
//        }

//        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedTicks} тиков");
//    }

//    static void ReportFoundAccounts(DataTable table, string criteria)
//    {
//        DataRow[] rows = table.Select(criteria);

//        Console.WriteLine($"Отчёт о найденных счётах по критерию '{criteria}':");
//        Console.WriteLine($"Всего найдено: {rows.Length} счётов");

//        decimal totalBalance = 0;
//        foreach (DataRow row in rows)
//        {
//            totalBalance += (decimal)row["Balance"];
//        }

//        Console.WriteLine($"Общий баланс: {totalBalance:C}");
//    }

//    static void MeasurePerformance(DataTable table)
//    {
//        int iterations = 1000;
//        Random random = new Random();

//        // Измерение производительности Find()
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        for (int i = 0; i < iterations; i++)
//        {
//            int accountID = 1001 + random.Next(199);
//            DataRow row = table.Rows.Find(accountID);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Find(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности Select()
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = random.Next(3) == 0 ? "Активный" : (random.Next(2) == 0 ? "Замороженный" : "Закрытый");
//            DataRow[] rows = table.Select($"Status = '{status}'");
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Select(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности DataView
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            string status = random.Next(3) == 0 ? "Активный" : (random.Next(2) == 0 ? "Замороженный" : "Закрытый");
//            DataView view = new DataView(table, $"Status = '{status}'", "", DataViewRowState.CurrentRows);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"DataView: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");
//    }

//    static void PrintAccountInfo(DataRow row)
//    {
//        Console.WriteLine(
//            $"AccountID: {row["AccountID"]}, " +
//            $"CustomerName: {row["CustomerName"]}, " +
//            $"Balance: {row["Balance"]:C}, " +
//            $"AccountType: {row["AccountType"]}, " +
//            $"OpenDate: {((DateTime)row["OpenDate"]).ToShortDateString()}, " +
//            $"Status: {row["Status"]}"
//        );
//    }
//}


//13
//using System;
//using System.Data;
//using System.IO;
//using System.Collections.Generic;

//class EventFilteringDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "События"
//        DataTable eventsTable = CreateEventsTable();
//        FillTestData(eventsTable);

//        // Создаём DataView
//        DataView eventsView = new DataView(eventsTable);

//        Console.WriteLine("=== МНОГОУРОВНЕВАЯ ФИЛЬТРАЦИЯ СОБЫТИЙ ===\n");

//        // Уровень 1: Фильтр по категории
//        Console.WriteLine("Уровень 1: Фильтр по категории 'Конференция'");
//        ApplyCategoryFilter(eventsView, "Категория = 'Конференция'");

//        // Уровень 2: Добавление фильтра по приоритету
//        Console.WriteLine("\nУровень 2: Добавление фильтра по приоритету 'Высокий'");
//        ApplyPriorityFilter(eventsView, "Приоритет = 'Высокий'");

//        // Уровень 3: Добавление фильтра по дате
//        Console.WriteLine("\nУровень 3: Добавление фильтра по дате (будущие события)");
//        ApplyDateFilter(eventsView, DateTime.Now);

//        // Уровень 4: Добавление фильтра по количеству участников
//        Console.WriteLine("\nУровень 4: Добавление фильтра по количеству участников (> 50)");
//        ApplyParticipantsFilter(eventsView, 50);

//        // Удаление фильтра по дате
//        Console.WriteLine("\nУдаление фильтра по дате");
//        RemoveDateFilter(eventsView);

//        // Сброс всех фильтров
//        Console.WriteLine("\nСброс всех фильтров");
//        ResetAllFilters(eventsView);

//        // Сохранение конфигурации фильтров
//        Console.WriteLine("\nСохранение конфигурации фильтров");
//        SaveFilterConfiguration(eventsView, "filters.cfg");

//        // Загрузка конфигурации фильтров
//        Console.WriteLine("\nЗагрузка конфигурации фильтров");
//        LoadFilterConfiguration(eventsView, "filters.cfg");
//    }

//    static DataTable CreateEventsTable()
//    {
//        DataTable table = new DataTable("События");

//        table.Columns.Add("EventID", typeof(int));
//        table.Columns.Add("EventName", typeof(string));
//        table.Columns.Add("Дата", typeof(DateTime));
//        table.Columns.Add("Место", typeof(string));
//        table.Columns.Add("Категория", typeof(string));
//        table.Columns.Add("Приоритет", typeof(string));
//        table.Columns.Add("Статус", typeof(string));
//        table.Columns.Add("Участники", typeof(int));

//        table.PrimaryKey = new DataColumn[] { table.Columns["EventID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] eventNames = { "Конференция по IT", "Выставка технологий", "Семинар по маркетингу", "Встреча партнеров", "Тренинг по продажам" };
//        string[] places = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };
//        string[] categories = { "Конференция", "Выставка", "Семинар", "Встреча", "Тренинг" };
//        string[] priorities = { "Низкий", "Средний", "Высокий" };
//        string[] statuses = { "Планируется", "Идет", "Завершено" };

//        for (int i = 1; i <= 300; i++)
//        {
//            table.Rows.Add(
//                i,
//                eventNames[random.Next(eventNames.Length)] + " " + i,
//                DateTime.Now.AddDays(random.Next(-30, 90)),
//                places[random.Next(places.Length)],
//                categories[random.Next(categories.Length)],
//                priorities[random.Next(priorities.Length)],
//                statuses[random.Next(statuses.Length)],
//                random.Next(100)
//            );
//        }
//    }

//    static void ApplyCategoryFilter(DataView view, string category)
//    {
//        view.RowFilter = category;
//        Console.WriteLine($"Применён фильтр по категории. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyPriorityFilter(DataView view, string priority)
//    {
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = priority;
//        }
//        else
//        {
//            view.RowFilter += $" AND {priority}";
//        }
//        Console.WriteLine($"Добавлен фильтр по приоритету. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyDateFilter(DataView view, DateTime currentDate)
//    {
//        string dateFilter = $"Дата > #{currentDate:yyyy-MM-dd}#";
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = dateFilter;
//        }
//        else
//        {
//            view.RowFilter += $" AND {dateFilter}";
//        }
//        Console.WriteLine($"Добавлен фильтр по дате. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ApplyParticipantsFilter(DataView view, int minParticipants)
//    {
//        string participantsFilter = $"Участники > {minParticipants}";
//        if (string.IsNullOrEmpty(view.RowFilter))
//        {
//            view.RowFilter = participantsFilter;
//        }
//        else
//        {
//            view.RowFilter += $" AND {participantsFilter}";
//        }
//        Console.WriteLine($"Добавлен фильтр по количеству участников. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void RemoveDateFilter(DataView view)
//    {
//        if (!string.IsNullOrEmpty(view.RowFilter))
//        {
//            string[] filters = view.RowFilter.Split(new[] { " AND " }, StringSplitOptions.None);
//            List<string> newFilters = new List<string>();

//            foreach (string filter in filters)
//            {
//                if (!filter.Contains("Дата > #"))
//                {
//                    newFilters.Add(filter);
//                }
//            }

//            view.RowFilter = string.Join(" AND ", newFilters);
//        }
//        Console.WriteLine($"Фильтр по дате удалён. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void ResetAllFilters(DataView view)
//    {
//        view.RowFilter = "";
//        Console.WriteLine($"Все фильтры сброшены. Найдено событий: {view.Count}");
//        PrintDataViewInfo(view);
//    }

//    static void SaveFilterConfiguration(DataView view, string filePath)
//    {
//        File.WriteAllText(filePath, view.RowFilter);
//        Console.WriteLine($"Конфигурация фильтров сохранена в файл {filePath}");
//    }

//    static void LoadFilterConfiguration(DataView view, string filePath)
//    {
//        if (File.Exists(filePath))
//        {
//            string filter = File.ReadAllText(filePath);
//            view.RowFilter = filter;
//            Console.WriteLine($"Конфигурация фильтров загружена из файла {filePath}. Найдено событий: {view.Count}");
//            PrintDataViewInfo(view);
//        }
//        else
//        {
//            Console.WriteLine($"Файл {filePath} не найден.");
//        }
//    }

//    static void PrintDataViewInfo(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        Console.WriteLine("Первые 5 событий:");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"EventID: {rowView["EventID"]}, " +
//                $"EventName: {rowView["EventName"]}, " +
//                $"Дата: {((DateTime)rowView["Дата"]).ToShortDateString()}, " +
//                $"Категория: {rowView["Категория"]}, " +
//                $"Приоритет: {rowView["Приоритет"]}, " +
//                $"Участники: {rowView["Участники"]}"
//            );
//        }
//    }
//}


////14
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;

//class SalesSortingDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Продажи сотрудников"
//        DataTable salesTable = CreateSalesTable();
//        FillTestData(salesTable);

//        // Создаём DataView
//        DataView salesView = new DataView(salesTable);

//        Console.WriteLine("=== МНОГОУРОВНЕВАЯ СОРТИРОВКА В DATAVIEW ===\n");

//        // Сортировка по одной колонке (Сумма DESC)
//        Console.WriteLine("Сортировка по одной колонке (Сумма DESC):");
//        SortBySingleColumn(salesView, "Сумма DESC");

//        // Сортировка по соседним колонкам (Отдел ASC, Сумма DESC)
//        Console.WriteLine("\nСортировка по соседним колонкам (Отдел ASC, Сумма DESC):");
//        SortByMultipleColumns(salesView, "Отдел ASC, Сумма DESC");

//        // Сортировка по трём колонкам (Регион ASC, Отдел ASC, Дата DESC)
//        Console.WriteLine("\nСортировка по трём колонкам (Регион ASC, Отдел ASC, Дата DESC):");
//        SortByThreeColumns(salesView, "Регион ASC, Отдел ASC, Дата DESC");

//        // Динамическая сортировка на основе пользовательского выбора
//        Console.WriteLine("\nДинамическая сортировка на основе пользовательского выбора:");
//        DynamicSorting(salesView);

//        // Переключение между возрастанием и убыванием по одной колонке
//        Console.WriteLine("\nПереключение между возрастанием и убыванием по колонке 'Сумма':");
//        ToggleSortDirection(salesView, "Сумма");

//        // Сравнение сортировки в DataView с сортировкой через Select()
//        Console.WriteLine("\nСравнение сортировки в DataView с сортировкой через Select():");
//        CompareSortingPerformance(salesTable);
//    }

//    static DataTable CreateSalesTable()
//    {
//        DataTable table = new DataTable("Продажи сотрудников");

//        table.Columns.Add("SalesID", typeof(int));
//        table.Columns.Add("Имя Сотрудника", typeof(string));
//        table.Columns.Add("Отдел", typeof(string));
//        table.Columns.Add("Сумма", typeof(decimal));
//        table.Columns.Add("Дата", typeof(DateTime));
//        table.Columns.Add("Квартал", typeof(int));
//        table.Columns.Add("Регион", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["SalesID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] departments = { "Продажи", "Маркетинг", "IT", "Финансы", "HR" };
//        string[] regions = { "Москва", "Санкт-Петербург", "Новосибирск", "Екатеринбург", "Казань" };

//        for (int i = 1; i <= 500; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                departments[random.Next(departments.Length)],
//                Math.Round(1000 + random.NextDouble() * 99000, 2),
//                DateTime.Now.AddDays(-random.Next(365)),
//                random.Next(1, 4),
//                regions[random.Next(regions.Length)]
//            );
//        }
//    }

//    static void SortBySingleColumn(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void SortByMultipleColumns(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void SortByThreeColumns(DataView view, string sortExpression)
//    {
//        view.Sort = sortExpression;
//        Console.WriteLine($"Применена сортировка: {sortExpression}");
//        PrintDataViewInfo(view);
//    }

//    static void DynamicSorting(DataView view)
//    {
//        Console.WriteLine("Выберите колонку для сортировки:");
//        Console.WriteLine("1. Имя Сотрудника");
//        Console.WriteLine("2. Отдел");
//        Console.WriteLine("3. Сумма");
//        Console.WriteLine("4. Дата");
//        Console.WriteLine("5. Квартал");
//        Console.WriteLine("6. Регион");

//        Console.Write("Введите номер колонки: ");
//        int columnChoice = int.Parse(Console.ReadLine());

//        Console.WriteLine("Выберите направление сортировки:");
//        Console.WriteLine("1. По возрастанию (ASC)");
//        Console.WriteLine("2. По убыванию (DESC)");

//        Console.Write("Введите номер направления: ");
//        int directionChoice = int.Parse(Console.ReadLine());

//        string columnName = "";
//        switch (columnChoice)
//        {
//            case 1:
//                columnName = "Имя Сотрудника";
//                break;
//            case 2:
//                columnName = "Отдел";
//                break;
//            case 3:
//                columnName = "Сумма";
//                break;
//            case 4:
//                columnName = "Дата";
//                break;
//            case 5:
//                columnName = "Квартал";
//                break;
//            case 6:
//                columnName = "Регион";
//                break;
//            default:
//                Console.WriteLine("Некорректный выбор.");
//                return;
//        }

//        string direction = directionChoice == 1 ? "ASC" : "DESC";
//        view.Sort = $"{columnName} {direction}";
//        Console.WriteLine($"Применена динамическая сортировка: {columnName} {direction}");
//        PrintDataViewInfo(view);
//    }

//    static void ToggleSortDirection(DataView view, string columnName)
//    {
//        if (view.Sort.Contains(columnName))
//        {
//            if (view.Sort.Contains("DESC"))
//            {
//                view.Sort = view.Sort.Replace("DESC", "ASC");
//                Console.WriteLine($"Сортировка изменена на {columnName} ASC");
//            }
//            else if (view.Sort.Contains("ASC"))
//            {
//                view.Sort = view.Sort.Replace("ASC", "DESC");
//                Console.WriteLine($"Сортировка изменена на {columnName} DESC");
//            }
//            else
//            {
//                view.Sort = $"{columnName} DESC";
//                Console.WriteLine($"Сортировка установлена на {columnName} DESC");
//            }
//        }
//        else
//        {
//            view.Sort = $"{columnName} DESC";
//            Console.WriteLine($"Сортировка установлена на {columnName} DESC");
//        }
//        PrintDataViewInfo(view);
//    }

//    static void CompareSortingPerformance(DataTable table)
//    {
//        int iterations = 1000;
//        Random random = new Random();

//        // Измерение производительности сортировки в DataView
//        Stopwatch stopwatch = Stopwatch.StartNew();
//        for (int i = 0; i < iterations; i++)
//        {
//            DataView view = new DataView(table, "", "Сумма DESC", DataViewRowState.CurrentRows);
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Сортировка в DataView: {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");

//        // Измерение производительности сортировки через Select()
//        stopwatch.Restart();
//        for (int i = 0; i < iterations; i++)
//        {
//            DataRow[] rows = table.Select("", "Сумма DESC");
//        }
//        stopwatch.Stop();
//        Console.WriteLine($"Сортировка через Select(): {stopwatch.ElapsedMilliseconds} мс за {iterations} итераций");
//    }

//    static void PrintDataViewInfo(DataView view)
//    {
//        if (view.Count == 0)
//        {
//            Console.WriteLine("Нет данных для отображения.");
//            return;
//        }

//        Console.WriteLine($"Найдено записей: {view.Count}");
//        Console.WriteLine("Первые 5 записей:");
//        for (int i = 0; i < Math.Min(5, view.Count); i++)
//        {
//            DataRowView rowView = view[i];
//            Console.WriteLine(
//                $"SalesID: {rowView["SalesID"]}, " +
//                $"Имя Сотрудника: {rowView["Имя Сотрудника"]}, " +
//                $"Отдел: {rowView["Отдел"]}, " +
//                $"Сумма: {rowView["Сумма"]:C}, " +
//                $"Дата: {((DateTime)rowView["Дата"]).ToShortDateString()}, " +
//                $"Регион: {rowView["Регион"]}"
//            );
//        }
//    }
//}


//15
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;

//class StudentDataViewDemo
//{
//    static void Main()
//    {
//        // Создаём и заполняем DataTable "Студенты"
//        DataTable studentsTable = CreateStudentsTable();
//        FillTestData(studentsTable);

//        Console.WriteLine("=== РАБОТА С НЕСКОЛЬКИМИ DATAVIEW ===\n");

//        // Создаём несколько DataView
//        DataView topStudentsView = new DataView(studentsTable, "СреднийБалл > 4.0", "", DataViewRowState.CurrentRows);
//        DataView firstYearStudentsView = new DataView(studentsTable, "Год = 1", "", DataViewRowState.CurrentRows);
//        DataView academicLeaveStudentsView = new DataView(studentsTable, "Статус = 'Академический отпуск'", "", DataViewRowState.CurrentRows);
//        DataView allStudentsByFacultyView = new DataView(studentsTable, "", "Факультет ASC, Имя ASC", DataViewRowState.CurrentRows);

//        // Анализ каждого DataView
//        Console.WriteLine("Анализ DataView 1: Лучшие студенты (GPA > 4.0)");
//        AnalyzeDataView(topStudentsView);

//        Console.WriteLine("\nАнализ DataView 2: Студенты первого курса");
//        AnalyzeDataView(firstYearStudentsView);

//        Console.WriteLine("\nАнализ DataView 3: Студенты в академическом отпуске");
//        AnalyzeDataView(academicLeaveStudentsView);

//        Console.WriteLine("\nАнализ DataView 4: Все студенты, отсортированные по факультетам");
//        AnalyzeDataView(allStudentsByFacultyView);

//        // Поиск студента одновременно во всех DataView
//        Console.WriteLine("\nПоиск студента одновременно во всех DataView (StudentID = 1001):");
//        SearchStudentInAllViews(new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView }, 1001);

//        // Добавление нового студента
//        Console.WriteLine("\nДобавление нового студента:");
//        AddNewStudent(studentsTable, 201, "Новый Студент", 4.5m, "Информатика", 1, "Активен");

//        // Проверка влияния на все DataView
//        Console.WriteLine("\nПроверка влияния на все DataView после добавления нового студента:");
//        AnalyzeDataView(topStudentsView);
//        AnalyzeDataView(firstYearStudentsView);
//        AnalyzeDataView(academicLeaveStudentsView);
//        AnalyzeDataView(allStudentsByFacultyView);

//        // Отчёт о всех представлениях
//        Console.WriteLine("\nОтчёт о всех представлениях:");
//        ReportAllViews(new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView });

//        // Измерение использования памяти
//        Console.WriteLine("\nИзмерение использования памяти:");
//        MeasureMemoryUsage(studentsTable, new DataView[] { topStudentsView, firstYearStudentsView, academicLeaveStudentsView, allStudentsByFacultyView });
//    }

//    static DataTable CreateStudentsTable()
//    {
//        DataTable table = new DataTable("Студенты");

//        table.Columns.Add("StudentID", typeof(int));
//        table.Columns.Add("Имя", typeof(string));
//        table.Columns.Add("СреднийБалл", typeof(decimal));
//        table.Columns.Add("Факультет", typeof(string));
//        table.Columns.Add("Год", typeof(int));
//        table.Columns.Add("Статус", typeof(string));

//        table.PrimaryKey = new DataColumn[] { table.Columns["StudentID"] };

//        return table;
//    }

//    static void FillTestData(DataTable table)
//    {
//        Random random = new Random();
//        string[] names = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Васильев", "Смирнов", "Новиков", "Федоров", "Морозов", "Волков" };
//        string[] faculties = { "Информатика", "Математика", "Физика", "Химия", "Биология" };
//        string[] statuses = { "Активен", "Академический отпуск", "Отчислен" };

//        for (int i = 1; i <= 200; i++)
//        {
//            table.Rows.Add(
//                1000 + i,
//                $"{names[random.Next(names.Length)]} {names[random.Next(names.Length)]}",
//                Math.Round(2.0 + random.NextDouble() * 3, 2),
//                faculties[random.Next(faculties.Length)],
//                random.Next(1, 5),
//                statuses[random.Next(statuses.Length)]
//            );
//        }
//    }

//    static void AnalyzeDataView(DataView view)
//    {
//        Console.WriteLine($"Количество студентов: {view.Count}");

//        if (view.Count > 0)
//        {
//            Console.WriteLine("Первые 5 студентов:");
//            for (int i = 0; i < Math.Min(5, view.Count); i++)
//            {
//                DataRowView rowView = view[i];
//                Console.WriteLine(
//                    $"StudentID: {rowView["StudentID"]}, " +
//                    $"Имя: {rowView["Имя"]}, " +
//                    $"СреднийБалл: {rowView["СреднийБалл"]}, " +
//                    $"Факультет: {rowView["Факультет"]}, " +
//                    $"Год: {rowView["Год"]}, " +
//                    $"Статус: {rowView["Статус"]}"
//                );
//            }

//            decimal totalGPA = 0;
//            foreach (DataRowView rowView in view)
//            {
//                totalGPA += Convert.ToDecimal(rowView["СреднийБалл"]);
//            }
//            decimal averageGPA = totalGPA / view.Count;
//            Console.WriteLine($"Среднее GPA: {averageGPA:F2}");
//        }
//        else
//        {
//            Console.WriteLine("Нет данных для отображения.");
//        }
//    }

//    static void SearchStudentInAllViews(DataView[] views, int studentID)
//    {
//        foreach (DataView view in views)
//        {
//            bool found = false;
//            foreach (DataRowView rowView in view)
//            {
//                if (Convert.ToInt32(rowView["StudentID"]) == studentID)
//                {
//                    Console.WriteLine($"Студент найден в представлении с фильтром '{view.RowFilter}'");
//                    Console.WriteLine(
//                        $"StudentID: {rowView["StudentID"]}, " +
//                        $"Имя: {rowView["Имя"]}, " +
//                        $"СреднийБалл: {rowView["СреднийБалл"]}, " +
//                        $"Факультет: {rowView["Факультет"]}"
//                    );
//                    found = true;
//                    break;
//                }
//            }
//            if (!found)
//            {
//                Console.WriteLine($"Студент не найден в представлении с фильтром '{view.RowFilter}'");
//            }
//        }
//    }

//    static void AddNewStudent(DataTable table, int studentID, string name, decimal gpa, string faculty, int year, string status)
//    {
//        DataRow newRow = table.NewRow();
//        newRow["StudentID"] = studentID;
//        newRow["Имя"] = name;
//        newRow["СреднийБалл"] = gpa;
//        newRow["Факультет"] = faculty;
//        newRow["Год"] = year;
//        newRow["Статус"] = status;
//        table.Rows.Add(newRow);

//        Console.WriteLine($"Студент {name} добавлен.");
//    }

//    static void ReportAllViews(DataView[] views)
//    {
//        foreach (DataView view in views)
//        {
//            Console.WriteLine($"\nФильтр: {view.RowFilter}, Сортировка: {view.Sort}");
//            Console.WriteLine($"Количество студентов: {view.Count}");
//        }
//    }

//    static void MeasureMemoryUsage(DataTable table, DataView[] views)
//    {
//        long beforeMemory = GC.GetTotalMemory(true);
//        Console.WriteLine($"Память до создания DataView: {beforeMemory / 1024} KB");

//        long afterMemory = GC.GetTotalMemory(true);
//        Console.WriteLine($"Память после создания DataView: {afterMemory / 1024} KB");

//        Console.WriteLine($"Использование памяти для DataView: {(afterMemory - beforeMemory) / 1024} KB");
//    }
//}


//15
//using System;
//using System.Data;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Студенты");
//        dt.Columns.Add("StudentID", typeof(int));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("GPA", typeof(double));
//        dt.Columns.Add("Faculty", typeof(string));
//        dt.Columns.Add("Year", typeof(int));
//        dt.Columns.Add("Status", typeof(string));

//        string[] faculties = { "Математический", "Физический", "Биологический", "Химический", "Информатика" };
//        string[] statuses = { "Активен", "Академический отпуск", "Отчислен" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 200; i++)
//        {
//            dt.Rows.Add(i, $"Студент{i}", Math.Round(2.0 + rnd.NextDouble() * 3.0, 2),
//                faculties[rnd.Next(faculties.Length)], rnd.Next(1, 6), statuses[rnd.Next(statuses.Length)]);
//        }

//        long memBefore = GC.GetTotalMemory(true);

//        DataView dv1 = new DataView(dt) { RowFilter = "GPA > 4.0" };
//        DataView dv2 = new DataView(dt) { RowFilter = "Year = 1" };
//        DataView dv3 = new DataView(dt) { RowFilter = "Status = 'Академический отпуск'" };
//        DataView dv4 = new DataView(dt) { Sort = "Faculty" };

//        long memAfter = GC.GetTotalMemory(true);
//        Console.WriteLine($"Потребление памяти на 4 DataView: {(memAfter - memBefore)} байт\n");

//        DataView[] views = { dv1, dv2, dv3, dv4 };
//        string[] names = { "Лучшие студенты (GPA > 4.0)", "Студенты 1-го курса", "Студенты в академическом отпуске", "Все студенты (отсортированы по факультету)" };

//        Console.WriteLine("=== Начальный отчёт ===\n");
//        for (int i = 0; i < views.Length; i++)
//        {
//            DataView dv = views[i];
//            string filter = i == 3 ? "" : dv.RowFilter;
//            int count = dv.Count;
//            double avg = filter == "" ? (double)dt.Compute("AVG(GPA)", null) : (double)dt.Compute("AVG(GPA)", filter);

//            Console.WriteLine($"Представление: {names[i]}");
//            Console.WriteLine($"Количество студентов: {count}");
//            Console.WriteLine($"Средний GPA: {avg:F2}\n");
//            Console.WriteLine("Список студентов:");
//            foreach (DataRowView row in dv)
//                Console.WriteLine($"{row["StudentID"]}\t{row["Name"]}\t{row["GPA"]}\t{row["Faculty"]}\t{row["Year"]}\t{row["Status"]}");
//            Console.WriteLine(new string('-', 50));
//        }

//        Console.WriteLine("\n=== Поиск студента 'Студент100' во всех представлениях ===");
//        string search = "Студент100";
//        for (int i = 0; i < views.Length; i++)
//        {
//            bool found = false;
//            foreach (DataRowView row in views[i])
//                if (row["Name"].ToString() == search) { found = true; break; }
//            Console.WriteLine($"В представлении \"{names[i]}\": {(found ? "найден" : "не найден")}");
//        }

//        Console.WriteLine("\n=== Добавляем нового студента ===");
//        dt.Rows.Add(201, "НовыйСтудент", 4.8, "Информатика", 1, "Активен");

//        Console.WriteLine("Количество студентов после добавления:");
//        for (int i = 0; i < views.Length; i++)
//            Console.WriteLine($"{names[i]} → {views[i].Count} студентов");

//        Console.WriteLine("\n=== Обновлённый отчёт (после добавления) ===");
//        for (int i = 0; i < views.Length; i++)
//        {
//            DataView dv = views[i];
//            string filter = i == 3 ? "" : dv.RowFilter;
//            int count = dv.Count;
//            double avg = filter == "" ? (double)dt.Compute("AVG(GPA)", null) : (double)dt.Compute("AVG(GPA)", filter);

//            Console.WriteLine($"\nПредставление: {names[i]}");
//            Console.WriteLine($"Количество: {count}");
//            Console.WriteLine($"Средний GPA: {avg:F2}");
//        }
//    }
//}

//16
//using System;
//using System.Data;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Фильмы");
//    static DataView dv;

//    static string текущийФильтр = "";
//    static string текущаяСортировка = "Year DESC";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        dv = new DataView(dt);

//        Console.WriteLine("=== Динамический фильтр фильмов ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  g <жанр>     — фильтр по жанру (или g all — снять)");
//        Console.WriteLine("  t <текст>    — поиск по названию (или t — снять)");
//        Console.WriteLine("  r <мин> <макс> — диапазон рейтинга (4.0–10.0)");
//        Console.WriteLine("  s year | rating | box — сортировка");
//        Console.WriteLine("  reset        — сброс всех фильтров");
//        Console.WriteLine("  show         — показать текущие результаты");
//        Console.WriteLine("  count        — показать количество");
//        Console.WriteLine("  exit         — выход\n");

//        ПрименитьФильтр();
//        ПоказатьКоличество();

//        while (true)
//        {
//            Console.Write("\n> ");
//            string input = Console.ReadLine().Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "g":
//                        if (parts.Length == 1) { Console.WriteLine("Укажите жанр или 'all'"); break; }
//                        string жанр = string.Join(" ", parts.Skip(1));
//                        if (жанр.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            текущийФильтр = текущийФильтр.Replace($"Genre = '{текущийФильтр.Split('\'')[1]}'", "").Trim();
//                        else
//                            текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Genre = '{жанр}'");
//                        ПрименитьФильтр();
//                        break;

//                    case "t":
//                        string текст = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : "";
//                        текущийФильтр = УбратьУсловие(текущийФильтр, "Title LIKE");
//                        if (!string.IsNullOrEmpty(текст))
//                            текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Title LIKE '%{текст.Replace("'", "''")}%'");
//                        ПрименитьФильтр();
//                        break;

//                    case "r":
//                        if (parts.Length != 3) { Console.WriteLine("Использование: r 4.5 9.0"); break; }
//                        double min = double.Parse(parts[1]);
//                        double max = double.Parse(parts[2]);
//                        текущийФильтр = УбратьУсловие(текущийФильтр, "Rating >=");
//                        текущийФильтр = ДобавитьУсловие(текущийФильтр, $"Rating >= {min} AND Rating <= {max}");
//                        ПрименитьФильтр();
//                        break;

//                    case "s":
//                        if (parts.Length != 2) { Console.WriteLine("s year | rating | box"); break; }
//                        switch (parts[1].ToLower())
//                        {
//                            case "year": текущаяСортировка = "Year DESC"; break;
//                            case "rating": текущаяСортировка = "Rating DESC"; break;
//                            case "box": текущаяСортировка = "BoxOffice DESC"; break;
//                            default: Console.WriteLine("Неизвестный тип сортировки"); break;
//                        }
//                        ПрименитьФильтр();
//                        break;

//                    case "reset":
//                        текущийФильтр = "";
//                        текущаяСортировка = "Year DESC";
//                        ПрименитьФильтр();
//                        Console.WriteLine("Все фильтры сброшены");
//                        break;

//                    case "show":
//                        ПоказатьРезультаты();
//                        break;

//                    case "count":
//                        ПоказатьКоличество();
//                        break;

//                    case "exit":
//                        Console.WriteLine($"Последний фильтр сохранён:\nФильтр: {dv.RowFilter}\nСортировка: {dv.Sort}");
//                        return;

//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка: " + ex.Message);
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("MovieID", typeof(int));
//        dt.Columns.Add("Title", typeof(string));
//        dt.Columns.Add("Director", typeof(string));
//        dt.Columns.Add("Year", typeof(int));
//        dt.Columns.Add("Genre", typeof(string));
//        dt.Columns.Add("Rating", typeof(double));
//        dt.Columns.Add("BoxOffice", typeof(decimal));

//        string[] жанры = { "Боевик", "Комедия", "Драма", "Фантастика", "Ужасы", "Триллер", "Мелодрама", "Документальный" };
//        string[] режиссеры = { "Нолан", "Спилберг", "Тарантино", "Кэмерон", "Скорсезе", "Финчер", "Вильнёв" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 300; i++)
//        {
//            string жанр = жанры[rnd.Next(жанры.Length)];
//            dt.Rows.Add(
//                i,
//                $"Фильм {i}: {жанр}",
//                режиссеры[rnd.Next(режиссеры.Length)],
//                1980 + rnd.Next(45),
//                жанр,
//                Math.Round(4.0 + rnd.NextDouble() * 6.0, 1),
//                rnd.Next(10, 2000) * 1000000M
//            );
//        }
//    }

//    static string ДобавитьУсловие(string текущий, string новое)
//    {
//        if (string.IsNullOrEmpty(текущий) || текущий.Trim() == "1=1")
//            return новое;
//        return $"{текущий} AND {новое}";
//    }

//    static string УбратьУсловие(string фильтр, string ключевое)
//    {
//        if (string.IsNullOrEmpty(фильтр)) return "";
//        var части = фильтр.Split(new[] { " AND " }, StringSplitOptions.None)
//            .Where(p => !p.Contains(ключевое)).ToArray();
//        return части.Length == 0 ? "" : string.Join(" AND ", части);
//    }

//    static void ПрименитьФильтр()
//    {
//        string filter = string.IsNullOrEmpty(текущийФильтр) ? "1=1" : текущийФильтр;
//        dv.RowFilter = filter;
//        dv.Sort = текущаяСортировка;
//        ПоказатьКоличество();
//    }

//    static void ПоказатьКоличество()
//    {
//        Console.WriteLine($"\nНайдено фильмов: {dv.Count} из {dt.Rows.Count}");
//        Console.WriteLine($"Текущий фильтр: {(dv.RowFilter == "1=1" ? "нет" : dv.RowFilter)}");
//        Console.WriteLine($"Сортировка: {dv.Sort}");
//    }

//    static void ПоказатьРезультаты()
//    {
//        Console.WriteLine("\n" + new string('=', 80));
//        Console.WriteLine($"{"ID",-4} {"Название",-35} {"Режиссёр",-15} {"Год",-5} {"Жанр",-12} {"Рейтинг",-7} {"Касса, млн$",-10}");
//        Console.WriteLine(new string('-', 80));

//        int показывать = Math.Min(20, dv.Count);
//        for (int i = 0; i < показывать; i++)
//        {
//            var row = dv[i].Row;
//            Console.WriteLine($"{row["MovieID"],-4} {row["Title"].ToString().Truncate(34),-35} " +
//                $"{row["Director"].ToString().Truncate(14),-15} {row["Year"],-5} " +
//                $"{row["Genre"].ToString().Truncate(11),-12} {row["Rating"],-7} {(decimal)row["BoxOffice"] / 1000000M,-10:F0}");
//        }

//        if (dv.Count > 20)
//            Console.WriteLine($"... и ещё {dv.Count - 20} фильмов (всего {dv.Count})");
//        Console.WriteLine(new string('=', 80));
//    }
//}

//static class Ext { public static string Truncate(this string s, int len) => s.Length > len ? s.Substring(0, len - 1) + "…" : s; }

//using System.Data;

//17
//using System;
//using System.Data;
//using System.Linq;
//using System.Collections.Generic;
//using System.Text;

//class Program
//{
//    static DataTable dt = new DataTable("Продажи по месяцам");
//    static DataView dvRegions, dvProducts, dvMonths;
//    static string currentFilter = "";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        СоздатьПредставления();

//        Console.WriteLine("=== Визуализация продаж ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter region <регион>  — фильтр по региону (или region all — снять)");
//        Console.WriteLine("  filter product <продукт> — фильтр по продукту (или product all — снять)");
//        Console.WriteLine("  show months             — график продаж по месяцам");
//        Console.WriteLine("  show regions            — диаграмма продаж по регионам");
//        Console.WriteLine("  show products           — диаграмма продаж по продуктам");
//        Console.WriteLine("  totals                  — итоговые значения");
//        Console.WriteLine("  reset                   — сброс фильтров");
//        Console.WriteLine("  exit                    — выход\n");

//        ОбновитьПредставления();
//        ПоказатьИтоговые();

//        while (true)
//        {
//            Console.Write("\n> ");
//            string input = Console.ReadLine().Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                if (cmd == "filter" && parts.Length >= 3)
//                {
//                    string type = parts[1].ToLower();
//                    string value = string.Join(" ", parts.Skip(2));
//                    if (type == "region")
//                    {
//                        currentFilter = УбратьУсловие(currentFilter, "Region =");
//                        if (!value.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            currentFilter = ДобавитьУсловие(currentFilter, $"Region = '{value.Replace("'", "''")}'");
//                    }
//                    else if (type == "product")
//                    {
//                        currentFilter = УбратьУсловие(currentFilter, "Product =");
//                        if (!value.Equals("all", StringComparison.OrdinalIgnoreCase))
//                            currentFilter = ДобавитьУсловие(currentFilter, $"Product = '{value.Replace("'", "''")}'");
//                    }
//                    ОбновитьПредставления();
//                    Console.WriteLine($"Фильтр применён: {(string.IsNullOrEmpty(currentFilter) ? "нет" : currentFilter)}");
//                }
//                else if (cmd == "show")
//                {
//                    if (parts.Length < 2) { Console.WriteLine("Укажите: months, regions или products"); continue; }
//                    string chartType = parts[1].ToLower();
//                    switch (chartType)
//                    {
//                        case "months": ПоказатьГрафикПоМесяцам(); break;
//                        case "regions": ПоказатьДиаграммуПоРегионам(); break;
//                        case "products": ПоказатьДиаграммуПоПродуктам(); break;
//                        default: Console.WriteLine("Неизвестный тип: months, regions или products"); break;
//                    }
//                }
//                else if (cmd == "totals")
//                {
//                    ПоказатьИтоговые();
//                }
//                else if (cmd == "reset")
//                {
//                    currentFilter = "";
//                    ОбновитьПредставления();
//                    Console.WriteLine("Фильтры сброшены");
//                }
//                else if (cmd == "exit")
//                {
//                    Console.WriteLine($"Последний фильтр: {currentFilter}");
//                    return;
//                }
//                else
//                {
//                    Console.WriteLine("Неизвестная команда");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка: " + ex.Message);
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("SalesID", typeof(int));
//        dt.Columns.Add("Month", typeof(string));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Amount", typeof(decimal));
//        dt.Columns.Add("Region", typeof(string));

//        string[] месяцы = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
//        string[] продукты = { "Товар A", "Товар B", "Товар C", "Товар D" };
//        string[] регионы = { "Север", "Юг", "Восток", "Запад" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 120; i++) // 10 записей на месяц
//        {
//            string месяц = месяцы[(i - 1) % 12];
//            dt.Rows.Add(i, месяц, продукты[rnd.Next(продукты.Length)], (decimal)rnd.Next(100, 5000), регионы[rnd.Next(регионы.Length)]);
//        }
//    }

//    static void СоздатьПредставления()
//    {
//        dvRegions = new DataView(dt) { Sort = "Region" };
//        dvProducts = new DataView(dt) { Sort = "Product" };
//        dvMonths = new DataView(dt) { Sort = "Month" };
//    }

//    static void ОбновитьПредставления()
//    {
//        string filter = string.IsNullOrEmpty(currentFilter) ? null : currentFilter;
//        dvRegions.RowFilter = filter;
//        dvProducts.RowFilter = filter;
//        dvMonths.RowFilter = filter;
//    }

//    static string ДобавитьУсловие(string текущий, string новое)
//    {
//        return string.IsNullOrEmpty(текущий) ? новое : $"{текущий} AND {новое}";
//    }

//    static string УбратьУсловие(string фильтр, string ключевое)
//    {
//        if (string.IsNullOrEmpty(фильтр)) return "";
//        var части = фильтр.Split(new[] { " AND " }, StringSplitOptions.None)
//            .Where(p => !p.StartsWith(ключевое)).ToArray();
//        return string.Join(" AND ", части);
//    }

//    static void ПоказатьИтоговые()
//    {
//        decimal total = dvMonths.Table.Compute("SUM(Amount)", dvMonths.RowFilter) is DBNull ? 0 : (decimal)dvMonths.Table.Compute("SUM(Amount)", dvMonths.RowFilter);
//        Console.WriteLine($"\nИтоговые значения:");
//        Console.WriteLine($"Общие продажи: {total:C}");
//        Console.WriteLine($"По регионам: {dvRegions.Count} записей");
//        Console.WriteLine($"По продуктам: {dvProducts.Count} записей");
//        Console.WriteLine($"По месяцам: {dvMonths.Count} записей");
//    }

//    static void ПоказатьГрафикПоМесяцам()
//    {
//        var data = dvMonths.Table.AsEnumerable()
//            .Where(r => dvMonths.RowFilter == null || dvMonths.Table.Select(dvMonths.RowFilter).Contains(r))
//            .GroupBy(r => r["Month"])
//            .Select(g => new { Month = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) })
//            .OrderBy(x => Array.IndexOf(new[] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" }, x.Month));

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nГрафик продаж по месяцам (линейный, ASCII):");
//        Console.WriteLine("Месяц     | Сумма          | График (масштаб: * = 100 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 100);
//            Console.WriteLine($"{item.Month,-10}| {item.Sum,-14:C} | {new string('*', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: * представляет 100 руб. Ось X: месяцы, Ось Y: сумма продаж.");
//    }

//    static void ПоказатьДиаграммуПоРегионам()
//    {
//        var data = dvRegions.ToTable(true, "Region", "Amount").AsEnumerable()
//            .GroupBy(r => r["Region"])
//            .Select(g => new { Region = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) });

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nДиаграмма продаж по регионам (столбчатая, ASCII):");
//        Console.WriteLine("Регион    | Сумма          | Диаграмма (масштаб: # = 500 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 500);
//            Console.WriteLine($"{item.Region,-10}| {item.Sum,-14:C} | {new string('#', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: # представляет 500 руб. Ось X: регионы, Ось Y: сумма продаж.");
//    }

//    static void ПоказатьДиаграммуПоПродуктам()
//    {
//        var data = dvProducts.ToTable(true, "Product", "Amount").AsEnumerable()
//            .GroupBy(r => r["Product"])
//            .Select(g => new { Product = g.Key, Sum = g.Sum(r => (decimal)r["Amount"]) });

//        decimal max = data.Max(x => x.Sum);
//        Console.WriteLine("\nДиаграмма продаж по продуктам (столбчатая, ASCII):");
//        Console.WriteLine("Продукт   | Сумма          | Диаграмма (масштаб: @ = 1000 руб)");
//        Console.WriteLine(new string('-', 50));

//        foreach (var item in data)
//        {
//            int bars = (int)(item.Sum / 1000);
//            Console.WriteLine($"{item.Product,-10}| {item.Sum,-14:C} | {new string('@', bars)}");
//        }

//        Console.WriteLine("\nЛегенда: @ представляет 1000 руб. Ось X: продукты, Ось Y: сумма продаж.");
//    }
//}

//18
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Цены на акции");
//        dt.Columns.Add("StockID", typeof(int));
//        dt.Columns.Add("CompanyName", typeof(string));
//        dt.Columns.Add("Price", typeof(double));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Volume", typeof(int));

//        List<string> companies = new List<string>();
//        for (int i = 1; i <= 20; i++) companies.Add($"Компания {i}");
//        DateTime startDate = new DateTime(2020, 1, 1);
//        Random rnd = new Random(42);

//        for (int i = 1; i <= 1500; i++)
//        {
//            dt.Rows.Add(i, companies[rnd.Next(companies.Count)], Math.Round(50 + rnd.NextDouble() * 450, 2), startDate.AddDays(rnd.Next(1000)), rnd.Next(1000, 100000));
//        }

//        DataView dvPrice = new DataView(dt) { Sort = "Price ASC" };
//        DataView dvDate = new DataView(dt) { Sort = "CompanyName ASC, Date ASC" };

//        double testPrice = 300.0;
//        double rangeLow = 200.0;
//        double rangeHigh = 300.0;

//        Console.WriteLine("Пример поиска ближайшей цены ниже {0}:", testPrice);
//        var below = GetNearestBelow(dvPrice, testPrice);
//        if (below != null) ВывестиСтроку(below);
//        else Console.WriteLine("Нет цены ниже.");

//        Console.WriteLine("\nПример поиска ближайшей цены выше {0}:", testPrice);
//        var above = GetNearestAbove(dvPrice, testPrice);
//        if (above != null) ВывестиСтроку(above);
//        else Console.WriteLine("Нет цены выше.");

//        Console.WriteLine("\nКомпании в диапазоне цен {0} - {1}:", rangeLow, rangeHigh);
//        var range = GetRange(dvPrice, rangeLow, rangeHigh);
//        foreach (var row in range) ВывестиСтроку(row);

//        Console.WriteLine("\nАкция с минимальной ценой в диапазоне:");
//        var minRange = GetMinInRange(dvPrice, rangeLow, rangeHigh);
//        if (minRange != null) ВывестиСтроку(minRange);
//        else Console.WriteLine("Диапазон пуст.");

//        Console.WriteLine("\nАкция с максимальной ценой в диапазоне:");
//        var maxRange = GetMaxInRange(dvPrice, rangeLow, rangeHigh);
//        if (maxRange != null) ВывестиСтроку(maxRange);
//        else Console.WriteLine("Диапазон пуст.");

//        Console.WriteLine("\nСкачки цены > 10%:");
//        var jumps = FindPriceJumps(dvDate);
//        foreach (var jump in jumps)
//        {
//            Console.WriteLine($"Компания: {jump.Company}, Дата: {jump.Date:yyyy-MM-dd}, Предыдущая: {jump.PrevPrice:F2}, Текущая: {jump.CurrPrice:F2}, Изменение: {jump.Change:F2}%");
//        }

//        Граничные случаи
//        Console.WriteLine("\nГраничные случаи:");
//        double lowEdge = (double)dvPrice[0]["Price"] - 1;
//        Console.WriteLine("Ближайшая ниже {0}: Нет", lowEdge);
//        var aboveEdge = GetNearestAbove(dvPrice, lowEdge);
//        if (aboveEdge != null) ВывестиСтроку(aboveEdge);

//        double highEdge = (double)dvPrice[dvPrice.Count - 1]["Price"] + 1;
//        var belowEdge = GetNearestBelow(dvPrice, highEdge);
//        if (belowEdge != null) ВывестиСтроку(belowEdge);
//        Console.WriteLine("Ближайшая выше {0}: Нет", highEdge);

//        Производительность
//       Stopwatch sw = new Stopwatch();
//        sw.Start();
//        for (int i = 0; i < 1000; i++)
//        {
//            GetNearestBelow(dvPrice, testPrice);
//        }
//        sw.Stop();
//        Console.WriteLine("\nПроизводительность бинарного поиска (1000 раз): {0} мс", sw.ElapsedMilliseconds);

//        sw.Reset();
//        sw.Start();
//        for (int i = 0; i < 1000; i++)
//        {
//            LinearNearestBelow(dt, testPrice);
//        }
//        sw.Stop();
//        Console.WriteLine("Производительность линейного поиска (1000 раз): {0} мс", sw.ElapsedMilliseconds);
//    }

//    static int LowerBound(DataView dv, double val)
//    {
//        int low = 0;
//        int high = dv.Count;
//        while (low < high)
//        {
//            int mid = (low + high) / 2;
//            double p = (double)dv[mid]["Price"];
//            if (p < val) low = mid + 1;
//            else high = mid;
//        }
//        return low;
//    }

//    static int UpperBound(DataView dv, double val)
//    {
//        int low = 0;
//        int high = dv.Count;
//        while (low < high)
//        {
//            int mid = (low + high) / 2;
//            double p = (double)dv[mid]["Price"];
//            if (p <= val) low = mid + 1;
//            else high = mid;
//        }
//        return low;
//    }

//    static DataRowView GetNearestBelow(DataView dv, double val)
//    {
//        int idx = LowerBound(dv, val);
//        if (idx > 0) return dv[idx - 1];
//        return null;
//    }

//    static DataRowView GetNearestAbove(DataView dv, double val)
//    {
//        int idx = UpperBound(dv, val);
//        if (idx < dv.Count) return dv[idx];
//        return null;
//    }

//    static List<DataRowView> GetRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        List<DataRowView> result = new List<DataRowView>();
//        for (int i = start; i < end; i++) result.Add(dv[i]);
//        return result;
//    }

//    static DataRowView GetMinInRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        if (start >= end) return null;
//        return dv[start];
//    }

//    static DataRowView GetMaxInRange(DataView dv, double lowVal, double highVal)
//    {
//        int start = LowerBound(dv, lowVal);
//        int end = UpperBound(dv, highVal);
//        if (start >= end) return null;
//        return dv[end - 1];
//    }

//    static List<PriceJump> FindPriceJumps(DataView dv)
//    {
//        List<PriceJump> jumps = new List<PriceJump>();
//        if (dv.Count < 2) return jumps;

//        string prevCompany = (string)dv[0]["CompanyName"];
//        double prevPrice = (double)dv[0]["Price"];
//        DateTime prevDate = (DateTime)dv[0]["Date"];

//        for (int i = 1; i < dv.Count; i++)
//        {
//            string company = (string)dv[i]["CompanyName"];
//            if (company != prevCompany)
//            {
//                prevCompany = company;
//                prevPrice = (double)dv[i]["Price"];
//                prevDate = (DateTime)dv[i]["Date"];
//                continue;
//            }

//            double currPrice = (double)dv[i]["Price"];
//            DateTime currDate = (DateTime)dv[i]["Date"];
//            double change = (currPrice - prevPrice) / prevPrice;
//            if (Math.Abs(change) > 0.1)
//            {
//                jumps.Add(new PriceJump { Company = company, Date = currDate, PrevPrice = prevPrice, CurrPrice = currPrice, Change = change * 100 });
//            }
//            prevPrice = currPrice;
//            prevDate = currDate;
//        }
//        return jumps;
//    }

//    static DataRow LinearNearestBelow(DataTable dt, double val)
//    {
//        DataRow below = null;
//        foreach (DataRow row in dt.Rows)
//        {
//            double p = (double)row["Price"];
//            if (p < val)
//            {
//                if (below == null || (double)below["Price"] < p)
//                    below = row;
//            }
//        }
//        return below;
//    }

//    static void ВывестиСтроку(DataRowView drv)
//    {
//        if (drv == null) return;
//        Console.WriteLine($"ID: {drv["StockID"]}, Компания: {drv["CompanyName"]}, Цена: {drv["Price"]:F2}, Дата: {((DateTime)drv["Date"]):yyyy-MM-dd}, Объём: {drv["Volume"]}");
//    }
//}

//class PriceJump
//{
//    public string Company { get; set; }
//    public DateTime Date { get; set; }
//    public double PrevPrice { get; set; }
//    public double CurrPrice { get; set; }
//    public double Change { get; set; }
//}

//19
//using System;
//using System.Data;
//using System.Text.RegularExpressions;
//using System.Collections.Generic;

//class Program
//{
//    static DateTime currentDate = new DateTime(2025, 12, 4);
//    static Dictionary<string, int> errorStats = new Dictionary<string, int>
//    {
//        { "Name", 0 }, { "Email", 0 }, { "Phone", 0 }, { "Address", 0 }, { "BirthDate", 0 }
//    };
//    static List<string> editLogs = new List<string>();

//    static void Main()
//    {
//        DataTable dt = new DataTable("Контакты");
//        dt.Columns.Add("ContactID", typeof(int));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("Email", typeof(string));
//        dt.Columns.Add("Phone", typeof(string));
//        dt.Columns.Add("Address", typeof(string));
//        dt.Columns.Add("BirthDate", typeof(DateTime));

//        Random rnd = new Random();
//        for (int i = 1; i <= 100; i++)
//        {
//            string name = $"Имя {i}";
//            string email = $"email{i}@example.com";
//            string phone = $"({rnd.Next(100, 1000):D3}){rnd.Next(0, 1000):D3}-{rnd.Next(0, 10000):D4}";
//            string address = $"Адрес {i}";
//            DateTime birth = currentDate.AddYears(-rnd.Next(18, 60)).AddDays(rnd.Next(-365, 365));
//            dt.Rows.Add(i, name, email, phone, address, birth);
//        }

//        dt.ColumnChanging += ВалидацияПриИзменении;

//        DataView dv = new DataView(dt);

//        Console.WriteLine("Демонстрация редактирования с валидацией (через DataView):");

//        ПопыткаРедактирования(dv, 1, "Name", "Новое имя");
//        ПопыткаРедактирования(dv, 1, "Name", "");
//        ПопыткаРедактирования(dv, 2, "Email", "valid@email.com");
//        ПопыткаРедактирования(dv, 2, "Email", "invalid-email");
//        ПопыткаРедактирования(dv, 3, "Phone", "1234567890");
//        ПопыткаРедактирования(dv, 3, "Phone", "invalid");
//        ПопыткаРедактирования(dv, 4, "Address", "Новый адрес");
//        ПопыткаРедактирования(dv, 4, "Address", "");
//        ПопыткаРедактирования(dv, 5, "BirthDate", currentDate.AddYears(-30));
//        ПопыткаРедактирования(dv, 5, "BirthDate", currentDate.AddDays(1));

//        Console.WriteLine("\nЖурнал редактирований:");
//        foreach (string log in editLogs)
//        {
//            Console.WriteLine(log);
//        }

//        Console.WriteLine("\nСтатистика ошибок валидации:");
//        foreach (var kvp in errorStats)
//        {
//            Console.WriteLine($"{kvp.Key}: {kvp.Value} ошибок");
//        }

//        Console.WriteLine("\nПроверка, что исходная таблица не содержит ошибок (пример первых 5 записей):");
//        for (int i = 0; i < 5; i++)
//        {
//            DataRow row = dt.Rows[i];
//            Console.WriteLine($"ID: {row["ContactID"]}, Имя: {row["Name"]}, Email: {row["Email"]}, Телефон: {row["Phone"]}, Адрес: {row["Address"]}, Дата рождения: {((DateTime)row["BirthDate"]):yyyy-MM-dd}");
//        }
//    }

//    static void ВалидацияПриИзменении(object sender, DataColumnChangeEventArgs e)
//    {
//        string column = e.Column.ColumnName;
//        object value = e.ProposedValue;
//        string error = null;

//        switch (column)
//        {
//            case "Name":
//                if (string.IsNullOrEmpty(value as string))
//                    error = "Имя не может быть пустым";
//                break;
//            case "Email":
//                string email = value as string;
//                if (string.IsNullOrEmpty(email) || !email.Contains("@"))
//                    error = "Email должен содержать '@'";
//                break;
//            case "Phone":
//                string phone = value as string;
//                if (!Regex.IsMatch(phone, @"^\(\d{3}\)\d{3}-\d{4}$"))
//                    error = "Телефон должен быть в формате (XXX)XXX-XXXX";
//                break;
//            case "Address":
//                if (string.IsNullOrEmpty(value as string))
//                    error = "Адрес не может быть пустым";
//                break;
//            case "BirthDate":
//                DateTime birth = (DateTime)value;
//                if (birth > currentDate)
//                    error = "Дата рождения не может быть в будущем";
//                break;
//        }

//        if (error != null)
//        {
//            errorStats[column]++;
//            throw new ArgumentException(error);
//        }
//    }

//    static void ПопыткаРедактирования(DataView dv, int contactId, string column, object newValue)
//    {
//        DataRowView rowView = null;
//        foreach (DataRowView drv in dv)
//        {
//            if ((int)drv["ContactID"] == contactId)
//            {
//                rowView = drv;
//                break;
//            }
//        }

//        if (rowView == null) return;

//        string log = $"Попытка редактирования ID {contactId}, столбец {column} на '{newValue}'";
//        editLogs.Add(log);

//        try
//        {
//            if (column == "Phone" && newValue is string phoneInput)
//            {
//                string digits = new string(phoneInput.Where(char.IsDigit).ToArray());
//                if (digits.Length == 10)
//                {
//                    newValue = $"({digits.Substring(0, 3)}){digits.Substring(3, 3)}-{digits.Substring(6)}";
//                    editLogs.Add("Автоформатирование телефона применено");
//                }
//            }
//            else if (column == "BirthDate" && newValue is string dateStr)
//            {
//                newValue = DateTime.Parse(dateStr);
//            }

//            rowView[column] = newValue;
//            editLogs.Add("Успех");
//        }
//        catch (ArgumentException ex)
//        {
//            Console.WriteLine($"Ошибка валидации: {ex.Message}");
//            editLogs.Add($"Неудача: {ex.Message}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Общая ошибка: {ex.Message}");
//            editLogs.Add($"Неудача: {ex.Message}");
//        }
//    }
//}

//20
//using System;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using System.Xml;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Отчёт о продажах");
//        dt.Columns.Add("ReportID", typeof(int));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Quantity", typeof(int));
//        dt.Columns.Add("Price", typeof(double));
//        dt.Columns.Add("Salesperson", typeof(string));
//        dt.Columns.Add("Region", typeof(string));

//        string[] products = { "Товар A", "Товар B", "Товар C", "Товар D", "Товар E" };
//        string[] salespersons = { "Продавец 1", "Продавец 2", "Продавец 3" };
//        string[] regions = { "Север", "Юг", "Восток", "Запад" };
//        Random rnd = new Random();
//        DateTime startDate = new DateTime(2023, 1, 1);

//        for (int i = 1; i <= 500; i++)
//        {
//            dt.Rows.Add(i, startDate.AddDays(rnd.Next(365)), products[rnd.Next(products.Length)], rnd.Next(1, 100), Math.Round(rnd.NextDouble() * 1000, 2), salespersons[rnd.Next(salespersons.Length)], regions[rnd.Next(regions.Length)]);
//        }

//        DataView dv = new DataView(dt);
//        dv.RowFilter = "Quantity > 10";
//        dv.Sort = "Date DESC";

//        string exportInfo = $"Дата экспорта: {DateTime.Now:yyyy-MM-dd HH:mm:ss}, Количество строк: {dv.Count}, Фильтр: {dv.RowFilter ?? "нет"}, Сортировка: {dv.Sort ?? "нет"}";

//        Экспорт
//        ExportToCsv(dv, "export.csv", exportInfo);
//        ExportToXml(dv, "export.xml", exportInfo);
//        ExportToJson(dv, "export.json", exportInfo);
//        ExportToHtml(dv, "export.html", exportInfo);

//        Варианты для CSV(например, с точкой с запятой)
//        ExportToCsv(dv, "export_semicolon.csv", exportInfo, ';');

//        Импорт и проверка
//       DataTable importedCsv = ImportFromCsv("export.csv");
//        Console.WriteLine($"Импорт из CSV: {importedCsv.Rows.Count} строк");

//        DataTable importedXml = ImportFromXml("export.xml");
//        Console.WriteLine($"Импорт из XML: {importedXml.Rows.Count} строк");

//        DataTable importedJson = ImportFromJson("export.json");
//        Console.WriteLine($"Импорт из JSON: {importedJson.Rows.Count} строк");

//        DataTable importedHtml = ImportFromHtml("export.html");
//        Console.WriteLine($"Импорт из HTML: {importedHtml.Rows.Count} строк");

//        Для Excel не реализовано, так как требует внешней библиотеки(EPPlus), но можно использовать CSV для открытия в Excel
//        Console.WriteLine("Экспорт в Excel не реализован без EPPlus, используйте CSV.");
//    }

//    static void ExportToCsv(DataView dv, string filePath, string exportInfo, char separator = ',')
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine(exportInfo.Replace(separator, ' ')); // Заголовок

//        Заголовки столбцов
//        foreach (DataColumn col in dv.Table.Columns)
//        {
//            sb.Append(EscapeCsv(col.ColumnName, separator) + separator);
//        }
//        sb.Length--; // Удалить последний separator
//        sb.AppendLine();

//        Данные
//        foreach (DataRowView drv in dv)
//        {
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                string val = drv[col].ToString();
//                if (col.DataType == typeof(DateTime))
//                    val = ((DateTime)drv[col]).ToString("yyyy-MM-dd");
//                sb.Append(EscapeCsv(val, separator) + separator);
//            }
//            sb.Length--;
//            sb.AppendLine();
//        }

//        File.WriteAllText(filePath, sb.ToString());
//        Console.WriteLine($"Экспортировано в CSV: {filePath}");
//    }

//    static string EscapeCsv(string value, char separator)
//    {
//        if (value.Contains(separator) || value.Contains('"') || value.Contains('\n'))
//        {
//            return '"' + value.Replace("\"", "\"\"") + '"';
//        }
//        return value;
//    }

//    static void ExportToXml(DataView dv, string filePath, string exportInfo)
//    {
//        DataTable exportDt = dv.ToTable();
//        using (XmlWriter writer = XmlWriter.Create(filePath))
//        {
//            writer.WriteStartDocument();
//            writer.WriteStartElement("Export");
//            writer.WriteElementString("Info", exportInfo);
//            writer.WriteStartElement("Data");
//            exportDt.WriteXml(writer, XmlWriteMode.WriteSchema);
//            writer.WriteEndElement();
//            writer.WriteEndElement();
//            writer.WriteEndDocument();
//        }
//        Console.WriteLine($"Экспортировано в XML: {filePath}");
//    }

//    static void ExportToJson(DataView dv, string filePath, string exportInfo)
//    {
//        var data = new List<Dictionary<string, object>>();
//        foreach (DataRowView drv in dv)
//        {
//            var dict = new Dictionary<string, object>();
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                dict[col.ColumnName] = drv[col];
//            }
//            data.Add(dict);
//        }

//        var jsonObj = new { Info = exportInfo, Data = data };
//        string json = JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
//        File.WriteAllText(filePath, json);
//        Console.WriteLine($"Экспортировано в JSON: {filePath}");
//    }

//    static void ExportToHtml(DataView dv, string filePath, string exportInfo)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine("<html><body>");
//        sb.AppendLine($"<p>{exportInfo.Replace("<", "&lt;").Replace(">", "&gt;")}</p>");
//        sb.AppendLine("<table border='1'>");

//        Заголовки
//        sb.Append("<tr>");
//        foreach (DataColumn col in dv.Table.Columns)
//        {
//            sb.Append($"<th>{col.ColumnName}</th>");
//        }
//        sb.AppendLine("</tr>");

//        Данные
//        foreach (DataRowView drv in dv)
//        {
//            sb.Append("<tr>");
//            foreach (DataColumn col in dv.Table.Columns)
//            {
//                string val = drv[col].ToString();
//                if (col.DataType == typeof(DateTime))
//                    val = ((DateTime)drv[col]).ToString("yyyy-MM-dd");
//                sb.Append($"<td>{val.Replace("<", "&lt;").Replace(">", "&gt;")}</td>");
//            }
//            sb.AppendLine("</tr>");
//        }

//        sb.AppendLine("</table></body></html>");
//        File.WriteAllText(filePath, sb.ToString());
//        Console.WriteLine($"Экспортировано в HTML: {filePath}");
//    }

//    static DataTable ImportFromCsv(string filePath, char separator = ',')
//    {
//        DataTable dt = new DataTable();
//        string[] lines = File.ReadAllLines(filePath);
//        if (lines.Length < 2) return dt;

//        Пропустить заголовок экспорта(первая строка)
//        string[] headers = ParseCsvLine(lines[1], separator);
//        foreach (string header in headers)
//        {
//            dt.Columns.Add(header);
//        }

//        for (int i = 2; i < lines.Length; i++)
//        {
//            string[] values = ParseCsvLine(lines[i], separator);
//            if (values.Length == headers.Length)
//            {
//                dt.Rows.Add(values);
//            }
//        }
//        return dt;
//    }

//    static string[] ParseCsvLine(string line, char separator)
//    {
//        List<string> values = new List<string>();
//        StringBuilder sb = new StringBuilder();
//        bool inQuotes = false;
//        foreach (char c in line)
//        {
//            if (c == '"' && !inQuotes)
//            {
//                inQuotes = true;
//            }
//            else if (c == '"' && inQuotes)
//            {
//                if (sb.Length > 0 && sb[sb.Length - 1] == '"')
//                {
//                    sb.Length--; // Удалить предыдущий ", если удвоенный
//                    values.Add(sb.ToString());
//                    sb.Clear();
//                    inQuotes = false;
//                }
//            }
//            else if (c == separator && !inQuotes)
//            {
//                values.Add(sb.ToString());
//                sb.Clear();
//            }
//            else
//            {
//                sb.Append(c);
//            }
//        }
//        if (sb.Length > 0) values.Add(sb.ToString());
//        return values.ToArray();
//    }

//    static DataTable ImportFromXml(string filePath)
//    {
//        DataSet ds = new DataSet();
//        ds.ReadXml(filePath);
//        return ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
//    }

//    static DataTable ImportFromJson(string filePath)
//    {
//        string json = File.ReadAllText(filePath);
//        var jsonObj = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
//        if (jsonObj.ContainsKey("Data") && jsonObj["Data"] is JsonElement dataElem && dataElem.ValueKind == JsonValueKind.Array)
//        {
//            DataTable dt = new DataTable();
//            bool first = true;
//            foreach (JsonElement rowElem in dataElem.EnumerateArray())
//            {
//                if (first)
//                {
//                    foreach (JsonProperty prop in rowElem.EnumerateObject())
//                    {
//                        dt.Columns.Add(prop.Name);
//                    }
//                    first = false;
//                }
//                var row = dt.NewRow();
//                foreach (JsonProperty prop in rowElem.EnumerateObject())
//                {
//                    row[prop.Name] = prop.Value.ToString(); // Простая конверсия, для типов нужно парсить
//                }
//                dt.Rows.Add(row);
//            }
//            return dt;
//        }
//        return new DataTable();
//    }

//    static DataTable ImportFromHtml(string filePath)
//    {
//        string html = File.ReadAllText(filePath);
//        DataTable dt = new DataTable();

//        Простой парсер для<table>
//       Match tableMatch = Regex.Match(html, @"<table[^>]*>(.*?)</table>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        if (!tableMatch.Success) return dt;

//        string tableContent = tableMatch.Groups[1].Value;

//        Заголовки<th>
//       MatchCollection thMatches = Regex.Matches(tableContent, @"<th[^>]*>(.*?)</th>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        foreach (Match th in thMatches)
//        {
//            dt.Columns.Add(th.Groups[1].Value.Trim());
//        }

//        Строки<tr>
//       MatchCollection trMatches = Regex.Matches(tableContent, @"<tr[^>]*>(.*?)</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//        foreach (Match tr in trMatches)
//        {
//            string rowContent = tr.Groups[1].Value;
//            MatchCollection tdMatches = Regex.Matches(rowContent, @"<td[^>]*>(.*?)</td>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
//            if (tdMatches.Count == dt.Columns.Count)
//            {
//                object[] values = new object[dt.Columns.Count];
//                for (int i = 0; i < tdMatches.Count; i++)
//                {
//                    values[i] = tdMatches[i].Groups[1].Value.Trim();
//                }
//                dt.Rows.Add(values);
//            }
//        }

//        return dt;
//    }
//}

//21
//using System;
//using System.Data;
//using System.Collections.Generic;

//class Program
//{
//    static DataTable categories = new DataTable("Категории");
//    static DataTable products = new DataTable("Товары");
//    static Dictionary<int, bool> expandedStates = new Dictionary<int, bool>();

//    static void Main()
//    {
//        SetupTables();
//        FillData();

//        Console.WriteLine("=== Иерархическая структура категорий и товаров (консольное приложение) ===\n");
//        Console.WriteLine("Команды:");
//        Console.WriteLine("  show                 — показать иерархию");
//        Console.WriteLine("  expand <CategoryID>  — развернуть категорию");
//        Console.WriteLine("  collapse <CategoryID>— свернуть категорию");
//        Console.WriteLine("  search <ProductName> — поиск товара (частичное совпадение)");
//        Console.WriteLine("  exit                 — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "show":
//                        ShowHierarchy();
//                        break;
//                    case "expand" when parts.Length == 2:
//                        int expId = int.Parse(parts[1]);
//                        Expand(expId);
//                        Console.WriteLine($"Категория {expId} развернута.");
//                        break;
//                    case "collapse" when parts.Length == 2:
//                        int colId = int.Parse(parts[1]);
//                        Collapse(colId);
//                        Console.WriteLine($"Категория {colId} свернута.");
//                        break;
//                    case "search" when parts.Length > 1:
//                        string searchName = string.Join(" ", parts.AsSpan(1));
//                        SearchProduct(searchName);
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда или неверные аргументы.");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void SetupTables()
//    {
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.Columns.Add("ParentCategoryID", typeof(int));
//        categories.Columns.Add("Description", typeof(string));
//        categories.PrimaryKey = new[] { categories.Columns["CategoryID"] };

//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("ProductName", typeof(string));
//        products.Columns.Add("CategoryID", typeof(int));
//        products.Columns.Add("Price", typeof(double));
//        products.PrimaryKey = new[] { products.Columns["ProductID"] };
//    }

//    static void FillData()
//    {
//        Корневые категории
//        categories.Rows.Add(1, "Электроника", DBNull.Value, "Электронные устройства");
//        categories.Rows.Add(6, "Одежда", DBNull.Value, "Одежда и аксессуары");

//        Подкатегории Электроника
//        categories.Rows.Add(2, "Смартфоны", 1, "Мобильные телефоны");
//        categories.Rows.Add(3, "Ноутбуки", 1, "Портативные компьютеры");
//        categories.Rows.Add(4, "Аксессуары", 1, "Дополнительные гаджеты");
//        categories.Rows.Add(5, "Чехлы", 4, "Защитные чехлы");

//        Подкатегории Одежда
//        categories.Rows.Add(7, "Мужская", 6, "Мужская одежда");
//        categories.Rows.Add(8, "Женская", 6, "Женская одежда");
//        categories.Rows.Add(9, "Обувь", 6, "Обувь для всех");
//        categories.Rows.Add(10, "Кроссовки", 9, "Спортивная обувь");

//        Random rnd = new Random();
//        for (int i = 1; i <= 100; i++)
//        {
//            int catId = rnd.Next(1, 11);
//            products.Rows.Add(i, $"Товар {i} ({categories.Rows.Find(catId)["CategoryName"]})", catId, Math.Round(rnd.NextDouble() * 900 + 100, 2));
//            expandedStates[i] = false; // Для категорий, но инициализируем для всех ID категорий
//        }

//        Инициализация состояний для категорий
//        foreach (DataRow cat in categories.Rows)
//        {
//            expandedStates[(int)cat["CategoryID"]] = false;
//        }
//    }

//    static void ShowHierarchy()
//    {
//        Console.WriteLine("\nТекущая иерархия:");
//        DataView topViews = new DataView(categories) { RowFilter = "ParentCategoryID IS NULL", Sort = "CategoryName" };
//        foreach (DataRowView top in topViews)
//        {
//            PrintCategory(top.Row, 0);
//        }
//    }

//    static void PrintCategory(DataRow catRow, int level)
//    {
//        int catId = (int)catRow["CategoryID"];
//        bool isExpanded = expandedStates.GetValueOrDefault(catId, false);
//        string indent = new string(' ', level * 2);
//        string expander = isExpanded ? "-" : "+";
//        Console.WriteLine($"{indent}{expander} {catRow["CategoryName"]} (ID: {catId}, Описание: {catRow["Description"]})");

//        if (!isExpanded) return;

//        Подкатегории
//       DataView subCatView = new DataView(categories) { RowFilter = $"ParentCategoryID = {catId}", Sort = "CategoryName" };
//        foreach (DataRowView sub in subCatView)
//        {
//            PrintCategory(sub.Row, level + 1);
//        }

//        Товары в этой категории
//        DataView prodView = new DataView(products) { RowFilter = $"CategoryID = {catId}", Sort = "ProductName" };
//        foreach (DataRowView prod in prodView)
//        {
//            Console.WriteLine($"{indent}  * {prod["ProductName"]} (ID: {prod["ProductID"]}, Цена: {prod["Price"]:F2})");
//        }
//    }

//    static void Expand(int catId)
//    {
//        if (categories.Rows.Find(catId) == null)
//        {
//            throw new Exception("Категория не найдена");
//        }
//        expandedStates[catId] = true;
//    }

//    static void Collapse(int catId)
//    {
//        if (categories.Rows.Find(catId) == null)
//        {
//            throw new Exception("Категория не найдена");
//        }
//        expandedStates[catId] = false;
//    }

//    static void SearchProduct(string name)
//    {
//        DataView searchView = new DataView(products) { RowFilter = $"ProductName LIKE '%{name.Replace("'", "''")}%'", Sort = "ProductName" };
//        if (searchView.Count == 0)
//        {
//            Console.WriteLine("\nТовары не найдены.");
//            return;
//        }

//        Console.WriteLine("\nНайденные товары:");
//        foreach (DataRowView prod in searchView)
//        {
//            int catId = (int)prod["CategoryID"];
//            string path = GetParentPath(catId);
//            Console.WriteLine($"- {prod["ProductName"]} (ID: {prod["ProductID"]}, Цена: {prod["Price"]:F2}, Путь: {path})");
//        }
//    }

//    static string GetParentPath(int catId)
//    {
//        List<string> pathList = new List<string>();
//        DataRow currentCat = categories.Rows.Find(catId);
//        while (currentCat != null)
//        {
//            pathList.Add((string)currentCat["CategoryName"]);
//            object parentId = currentCat["ParentCategoryID"];
//            currentCat = parentId is DBNull ? null : categories.Rows.Find((int)parentId);
//        }
//        pathList.Reverse();
//        return string.Join(" > ", pathList);
//    }

//    Получение всех товаров в категории и подкатегориях(для примера, но не используется напрямую)
//    static DataView GetAllProductsInSubtree(int catId)
//    {
//        List<int> allCatIds = GetAllSubCatIds(catId);
//        string idList = string.Join(",", allCatIds);
//        DataView allProds = new DataView(products) { RowFilter = $"CategoryID IN ({idList})" };
//        return allProds;
//    }

//    static List<int> GetAllSubCatIds(int catId)
//    {
//        List<int> ids = new List<int> { catId };
//        DataView subs = new DataView(categories) { RowFilter = $"ParentCategoryID = {catId}" };
//        foreach (DataRowView sub in subs)
//        {
//            ids.AddRange(GetAllSubCatIds((int)sub["CategoryID"]));
//        }
//        return ids;
//    }

//    Получение родительской категории
//    static DataRow GetParentCategory(int catId)
//    {
//        DataRow cat = categories.Rows.Find(catId);
//        if (cat == null || cat["ParentCategoryID"] is DBNull) return null;
//        return categories.Rows.Find((int)cat["ParentCategoryID"]);
//    }

//    Получение подкатегорий
//    static DataView GetSubCategories(int catId)
//    {
//        DataView subs = new DataView(categories);
//        subs.RowFilter = $"ParentCategoryID = {catId}";
//        subs.Sort = "CategoryName";
//        return subs;
//    }
//}

//using System.Data;
//using System.Diagnostics;

//22
//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Collections.Generic;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Документы");
//    static DataView dv;
//    static List<string> searchLogs = new List<string>();
//    static string currentFilter = "";

//    static void Main()
//    {
//        СоздатьТаблицуИДанные();
//        dv = new DataView(dt) { Sort = "Date DESC" };

//        Console.WriteLine("=== Продвинутый поиск документов ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter <строка_фильтра>  — установить фильтр (используйте SQL-like синтаксис)");
//        Console.WriteLine("    Пример: Title LIKE '%отчет%' AND Date BETWEEN #2020-01-01# AND #2023-12-31#");
//        Console.WriteLine("    Для дат используйте #MM/DD/YYYY#");
//        Console.WriteLine("    Для IN: Type IN ('Отчет', 'Письмо')");
//        Console.WriteLine("    Комбинируйте с AND/OR/NOT");
//        Console.WriteLine("  search                   — выполнить поиск и показать результаты (топ 20)");
//        Console.WriteLine("  count                    — показать количество результатов");
//        Console.WriteLine("  performance <фильтр>     — измерить производительность указанного фильтра");
//        Console.WriteLine("  suggest <столбец> <начало> — автодополнение для значений столбца");
//        Console.WriteLine("    Столбцы: Author, Type, Status");
//        Console.WriteLine("  report                   — отчёт о сложных поисках");
//        Console.WriteLine("  reset                    — сброс фильтра");
//        Console.WriteLine("  exit                     — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "filter" when parts.Length > 1:
//                        currentFilter = string.Join(" ", parts.Skip(1));
//                        dv.RowFilter = currentFilter;
//                        searchLogs.Add($"Сложный фильтр: {currentFilter} (результатов: {dv.Count})");
//                        Console.WriteLine($"Фильтр установлен: {currentFilter}");
//                        break;
//                    case "search":
//                        ПоказатьРезультаты();
//                        break;
//                    case "count":
//                        Console.WriteLine($"Количество документов по фильтру: {dv.Count}");
//                        break;
//                    case "performance" when parts.Length > 1:
//                        string perfFilter = string.Join(" ", parts.Skip(1));
//                        ИзмеритьПроизводительность(perfFilter);
//                        break;
//                    case "suggest" when parts.Length == 3:
//                        string column = parts[1];
//                        string prefix = parts[2];
//                        ПредложитьАвтодополнение(column, prefix);
//                        break;
//                    case "report":
//                        ПоказатьОтчёт();
//                        break;
//                    case "reset":
//                        currentFilter = "";
//                        dv.RowFilter = "";
//                        Console.WriteLine("Фильтр сброшен");
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message} (проверьте синтаксис фильтра)");
//            }
//        }
//    }

//    static void СоздатьТаблицуИДанные()
//    {
//        dt.Columns.Add("DocumentID", typeof(int));
//        dt.Columns.Add("Title", typeof(string));
//        dt.Columns.Add("Content", typeof(string));
//        dt.Columns.Add("Author", typeof(string));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Type", typeof(string));
//        dt.Columns.Add("Status", typeof(string));

//        string[] types = { "Отчет", "Меморандум", "Письмо", "Контракт", "Инструкция" };
//        string[] statuses = { "Черновик", "Утвержден", "Архив", "Отклонен" };
//        string[] authors = { "Иванов", "Петров", "Сидоров", "Козлов", "Смирнов" };
//        Random rnd = new Random();
//        DateTime baseDate = new DateTime(2010, 1, 1);

//        for (int i = 1; i <= 1000; i++)
//        {
//            string title = $"Документ {i}: {types[rnd.Next(types.Length)]}";
//            string content = new string('a', rnd.Next(100, 500)); // Случайный текст
//            string author = authors[rnd.Next(authors.Length)];
//            DateTime date = baseDate.AddDays(rnd.Next(5000));
//            string type = types[rnd.Next(types.Length)];
//            string status = statuses[rnd.Next(statuses.Length)];

//            dt.Rows.Add(i, title, content, author, date, type, status);
//        }
//    }

//    static void ПоказатьРезультаты(int max = 20)
//    {
//        Console.WriteLine("\nРезультаты поиска (топ 20):");
//        Console.WriteLine($"{"ID",-5} {"Заголовок",-30} {"Автор",-10} {"Дата",-10} {"Тип",-12} {"Статус",-10}");
//        Console.WriteLine(new string('-', 80));

//        int count = Math.Min(max, dv.Count);
//        for (int i = 0; i < count; i++)
//        {
//            var row = dv[i];
//            Console.WriteLine($"{row["DocumentID"],-5} {row["Title"].ToString().Truncate(29),-30} {row["Author"],-10} {((DateTime)row["Date"]).ToString("yyyy-MM-dd"),-10} {row["Type"],-12} {row["Status"],-10}");
//        }
//        if (dv.Count > max) Console.WriteLine($"... и ещё {dv.Count - max} документов");
//    }

//    static void ИзмеритьПроизводительность(string filter)
//    {
//        Stopwatch sw = new Stopwatch();
//        sw.Start();
//        for (int i = 0; i < 100; i++)
//        {
//            dv.RowFilter = filter;
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"Производительность для '{filter}' (100 применений): {sw.ElapsedMilliseconds} мс, результатов: {dv.Count}");
//    }

//    static void ПредложитьАвтодополнение(string column, string prefix)
//    {
//        if (!new[] { "Author", "Type", "Status" }.Contains(column))
//        {
//            Console.WriteLine("Поддерживаемые столбцы: Author, Type, Status");
//            return;
//        }

//        var uniqueValues = dt.AsEnumerable()
//            .Select(r => r.Field<string>(column))
//            .Distinct()
//            .Where(v => v.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
//            .OrderBy(v => v)
//            .ToList();

//        if (uniqueValues.Count == 0)
//        {
//            Console.WriteLine("Нет совпадений");
//            return;
//        }

//        Console.WriteLine("\nВозможные варианты:");
//        foreach (string val in uniqueValues)
//        {
//            Console.WriteLine(val);
//        }
//    }

//    static void ПоказатьОтчёт()
//    {
//        Console.WriteLine("\nОтчёт о сложных поисках:");
//        foreach (string log in searchLogs)
//        {
//            Console.WriteLine(log);
//        }
//        if (searchLogs.Count == 0) Console.WriteLine("Нет записей");
//    }
//}

//static class Extensions
//{
//    public static string Truncate(this string value, int maxLength)
//    {
//        if (string.IsNullOrEmpty(value)) return value;
//        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 1) + "…";
//    }
//}

//23
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//class Program
//{
//    static DataTable dt = new DataTable("Заказы");

//    static void Main()
//    {
//        НастроитьТаблицуИДанные();

//        DataView dv = new DataView(dt);
//        dv.Sort = "Date DESC";

//        Console.WriteLine("=== Отчёты с группировкой (консольная версия) ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  filter <фильтр>  — установить фильтр (пример: Status = 'Успех')");
//        Console.WriteLine("  report region    — отчёт с группировкой по регионам");
//        Console.WriteLine("  report status    — отчёт с группировкой по статусу");
//        Console.WriteLine("  report month     — отчёт с группировкой по месяцам");
//        Console.WriteLine("  report multi     — многоуровневая группировка (регион → месяц → статус)");
//        Console.WriteLine("  export <тип> <файл> — экспорт отчёта в файл (тип: region, status, month, multi)");
//        Console.WriteLine("  reset            — сброс фильтра");
//        Console.WriteLine("  exit             — выход\n");

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "filter" when parts.Length > 1:
//                        string filter = string.Join(" ", parts.Skip(1));
//                        dv.RowFilter = filter;
//                        Console.WriteLine($"Фильтр установлен: {filter} (результатов: {dv.Count})");
//                        break;
//                    case "report" when parts.Length == 2:
//                        string type = parts[1].ToLower();
//                        switch (type)
//                        {
//                            case "region": ПоказатьОтчётПоРегионам(dv); break;
//                            case "status": ПоказатьОтчётПоСтатусу(dv); break;
//                            case "month": ПоказатьОтчётПоМесяцам(dv); break;
//                            case "multi": ПоказатьМногоуровневыйОтчёт(dv); break;
//                            default: Console.WriteLine("Неизвестный тип отчёта"); break;
//                        }
//                        break;
//                    case "export" when parts.Length == 3:
//                        string expType = parts[1].ToLower();
//                        string file = parts[2];
//                        ЭкспортироватьОтчёт(dv, expType, file);
//                        break;
//                    case "reset":
//                        dv.RowFilter = "";
//                        Console.WriteLine("Фильтр сброшен");
//                        break;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда");
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void НастроитьТаблицуИДанные()
//    {
//        dt.Columns.Add("OrderID", typeof(int));
//        dt.Columns.Add("CustomerName", typeof(string));
//        dt.Columns.Add("Product", typeof(string));
//        dt.Columns.Add("Amount", typeof(double));
//        dt.Columns.Add("Date", typeof(DateTime));
//        dt.Columns.Add("Region", typeof(string));
//        dt.Columns.Add("Status", typeof(string));

//        string[] регионы = { "Север", "Юг", "Восток", "Запад" };
//        string[] статусы = { "Успех", "В ожидании", "Отменен" };
//        string[] продукты = { "Товар A", "Товар B", "Товар C" };
//        Random rnd = new Random();
//        DateTime начало = new DateTime(2023, 1, 1);

//        for (int i = 1; i <= 1000; i++)
//        {
//            dt.Rows.Add(
//                i,
//                $"Клиент {i}",
//                продукты[rnd.Next(продукты.Length)],
//                Math.Round(rnd.NextDouble() * 10000, 2),
//                начало.AddDays(rnd.Next(730)),
//                регионы[rnd.Next(регионы.Length)],
//                статусы[rnd.Next(статусы.Length)]
//            );
//        }
//    }

//    Метод для создания таблицы с результатами группировки(общий)
//    static DataTable СоздатьТаблицуГруппировки(string[] столбцыГруппировки, string столбецЗначения)
//    {
//        DataTable таблица = new DataTable();
//        foreach (string col in столбцыГруппировки)
//            таблица.Columns.Add(col, typeof(string));
//        таблица.Columns.Add("Сумма", typeof(double));
//        таблица.Columns.Add("Количество", typeof(int));
//        return таблица;
//    }

//    Группировка по регионам
//    static DataTable ГруппировкаПоРегионам(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Region" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            string регион = (string)drv["Region"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(регион))
//                группы[регион] = (0, 0);

//            var (текСумма, текКол) = группы[регион];
//            группы[регион] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоРегионам(DataView dv)
//    {
//        var таблица = ГруппировкаПоРегионам(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по регионам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Region"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Группировка по статусу
//    static DataTable ГруппировкаПоСтатусу(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Status" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            string статус = (string)drv["Status"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(статус))
//                группы[статус] = (0, 0);

//            var (текСумма, текКол) = группы[статус];
//            группы[статус] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоСтатусу(DataView dv)
//    {
//        var таблица = ГруппировкаПоСтатусу(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по статусам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Status"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Группировка по месяцам
//    static DataTable ГруппировкаПоМесяцам(DataView dv)
//    {
//        DataTable результат = СоздатьТаблицуГруппировки(new[] { "Месяц" }, "Amount");
//        var группы = new Dictionary<string, (double сумма, int кол)>();

//        foreach (DataRowView drv in dv)
//        {
//            DateTime дата = (DateTime)drv["Date"];
//            string месяц = дата.ToString("yyyy-MM");
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(месяц))
//                группы[месяц] = (0, 0);

//            var (текСумма, текКол) = группы[месяц];
//            группы[месяц] = (текСумма + сумма, текКол + 1);
//        }

//        foreach (var kvp in группы.OrderBy(k => k.Key))
//        {
//            результат.Rows.Add(kvp.Key, kvp.Value.сумма, kvp.Value.кол);
//        }

//        return результат;
//    }

//    static void ПоказатьОтчётПоМесяцам(DataView dv)
//    {
//        var таблица = ГруппировкаПоМесяцам(dv);
//        double итогоСумма = (double)таблица.Compute("SUM(Сумма)", null);
//        int итогоКол = (int)таблица.Compute("SUM(Количество)", null);

//        Console.WriteLine("\nОтчёт по месяцам:");
//        foreach (DataRow row in таблица.Rows)
//        {
//            Console.WriteLine($"{row["Месяц"]}: Сумма = {row["Сумма"]:F2}, Количество = {row["Количество"]}");
//        }
//        Console.WriteLine($"Итого: Сумма = {итогоСумма:F2}, Количество = {итогоКол}");
//    }

//    Многоуровневая группировка
//    static Dictionary<string, Dictionary<string, Dictionary<string, (double сумма, int кол)>>> МногоуровневаяГруппировка(DataView dv)
//    {
//        var группы = new Dictionary<string, Dictionary<string, Dictionary<string, (double, int)>>>();

//        foreach (DataRowView drv in dv)
//        {
//            string регион = (string)drv["Region"];
//            DateTime дата = (DateTime)drv["Date"];
//            string месяц = дата.ToString("yyyy-MM");
//            string статус = (string)drv["Status"];
//            double сумма = (double)drv["Amount"];

//            if (!группы.ContainsKey(регион))
//                группы[регион] = new Dictionary<string, Dictionary<string, (double, int)>>();

//            var месяцы = группы[регион];
//            if (!месяцы.ContainsKey(месяц))
//                месяцы[месяц] = new Dictionary<string, (double, int)>();

//            var статусы = месяцы[месяц];
//            if (!статусы.ContainsKey(статус))
//                статусы[статус] = (0, 0);

//            var (текСумма, текКол) = статусы[статус];
//            статусы[статус] = (текСумма + сумма, текКол + 1);
//        }

//        return группы;
//    }

//    static void ПоказатьМногоуровневыйОтчёт(DataView dv)
//    {
//        var группы = МногоуровневаяГруппировка(dv);
//        double глобИтогоСумма = 0;
//        int глобИтогоКол = 0;

//        Console.WriteLine("\nМногоуровневый отчёт (регион → месяц → статус):");

//        foreach (var регионKvp in группы.OrderBy(k => k.Key))
//        {
//            Console.WriteLine(регионKvp.Key);
//            double регионСумма = 0;
//            int регионКол = 0;

//            foreach (var месяцKvp in регионKvp.Value.OrderBy(k => k.Key))
//            {
//                Console.WriteLine($"  {месяцKvp.Key}");
//                double месяцСумма = 0;
//                int месяцКол = 0;

//                foreach (var статусKvp in месяцKvp.Value.OrderBy(k => k.Key))
//                {
//                    Console.WriteLine($"    {статусKvp.Key}: Сумма = {статусKvp.Value.сумма:F2}, Количество = {статусKvp.Value.кол}");
//                    месяцСумма += статусKvp.Value.сумма;
//                    месяцКол += статусKvp.Value.кол;
//                }

//                Console.WriteLine($"    Итого по месяцу: Сумма = {месяцСумма:F2}, Количество = {месяцКол}");
//                регионСумма += месяцСумма;
//                регионКол += месяцКол;
//            }

//            Console.WriteLine($"  Итого по региону: Сумма = {регионСумма:F2}, Количество = {регионКол}");
//            глобИтогоСумма += регионСумма;
//            глобИтогоКол += регионКол;
//        }

//        Console.WriteLine($"Глобальный итог: Сумма = {глобИтогоСумма:F2}, Количество = {глобИтогоКол}");
//    }

//    static void ЭкспортироватьОтчёт(DataView dv, string тип, string файл)
//    {
//        using (StreamWriter writer = new StreamWriter(файл))
//        {
//            Console.SetOut(writer);
//            switch (тип)
//            {
//                case "region": ПоказатьОтчётПоРегионам(dv); break;
//                case "status": ПоказатьОтчётПоСтатусу(dv); break;
//                case "month": ПоказатьОтчётПоМесяцам(dv); break;
//                case "multi": ПоказатьМногоуровневыйОтчёт(dv); break;
//                default: Console.WriteLine("Неизвестный тип"); return;
//            }
//            Console.SetOut(Console.Out);
//            Console.WriteLine($"Отчёт экспортирован в {файл}");
//        }
//    }
//}

//24
//using System;
//using System.Data;
//using System.Diagnostics;

//class Program
//{
//    static DataTable dt = new DataTable("Заказы товаров");
//    static DataView dv;

//    static void Main()
//    {
//        НастроитьТаблицу();
//        ЗаполнитьДанные();

//        dv = new DataView(dt);
//        dv.Sort = "DiscountedTotal DESC";

//        Console.WriteLine("=== DataView с вычисляемыми столбцами ===\n");

//        ПоказатьПример(10);

//        Console.WriteLine("\nЗаказы с итоговой суммой после скидки > 1000:");
//        dv.RowFilter = "DiscountedTotal > 1000";
//        ПоказатьПример(8);
//        Console.WriteLine($"Найдено заказов: {dv.Count}");

//        dv.Sort = "DiscountedTotal DESC";
//        Console.WriteLine("\nТоп-5 самых дорогих заказов после скидки:");
//        ПоказатьПример(5);

//        double макс = (double)dt.Compute("MAX(DiscountedTotal)", "");
//        double мин = (double)dt.Compute("MIN(DiscountedTotal)", "");
//        double сумма = (double)dt.Compute("SUM(DiscountedTotal)", "");
//        double среднее = (double)dt.Compute("AVG(DiscountedTotal)", "");

//        Console.WriteLine($"\nСтатистика DiscountedTotal:");
//        Console.WriteLine($"  Максимум: {макс:F2}");
//        Console.WriteLine($"  Минимум:  {мин:F2}");
//        Console.WriteLine($"  Сумма:    {сумма:F2}");
//        Console.WriteLine($"  Среднее:  {среднее:F2}");

//        Console.WriteLine("\nДемонстрация обновления вычисляемых полей:");
//        Console.WriteLine("До изменения (строка с OrderDetailID = 5):");
//        dv.RowFilter = "OrderDetailID = 5";
//        ПоказатьПример(1);

//        DataRow ряд = dt.Rows.Find(5);
//        if (ряд != null)
//        {
//            ряд["Quantity"] = 20;
//            ряд["Discount"] = 0.3;
//            Console.WriteLine("Изменено: Quantity = 20, Discount = 30%");
//        }

//        Console.WriteLine("После изменения:");
//        ПоказатьПример(1);
//        dv.RowFilter = "";

//        Console.WriteLine("\nСравнение производительности фильтрации (1000 применений):");

//        Stopwatch sw = new Stopwatch();

//        sw.Restart();
//        for (int i = 0; i < 1000; i++)
//        {
//            dv.RowFilter = "UnitPrice > 50";
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"По обычному полю (UnitPrice > 50): {sw.ElapsedMilliseconds} мс");

//        sw.Restart();
//        for (int i = 0; i < 1000; i++)
//        {
//            dv.RowFilter = "DiscountedTotal > 1000";
//            int count = dv.Count;
//        }
//        sw.Stop();
//        Console.WriteLine($"По вычисляемому полю (DiscountedTotal > 1000): {sw.ElapsedMilliseconds} мс");

//        dv.RowFilter = "";
//    }

//    static void НастроитьТаблицу()
//    {
//        dt.Columns.Add("OrderDetailID", typeof(int));
//        dt.Columns.Add("OrderID", typeof(int));
//        dt.Columns.Add("ProductName", typeof(string));
//        dt.Columns.Add("Quantity", typeof(int));
//        dt.Columns.Add("UnitPrice", typeof(double));
//        dt.Columns.Add("Discount", typeof(double));
//        var totalCol = dt.Columns.Add("Total", typeof(double));
//        totalCol.Expression = "Quantity * UnitPrice";

//        var discountedCol = dt.Columns.Add("DiscountedTotal", typeof(double));
//        discountedCol.Expression = "Total * (1 - Discount)";

//        dt.PrimaryKey = new[] { dt.Columns["OrderDetailID"] };
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random();
//        string[] продукты = { "Ноутбук", "Монитор", "Клавиатура", "Мышь", "Принтер", "Сканер", "Веб-камера" };

//        for (int i = 1; i <= 250; i++)
//        {
//            dt.Rows.Add(
//                i,
//                rnd.Next(1, 100),
//                продукты[rnd.Next(продукты.Length)],
//                rnd.Next(1, 30),
//                Math.Round(rnd.NextDouble() * 200 + 20, 2),
//                Math.Round(rnd.NextDouble() * 0.4, 2)
//            );
//        }
//    }

//    static void ПоказатьПример(int количество)
//    {
//        int макс = Math.Min(количество, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Товар",-15} {"Кол-во",-6} {"Цена",-8} {"Скидка",-7} {"Total",-10} {"DiscountedTotal",-15}");
//        Console.WriteLine(new string('-', 80));

//        for (int i = 0; i < макс; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine(
//                $"{row["OrderDetailID"],-4} " +
//                $"{row["ProductName"].ToString().Truncate(14),-15} " +
//                $"{row["Quantity"],-6} " +
//                $"{row["UnitPrice"],-8:F2} " +
//                $"{row["Discount"],-7:P0} " +
//                $"{row["Total"],-10:F2} " +
//                $"{row["DiscountedTotal"],-15:F2}"
//            );
//        }
//        if (dv.Count > макс)
//            Console.WriteLine($"... и ещё {dv.Count - макс} строк");
//        Console.WriteLine();
//    }
//}

//static class StringExt
//{
//    public static string Truncate(this string s, int len) =>
//        s.Length <= len ? s : s.Substring(0, len - 1) + "…";
//}

//25
//using System;
//using System.Data;
//using System.IO;
//using System.Collections.Generic;

//class Program
//{
//    static DataTable dt = new DataTable("Электронные письма");

//    static void Main()
//    {
//        НастроитьТаблицу();
//        ЗаполнитьДанные();
//        ОтметитьТочныеДубликаты();
//        ОтметитьПохожиеДубликаты();

//        DataView dvВсе = new DataView(dt) { Sort = "EmailID" };
//        DataView dvДубликаты = new DataView(dt) { RowFilter = "Duplicates > 0", Sort = "EmailAddress" };
//        DataView dvУникальные = new DataView(dt) { RowFilter = "Duplicates = 0", Sort = "EmailID" };

//        int колДубликатов = dvДубликаты.Count;
//        int колУникальных = dvУникальные.Count;
//        Console.WriteLine($"Статистика: Дубликатов: {колДубликатов}, Уникальных: {колУникальных}, Всего: {dt.Rows.Count}");

//        Console.WriteLine("\nПример дубликатов:");
//        ПоказатьСтроки(dvДубликаты, 20);

//        ОбъединитьДубликаты();

//        dvДубликаты.RowFilter = "Duplicates > 0";
//        dvУникальные.RowFilter = "Duplicates = 0";
//        колДубликатов = dvДубликаты.Count;
//        колУникальных = dvУникальные.Count;
//        Console.WriteLine($"\nПосле объединения: Дубликатов: {колДубликатов}, Уникальных: {колУникальных}, Всего: {dt.Rows.Count}");

//        СоздатьОтчёт(dvДубликаты);
//    }

//    static void НастроитьТаблицу()
//    {
//        dt.Columns.Add("EmailID", typeof(int));
//        dt.Columns.Add("EmailAddress", typeof(string));
//        dt.Columns.Add("Name", typeof(string));
//        dt.Columns.Add("Domain", typeof(string));
//        dt.Columns.Add("Duplicates", typeof(int));

//        dt.PrimaryKey = new[] { dt.Columns["EmailID"] };
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random(42);
//        string[] домены = { "gmail.com", "yandex.ru", "mail.ru", "outlook.com", "protonmail.com" };

//        HashSet<string> emails = new HashSet<string>();

//        for (int i = 1; i <= 500; i++)
//        {
//            string имя = СлучайноеИмя(rnd, rnd.Next(5, 11));
//            string домен = домены[rnd.Next(домены.Length)];
//            string email = $"{имя}@{домен}";

//            if (rnd.NextDouble() < 0.2 && emails.Count > 0)
//            {
//                email = emails.ElementAt(rnd.Next(emails.Count));
//            }
//            else if (rnd.NextDouble() < 0.1 && emails.Count > 0)
//            {
//                string существующий = emails.ElementAt(rnd.Next(emails.Count));
//                string староеИмя = существующий.Split('@')[0];
//                int поз = rnd.Next(староеИмя.Length);
//                char новыйСимвол = (char)rnd.Next('a', 'z' + 1);
//                имя = староеИмя.Substring(0, поз) + новыйСимвол + староеИмя.Substring(поз + 1);
//                домен = домены[rnd.Next(домены.Length)];
//                email = $"{имя}@{домен}";
//            }

//            emails.Add(email);
//            dt.Rows.Add(i, email, имя, домен, 0);
//        }
//    }

//    static string СлучайноеИмя(Random rnd, int длина)
//    {
//        string символы = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
//        char[] имя = new char[длина];
//        for (int j = 0; j < длина; j++)
//            имя[j] = символы[rnd.Next(символы.Length)];
//        return new string(имя);
//    }

//    static void ОтметитьТочныеДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "EmailAddress" };
//        string предыдущийEmail = null;
//        int счётчик = 0;

//        for (int i = 0; i < dv.Count; i++)
//        {
//            string текущийEmail = (string)dv[i]["EmailAddress"];

//            if (текущийEmail == предыдущийEmail)
//            {
//                счётчик++;
//                dv[i]["Duplicates"] = счётчик;

//                for (int j = i - 1; j >= 0; j--)
//                {
//                    if ((string)dv[j]["EmailAddress"] == текущийEmail)
//                        dv[j]["Duplicates"] = счётчик + 1;
//                    else
//                        break;
//                }
//            }
//            else
//            {
//                счётчик = 0;
//            }

//            предыдущийEmail = текущийEmail;
//        }
//    }

//    static void ОтметитьПохожиеДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "Name" };

//        for (int i = 0; i < dv.Count - 1; i++)
//        {
//            for (int j = i + 1; j < dv.Count; j++)
//            {
//                string имя1 = (string)dv[i]["Name"];
//                string имя2 = (string)dv[j]["Name"];
//                string домен1 = (string)dv[i]["Domain"];
//                string домен2 = (string)dv[j]["Domain"];

//                if (домен1 != домен2 && РасстояниеЛевенштейна(имя1, имя2) <= 1)
//                {
//                    dv[i]["Duplicates"] = (int)dv[i]["Duplicates"] + 1;
//                    dv[j]["Duplicates"] = (int)dv[j]["Duplicates"] + 1;
//                }

//                if (Math.Abs(имя1.Length - имя2.Length) > 1 || имя1[0] != имя2[0])
//                    break;
//            }
//        }
//    }

//    static int РасстояниеЛевенштейна(string s1, string s2)
//    {
//        int len1 = s1.Length;
//        int len2 = s2.Length;
//        int[,] матрица = new int[len1 + 1, len2 + 1];

//        for (int i = 0; i <= len1; i++) матрица[i, 0] = i;
//        for (int j = 0; j <= len2; j++) матрица[0, j] = j;

//        for (int i = 1; i <= len1; i++)
//        {
//            for (int j = 1; j <= len2; j++)
//            {
//                int стоимость = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
//                матрица[i, j] = Math.Min(
//                    Math.Min(матрица[i - 1, j] + 1, матрица[i, j - 1] + 1),
//                    матрица[i - 1, j - 1] + стоимость
//                );
//            }
//        }

//        return матрица[len1, len2];
//    }

//    static void ПоказатьСтроки(DataView dv, int макс)
//    {
//        int кол = Math.Min(макс, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Email",-30} {"Имя",-15} {"Домен",-15} {"Дубликатов",-10}");
//        Console.WriteLine(new string('-', 80));

//        for (int i = 0; i < кол; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine(
//                $"{row["EmailID"],-4} " +
//                $"{row["EmailAddress"],-30} " +
//                $"{row["Name"],-15} " +
//                $"{row["Domain"],-15} " +
//                $"{row["Duplicates"],-10}"
//            );
//        }
//    }

//    static void ОбъединитьДубликаты()
//    {
//        DataView dv = new DataView(dt) { Sort = "EmailAddress, EmailID" };
//        List<DataRow> кУдалению = new List<DataRow>();

//        string предыдущийEmail = null;
//        DataRow главная = null;

//        for (int i = 0; i < dv.Count; i++)
//        {
//            DataRow row = dv[i].Row;
//            string email = (string)row["EmailAddress"];

//            if (email != предыдущийEmail)
//            {
//                главная = row;
//                предыдущийEmail = email;
//            }
//            else
//            {
//                кУдалению.Add(row);
//            }
//        }

//        foreach (DataRow row in кУдалению)
//        {
//            dt.Rows.Remove(row);
//        }

//        foreach (DataRow row in dt.Rows)
//        {
//            row["Duplicates"] = 0;
//        }
//    }

//    static void СоздатьОтчёт(DataView dvДубликаты)
//    {
//        using (StreamWriter writer = new StreamWriter("дубликаты_отчёт.txt"))
//        {
//            writer.WriteLine("Отчёт о дубликатах электронных писем");
//            writer.WriteLine($"Дата: {DateTime.Now}");
//            writer.WriteLine($"Всего дубликатов: {dvДубликаты.Count}");
//            writer.WriteLine("\nСписок дубликатов:");

//            string предыдущийEmail = null;
//            foreach (DataRowView drv in dvДубликаты)
//            {
//                string email = (string)drv["EmailAddress"];
//                if (email != предыдущийEmail)
//                {
//                    writer.WriteLine($"\nГруппа дубликатов для {email}:");
//                    предыдущийEmail = email;
//                }
//                writer.WriteLine($"  ID: {drv["EmailID"]}, Имя: {drv["Name"]}, Домен: {drv["Domain"]}, Дубликатов: {drv["Duplicates"]}");
//            }

//            writer.WriteLine("\nРекомендации:");
//            writer.WriteLine("- Объедините дубликаты, выбрав главную запись с наиболее полной информацией.");
//            writer.WriteLine("- Для похожих дубликатов проверьте, являются ли они одним и тем же пользователем.");
//            writer.WriteLine("- Регулярно очищайте базу от дубликатов для оптимизации.");
//        }

//        Console.WriteLine("\nОтчёт создан: дубликаты_отчёт.txt");
//    }
//}

//using System.Data;

//26
//using System;
//using System.Data;

//class Program
//{
//    static DataTable products = new DataTable("Товары");
//    static DataTable categories = new DataTable("Категории");
//    static DataTable discounts = new DataTable("Скидки");
//    static DataSet ds = new DataSet();

//    static DataView dv1;
//    static DataView dv2;
//    static DataView dv3;

//    static int adds = 0, updates = 0, deletes = 0;
//    static string log = "";

//    static void Main()
//    {
//        НастроитьТаблицы();
//        ЗаполнитьДанные();

//        dv1 = new DataView(products) { Sort = "Category" };
//        dv2 = new DataView(products) { RowFilter = "Price > 1000" };
//        dv3 = new DataView(products) { Sort = "Price DESC" };

//        products.RowChanged += ЛогироватьИзменение;
//        products.RowDeleting += ЛогироватьУдаление;
//        products.RowDeleted += ЛогироватьУдаление;

//        Console.WriteLine("=== Синхронизация DataView (консольная версия) ===\n");

//        ПоказатьПредставления("Начальное состояние");

//        DataRow newRow = products.NewRow();
//        newRow["ProductID"] = 21;
//        newRow["Name"] = "Новый Товар";
//        newRow["Category"] = "Электроника";
//        newRow["Price"] = 1500.0;
//        newRow["Stock"] = 50;
//        products.Rows.Add(newRow);
//        ПоказатьПредставления("После добавления");

//        DataRow rowToUpdate = products.Rows.Find(1);
//        if (rowToUpdate != null)
//        {
//            rowToUpdate["Price"] = 1200.0;
//        }
//        ПоказатьПредставления("После изменения цены");

//        DataRow rowToDelete = products.Rows.Find(21);
//        if (rowToDelete != null)
//        {
//            rowToDelete.Delete();
//        }
//        ПоказатьПредставления("После удаления");

//        Console.WriteLine("\nЛог изменений:");
//        Console.WriteLine(log);
//        Console.WriteLine("\nСтатистика синхронизации:");
//        Console.WriteLine($"Добавлений: {adds}");
//        Console.WriteLine($"Изменений: {updates}");
//        Console.WriteLine($"Удалений: {deletes}");
//    }

//    static void НастроитьТаблицы()
//    {
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.PrimaryKey = new[] { categories.Columns["CategoryID"] };

//        discounts.Columns.Add("DiscountID", typeof(int));
//        discounts.Columns.Add("ProductID", typeof(int));
//        discounts.Columns.Add("DiscountPercent", typeof(double));
//        discounts.PrimaryKey = new[] { discounts.Columns["DiscountID"] };

//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("Name", typeof(string));
//        products.Columns.Add("Category", typeof(string));
//        products.Columns.Add("Price", typeof(double));
//        products.Columns.Add("Stock", typeof(int));
//        products.PrimaryKey = new[] { products.Columns["ProductID"] };

//        ds.Tables.Add(categories);
//        ds.Tables.Add(discounts);
//        ds.Tables.Add(products);

//        ds.Relations.Add("ProductDiscounts", products.Columns["ProductID"], discounts.Columns["ProductID"]);
//    }

//    static void ЗаполнитьДанные()
//    {
//        Random rnd = new Random();

//        categories.Rows.Add(1, "Электроника");
//        categories.Rows.Add(2, "Одежда");
//        categories.Rows.Add(3, "Книги");

//        string[] категории = { "Электроника", "Одежда", "Книги" };
//        for (int i = 1; i <= 20; i++)
//        {
//            products.Rows.Add(i, $"Товар {i}", категории[rnd.Next(3)], rnd.Next(500, 2000), rnd.Next(10, 100));
//        }

//        for (int i = 1; i <= 10; i++)
//        {
//            discounts.Rows.Add(i, rnd.Next(1, 21), rnd.NextDouble() * 0.3);
//        }
//    }

//    static void ПоказатьПредставления(string заголовок)
//    {
//        Console.WriteLine($"\n{заголовок}:");
//        Console.WriteLine("DV1 (сортировка по Category):");
//        ПоказатьТаблица(dv1, 5);

//        Console.WriteLine("DV2 (Price > 1000):");
//        ПоказатьТаблица(dv2, 5);

//        Console.WriteLine("DV3 (сортировка по Price DESC):");
//        ПоказатьТаблица(dv3, 5);
//    }

//    static void ПоказатьТаблица(DataView dv, int макс)
//    {
//        int кол = Math.Min(макс, dv.Count);
//        Console.WriteLine($"{"ID",-4} {"Имя",-15} {"Категория",-12} {"Цена",-8} {"Запас",-6}");
//        Console.WriteLine(new string('-', 50));

//        for (int i = 0; i < кол; i++)
//        {
//            DataRowView row = dv[i];
//            Console.WriteLine($"{row["ProductID"],-4} {row["Name"].ToString().Truncate(14),-15} {row["Category"],-12} {row["Price"],-8:F2} {row["Stock"],-6}");
//        }
//        if (dv.Count > макс) Console.WriteLine($"... + {dv.Count - макс} строк");
//        Console.WriteLine();
//    }

//    static void ЛогироватьИзменение(object sender, DataRowChangeEventArgs e)
//    {
//        if (e.Action == DataRowAction.Add)
//        {
//            adds++;
//            log += $"Добавлен: ID={e.Row["ProductID"]}, Имя={e.Row["Name"]}\n";
//        }
//        else if (e.Action == DataRowAction.Change)
//        {
//            updates++;
//            log += $"Изменён: ID={e.Row["ProductID"]}, Новые значения: Цена={e.Row["Price"]}, Запас={e.Row["Stock"]}\n";
//        }
//    }

//    static void ЛогироватьУдаление(object sender, DataRowChangeEventArgs e)
//    {
//        if (e.Action == DataRowAction.Delete)
//        {
//            deletes++;
//            log += $"Удалён: ID={e.Row["ProductID"]}\n";
//        }
//    }
//}

//static class Ext
//{
//    public static string Truncate(this string s, int len) => s.Length > len ? s.Substring(0, len - 1) + "…" : s;
//}

//27
//using System;
//using System.Data;
//using System.Diagnostics;

//class Program
//{
//    static void Main()
//    {
//        DataTable dt = new DataTable("Логи");
//        dt.Columns.Add("LogID", typeof(int));
//        dt.Columns.Add("Timestamp", typeof(DateTime));
//        dt.Columns.Add("Level", typeof(string));
//        dt.Columns.Add("Message", typeof(string));
//        dt.Columns.Add("Source", typeof(string));
//        dt.Columns.Add("EventID", typeof(int));

//        dt.PrimaryKey = new[] { dt.Columns["LogID"] };

//        Random rnd = new Random();
//        string[] levels = { "Info", "Warning", "Error", "Debug" };
//        string[] sources = { "App1", "App2", "Service", "API" };
//        DateTime baseTime = DateTime.Now.AddYears(-1);

//        Console.WriteLine("Генерация 1,000,000 записей...");
//        long memBefore = GC.GetTotalMemory(true);
//        Stopwatch sw = Stopwatch.StartNew();

//        for (int i = 1; i <= 1000000; i++)
//        {
//            dt.Rows.Add(
//                i,
//                baseTime.AddSeconds(rnd.Next(31536000)),
//                levels[rnd.Next(levels.Length)],
//                $"Сообщение {i}",
//                sources[rnd.Next(sources.Length)],
//                rnd.Next(1000)
//            );
//        }

//        sw.Stop();
//        long memAfter = GC.GetTotalMemory(true);
//        Console.WriteLine($"Генерация завершена. Время: {sw.ElapsedMilliseconds} мс, Память: {(memAfter - memBefore) / 1024 / 1024} МБ");

//        string filter = "Level = 'Error'";

//        Console.WriteLine("\nСценарий 1: Select() в DataTable");
//        long mem1Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataRow[] res1First = dt.Select(filter);
//        sw.Stop();
//        long mem1After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Результатов = {res1First.Length}, Память = {(mem1After - mem1Before) / 1024} КБ");

//        sw.Restart();
//        DataRow[] res1Repeat = dt.Select(filter);
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс");

//        Console.WriteLine("\nСценарий 2: Создание DataView с фильтром");
//        long mem2Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataView dvFirst = new DataView(dt) { RowFilter = filter };
//        int count2First = dvFirst.Count;
//        sw.Stop();
//        long mem2After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Результатов = {count2First}, Память = {(mem2After - mem2Before) / 1024} КБ");

//        Console.WriteLine("\nСценарий 3: Повторный поиск в DataView");
//        sw.Restart();
//        int count3Repeat = dvFirst.Count;
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс, Результатов = {count3Repeat}");

//        Console.WriteLine("\nСценарий 4: Поиск через Find() по PK");
//        int searchId = 500000;
//        long mem4Before = GC.GetTotalMemory(true);
//        sw.Restart();
//        DataRow row4First = dt.Rows.Find(searchId);
//        sw.Stop();
//        long mem4After = GC.GetTotalMemory(true);
//        Console.WriteLine($"Первое выполнение: Время = {sw.ElapsedMilliseconds} мс, Найдено = {(row4First != null ? "Да" : "Нет")}, Память = {(mem4After - mem4Before) / 1024} КБ");

//        sw.Restart();
//        DataRow row4Repeat = dt.Rows.Find(searchId);
//        sw.Stop();
//        Console.WriteLine($"Повторное: Время = {sw.ElapsedMilliseconds} мс");

//        Console.WriteLine("\nКэширование результатов Select:");
//        DataRow[] cachedRes = dt.Select(filter);
//        sw.Restart();
//        int cachedCount = cachedRes.Length;
//        sw.Stop();
//        Console.WriteLine($"Доступ к кэшу: Время = {sw.ElapsedMilliseconds} мс, Результатов = {cachedCount}");

//        GC.Collect();
//        long baseMem = GC.GetTotalMemory(true);

//        DataRow[] selectRes = dt.Select(filter);
//        GC.Collect();
//        long selectMem = GC.GetTotalMemory(true) - baseMem;

//        DataView dvMem = new DataView(dt) { RowFilter = filter };
//        GC.Collect();
//        long dvMemUsage = GC.GetTotalMemory(true) - baseMem - selectMem;

//        Console.WriteLine("\nСравнение памяти:");
//        Console.WriteLine($"Select(): ~{selectMem / 1024} КБ");
//        Console.WriteLine($"DataView: ~{dvMemUsage / 1024} КБ");

//        Console.WriteLine("\nОтчёт о производительности:");
//        Console.WriteLine("| Сценарий | Первое (мс) | Повтор (мс) | Память (КБ) |");
//        Console.WriteLine("|----------|-------------|-------------|-------------|");
//        Console.WriteLine("| 1. Select | ... | ... | ... |");  
//        Console.WriteLine("\nРекомендации по использованию DataView:");
//        Console.WriteLine("- Для повторяющихся запросов создавайте DataView один раз — повторные фильтры быстрые благодаря индексу.");
//        Console.WriteLine("- DataView оптимизирован для больших данных, где Select() сканирует таблицу каждый раз.");
//        Console.WriteLine("- Кэшируйте результаты для статичных данных, чтобы минимизировать вычисления.");
//        Console.WriteLine("- DataView использует больше памяти на индекс, но ускоряет операции.");
//        Console.WriteLine("- Комбинируйте с PrimaryKey для быстрого Find() по ключу.");
//    }
//}

//28
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        DataTable library = new DataTable("Библиотека");
//        library.Columns.Add("BookID", typeof(int));
//        library.Columns.Add("Title", typeof(string));
//        library.Columns.Add("Author", typeof(string));
//        library.Columns.Add("Year", typeof(int));
//        library.Columns.Add("ISBN", typeof(string));
//        library.Columns.Add("Genre", typeof(string));
//        library.Columns.Add("Rating", typeof(double));
//        library.Columns.Add("Pages", typeof(int));

//        ЗаполнитьБиблиотеку(library);

//        SearchEngine engine = new SearchEngine(library);

//        Console.WriteLine("=== Библиотека с продвинутым поиском (консольный интерфейс) ===\n");
//        Console.WriteLine("Доступные команды:");
//        Console.WriteLine("  author <автор>          — поиск по автору (частичное совпадение)");
//        Console.WriteLine("  title <название>        — поиск по названию");
//        Console.WriteLine("  genre <жанр>            — поиск по жанру");
//        Console.WriteLine("  year <от> <до>          — поиск по году (диапазон)");
//        Console.WriteLine("  rating <мин>            — поиск по рейтингу (минимум)");
//        Console.WriteLine("  combined                — комбинированный поиск (вводите параметры по запросу)");
//        Console.WriteLine("  history                 — показать историю поисков");
//        Console.WriteLine("  recommend               — рекомендации на основе истории");
//        Console.WriteLine("  save <файл>             — сохранить последние результаты в CSV");
//        Console.WriteLine("  exit                    — выход\n");

//        DataView lastResults = null;

//        while (true)
//        {
//            Console.Write("> ");
//            string input = Console.ReadLine()?.Trim();
//            if (string.IsNullOrEmpty(input)) continue;

//            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//            string cmd = parts[0].ToLower();

//            try
//            {
//                switch (cmd)
//                {
//                    case "author" when parts.Length > 1:
//                        string author = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByAuthor(author);
//                        break;
//                    case "title" when parts.Length > 1:
//                        string title = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByTitle(title);
//                        break;
//                    case "genre" when parts.Length > 1:
//                        string genre = string.Join(" ", parts.Skip(1));
//                        lastResults = engine.SearchByGenre(genre);
//                        break;
//                    case "year" when parts.Length == 3:
//                        int fromYear = int.Parse(parts[1]);
//                        int toYear = int.Parse(parts[2]);
//                        lastResults = engine.SearchByYear(fromYear, toYear);
//                        break;
//                    case "rating" when parts.Length == 2:
//                        double minRating = double.Parse(parts[1]);
//                        lastResults = engine.SearchByRating(minRating);
//                        break;
//                    case "combined":
//                        lastResults = КомбинированныйПоиск(engine);
//                        break;
//                    case "history":
//                        engine.ShowHistory();
//                        continue;
//                    case "recommend":
//                        engine.GetRecommendations();
//                        continue;
//                    case "save" when parts.Length == 2 && lastResults != null:
//                        СохранитьРезультаты(lastResults, parts[1]);
//                        continue;
//                    case "exit":
//                        return;
//                    default:
//                        Console.WriteLine("Неизвестная команда или неверные аргументы.");
//                        continue;
//                }

//                if (lastResults != null)
//                {
//                    ПоказатьРезультаты(lastResults);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка: {ex.Message}");
//            }
//        }
//    }

//    static void ЗаполнитьБиблиотеку(DataTable table)
//    {
//        string[] authors = { "Лев Толстой", "Фёдор Достоевский", "Джейн Остин", "Джон Толкин", "Джоан Роулинг", "Стивен Кинг", "Агата Кристи", "Марк Твен", "Джордж Орвелл", "Рэй Брэдбери" };
//        string[] genres = { "Фантастика", "Классика", "Детектив", "Фэнтези", "Ужасы", "Приключения", "Роман" };
//        Random rnd = new Random();

//        for (int i = 1; i <= 500; i++)
//        {
//            table.Rows.Add(
//                i,
//                $"Книга {i}: {genres[rnd.Next(genres.Length)]}",
//                authors[rnd.Next(authors.Length)],
//                rnd.Next(1800, 2024),
//                $"978-{rnd.Next(1000000000, 2000000000)}",
//                genres[rnd.Next(genres.Length)],
//                Math.Round(rnd.NextDouble() * 5, 1),
//                rnd.Next(100, 1000)
//            );
//        }
//    }

//    static DataView КомбинированныйПоиск(SearchEngine engine)
//    {
//        Console.Write("Автор (или пусто): ");
//        string author = Console.ReadLine()?.Trim();

//        Console.Write("Название (или пусто): ");
//        string title = Console.ReadLine()?.Trim();

//        Console.Write("Жанр (или пусто): ");
//        string genre = Console.ReadLine()?.Trim();

//        Console.Write("Год от (или 0): ");
//        int fromYear = int.TryParse(Console.ReadLine(), out int fy) ? fy : 0;

//        Console.Write("Год до (или 0): ");
//        int toYear = int.TryParse(Console.ReadLine(), out int ty) ? ty : 0;

//        Console.Write("Мин. рейтинг (или 0): ");
//        double minRating = double.TryParse(Console.ReadLine(), out double mr) ? mr : 0;

//        return engine.CombinedSearch(author, title, genre, fromYear > 0 ? fromYear : (int?)null, toYear > 0 ? toYear : (int?)null, minRating > 0 ? minRating : (double?)null);
//    }

//    static void ПоказатьРезультаты(DataView results)
//    {
//        Console.WriteLine("\nРезультаты поиска:");
//        Console.WriteLine($"{"ID",-4} {"Название",-30} {"Автор",-20} {"Год",-5} {"ISBN",-15} {"Жанр",-15} {"Рейтинг",-8} {"Страниц",-7}");
//        Console.WriteLine(new string('-', 110));

//        foreach (DataRowView row in results)
//        {
//            Console.WriteLine(
//                $"{row["BookID"],-4} " +
//                $"{row["Title"].ToString().Truncate(29),-30} " +
//                $"{row["Author"].ToString().Truncate(19),-20} " +
//                $"{row["Year"],-5} " +
//                $"{row["ISBN"],-15} " +
//                $"{row["Genre"],-15} " +
//                $"{row["Rating"],-8:F1} " +
//                $"{row["Pages"],-7}"
//            );
//        }

//        Console.WriteLine($"\nНайдено: {results.Count} книг\n");
//    }

//    static void СохранитьРезультаты(DataView results, string filePath)
//    {
//        using (StreamWriter writer = new StreamWriter(filePath))
//        {
//            writer.WriteLine("BookID,Title,Author,Year,ISBN,Genre,Rating,Pages");

//            foreach (DataRowView row in results)
//            {
//                writer.WriteLine(
//                    $"{row["BookID"]}," +
//                    $"\"{row["Title"].ToString().Replace("\"", "\"\"")}\"," +
//                    $"\"{row["Author"].ToString().Replace("\"", "\"\"")}\"," +
//                    $"{row["Year"]}," +
//                    $"{row["ISBN"]}," +
//                    $"{row["Genre"]}," +
//                    $"{row["Rating"]}," +
//                    $"{row["Pages"]}"
//                );
//            }
//        }

//        Console.WriteLine($"Результаты сохранены в {filePath}\n");
//    }
//}

//class SearchEngine
//{
//    private DataTable _library;
//    private List<string> _history = new List<string>();

//    public SearchEngine(DataTable library)
//    {
//        _library = library;
//    }

//    public DataView SearchByAuthor(string author)
//    {
//        string filter = $"Author LIKE '%{author.Replace("'", "''")}%'";
//        return СоздатьView(filter, "По автору: " + author);
//    }

//    public DataView SearchByTitle(string title)
//    {
//        string filter = $"Title LIKE '%{title.Replace("'", "''")}%'";
//        return СоздатьView(filter, "По названию: " + title);
//    }

//    public DataView SearchByGenre(string genre)
//    {
//        string filter = $"Genre = '{genre.Replace("'", "''")}'";
//        return СоздатьView(filter, "По жанру: " + genre);
//    }

//    public DataView SearchByYear(int from, int to)
//    {
//        string filter = $"Year >= {from} AND Year <= {to}";
//        return СоздатьView(filter, $"По году: {from}-{to}");
//    }

//    public DataView SearchByRating(double min)
//    {
//        string filter = $"Rating >= {min}";
//        return СоздатьView(filter, $"По рейтингу: >= {min}");
//    }

//    public DataView CombinedSearch(string author = null, string title = null, string genre = null, int? fromYear = null, int? toYear = null, double? minRating = null)
//    {
//        List<string> filters = new List<string>();
//        string desc = "Комбинированный: ";

//        if (!string.IsNullOrEmpty(author))
//        {
//            filters.Add($"Author LIKE '%{author.Replace("'", "''")}%'");
//            desc += $"Автор={author}; ";
//        }
//        if (!string.IsNullOrEmpty(title))
//        {
//            filters.Add($"Title LIKE '%{title.Replace("'", "''")}%'");
//            desc += $"Название={title}; ";
//        }
//        if (!string.IsNullOrEmpty(genre))
//        {
//            filters.Add($"Genre = '{genre.Replace("'", "''")}'");
//            desc += $"Жанр={genre}; ";
//        }
//        if (fromYear.HasValue && toYear.HasValue)
//        {
//            filters.Add($"Year >= {fromYear} AND Year <= {toYear}");
//            desc += $"Год={fromYear}-{toYear}; ";
//        }
//        if (minRating.HasValue)
//        {
//            filters.Add($"Rating >= {minRating}");
//            desc += $"Рейтинг>={minRating}; ";
//        }

//        string filter = string.Join(" AND ", filters);
//        return СоздатьView(filter, desc);
//    }

//    private DataView СоздатьView(string filter, string description)
//    {
//        DataView view = new DataView(_library)
//        {
//            RowFilter = filter,
//            Sort = "Rating DESC"
//        };

//        _history.Add($"{DateTime.Now:yyyy-MM-dd HH:mm}: {description} (найдено: {view.Count})");

//        return view;
//    }

//    public void ShowHistory()
//    {
//        Console.WriteLine("\nИстория поисков:");
//        foreach (string entry in _history)
//        {
//            Console.WriteLine(entry);
//        }
//        Console.WriteLine();
//    }

//    public void GetRecommendations()
//    {
//        if (_history.Count == 0)
//        {
//            Console.WriteLine("Нет истории для рекомендаций.");
//            return;
//        }

//        Dictionary<string, int> genreCount = new Dictionary<string, int>();
//        Dictionary<string, int> authorCount = new Dictionary<string, int>();

//        foreach (string entry in _history)
//        {
//            if (entry.Contains("Жанр="))
//            {
//                string genre = entry.Split("Жанр=")[1].Split(';')[0].Trim();
//                genreCount[genre] = genreCount.GetValueOrDefault(genre, 0) + 1;
//            }
//            if (entry.Contains("Автор="))
//            {
//                string author = entry.Split("Автор=")[1].Split(';')[0].Trim();
//                authorCount[author] = authorCount.GetValueOrDefault(author, 0) + 1;
//            }
//        }

//        string recGenre = genreCount.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;
//        string recAuthor = authorCount.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

//        DataView recView = new DataView(_library);
//        string recFilter = "";
//        if (!string.IsNullOrEmpty(recGenre))
//            recFilter += $"Genre = '{recGenre}'";
//        if (!string.IsNullOrEmpty(recAuthor))
//            recFilter += string.IsNullOrEmpty(recFilter) ? "" : " OR " + $"Author LIKE '%{recAuthor}%'";

//        recView.RowFilter = recFilter;
//        recView.Sort = "Rating DESC";

//        Console.WriteLine("\nРекомендации на основе истории:");
//        if (!string.IsNullOrEmpty(recGenre)) Console.WriteLine($"Частый жанр: {recGenre}");
//        if (!string.IsNullOrEmpty(recAuthor)) Console.WriteLine($"Частый автор: {recAuthor}");
//        ПоказатьРезультаты(recView, 5);
//    }

//    private void ПоказатьРезультаты(DataView results, int max = int.MaxValue)
//    {
//        int count = Math.Min(max, results.Count);
//        Console.WriteLine($"{"ID",-4} {"Название",-30} {"Автор",-20} {"Год",-5} {"ISBN",-15} {"Жанр",-15} {"Рейтинг",-8} {"Страниц",-7}");
//        Console.WriteLine(new string('-', 110));

//        for (int i = 0; i < count; i++)
//        {
//            DataRowView row = results[i];
//            Console.WriteLine(
//                $"{row["BookID"],-4} " +
//                $"{row["Title"].ToString().Truncate(29),-30} " +
//                $"{row["Author"].ToString().Truncate(19),-20} " +
//                $"{row["Year"],-5} " +
//                $"{row["ISBN"],-15} " +
//                $"{row["Genre"],-15} " +
//                $"{row["Rating"],-8:F1} " +
//                $"{row["Pages"],-7}"
//            );
//        }

//        if (results.Count > max) Console.WriteLine($"... и ещё {results.Count - max} книг");
//        Console.WriteLine();
//    }
//}

//static class StringExtensions
//{
//    public static string Truncate(this string value, int maxLength)
//    {
//        if (string.IsNullOrEmpty(value)) return value;
//        return value.Length <= maxLength ? value : value.Substring(0, maxLength - 1) + "…";
//    }
//}

//29
//using System;
//using System.Data;
//using System.Windows.Forms;
//using System.IO;
//using System.Collections.Generic;

//namespace EmployeeApp
//{
//    public class MainForm : Form
//    {
//        private DataTable employees = new DataTable("Сотрудники");
//        private List<string> accessLogs = new List<string>();
//        private List<string> changeLogs = new List<string>();
//        private DataGridView dgv;
//        private ComboBox cmbRole;
//        private TextBox txtDepartment;
//        private Button btnReport, btnExport;

//        public MainForm()
//        {
//            InitializeData();
//            InitializeUI();
//            employees.TableNewRow += (s, e) => LogChange("Добавлена новая строка");
//            employees.RowChanged += (s, e) => LogChange($"Изменена строка ID={e.Row["EmployeeID"]}, Действие={e.Action}");
//            employees.RowDeleted += (s, e) => LogChange($"Удалена строка ID={e.Row["EmployeeID"]}, Действие={e.Action}");
//        }

//        private void InitializeData()
//        {
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("Name", typeof(string));
//            employees.Columns.Add("Department", typeof(string));
//            employees.Columns.Add("Salary", typeof(double));
//            employees.Columns.Add("HireDate", typeof(DateTime));
//            employees.Columns.Add("Status", typeof(string));

//            employees.PrimaryKey = new[] { employees.Columns["EmployeeID"] };

//            Random rnd = new Random();
//            string[] departments = { "HR", "IT", "Finance", "Sales" };
//            string[] statuses = { "Active", "Inactive" };
//            DateTime baseDate = new DateTime(2010, 1, 1);

//            for (int i = 1; i <= 100; i++)
//            {
//                employees.Rows.Add(i, $"Employee {i}", departments[rnd.Next(departments.Length)], rnd.Next(3000, 10000), baseDate.AddDays(rnd.Next(5000)), statuses[rnd.Next(statuses.Length)]);
//            }
//        }

//        private void InitializeUI()
//        {
//            this.Text = "Система просмотра сотрудников";
//            this.Size = new System.Drawing.Size(800, 600);

//            Label lblRole = new Label { Text = "Роль:", Left = 20, Top = 20 };
//            cmbRole = new ComboBox { Left = 80, Top = 17, Width = 150 };
//            cmbRole.Items.AddRange(new[] { "HR Manager", "Department Head", "Finance", "General" });
//            cmbRole.SelectedIndexChanged += UpdateView;

//            Label lblDept = new Label { Text = "Отдел (для Head):", Left = 250, Top = 20 };
//            txtDepartment = new TextBox { Left = 380, Top = 17, Width = 150 };

//            dgv = new DataGridView { Left = 20, Top = 60, Width = 750, Height = 450, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

//            btnReport = new Button { Text = "Отчёт о доступе", Left = 20, Top = 520, Width = 150 };
//            btnReport.Click += (s, e) => ShowReport();

//            btnExport = new Button { Text = "Экспорт в CSV", Left = 180, Top = 520, Width = 150 };
//            btnExport.Click += (s, e) => ExportToCsv();

//            this.Controls.AddRange(new Control[] { lblRole, cmbRole, lblDept, txtDepartment, dgv, btnReport, btnExport });
//        }

//        private void UpdateView(object sender, EventArgs e)
//        {
//            string role = cmbRole.SelectedItem?.ToString();
//            if (string.IsNullOrEmpty(role)) return;

//            LogAccess(role);

//            DataView view = new DataView(employees);
//            bool canEdit = false;

//            switch (role)
//            {
//                case "HR Manager":
//                    view.Sort = "Salary DESC";
//                    canEdit = true;
//                    break;
//                case "Department Head":
//                    string dept = txtDepartment.Text.Trim();
//                    if (string.IsNullOrEmpty(dept))
//                    {
//                        MessageBox.Show("Введите отдел");
//                        return;
//                    }
//                    view.RowFilter = $"Department = '{dept.Replace("'", "''")}'";
//                    canEdit = true;
//                    break;
//                case "Finance":
//                    view.RowFilter = "Status = 'Active' AND Salary > 5000";
//                    break;
//                case "General":
//                    view = new DataView(employees);
//                    dgv.DataSource = view;
//                    dgv.Columns["EmployeeID"].Visible = false;
//                    dgv.Columns["Salary"].Visible = false;
//                    dgv.Columns["HireDate"].Visible = false;
//                    dgv.Columns["Status"].Visible = false;
//                    return; 
//            }

//            dgv.DataSource = view;
//            dgv.ReadOnly = !canEdit;
//        }

//        private void LogAccess(string role)
//        {
//            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Роль: {role} просмотрел данные (строк: {dgv.RowCount})";
//            accessLogs.Add(logEntry);
//        }

//        private void LogChange(string message)
//        {
//            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Изменение: {message}";
//            changeLogs.Add(logEntry);
//        }

//        private void ShowReport()
//        {
//            string report = "Отчёт о доступе:\n" + string.Join("\n", accessLogs) +
//                            "\n\nАудит изменений:\n" + string.Join("\n", changeLogs);
//            MessageBox.Show(report);
//        }

//        private void ExportToCsv()
//        {
//            if (dgv.DataSource is not DataView view) return;

//            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV|*.csv" })
//            {
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
//                    {
//                        foreach (DataGridViewColumn col in dgv.Columns)
//                        {
//                            if (col.Visible) sw.Write(col.HeaderText + ",");
//                        }
//                        sw.WriteLine();

//                        foreach (DataRowView row in view)
//                        {
//                            foreach (DataGridViewColumn col in dgv.Columns)
//                            {
//                                if (col.Visible) sw.Write(row[col.DataPropertyName] + ",");
//                            }
//                            sw.WriteLine();
//                        }
//                    }
//                    MessageBox.Show("Экспорт завершён");
//                }
//            }
//        }
//    }

//    static void Main()
//        {
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new MainForm());
//        }
//    }