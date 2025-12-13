////1
//using System;
//using System.Data;

//namespace ConsoleApp19
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            var storeDataSet = new DataSet("StoreDataSet");

//            var categoriesTable = new DataTable("Категории");

//            var categoryIdColumn = new DataColumn("CategoryID", typeof(int))
//            {
//                AllowDBNull = false,
//                Unique = true,
//                AutoIncrement = true,
//                AutoIncrementSeed = 1,
//                AutoIncrementStep = 1
//            };

//            var categoryNameColumn = new DataColumn("CategoryName", typeof(string))
//            {
//                AllowDBNull = false,
//                MaxLength = 100
//            };

//            var descriptionColumn = new DataColumn("Description", typeof(string))
//            {
//                AllowDBNull = true,
//                MaxLength = 500
//            };

//            categoriesTable.Columns.Add(categoryIdColumn);
//            categoriesTable.Columns.Add(categoryNameColumn);
//            categoriesTable.Columns.Add(descriptionColumn);
//            categoriesTable.PrimaryKey = new DataColumn[] { categoryIdColumn };

//            categoriesTable.Rows.Add(null, "Напитки", "Безалкогольные и алкогольные напитки");
//            categoriesTable.Rows.Add(null, "Снеки", "Чипсы, орешки, сухарики");
//            categoriesTable.Rows.Add(null, "Молочные продукты", "Молоко, сыр, йогурты и т.д.");

//            storeDataSet.Tables.Add(categoriesTable);

//            var productsTable = new DataTable("Товары");

//            var productIdColumn = new DataColumn("ProductID", typeof(int))
//            {
//                AllowDBNull = false,
//                Unique = true,
//                AutoIncrement = true,
//                AutoIncrementSeed = 1,
//                AutoIncrementStep = 1
//            };

//            var productNameColumn = new DataColumn("ProductName", typeof(string))
//            {
//                AllowDBNull = false,
//                MaxLength = 150
//            };

//            var priceColumn = new DataColumn("Price", typeof(decimal))
//            {
//                AllowDBNull = false
//            };

//            var productCategoryIdColumn = new DataColumn("CategoryID", typeof(int))
//            {
//                AllowDBNull = false
//            };

//            productsTable.Columns.Add(productIdColumn);
//            productsTable.Columns.Add(productNameColumn);
//            productsTable.Columns.Add(priceColumn);
//            productsTable.Columns.Add(productCategoryIdColumn);
//            productsTable.PrimaryKey = new DataColumn[] { productIdColumn };

//            productsTable.Rows.Add(null, "Кола 1л", 89.90m, 1);
//            productsTable.Rows.Add(null, "Спрайт 0.5л", 65.50m, 1);
//            productsTable.Rows.Add(null, "Пиво светлое", 120.00m, 1);
//            productsTable.Rows.Add(null, "Чипсы Lays сыр", 89.00m, 2);
//            productsTable.Rows.Add(null, "Орешки солёные", 150.00m, 2);
//            productsTable.Rows.Add(null, "Сухарики Кириешки", 45.90m, 2);
//            productsTable.Rows.Add(null, "Молоко 3.2%", 78.50m, 3);
//            productsTable.Rows.Add(null, "Сыр Российский", 450.00m, 3);
//            productsTable.Rows.Add(null, "Йогурт Danone", 55.00m, 3);
//            productsTable.Rows.Add(null, "Кефир 1%", 72.30m, 3);

//            storeDataSet.Tables.Add(productsTable);

//            DataRelation? relation = null;

//            try
//            {
//                relation = new DataRelation(
//                    "Cat_Prod_Rel",
//                    categoryIdColumn,
//                    productCategoryIdColumn,
//                    true);

//                storeDataSet.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка при создании отношения:");
//                Console.WriteLine(ex.Message);
//                return;
//            }

//            Console.WriteLine("=== Информация об отношении ===");
//            Console.WriteLine($"Имя: {relation.RelationName}");
//            Console.WriteLine($"Родительская таблица: {relation.ParentTable.TableName}");
//            Console.WriteLine($"Дочерняя таблица: {relation.ChildTable.TableName}");
//            Console.WriteLine($"Родительская колонка: {relation.ParentColumns[0].ColumnName}");
//            Console.WriteLine($"Дочерняя колонка: {relation.ChildColumns[0].ColumnName}");
//            Console.WriteLine();

//            Console.WriteLine("=== Иерархия: Категории → Товары ===");
//            Console.WriteLine(new string('=', 80));

//            foreach (DataRow categoryRow in categoriesTable.Rows)
//            {
//                int catId = (int)categoryRow["CategoryID"];
//                string catName = (string)categoryRow["CategoryName"];
//                string desc = categoryRow["Description"] == DBNull.Value ? "(нет описания)" : (string)categoryRow["Description"];

//                Console.WriteLine($"[{catId}] {catName}");
//                Console.WriteLine($"    Описание: {desc}");

//                DataRow[] childRows = categoryRow.GetChildRows(relation);

//                if (childRows.Length == 0)
//                {
//                    Console.WriteLine("        → Нет товаров");
//                }
//                else
//                {
//                    Console.WriteLine("        Товары:");
//                    foreach (DataRow productRow in childRows)
//                    {
//                        int prodId = (int)productRow["ProductID"];
//                        string prodName = (string)productRow["ProductName"];
//                        decimal price = (decimal)productRow["Price"];

//                        Console.WriteLine($"            • [{prodId}] {prodName} — {price:C}");
//                    }
//                }
//                Console.WriteLine(new string('-', 80));
//            }

//            Console.WriteLine();
//            Console.WriteLine("=== Демонстрация ошибки при несовпадении типов ===");
//            try
//            {
//                var badTable = new DataTable("Bad");
//                badTable.Columns.Add("CategoryID", typeof(string));
//                badTable.Rows.Add("1");

//                var badRelation = new DataRelation("BadRel",
//                    categoryIdColumn,
//                    badTable.Columns["CategoryID"]!,
//                    true);

//                storeDataSet.Relations.Add(badRelation);
//            }
//            catch (ArgumentException aex)
//            {
//                Console.WriteLine("Ожидаемая ошибка (типы не совпадают):");
//                Console.WriteLine(aex.Message);
//            }

//            Console.WriteLine();
//            Console.WriteLine("Готово. Нажмите любую клавишу...");
//            Console.ReadKey();
//        }
//    }
//}


////2
//using System;
//using System.Data;
//using System.Linq;

//namespace DataRelationTask2
//{
//    internal class Program
//    {
//        static DataSet storeDataSet = new DataSet("Store");
//        static DataRelation relation;

//        static void Main(string[] args)
//        {
//            InitializeDataSet();
//            CreateRelation();

//            Console.WriteLine("=".PadRight(80, '='));
//            Console.WriteLine("           ПРАКТИЧЕСКОЕ ЗАДАНИЕ 2 — GetChildRows() в ADO.NET");
//            Console.WriteLine("=".PadRight(80, '='));
//            Console.WriteLine();

//            DisplayAllCategoriesWithProducts();
//            Console.WriteLine();

//            CountProductsInEachCategory();
//            Console.WriteLine();

//            CalculateTotalValuePerCategory();
//            Console.WriteLine();

//            SearchProductsInCategory(1);
//            Console.WriteLine();

//            SearchProductsInCategory(2);
//            Console.WriteLine();

//            DisplayCategoriesWithExpensiveProducts(100.00m);
//            Console.WriteLine();

//            DisplayCategoriesWithExpensiveProducts(200.00m);

//            Console.WriteLine("Нажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static void InitializeDataSet()
//        {
//            var categories = new DataTable("Категории");
//            var catId = new DataColumn("CategoryID", typeof(int)) { AutoIncrement = true, AutoIncrementSeed = 1, Unique = true };
//            var catName = new DataColumn("CategoryName", typeof(string)) { AllowDBNull = false };
//            var desc = new DataColumn("Description", typeof(string)) { AllowDBNull = true };

//            categories.Columns.AddRange(new[] { catId, catName, desc });
//            categories.PrimaryKey = new[] { catId };

//            categories.Rows.Add(null, "Напитки", "Безалкогольные и алкогольные напитки");
//            categories.Rows.Add(null, "Снеки", "Чипсы, сухарики, орешки");
//            categories.Rows.Add(null, "Молочные продукты", "Молоко, сыр, йогурты");
//            categories.Rows.Add(null, "Замороженные продукты", "Пельмени, мороженое и т.д.");

//            var products = new DataTable("Товары");
//            var prodId = new DataColumn("ProductID", typeof(int)) { AutoIncrement = true, AutoIncrementSeed = 1, Unique = true };
//            var prodName = new DataColumn("ProductName", typeof(string)) { AllowDBNull = false };
//            var price = new DataColumn("Price", typeof(decimal)) { AllowDBNull = false };
//            var quantity = new DataColumn("Quantity", typeof(int)) { DefaultValue = 1 };
//            var catIdFk = new DataColumn("CategoryID", typeof(int)) { AllowDBNull = false };

//            products.Columns.AddRange(new[] { prodId, prodName, price, quantity, catIdFk });
//            products.PrimaryKey = new[] { prodId };

//            products.Rows.Add(null, "Кола 1л", 89.90m, 10, 1);
//            products.Rows.Add(null, "Спрайт 0.5л", 65.50m, 15, 1);
//            products.Rows.Add(null, "Пиво светлое", 120.00m, 8, 1);
//            products.Rows.Add(null, "Чипсы Lays", 89.00m, 20, 2);
//            products.Rows.Add(null, "Орешки", 150.00m, 12, 2);
//            products.Rows.Add(null, "Сухарики", 45.90m, 30, 2);
//            products.Rows.Add(null, "Молоко 3.2%", 78.50m, 25, 3);
//            products.Rows.Add(null, "Сыр Российский", 450.00m, 5, 3);
//            products.Rows.Add(null, "Йогурт", 55.00m, 40, 3);
//            products.Rows.Add(null, "Кефир", 72.30m, 18, 3);
//            products.Rows.Add(null, "Пельмени", 320.00m, 7, 4);

//            storeDataSet.Tables.Add(categories);
//            storeDataSet.Tables.Add(products);
//        }

//        static void CreateRelation()
//        {
//            try
//            {
//                relation = new DataRelation("Cat_Prod",
//                    storeDataSet.Tables["Категории"].Columns["CategoryID"],
//                    storeDataSet.Tables["Товары"].Columns["CategoryID"],
//                    true);

//                storeDataSet.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Ошибка создания отношения: " + ex.Message);
//            }
//        }

//        static void DisplayAllCategoriesWithProducts()
//        {
//            Console.WriteLine("ИЕРАРХИЯ: КАТЕГОРИИ → ТОВАРЫ (через GetChildRows())");
//            Console.WriteLine(new string('-', 80));

//            foreach (DataRow catRow in storeDataSet.Tables["Категории"].Rows)
//            {
//                int catId = (int)catRow["CategoryID"];
//                string name = catRow["CategoryName"].ToString();
//                string desc = catRow["Description"] == DBNull.Value ? "" : catRow["Description"].ToString();

//                Console.WriteLine($"[{catId}] {name}");
//                if (!string.IsNullOrEmpty(desc))
//                    Console.WriteLine($"    Описание: {desc}");

//                DataRow[] products = catRow.GetChildRows(relation);

//                if (products.Length == 0)
//                {
//                    Console.WriteLine("        → Нет товаров в этой категории");
//                }
//                else
//                {
//                    Console.WriteLine($"        Товаров: {products.Length}");
//                    foreach (DataRow prod in products)
//                    {
//                        int id = (int)prod["ProductID"];
//                        string pName = prod["ProductName"].ToString();
//                        decimal price = (decimal)prod["Price"];
//                        int qty = (int)prod["Quantity"];

//                        Console.WriteLine($"            • [{id,2}] {pName,-25} | Цена: {price,6:C} | Кол-во: {qty,2} шт.");
//                    }
//                }
//                Console.WriteLine(new string('-', 80));
//            }
//        }

//        static void CountProductsInEachCategory()
//        {
//            Console.WriteLine("КОЛИЧЕСТВО ТОВАРОВ В КАЖДОЙ КАТЕГОРИИ");
//            Console.WriteLine(new string('-', 50));
//            Console.WriteLine($"{"Категория",-30} {"Кол-во товаров",10}");
//            Console.WriteLine(new string('-', 50));

//            foreach (DataRow cat in storeDataSet.Tables["Категории"].Rows)
//            {
//                string name = cat["CategoryName"].ToString();
//                int count = cat.GetChildRows(relation).Length;
//                Console.WriteLine($"{name,-30} {count,10}");
//            }
//        }

//        static void CalculateTotalValuePerCategory()
//        {
//            Console.WriteLine("ОБЩАЯ СТОИМОСТЬ ТОВАРОВ ПО КАТЕГОРИЯМ (Price × Quantity)");
//            Console.WriteLine(new string('-', 60));
//            Console.WriteLine($"{"Категория",-30} {"Общая стоимость",15}");
//            Console.WriteLine(new string('-', 60));

//            foreach (DataRow cat in storeDataSet.Tables["Категории"].Rows)
//            {
//                string name = cat["CategoryName"].ToString();
//                DataRow[] prods = cat.GetChildRows(relation);

//                decimal total = prods.Sum(p => (decimal)p["Price"] * (int)p["Quantity"]);

//                Console.WriteLine($"{name,-30} {total,15:C}");
//            }
//        }

//        static void SearchProductsInCategory(int categoryId)
//        {
//            DataRow[] cats = storeDataSet.Tables["Категории"].Select($"CategoryID = {categoryId}");
//            if (cats.Length == 0)
//            {
//                Console.WriteLine($"Категория с ID {categoryId} не найдена.");
//                return;
//            }

//            DataRow cat = cats[0];
//            string catName = cat["CategoryName"].ToString();

//            Console.WriteLine($"ПОИСК ТОВАРОВ В КАТЕГОРИИ: {catName} (ID={categoryId})");
//            Console.WriteLine(new string('-', 70));

//            DataRow[] products = cat.GetChildRows(relation);

//            if (products.Length == 0)
//            {
//                Console.WriteLine("    → В этой категории нет товаров.");
//            }
//            else
//            {
//                foreach (DataRow p in products)
//                {
//                    Console.WriteLine($"    • {p["ProductName"]} — {p["Price"]:C} × {p["Quantity"]} = {(decimal)p["Price"] * (int)p["Quantity"]:C}");
//                }
//            }
//            Console.WriteLine();
//        }

//        static void DisplayCategoriesWithExpensiveProducts(decimal minPrice)
//        {
//            Console.WriteLine($"КАТЕГОРИИ, СОДЕРЖАЩИЕ ТОВАРЫ ДОРОЖЕ {minPrice:C}");
//            Console.WriteLine(new string('-', 70));

//            bool foundAny = false;

//            foreach (DataRow cat in storeDataSet.Tables["Категории"].Rows)
//            {
//                DataRow[] expensive = cat.GetChildRows(relation)
//                    .Where(p => (decimal)p["Price"] > minPrice)
//                    .ToArray();

//                if (expensive.Length > 0)
//                {
//                    foundAny = true;
//                    Console.WriteLine($"[{cat["CategoryID"]}] {cat["CategoryName"]}");
//                    foreach (DataRow p in expensive)
//                    {
//                        Console.WriteLine($"        → {p["ProductName"]} — {p["Price"]:C}");
//                    }
//                }
//            }

//            if (!foundAny)
//            {
//                Console.WriteLine($"    Нет категорий с товарами дороже {minPrice:C}");
//            }
//            Console.WriteLine();
//        }
//    }
////}
////3
//using System;
//using System.Data;
//using System.Linq;

//namespace DataRelationTask3_Final
//{
//    internal class Program
//    {
//        private static DataSet storeDataSet = new DataSet("StoreDB");
//        private static DataRelation relation;

//        static void Main(string[] args)
//        {
//            InitializeDataWithOrphansAndViolations();
//            CreateRelationSafely();

//            DisplayAllProductsWithTheirCategories();
//            Console.WriteLine();

//            SearchProductByIdAndShowCategory(1);
//            SearchProductByIdAndShowCategory(11);
//            SearchProductByIdAndShowCategory(12);
//            SearchProductByIdAndShowCategory(999);
//            Console.WriteLine();

//            GenerateFullReport_Product_Category_Description();
//            Console.WriteLine();

//            DemonstrateDataRowVersionDifferences();
//            Console.WriteLine();

//            DetectOrphanedRecordsAndIntegrityViolations();

//            Console.ReadKey();
//        }

//        static void InitializeDataWithOrphansAndViolations()
//        {
//            var categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Description", typeof(string));
//            categories.PrimaryKey = new[] { categories.Columns["CategoryID"] };

//            categories.Rows.Add(1, "Напитки", "Газированные напитки, соки, вода");
//            categories.Rows.Add(2, "Снеки", "Чипсы, сухарики, орешки");
//            categories.Rows.Add(3, "Молочка", "Молоко, сыр, творог, йогурты");

//            var products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("CategoryID", typeof(int));

//            products.PrimaryKey = new[] { products.Columns["ProductID"] };

//            products.Rows.Add(1, "Кола 1л", 89.90m, 1);
//            products.Rows.Add(2, "Спрайт 0.5л", 65.50m, 1);
//            products.Rows.Add(3, "Чипсы Lays", 95.00m, 2);
//            products.Rows.Add(4, "Сыр Российский", 520.00m, 3);
//            products.Rows.Add(11, "Пельмени (нет категории)", 380.00m, 99);
//            products.Rows.Add(12, "Мороженое (NULL)", 180.00m, DBNull.Value);

//            storeDataSet.Tables.Add(categories);
//            storeDataSet.Tables.Add(products);
//        }

//        static void CreateRelationSafely()
//        {
//            relation = new DataRelation("Cat_Prod",
//                storeDataSet.Tables["Категории"].Columns["CategoryID"],
//                storeDataSet.Tables["Товары"].Columns["CategoryID"],
//                false);

//            storeDataSet.Relations.Add(relation);
//        }

//        static void DisplayAllProductsWithTheirCategories()
//        {
//            Console.WriteLine("ТОВАРЫ → КАТЕГОРИЯ (GetParentRows())");
//            Console.WriteLine(new string('-', 90));

//            foreach (DataRow product in storeDataSet.Tables["Товары"].Rows)
//            {
//                int id = (int)product["ProductID"];
//                string name = product["ProductName"].ToString();
//                DataRow[] parents = product.GetParentRows(relation);

//                Console.Write($"[{id,2}] {name,-40}");

//                if (parents.Length > 0)
//                    Console.WriteLine($"→ [{parents[0]["CategoryID"]}] {parents[0]["CategoryName"]}");
//                else
//                    Console.WriteLine("→ НЕТ РОДИТЕЛЯ (сирота / нарушение FK)");
//            }
//            Console.WriteLine();
//        }

//        static void SearchProductByIdAndShowCategory(int productId)
//        {
//            var found = storeDataSet.Tables["Товары"].Select($"ProductID = {productId}");

//            Console.WriteLine($"ПОИСК ТОВАРА ID = {productId}");
//            Console.WriteLine(new string('-', 70));

//            if (found.Length == 0)
//            {
//                Console.WriteLine("    → Товар не найден.");
//                return;
//            }

//            DataRow p = found[0];
//            DataRow[] parents = p.GetParentRows(relation);

//            Console.WriteLine($"    Товар: {p["ProductName"]} | Цена: {p["Price"]:C}");

//            if (parents.Length > 0)
//                Console.WriteLine($"    Категория: {parents[0]["CategoryName"]} (ID: {parents[0]["CategoryID"]})");
//            else
//                Console.WriteLine("    Категория: ОТСУТСТВУЕТ (нарушение ссылочной целостности)");
//            Console.WriteLine();
//        }

//        static void GenerateFullReport_Product_Category_Description()
//        {
//            Console.WriteLine("ПОЛНЫЙ ОТЧЁТ: ТОВАР → КАТЕГОРИЯ → ОПИСАНИЕ");
//            Console.WriteLine(new string('=', 90));

//            foreach (DataRow p in storeDataSet.Tables["Товары"].Rows)
//            {
//                string name = p["ProductName"].ToString();
//                int id = (int)p["ProductID"];
//                DataRow[] parents = p.GetParentRows(relation);

//                Console.Write($"[{id,2}] {name,-35}");

//                if (parents.Length > 0)
//                {
//                    var cat = parents[0];
//                    string desc = cat["Description"] == DBNull.Value ? "(без описания)" : cat["Description"].ToString();
//                    Console.WriteLine($"→ {cat["CategoryName"]} → {desc}");
//                }
//                else
//                {
//                    Console.WriteLine("→ [НЕТ КАТЕГОРИИ] → нарушение ссылочной целостности");
//                }
//            }
//            Console.WriteLine();
//        }

//        static void DemonstrateDataRowVersionDifferences()
//        {
//            Console.WriteLine("ДЕМОНСТРАЦИЯ GetParentRows() с DataRowVersion");
//            Console.WriteLine(new string('-', 80));

//            DataRow test = storeDataSet.Tables["Товары"].Rows[0];
//            Console.WriteLine($"Тестовый товар: {test["ProductName"]} (CategoryID = {test["CategoryID"]})");

//            test.BeginEdit();
//            test["CategoryID"] = 2;
//            test.EndEdit();

//            var current = test.GetParentRows(relation, DataRowVersion.Current);
//            var original = test.GetParentRows(relation, DataRowVersion.Default);

//            Console.WriteLine($"После изменения CategoryID = 2:");
//            Console.WriteLine($"  Текущая версия:  {(current.Length > 0 ? current[0]["CategoryName"].ToString() : "НЕТ")}");
//            Console.WriteLine($"  Предыдущая версия: {(original.Length > 0 ? original[0]["CategoryName"].ToString() : "НЕТ")}");

//            test.CancelEdit();
//            Console.WriteLine("Изменение отменено (CancelEdit).");
//            Console.WriteLine();
//        }

//        static void DetectOrphanedRecordsAndIntegrityViolations()
//        {
//            Console.WriteLine("АНАЛИЗ НАРУШЕНИЙ ССЫЛОЧНОЙ ЦЕЛОСТНОСТИ");
//            Console.WriteLine(new string('=', 90));

//            var orphans = storeDataSet.Tables["Товары"].AsEnumerable()
//                .Where(p => p["CategoryID"] != DBNull.Value &&
//                            !storeDataSet.Tables["Категории"].AsEnumerable()
//                                .Any(c => c.Field<int>("CategoryID") == p.Field<int>("CategoryID")))
//                .ToList();

//            var nulls = storeDataSet.Tables["Товары"].AsEnumerable()
//                .Where(p => p["CategoryID"] == DBNull.Value)
//                .ToList();

//            Console.WriteLine($"Товаров с несуществующим CategoryID: {orphans.Count}");
//            Console.WriteLine($"Товаров с NULL в CategoryID:        {nulls.Count}");

//            if (orphans.Count + nulls.Count == 0)
//                Console.WriteLine("Нарушений не обнаружено.");
//            Console.WriteLine();
//        }
//    }
//}

//4
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace HierarchicalEmployeesTask
//{
//    internal class Program
//    {
//        private static DataSet companyDataSet = new DataSet("Company");
//        private static DataTable employees;
//        private static DataRelation managerRelation;

//        static void Main(string[] args)
//        {
//            CreateEmployeesTable();
//            FillHierarchicalData();
//            CreateSelfReferencingRelation();

//            Console.WriteLine("ИЕРАРХИЯ СОТРУДНИКОВ (рекурсивный вывод)");
//            Console.WriteLine(new string('=', 80));
//            DisplayHierarchyRecursive(FindRootEmployee(), 0);
//            Console.WriteLine();

//            Console.WriteLine("ПОДЧИНЁННЫЕ КАЖДОГО МЕНЕДЖЕРА (GetChildRows)");
//            Console.WriteLine(new string('-', 80));
//            DisplayAllSubordinates();
//            Console.WriteLine();

//            Console.WriteLine("РУКОВОДИТЕЛЬ КАЖДОГО СОТРУДНИКА (GetParentRows)");
//            Console.WriteLine(new string('-', 80));
//            DisplayAllManagers();
//            Console.WriteLine();

//            Console.WriteLine("ГЛУБИНА ИЕРАРХИИ КАЖДОГО СОТРУДНИКА");
//            Console.WriteLine(new string('-', 80));
//            DisplayEmployeeLevels();

//            Console.ReadKey();
//        }

//        static void CreateEmployeesTable()
//        {
//            employees = new DataTable("Сотрудники");

//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("Department", typeof(string));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.Columns.Add("ManagerID", typeof(int));
//            employees.Columns["ManagerID"].AllowDBNull = true;

//            employees.PrimaryKey = new[] { employees.Columns["EmployeeID"] };

//            companyDataSet.Tables.Add(employees);
//        }

//        static void FillHierarchicalData()
//        {
//            employees.Rows.Add(1, "Иванов Иван Иванович", "Генеральный директор", 500000m, DBNull.Value);
//            employees.Rows.Add(2, "Петров Пётр Петрович", "Финансовый отдел", 300000m, 1);
//            employees.Rows.Add(3, "Сидорова Анна Сергеевна", "IT-отдел", 280000m, 1);
//            employees.Rows.Add(4, "Козлов Алексей Викторович", "Продажи", 250000m, 1);

//            employees.Rows.Add(5, "Васильев Дмитрий Олегович", "Финансовый отдел", 180000m, 2);
//            employees.Rows.Add(6, "Морозова Екатерина Андреевна", "Финансовый отдел", 170000m, 2);

//            employees.Rows.Add(7, "Новиков Сергей Михайлович", "IT-отдел", 220000m, 3);
//            employees.Rows.Add(8, "Фёдорова Ольга Владимировна", "IT-отдел", 210000m, 3);
//            employees.Rows.Add(9, "Лебедев Артём Павлович", "IT-отдел", 190000m, 7);

//            employees.Rows.Add(10, "Смирнова Мария Николаевна", "Продажи", 200000m, 4);
//            employees.Rows.Add(11, "Кузнецов Роман Игоревич", "Продажи", 195000m, 4);

//            employees.Rows.Add(99, "ЦИКЛИЧЕСКАЯ ССЫЛКА (сам на себя)", "Тест", 1000m, 99);
//        }

//        static void CreateSelfReferencingRelation()
//        {
//            managerRelation = new DataRelation("Manager_Employees",
//                employees.Columns["EmployeeID"],
//                employees.Columns["ManagerID"],
//                false);

//            companyDataSet.Relations.Add(managerRelation);
//        }

//        static DataRow FindRootEmployee()
//        {
//            foreach (DataRow row in employees.Rows)
//            {
//                if (row["ManagerID"] == DBNull.Value)
//                    return row;
//            }
//            return null;
//        }

//        static void DisplayHierarchyRecursive(DataRow employee, int level)
//        {
//            if (employee == null) return;

//            string indent = new string(' ', level * 4);
//            string name = employee["EmployeeName"].ToString();
//            string dept = employee["Department"].ToString();
//            decimal salary = (decimal)employee["Salary"];

//            Console.WriteLine($"{indent}├─ [{employee["EmployeeID"]}] {name}");
//            Console.WriteLine($"{indent}   └─ Отдел: {dept} | Зарплата: {salary:C}");

//            DataRow[] subordinates = employee.GetChildRows(managerRelation);

//            foreach (DataRow sub in subordinates)
//            {
//                int subId = (int)sub["EmployeeID"];
//                int managerId = sub["ManagerID"] == DBNull.Value ? -1 : (int)sub["ManagerID"];

//                if (managerId == subId)
//                {
//                    Console.WriteLine($"{indent}       [ПРЕДУПРЕЖДЕНИЕ] Циклическая ссылка: сотрудник {subId} указывает сам на себя!");
//                    continue;
//                }

//                DisplayHierarchyRecursive(sub, level + 1);
//            }
//        }

//        static void DisplayAllSubordinates()
//        {
//            foreach (DataRow emp in employees.Rows)
//            {
//                int id = (int)emp["EmployeeID"];
//                string name = emp["EmployeeName"].ToString();

//                DataRow[] subs = emp.GetChildRows(managerRelation);

//                Console.WriteLine($"[{id}] {name} → подчинённых: {subs.Length}");

//                foreach (DataRow sub in subs)
//                {
//                    Console.WriteLine($"    • [{sub["EmployeeID"]}] {sub["EmployeeName"]} ({sub["Department"]})");
//                }
//            }
//            Console.WriteLine();
//        }

//        static void DisplayAllManagers()
//        {
//            foreach (DataRow emp in employees.Rows)
//            {
//                int id = (int)emp["EmployeeID"];
//                string name = emp["EmployeeName"].ToString();

//                DataRow[] parents = emp.GetParentRows(managerRelation);

//                if (parents.Length > 0)
//                {
//                    DataRow manager = parents[0];
//                    Console.WriteLine($"[{id}] {name} → руководитель: [{manager["EmployeeID"]}] {manager["EmployeeName"]}");
//                }
//                else
//                {
//                    Console.WriteLine($"[{id}] {name} → руководитель: НЕТ (глава компании)");
//                }
//            }
//            Console.WriteLine();
//        }

//        static void DisplayEmployeeLevels()
//        {
//            var visited = new HashSet<int>();
//            var levelMap = new Dictionary<int, int>();

//            DataRow root = FindRootEmployee();
//            if (root != null)
//            {
//                CalculateLevel(root, 0, visited, levelMap);
//            }

//            foreach (DataRow emp in employees.Rows)
//            {
//                int id = (int)emp["EmployeeID"];
//                string name = emp["EmployeeName"].ToString();

//                if (levelMap.TryGetValue(id, out int level))
//                {
//                    Console.WriteLine($"[{id,2}] {name,-35} → Уровень в иерархии: {level}");
//                }
//                else
//                {
//                    if ((int?)emp["ManagerID"] == id)
//                        Console.WriteLine($"[{id,2}] {name,-35} → ЦИКЛ (сам на себя)");
//                    else
//                        Console.WriteLine($"[{id,2}] {name,-35} → НЕ В ИЕРАРХИИ (сирота или цикл)");
//                }
//            }
//        }

//        static void CalculateLevel(DataRow employee, int currentLevel, HashSet<int> visited, Dictionary<int, int> levelMap)
//        {
//            int id = (int)employee["EmployeeID"];

//            if (visited.Contains(id))
//            {
//                Console.WriteLine($"[ОБНАРУЖЕН ЦИКЛ] при обработке сотрудника {id}");
//                return;
//            }

//            visited.Add(id);
//            levelMap[id] = currentLevel;

//            foreach (DataRow sub in employee.GetChildRows(managerRelation))
//            {
//                CalculateLevel(sub, currentLevel + 1, visited, levelMap);
//            }
//        }
//    }
//}
////5
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;

//namespace HierarchicalEmployeesTask5
//{
//    internal class Program
//    {
//        private static DataSet companyDataSet = new DataSet("Company");
//        private static DataTable employees;
//        private static DataRelation managerRelation;

//        static void Main(string[] args)
//        {
//            CreateEmployeesTable();
//            FillHierarchicalData();
//            CreateSelfReferencingRelation();

//            Console.WriteLine("ЗАДАНИЕ 5 — РАСШИРЕННАЯ РАБОТА С ИЕРАРХИЕЙ (САМ К СЕБЕ)\n");

//            Console.WriteLine("1. ПРЯМЫЕ ПОДЧИНЁННЫЕ МЕНЕДЖЕРА (ID = 1)");
//            DisplayDirectSubordinates(1);
//            Console.WriteLine();

//            Console.WriteLine("2. ЦЕПОЧКА РУКОВОДСТВА ДЛЯ СОТРУДНИКА (ID = 9)");
//            DisplayManagementChain(9);
//            Console.WriteLine();

//            Console.WriteLine("3. ВСЕ СОТРУДНИКИ НА УРОВНЕ 2 (руководители отделов)");
//            DisplayEmployeesByLevel(2);
//            Console.WriteLine();

//            Console.WriteLine("4. СТАТИСТИКА ПО ИЕРАРХИИ");
//            DisplayHierarchyStatistics();
//            Console.WriteLine();

//            Console.WriteLine("5. КОЛЛЕГИ СОТРУДНИКА (ID = 7) — один руководитель");
//            DisplayColleagues(7);
//            Console.WriteLine();

//            Console.WriteLine("6. ВСЕ ВЫШЕСТОЯЩИЕ РУКОВОДИТЕЛИ ДЛЯ СОТРУДНИКА (ID = 9)");
//            DisplayAllManagersAbove(9);

//            Console.WriteLine("\nНажмите любую клавишу для завершения...");
//            Console.ReadKey();
//        }

//        static void CreateEmployeesTable()
//        {
//            employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("Department", typeof(string));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.Columns.Add("ManagerID", typeof(int));
//            employees.Columns["ManagerID"].AllowDBNull = true;
//            employees.PrimaryKey = new[] { employees.Columns["EmployeeID"] };
//            companyDataSet.Tables.Add(employees);
//        }

//        static void FillHierarchicalData()
//        {
//            employees.Rows.Add(1, "Иванов Иван Иванович", "Генеральный директор", 500000m, DBNull.Value);
//            employees.Rows.Add(2, "Петров Пётр Петрович", "Финансовый отдел", 300000m, 1);
//            employees.Rows.Add(3, "Сидорова Анна Сергеевна", "IT-отдел", 280000m, 1);
//            employees.Rows.Add(4, "Козлов Алексей Викторович", "Продажи", 250000m, 1);

//            employees.Rows.Add(5, "Васильев Дмитрий Олегович", "Финансовый отдел", 180000m, 2);
//            employees.Rows.Add(6, "Морозова Екатерина Андреевна", "Финансовый отдел", 170000m, 2);

//            employees.Rows.Add(7, "Новиков Сергей Михайлович", "IT-отдел", 220000m, 3);
//            employees.Rows.Add(8, "Фёдорова Ольга Владимировна", "IT-отдел", 210000m, 3);
//            employees.Rows.Add(9, "Лебедев Артём Павлович", "IT-отдел", 190000m, 7);

//            employees.Rows.Add(10, "Смирнова Мария Николаевна", "Продажи", 200000m, 4);
//            employees.Rows.Add(11, "Кузнецов Роман Игоревич", "Продажи", 195000m, 4);
//        }

//        static void CreateSelfReferencingRelation()
//        {
//            managerRelation = new DataRelation("Manager_Employees",
//                employees.Columns["EmployeeID"],
//                employees.Columns["ManagerID"],
//                false);
//            companyDataSet.Relations.Add(managerRelation);
//        }

//        static DataRow FindEmployeeById(int id)
//        {
//            var rows = employees.Select($"EmployeeID = {id}");
//            return rows.Length > 0 ? rows[0] : null;
//        }

//        static void DisplayDirectSubordinates(int managerId)
//        {
//            var manager = FindEmployeeById(managerId);
//            if (manager == null)
//            {
//                Console.WriteLine($"    → Менеджер с ID {managerId} не найден.");
//                return;
//            }

//            Console.WriteLine($"    Менеджер: {manager["EmployeeName"]} ({manager["Department"]})");
//            var subs = manager.GetChildRows(managerRelation);

//            if (subs.Length == 0)
//            {
//                Console.WriteLine("    → Нет прямых подчинённых.");
//                return;
//            }

//            foreach (DataRow sub in subs)
//            {
//                Console.WriteLine($"    • [{sub["EmployeeID"]}] {sub["EmployeeName"]} — {sub["Department"]}");
//            }
//        }

//        static void DisplayManagementChain(int employeeId)
//        {
//            var emp = FindEmployeeById(employeeId);
//            if (emp == null)
//            {
//                Console.WriteLine($"    → Сотрудник с ID {employeeId} не найден.");
//                return;
//            }

//            Console.WriteLine($"    Цепочка руководства для: {emp["EmployeeName"]}");

//            var chain = new List<DataRow>();
//            var current = emp;

//            while (current != null)
//            {
//                chain.Add(current);
//                var parents = current.GetParentRows(managerRelation);
//                current = parents.Length > 0 ? parents[0] : null;
//            }

//            chain.Reverse();

//            for (int i = 0; i < chain.Count; i++)
//            {
//                string arrow = i < chain.Count - 1 ? " ↑ " : " → ";
//                Console.WriteLine($"    {new string(' ', i * 4)}{arrow} [{chain[i]["EmployeeID"]}] {chain[i]["EmployeeName"]}");
//            }
//        }

//        static void DisplayEmployeesByLevel(int level)
//        {
//            var root = employees.Select("ManagerID IS NULL")[0];
//            var levelEmployees = GetEmployeesAtLevel(root, level, 0);

//            Console.WriteLine($"    Сотрудники на уровне {level}: {levelEmployees.Count}");

//            foreach (var emp in levelEmployees)
//            {
//                Console.WriteLine($"    • [{emp["EmployeeID"]}] {emp["EmployeeName"]} — {emp["Department"]}");
//            }

//            if (levelEmployees.Count == 0)
//                Console.WriteLine("    → Нет сотрудников на этом уровне.");
//        }

//        static List<DataRow> GetEmployeesAtLevel(DataRow node, int targetLevel, int currentLevel)
//        {
//            var result = new List<DataRow>();

//            if (currentLevel == targetLevel)
//            {
//                result.Add(node);
//                return result;
//            }

//            foreach (DataRow child in node.GetChildRows(managerRelation))
//            {
//                result.AddRange(GetEmployeesAtLevel(child, targetLevel, currentLevel + 1));
//            }

//            return result;
//        }

//        static void DisplayHierarchyStatistics()
//        {
//            var root = employees.Select("ManagerID IS NULL")[0];
//            var levelMap = new Dictionary<int, List<DataRow>>();
//            var subordinateCount = new Dictionary<int, int>();

//            CalculateLevelsAndSubordinates(root, 0, levelMap, subordinateCount);

//            int maxLevel = levelMap.Keys.Max();
//            Console.WriteLine($"    Максимальная глубина иерархии: {maxLevel}");
//            Console.WriteLine($"    Всего сотрудников: {employees.Rows.Count}");
//            Console.WriteLine();

//            Console.WriteLine("    Распределение по уровням:");
//            for (int i = 0; i <= maxLevel; i++)
//            {
//                int count = levelMap.ContainsKey(i) ? levelMap[i].Count : 0;
//                Console.WriteLine($"      Уровень {i}: {count} чел.");
//            }

//            double avgSubordinates = subordinateCount.Values.Average();
//            Console.WriteLine($"\n    Среднее количество подчинённых у менеджера: {avgSubordinates:F1}");
//        }

//        static void CalculateLevelsAndSubordinates(DataRow node, int level, Dictionary<int, List<DataRow>> levelMap, Dictionary<int, int> subordinateCount)
//        {
//            if (!levelMap.ContainsKey(level))
//                levelMap[level] = new List<DataRow>();
//            levelMap[level].Add(node);

//            var children = node.GetChildRows(managerRelation);
//            subordinateCount[(int)node["EmployeeID"]] = children.Length;

//            foreach (DataRow child in children)
//            {
//                CalculateLevelsAndSubordinates(child, level + 1, levelMap, subordinateCount);
//            }
//        }

//        static void DisplayColleagues(int employeeId)
//        {
//            var emp = FindEmployeeById(employeeId);
//            if (emp == null)
//            {
//                Console.WriteLine($"    → Сотрудник не найден.");
//                return;
//            }

//            var parents = emp.GetParentRows(managerRelation);
//            if (parents.Length == 0)
//            {
//                Console.WriteLine($"    → У {emp["EmployeeName"]} нет руководителя → нет коллег.");
//                return;
//            }

//            var manager = parents[0];
//            var colleagues = manager.GetChildRows(managerRelation)
//                .Where(r => (int)r["EmployeeID"] != employeeId);

//            Console.WriteLine($"    Коллеги {emp["EmployeeName"]} (руководитель: {manager["EmployeeName"]}):");

//            if (!colleagues.Any())
//            {
//                Console.WriteLine("    → Нет коллег (единственный подчинённый).");
//                return;
//            }

//            foreach (var col in colleagues)
//            {
//                Console.WriteLine($"    • [{col["EmployeeID"]}] {col["EmployeeName"]} — {col["Department"]}");
//            }
//        }

//        static void DisplayAllManagersAbove(int employeeId)
//        {
//            var emp = FindEmployeeById(employeeId);
//            if (emp == null)
//            {
//                Console.WriteLine($"    → Сотрудник не найден.");
//                return;
//            }

//            Console.WriteLine($"    Все руководители над {emp["EmployeeName"]}:");

//            var current = emp;
//            int indent = 0;

//            while (true)
//            {
//                var parents = current.GetParentRows(managerRelation);
//                if (parents.Length == 0) break;

//                var manager = parents[0];
//                Console.WriteLine($"    {new string(' ', indent * 4)}↑ [{manager["EmployeeID"]}] {manager["EmployeeName"]} ({manager["Department"]})");
//                current = manager;
//                indent++;
//            }
//        }
//    }
//}
////6
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace ManyToManyRelationship
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== РАБОТА С ОТНОШЕНИЕМ МНОГИЕ-КО-МНОГИМ ===\n");

//            // 1. Получение всех курсов для конкретного студента
//            Console.WriteLine("1. КУРСЫ СТУДЕНТА 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            GetStudentCourses(ds, "Иван Петров");
//            Console.WriteLine();

//            // 2. Получение всех студентов на конкретном курсе
//            Console.WriteLine("2. СТУДЕНТЫ НА КУРСЕ 'C# Programming':");
//            Console.WriteLine("=====================================");
//            GetCourseStudents(ds, "C# Programming");
//            Console.WriteLine();

//            // 3. Статистика: количество студентов на каждом курсе
//            Console.WriteLine("3. КОЛИЧЕСТВО СТУДЕНТОВ НА КАЖДОМ КУРСЕ:");
//            Console.WriteLine("=====================================");
//            PrintStudentsPerCourseStatistics(ds);
//            Console.WriteLine();

//            // 4. Статистика: количество курсов для каждого студента
//            Console.WriteLine("4. КОЛИЧЕСТВО КУРСОВ ДЛЯ КАЖДОГО СТУДЕНТА:");
//            Console.WriteLine("=====================================");
//            PrintCoursesPerStudentStatistics(ds);
//            Console.WriteLine();

//            // 5. Полная информация о регистрациях
//            Console.WriteLine("5. ПОЛНАЯ ИНФОРМАЦИЯ О РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintAllRegistrations(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2024, 01, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2024, 01, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2024, 02, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2024, 01, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2024, 02, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2024, 01, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2024, 02, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2024, 02, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2024, 01, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2024, 01, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2024, 02, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // 1. Получение всех курсов для конкретного студента
//        static void GetStudentCourses(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];
//            Console.WriteLine($"Курсы студента: {studentName}");

//            // Получаем все регистрации студента
//            DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tСтудент не записан ни на один курс.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);

//                if (courseRows.Length > 0)
//                {
//                    DataRow courseRow = courseRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double grade = (double)regRow["Grade"];

//                    Console.WriteLine($"\t• {courseRow["CourseName"]}");
//                    Console.WriteLine($"\t  Преподаватель: {courseRow["Instructor"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {grade:F1}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 2. Получение всех студентов на конкретном курсе
//        static void GetCourseStudents(DataSet ds, string courseName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим курс по имени
//            DataRow[] courseRows = courses.Select($"CourseName = '{courseName}'");

//            if (courseRows.Length == 0)
//            {
//                Console.WriteLine($"Курс '{courseName}' не найден.");
//                return;
//            }

//            DataRow courseRow = courseRows[0];
//            Console.WriteLine($"Студенты на курсе: {courseName}");

//            // Получаем все регистрации курса
//            DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tНа этом курсе нет студентов.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);

//                if (studentRows.Length > 0)
//                {
//                    DataRow studentRow = studentRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double grade = (double)regRow["Grade"];

//                    Console.WriteLine($"\t• {studentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Email: {studentRow["Email"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {grade:F1}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 3. Статистика: количество студентов на каждом курсе
//        static void PrintStudentsPerCourseStatistics(DataSet ds)
//        {
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Курс                  | Количество студентов");
//            Console.WriteLine("─────────────────────────────────────────────");

//            foreach (DataRow courseRow in courses.Rows)
//            {
//                string courseName = (string)courseRow["CourseName"];

//                // Получаем все регистрации курса
//                DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//                Console.WriteLine($"{courseName,-20} | {registrationRows.Length,17}");
//            }
//        }

//        // 4. Статистика: количество курсов для каждого студента
//        static void PrintCoursesPerStudentStatistics(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент                | Количество курсов");
//            Console.WriteLine("──────────────────────────────────────────");

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];

//                // Получаем все регистрации студента
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                Console.WriteLine($"{studentName,-20} | {registrationRows.Length,17}");
//            }
//        }

//        // 5. Полная информация о регистрациях
//        static void PrintAllRegistrations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("ID | Студент              | Курс             | Дата         | Оценка");
//            Console.WriteLine("────────────────────────────────────────────────────────────────────");

//            foreach (DataRow regRow in registration.Rows)
//            {
//                int registrationID = (int)regRow["RegistrationID"];
//                DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                double grade = (double)regRow["Grade"];

//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);
//                string studentName = studentRows.Length > 0 ?
//                    (string)studentRows[0]["StudentName"] : "Неизвестен";

//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);
//                string courseName = courseRows.Length > 0 ?
//                    (string)courseRows[0]["CourseName"] : "Неизвестен";

//                Console.WriteLine($"{registrationID,2} | {studentName,-20} | {courseName,-15} | {enrollmentDate:dd.MM.yyyy} | {grade,6:F1}");
//            }
//        }
//    }
//}


////7
//using System;
//using System.Data;
//using System.Collections.Generic;
//using System.Linq;

//namespace ManyToManyNavigation
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== НАВИГАЦИЯ ПО ОТНОШЕНИЯМ МНОГИЕ-КО-МНОГИМ ===\n");

//            // 1. Получение всех курсов и оценок для студента
//            Console.WriteLine("1. КУРСЫ И ОЦЕНКИ СТУДЕНТА 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            GetStudentCoursesAndGrades(ds, "Иван Петров");
//            Console.WriteLine();

//            // 2. Получение всех студентов и их оценок для курса
//            Console.WriteLine("2. СТУДЕНТЫ И ОЦЕНКИ НА КУРСЕ 'C# Programming':");
//            Console.WriteLine("=====================================");
//            GetCourseStudentsAndGrades(ds, "C# Programming");
//            Console.WriteLine();

//            // 3. Поиск студентов, учащихся на одних и тех же курсах
//            Console.WriteLine("3. СТУДЕНТЫ, УЧАЩИЕСЯ НА ОДНИХ И ТЕХ ЖЕ КУРСАХ, ЧТО И 'Иван Петров':");
//            Console.WriteLine("=====================================");
//            FindStudentsWithSameCourses(ds, "Иван Петров");
//            Console.WriteLine();

//            // 4. Полная информация о регистрациях
//            Console.WriteLine("4. ПОЛНАЯ ИНФОРМАЦИЯ О РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintFullRegistrationInfo(ds);
//            Console.WriteLine();

//            // 5. Средняя оценка для каждого студента
//            Console.WriteLine("5. СРЕДНЯЯ ОЦЕНКА ДЛЯ КАЖДОГО СТУДЕНТА:");
//            Console.WriteLine("=====================================");
//            CalculateStudentAverageGrades(ds);
//            Console.WriteLine();

//            // 6. Средняя оценка студентов по каждому курсу
//            Console.WriteLine("6. СРЕДНЯЯ ОЦЕНКА ПО КУРСАМ:");
//            Console.WriteLine("=====================================");
//            CalculateCourseAverageGrades(ds);
//            Console.WriteLine();

//            // 7. Лучшие студенты (средний рейтинг выше 4.5)
//            Console.WriteLine("7. ЛУЧШИЕ СТУДЕНТЫ (средний рейтинг > 4.5):");
//            Console.WriteLine("=====================================");
//            FindTopStudents(ds, 4.5);
//            Console.WriteLine();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double)).AllowDBNull = true; // Оценка может быть NULL
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//            registration.Rows.Add(12, 104, "C004", new DateTime(2025, 12, 10), DBNull.Value); // Оценка отсутствует
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // 1. Получение всех курсов и оценок для студента
//        static void GetStudentCoursesAndGrades(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];
//            Console.WriteLine($"Курсы и оценки студента: {studentName}");

//            // Получаем все регистрации студента
//            DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tСтудент не записан ни на один курс.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);

//                if (courseRows.Length > 0)
//                {
//                    DataRow courseRow = courseRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double? grade = regRow["Grade"] as double?;

//                    Console.WriteLine($"\t• {courseRow["CourseName"]}");
//                    Console.WriteLine($"\t  Преподаватель: {courseRow["Instructor"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {(grade.HasValue ? grade.Value.ToString("F1") : "Нет оценки")}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 2. Получение всех студентов и их оценок для курса
//        static void GetCourseStudentsAndGrades(DataSet ds, string courseName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Находим курс по имени
//            DataRow[] courseRows = courses.Select($"CourseName = '{courseName}'");

//            if (courseRows.Length == 0)
//            {
//                Console.WriteLine($"Курс '{courseName}' не найден.");
//                return;
//            }

//            DataRow courseRow = courseRows[0];
//            Console.WriteLine($"Студенты и их оценки на курсе: {courseName}");

//            // Получаем все регистрации курса
//            DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//            if (registrationRows.Length == 0)
//            {
//                Console.WriteLine("\tНа этом курсе нет студентов.");
//                return;
//            }

//            foreach (DataRow regRow in registrationRows)
//            {
//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);

//                if (studentRows.Length > 0)
//                {
//                    DataRow studentRow = studentRows[0];
//                    DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                    double? grade = regRow["Grade"] as double?;

//                    Console.WriteLine($"\t• {studentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Email: {studentRow["Email"]}");
//                    Console.WriteLine($"\t  Дата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\t  Оценка: {(grade.HasValue ? grade.Value.ToString("F1") : "Нет оценки")}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 3. Поиск студентов, учащихся на одних и тех же курсах
//        static void FindStudentsWithSameCourses(DataSet ds, string studentName)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            // Находим студента по имени
//            DataRow[] studentRows = students.Select($"StudentName = '{studentName}'");

//            if (studentRows.Length == 0)
//            {
//                Console.WriteLine($"Студент '{studentName}' не найден.");
//                return;
//            }

//            DataRow studentRow = studentRows[0];

//            // Получаем все курсы студента
//            DataRow[] studentRegistrations = studentRow.GetChildRows(studentRegistrationRelation);
//            var studentCourseIDs = new HashSet<string>();

//            foreach (DataRow regRow in studentRegistrations)
//            {
//                studentCourseIDs.Add((string)regRow["CourseID"]);
//            }

//            Console.WriteLine($"Студенты, учащиеся на тех же курсах, что и {studentName}:");

//            // Находим других студентов, учащихся на тех же курсах
//            foreach (DataRow otherStudentRow in students.Rows)
//            {
//                if ((int)otherStudentRow["StudentID"] == (int)studentRow["StudentID"])
//                    continue;

//                DataRow[] otherStudentRegistrations = otherStudentRow.GetChildRows(studentRegistrationRelation);
//                var otherCourseIDs = new HashSet<string>();

//                foreach (DataRow regRow in otherStudentRegistrations)
//                {
//                    otherCourseIDs.Add((string)regRow["CourseID"]);
//                }

//                // Находим пересечение курсов
//                var commonCourses = studentCourseIDs.Intersect(otherCourseIDs).ToList();

//                if (commonCourses.Count > 0)
//                {
//                    Console.WriteLine($"\t• {(string)otherStudentRow["StudentName"]}");
//                    Console.WriteLine($"\t  Общие курсы: {string.Join(", ", commonCourses)}");
//                    Console.WriteLine();
//                }
//            }
//        }

//        // 4. Полная информация о регистрациях
//        static void PrintFullRegistrationInfo(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("ID | Студент              | Курс             | Дата         | Оценка");
//            Console.WriteLine("────────────────────────────────────────────────────────────────────");

//            foreach (DataRow regRow in registration.Rows)
//            {
//                int registrationID = (int)regRow["RegistrationID"];
//                DateTime enrollmentDate = (DateTime)regRow["EnrollmentDate"];
//                double? grade = regRow["Grade"] as double?;

//                // Получаем информацию о студенте
//                DataRow[] studentRows = regRow.GetParentRows(studentRegistrationRelation);
//                string studentName = studentRows.Length > 0 ?
//                    (string)studentRows[0]["StudentName"] : "Неизвестен";

//                // Получаем информацию о курсе
//                DataRow[] courseRows = regRow.GetParentRows(courseRegistrationRelation);
//                string courseName = courseRows.Length > 0 ?
//                    (string)courseRows[0]["CourseName"] : "Неизвестен";

//                Console.WriteLine($"{registrationID,2} | {studentName,-20} | {courseName,-15} | {enrollmentDate:dd.MM.yyyy} | {(grade.HasValue ? grade.Value.ToString("F1") : "Нет")}");
//            }
//        }

//        // 5. Средняя оценка для каждого студента
//        static void CalculateStudentAverageGrades(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент              | Количество курсов | Средняя оценка");
//            Console.WriteLine("─────────────────────────────────────────────────────────");

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                if (registrationRows.Length == 0)
//                {
//                    Console.WriteLine($"{studentName,-20} | {0,16} | Нет оценок");
//                    continue;
//                }

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                double averageGrade = gradeCount > 0 ? sumGrades / gradeCount : 0;
//                Console.WriteLine($"{studentName,-20} | {registrationRows.Length,16} | {(gradeCount > 0 ? averageGrade.ToString("F2") : "Нет оценок")}");
//            }
//        }

//        // 6. Средняя оценка студентов по каждому курсу
//        static void CalculateCourseAverageGrades(DataSet ds)
//        {
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Курс                  | Количество студентов | Средняя оценка");
//            Console.WriteLine("─────────────────────────────────────────────────────────────");

//            foreach (DataRow courseRow in courses.Rows)
//            {
//                string courseName = (string)courseRow["CourseName"];
//                DataRow[] registrationRows = courseRow.GetChildRows(courseRegistrationRelation);

//                if (registrationRows.Length == 0)
//                {
//                    Console.WriteLine($"{courseName,-20} | {0,20} | Нет оценок");
//                    continue;
//                }

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                double averageGrade = gradeCount > 0 ? sumGrades / gradeCount : 0;
//                Console.WriteLine($"{courseName,-20} | {registrationRows.Length,20} | {(gradeCount > 0 ? averageGrade.ToString("F2") : "Нет оценок")}");
//            }
//        }

//        // 7. Лучшие студенты (средний рейтинг выше 4.5)
//        static void FindTopStudents(DataSet ds, double minAverageGrade)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];

//            Console.WriteLine("Студент              | Средняя оценка | Курсы");
//            Console.WriteLine("─────────────────────────────────────────────");

//            bool foundStudents = false;

//            foreach (DataRow studentRow in students.Rows)
//            {
//                string studentName = (string)studentRow["StudentName"];
//                DataRow[] registrationRows = studentRow.GetChildRows(studentRegistrationRelation);

//                if (registrationRows.Length == 0)
//                    continue;

//                double sumGrades = 0;
//                int gradeCount = 0;

//                foreach (DataRow regRow in registrationRows)
//                {
//                    if (regRow["Grade"] != DBNull.Value)
//                    {
//                        sumGrades += (double)regRow["Grade"];
//                        gradeCount++;
//                    }
//                }

//                if (gradeCount == 0)
//                    continue;

//                double averageGrade = sumGrades / gradeCount;

//                if (averageGrade >= minAverageGrade)
//                {
//                    foundStudents = true;

//                    // Получаем информацию о курсах студента
//                    var courseNames = new List<string>();
//                    foreach (DataRow regRow in registrationRows)
//                    {
//                        DataRow[] courseRows = regRow.GetParentRows(ds.Relations["Courses_Registrations"]);
//                        if (courseRows.Length > 0)
//                        {
//                            courseNames.Add((string)courseRows[0]["CourseName"]);
//                        }
//                    }

//                    string courses = string.Join(", ", courseNames);
//                    Console.WriteLine($"{studentName,-20} | {averageGrade,14:F2} | {courses}");
//                }
//            }

//            if (!foundStudents)
//            {
//                Console.WriteLine($"Студентов со средней оценкой ≥ {minAverageGrade} не найдено");
//            }
//        }
//    }
//}


////8
//using System;
//using System.Data;

//namespace CalculatedFieldsWithDataRelation
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение между таблицами
//            CreateRelation(ds);

//            // Добавляем расчетные поля
//            AddCalculatedColumns(ds);

//            Console.WriteLine("=== РАСЧЁТНЫЕ ПОЛЯ С ИСПОЛЬЗОВАНИЕМ DATARELATION ===\n");

//            // Выводим информацию о категориях с расчетными полями
//            Console.WriteLine("1. ИНФОРМАЦИЯ О КАТЕГОРИЯХ С РАСЧЁТНЫМИ ПОЛЯМИ:");
//            Console.WriteLine("=====================================");
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при добавлении товара
//            Console.WriteLine("2. ДОБАВЛЕНИЕ НОВОГО ТОВАРА:");
//            Console.WriteLine("=====================================");
//            AddNewProduct(ds, "Новый товар", 1999.99m, 1, 10);
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при удалении товара
//            Console.WriteLine("3. УДАЛЕНИЕ ТОВАРА:");
//            Console.WriteLine("=====================================");
//            RemoveProduct(ds, 1);
//            PrintCategoriesWithCalculations(ds);
//            Console.WriteLine();

//            // Демонстрация обновления расчетных полей при изменении цены товара
//            Console.WriteLine("4. ИЗМЕНЕНИЕ ЦЕНЫ ТОВАРА:");
//            Console.WriteLine("=====================================");
//            UpdateProductPrice(ds, 2, 69990.00m);
//            PrintCategoriesWithCalculations(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("Quantity", typeof(int));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 10, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 5, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 20, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 30, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 15, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 10, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 50, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 20, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 100, 3);
//        }

//        // Создание отношения между таблицами
//        static void CreateRelation(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // Добавление расчетных полей
//        static void AddCalculatedColumns(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            // Добавление колонки для хранения количества товаров в категории
//            DataColumn totalProductCountColumn = new DataColumn("TotalProductCount", typeof(int));
//            totalProductCountColumn.Expression = "Count(Child.CategoryID)";
//            categories.Columns.Add(totalProductCountColumn);

//            // Добавление колонки для хранения общей стоимости товаров в категории
//            DataColumn totalCategoryValueColumn = new DataColumn("TotalCategoryValue", typeof(decimal));
//            categories.Columns.Add(totalCategoryValueColumn);

//            // Пересчет общей стоимости для всех категорий
//            UpdateCategoryValues(ds);
//        }

//        // Пересчет общей стоимости товаров в категориях
//        static void UpdateCategoryValues(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                int categoryID = (int)categoryRow["CategoryID"];
//                decimal totalValue = 0;

//                // Получаем все товары в категории
//                DataRow[] productRows = categoryRow.GetChildRows(relation);

//                foreach (DataRow productRow in productRows)
//                {
//                    decimal price = (decimal)productRow["Price"];
//                    int quantity = (int)productRow["Quantity"];
//                    totalValue += price * quantity;
//                }

//                // Обновляем значение общей стоимости
//                categoryRow["TotalCategoryValue"] = totalValue;
//            }
//        }

//        // Вывод информации о категориях с расчетными полями
//        static void PrintCategoriesWithCalculations(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];

//            Console.WriteLine("ID | Категория          | Описание               | Количество товаров | Общая стоимость");
//            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────");

//            foreach (DataRow categoryRow in categories.Rows)
//            {
//                int categoryID = (int)categoryRow["CategoryID"];
//                string categoryName = (string)categoryRow["CategoryName"];
//                string description = (string)categoryRow["Описание"];
//                int productCount = (int)categoryRow["TotalProductCount"];
//                decimal totalValue = (decimal)categoryRow["TotalCategoryValue"];

//                Console.WriteLine($"{categoryID,2} | {categoryName,-18} | {description,-20} | {productCount,17} | {totalValue,16:C}");
//            }
//        }

//        // Добавление нового товара
//        static void AddNewProduct(DataSet ds, string productName, decimal price, int quantity, int categoryID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            // Находим максимальный ProductID
//            int maxProductID = 0;
//            foreach (DataRow row in products.Rows)
//            {
//                int currentID = (int)row["ProductID"];
//                if (currentID > maxProductID)
//                {
//                    maxProductID = currentID;
//                }
//            }

//            // Добавляем новый товар
//            products.Rows.Add(maxProductID + 1, productName, price, quantity, categoryID);

//            Console.WriteLine($"Добавлен новый товар: {productName}");

//            // Обновляем расчетные поля
//            UpdateCategoryValues(ds);
//        }

//        // Удаление товара
//        static void RemoveProduct(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                string productName = (string)productRow["ProductName"];
//                productRow.Delete();

//                Console.WriteLine($"Удален товар: {productName}");

//                // Обновляем расчетные поля
//                UpdateCategoryValues(ds);
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }

//        // Изменение цены товара
//        static void UpdateProductPrice(DataSet ds, int productID, decimal newPrice)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null)
//            {
//                string productName = (string)productRow["ProductName"];
//                decimal oldPrice = (decimal)productRow["Price"];

//                productRow["Price"] = newPrice;

//                Console.WriteLine($"Цена товара '{productName}' изменена с {oldPrice:C} на {newPrice:C}");

//                // Обновляем расчетные поля
//                UpdateCategoryValues(ds);
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден.");
//            }
//        }
//    }
//}


////9
//using System;
//using System.Data;

//namespace DeleteRuleExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.WriteLine("=== ДЕМОНСТРАЦИЯ DELETE RULE ===\n");

//            // Вариант 1: DeleteRule.Cascade
//            Console.WriteLine("ВАРИАНТ 1: DELETE RULE CASCADE");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleCascade();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 2: DeleteRule.SetNull
//            Console.WriteLine("ВАРИАНТ 2: DELETE RULE SET NULL");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleSetNull();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 3: DeleteRule.None
//            Console.WriteLine("ВАРИАНТ 3: DELETE RULE NONE");
//            Console.WriteLine("=====================================");
//            DemonstrateDeleteRuleNone();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Отделы
//            DataTable departments = new DataTable("Отделы");
//            departments.Columns.Add("DepartmentID", typeof(int));
//            departments.Columns.Add("DepartmentName", typeof(string));
//            departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("DepartmentID", typeof(int));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(departments);
//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем отделы
//            departments.Rows.Add(1, "IT");
//            departments.Rows.Add(2, "HR");
//            departments.Rows.Add(3, "Finance");

//            // Добавляем сотрудников
//            employees.Rows.Add(101, "Иван Иванов", 1, 100000);
//            employees.Rows.Add(102, "Мария Петрова", 1, 95000);
//            employees.Rows.Add(103, "Петр Сидоров", 2, 85000);
//            employees.Rows.Add(104, "Анна Кузнецова", 2, 80000);
//            employees.Rows.Add(105, "Сергей Васильев", 3, 90000);
//        }

//        // Демонстрация DeleteRule.Cascade
//        static void DemonstrateDeleteRuleCascade()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.Cascade
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.Cascade
//            fkConstraint.DeleteRule = Rule.Cascade;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Удаляем отдел IT
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nУдаляем отдел: {itDepartment["DepartmentName"]}");
//                itDepartment.Delete();
//            }

//            // Выводим состояние после удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: Отдел и все его сотрудники были удалены (CASCADE).");
//        }

//        // Демонстрация DeleteRule.SetNull
//        static void DemonstrateDeleteRuleSetNull()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.SetNull
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Разрешаем NULL значения в DepartmentID
//            employees.Columns["DepartmentID"].AllowDBNull = true;

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.SetNull
//            fkConstraint.DeleteRule = Rule.SetNull;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Удаляем отдел IT
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nУдаляем отдел: {itDepartment["DepartmentName"]}");
//                itDepartment.Delete();
//            }

//            // Выводим состояние после удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: Отдел удалён, у сотрудников DepartmentID установлен в NULL (SET NULL).");
//        }

//        // Демонстрация DeleteRule.None
//        static void DemonstrateDeleteRuleNone()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём отношение с DeleteRule.None
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Создаем ограничение внешнего ключа
//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем DeleteRule.None
//            fkConstraint.DeleteRule = Rule.None;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до удаления
//            Console.WriteLine("СОСТОЯНИЕ ДО УДАЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Пробуем удалить отдел IT
//            try
//            {
//                DataRow itDepartment = departments.Rows.Find(1);
//                if (itDepartment != null)
//                {
//                    Console.WriteLine($"\nПытаемся удалить отдел: {itDepartment["DepartmentName"]}");
//                    itDepartment.Delete();
//                    Console.WriteLine("Удаление прошло успешно.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nОШИБКА: {ex.Message}");
//                Console.WriteLine("Удаление отменено, так как есть связанные сотрудники (NONE).");
//            }

//            // Выводим состояние после попытки удаления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ПОПЫТКИ УДАЛЕНИЯ:");
//            PrintDataSetState(ds);
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            Console.WriteLine("\nОТДЕЛЫ:");
//            foreach (DataRow row in departments.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DepartmentID"]}: {row["DepartmentName"]}");
//                }
//            }

//            Console.WriteLine("\nСОТРУДНИКИ:");
//            foreach (DataRow row in employees.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int deptID = row["DepartmentID"] != DBNull.Value ? (int)row["DepartmentID"] : -1;
//                    string deptName = deptID != -1 ? deptID.ToString() : "NULL";
//                    Console.WriteLine($"{row["EmployeeID"]}: {row["EmployeeName"]}, Отдел: {deptName}, Зарплата: {row["Salary"]}");
//                }
//            }
//        }
//    }
//}


////10
//using System;
//using System.Data;

//namespace UpdateRuleExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            Console.WriteLine("=== ДЕМОНСТРАЦИЯ UPDATE RULE ===\n");

//            // Вариант 1: UpdateRule.Cascade
//            Console.WriteLine("ВАРИАНТ 1: UPDATE RULE CASCADE");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleCascade();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 2: UpdateRule.SetNull
//            Console.WriteLine("ВАРИАНТ 2: UPDATE RULE SET NULL");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleSetNull();
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Вариант 3: UpdateRule.None
//            Console.WriteLine("ВАРИАНТ 3: UPDATE RULE NONE");
//            Console.WriteLine("=====================================");
//            DemonstrateUpdateRuleNone();
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("CompanyDataSet");

//            // Таблица Отделы
//            DataTable departments = new DataTable("Отделы");
//            departments.Columns.Add("DepartmentID", typeof(int));
//            departments.Columns.Add("DepartmentName", typeof(string));
//            departments.PrimaryKey = new DataColumn[] { departments.Columns["DepartmentID"] };

//            // Таблица Сотрудники
//            DataTable employees = new DataTable("Сотрудники");
//            employees.Columns.Add("EmployeeID", typeof(int));
//            employees.Columns.Add("EmployeeName", typeof(string));
//            employees.Columns.Add("DepartmentID", typeof(int));
//            employees.Columns.Add("Salary", typeof(decimal));
//            employees.PrimaryKey = new DataColumn[] { employees.Columns["EmployeeID"] };

//            ds.Tables.Add(departments);
//            ds.Tables.Add(employees);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Добавляем отделы
//            departments.Rows.Add(1, "IT");
//            departments.Rows.Add(2, "HR");
//            departments.Rows.Add(3, "Finance");

//            // Добавляем сотрудников
//            employees.Rows.Add(101, "Иван Иванов", 1, 100000);
//            employees.Rows.Add(102, "Мария Петрова", 1, 95000);
//            employees.Rows.Add(103, "Петр Сидоров", 2, 85000);
//            employees.Rows.Add(104, "Анна Кузнецова", 2, 80000);
//            employees.Rows.Add(105, "Сергей Васильев", 3, 90000);
//        }

//        // Демонстрация UpdateRule.Cascade
//        static void DemonstrateUpdateRuleCascade()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.Cascade
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.Cascade
//            fkConstraint.UpdateRule = Rule.Cascade;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Обновляем ID отдела IT с 1 на 101
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nОбновляем ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                itDepartment.BeginEdit();
//                itDepartment["DepartmentID"] = 101;
//                itDepartment.EndEdit();
//            }

//            // Выводим состояние после обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: ID отдела обновлен, и все связанные сотрудники также получили новый DepartmentID (CASCADE).");
//        }

//        // Демонстрация UpdateRule.SetNull
//        static void DemonstrateUpdateRuleSetNull()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.SetNull
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            // Разрешаем NULL значения в DepartmentID
//            employees.Columns["DepartmentID"].AllowDBNull = true;

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.SetNull
//            fkConstraint.UpdateRule = Rule.SetNull;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Обновляем ID отдела IT с 1 на 101
//            DataRow itDepartment = departments.Rows.Find(1);
//            if (itDepartment != null)
//            {
//                Console.WriteLine($"\nОбновляем ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                itDepartment.BeginEdit();
//                itDepartment["DepartmentID"] = 101;
//                itDepartment.EndEdit();
//            }

//            // Выводим состояние после обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            Console.WriteLine("\nРЕЗУЛЬТАТ: ID отдела обновлен, у сотрудников DepartmentID установлен в NULL (SET NULL).");
//        }

//        // Демонстрация UpdateRule.None
//        static void DemonstrateUpdateRuleNone()
//        {
//            DataSet ds = CreateDataSet();
//            FillTestData(ds);

//            // Создаём ограничение внешнего ключа с UpdateRule.None
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
//                "FK_Employees_Departments",
//                departments.Columns["DepartmentID"],
//                employees.Columns["DepartmentID"]);

//            // Устанавливаем UpdateRule.None
//            fkConstraint.UpdateRule = Rule.None;

//            // Добавляем ограничение в таблицу сотрудников
//            employees.Constraints.Add(fkConstraint);

//            // Выводим состояние до обновления
//            Console.WriteLine("СОСТОЯНИЕ ДО ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);

//            // Пробуем обновить ID отдела IT с 1 на 101
//            try
//            {
//                DataRow itDepartment = departments.Rows.Find(1);
//                if (itDepartment != null)
//                {
//                    Console.WriteLine($"\nПытаемся обновить ID отдела '{itDepartment["DepartmentName"]}' с 1 на 101");
//                    itDepartment.BeginEdit();
//                    itDepartment["DepartmentID"] = 101;
//                    itDepartment.EndEdit();
//                    Console.WriteLine("Обновление прошло успешно.");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"\nОШИБКА: {ex.Message}");
//                Console.WriteLine("Обновление отменено, так как есть связанные сотрудники (NONE).");
//            }

//            // Выводим состояние после попытки обновления
//            Console.WriteLine("\nСОСТОЯНИЕ ПОСЛЕ ПОПЫТКИ ОБНОВЛЕНИЯ:");
//            PrintDataSetState(ds);
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable departments = ds.Tables["Отделы"];
//            DataTable employees = ds.Tables["Сотрудники"];

//            Console.WriteLine("\nОТДЕЛЫ:");
//            foreach (DataRow row in departments.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DepartmentID"]}: {row["DepartmentName"]}");
//                }
//            }

//            Console.WriteLine("\nСОТРУДНИКИ:");
//            foreach (DataRow row in employees.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int deptID = row["DepartmentID"] != DBNull.Value ? (int)row["DepartmentID"] : -1;
//                    string deptName = deptID != -1 ? deptID.ToString() : "NULL";
//                    Console.WriteLine($"{row["EmployeeID"]}: {row["EmployeeName"]}, Отдел: {deptName}, Зарплата: {row["Salary"]}");
//                }
//            }
//        }
//    }
//}


////11
//using System;
//using System.Data;

//namespace OrderManagementSystem
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения с правилами DeleteRule и UpdateRule
//            CreateRelations(ds);

//            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ЗАКАЗАМИ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 1. Добавление нового клиента
//            Console.WriteLine("1. ДОБАВЛЕНИЕ НОВОГО КЛИЕНТА:");
//            Console.WriteLine("=====================================");
//            AddNewCustomer(ds, "Новый Клиент", "newclient@example.com");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 2. Удаление заказчика
//            Console.WriteLine("2. УДАЛЕНИЕ ЗАКАЗЧИКА (ID: 1):");
//            Console.WriteLine("=====================================");
//            DeleteCustomer(ds, 1);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 3. Изменение идентификатора заказчика
//            Console.WriteLine("3. ИЗМЕНЕНИЕ ID ЗАКАЗЧИКА (с 2 на 102):");
//            Console.WriteLine("=====================================");
//            UpdateCustomerID(ds, 2, 102);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 4. Добавление заказа
//            Console.WriteLine("4. ДОБАВЛЕНИЕ НОВОГО ЗАКАЗА:");
//            Console.WriteLine("=====================================");
//            AddNewOrder(ds, 102, new DateTime(2023, 12, 15), 1500.00m);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 5. Удаление заказа
//            Console.WriteLine("5. УДАЛЕНИЕ ЗАКАЗА (ID: 2):");
//            Console.WriteLine("=====================================");
//            DeleteOrder(ds, 2);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 6. Изменение ID заказа
//            Console.WriteLine("6. ИЗМЕНЕНИЕ ID ЗАКАЗА (с 3 на 103):");
//            Console.WriteLine("=====================================");
//            UpdateOrderID(ds, 3, 103);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 7. Отчёт о безопасности операций
//            Console.WriteLine("7. ОТЧЁТ О БЕЗОПАСНОСТИ ОПЕРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            PrintOperationReport(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("OrderManagement");

//            // Таблица Заказчики
//            DataTable customers = new DataTable("Заказчики");
//            customers.Columns.Add("CustomerID", typeof(int));
//            customers.Columns.Add("CustomerName", typeof(string));
//            customers.Columns.Add("Email", typeof(string));
//            customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//            // Таблица Заказы
//            DataTable orders = new DataTable("Заказы");
//            orders.Columns.Add("OrderID", typeof(int));
//            orders.Columns.Add("OrderDate", typeof(DateTime));
//            orders.Columns.Add("CustomerID", typeof(int));
//            orders.Columns.Add("Total", typeof(decimal));
//            orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//            // Таблица ОрдеротовыеДетали
//            DataTable orderDetails = new DataTable("ОрдеротовыеДетали");
//            orderDetails.Columns.Add("DetailID", typeof(int));
//            orderDetails.Columns.Add("OrderID", typeof(int));
//            orderDetails.Columns.Add("ProductID", typeof(int));
//            orderDetails.Columns.Add("Quantity", typeof(int));
//            orderDetails.Columns.Add("Price", typeof(decimal));
//            orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//            ds.Tables.Add(customers);
//            ds.Tables.Add(orders);
//            ds.Tables.Add(orderDetails);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            // Добавляем заказчиков
//            customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//            customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//            // Добавляем заказы
//            orders.Rows.Add(1, new DateTime(2025, 10, 15), 1, 1000.00m);
//            orders.Rows.Add(2, new DateTime(2025, 11, 20), 1, 1500.00m);
//            orders.Rows.Add(3, new DateTime(2025, 12, 05), 2, 2000.00m);

//            // Добавляем детали заказов
//            orderDetails.Rows.Add(1, 1, 101, 2, 500.00m);
//            orderDetails.Rows.Add(2, 1, 102, 1, 500.00m);
//            orderDetails.Rows.Add(3, 2, 103, 3, 500.00m);
//            orderDetails.Rows.Add(4, 2, 104, 1, 1000.00m);
//            orderDetails.Rows.Add(5, 3, 105, 1, 2000.00m);
//        }

//        // Создание отношений с правилами DeleteRule и UpdateRule
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            try
//            {
//                // Отношение: Заказчики → Заказы (DeleteRule=Cascade, UpdateRule=Cascade)
//                ForeignKeyConstraint customerOrderConstraint = new ForeignKeyConstraint(
//                    "FK_Customers_Orders",
//                    customers.Columns["CustomerID"],
//                    orders.Columns["CustomerID"]);

//                customerOrderConstraint.DeleteRule = Rule.Cascade;
//                customerOrderConstraint.UpdateRule = Rule.Cascade;

//                orders.Constraints.Add(customerOrderConstraint);

//                // Отношение: Заказы → ОрдеротовыеДетали (DeleteRule=Cascade, UpdateRule=Cascade)
//                ForeignKeyConstraint orderDetailConstraint = new ForeignKeyConstraint(
//                    "FK_Orders_OrderDetails",
//                    orders.Columns["OrderID"],
//                    orderDetails.Columns["OrderID"]);

//                orderDetailConstraint.DeleteRule = Rule.Cascade;
//                orderDetailConstraint.UpdateRule = Rule.Cascade;

//                orderDetails.Constraints.Add(orderDetailConstraint);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            Console.WriteLine("\nЗАКАЗЧИКИ:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CustomerID"]}: {row["CustomerName"]}, Email: {row["Email"]}");
//                }
//            }

//            Console.WriteLine("\nЗАКАЗЫ:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["OrderID"]}: Дата: {((DateTime)row["OrderDate"]).ToShortDateString()}, Заказчик: {row["CustomerID"]}, Сумма: {row["Total"]:C}");
//                }
//            }

//            Console.WriteLine("\nДЕТАЛИ ЗАКАЗОВ:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DetailID"]}: Заказ: {row["OrderID"]}, Продукт: {row["ProductID"]}, Кол-во: {row["Quantity"]}, Цена: {row["Price"]:C}");
//                }
//            }
//        }

//        // 1. Добавление нового клиента
//        static void AddNewCustomer(DataSet ds, string customerName, string email)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            // Находим максимальный CustomerID
//            int maxCustomerID = 0;
//            foreach (DataRow row in customers.Rows)
//            {
//                int currentID = (int)row["CustomerID"];
//                if (currentID > maxCustomerID)
//                {
//                    maxCustomerID = currentID;
//                }
//            }

//            // Добавляем нового клиента
//            customers.Rows.Add(maxCustomerID + 1, customerName, email);

//            Console.WriteLine($"Добавлен новый клиент: {customerName}");
//        }

//        // 2. Удаление заказчика
//        static void DeleteCustomer(DataSet ds, int customerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(customerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];
//                customerRow.Delete();

//                Console.WriteLine($"Удален заказчик: {customerName} (ID: {customerID})");
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            }
//        }

//        // 3. Изменение идентификатора заказчика
//        static void UpdateCustomerID(DataSet ds, int oldCustomerID, int newCustomerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(oldCustomerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];

//                try
//                {
//                    customerRow.BeginEdit();
//                    customerRow["CustomerID"] = newCustomerID;
//                    customerRow.EndEdit();

//                    Console.WriteLine($"ID заказчика '{customerName}' изменен с {oldCustomerID} на {newCustomerID}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка при изменении ID заказчика: {ex.Message}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {oldCustomerID} не найден.");
//            }
//        }

//        // 4. Добавление заказа
//        static void AddNewOrder(DataSet ds, int customerID, DateTime orderDate, decimal total)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            // Находим максимальный OrderID
//            int maxOrderID = 0;
//            foreach (DataRow row in orders.Rows)
//            {
//                int currentID = (int)row["OrderID"];
//                if (currentID > maxOrderID)
//                {
//                    maxOrderID = currentID;
//                }
//            }

//            // Добавляем новый заказ
//            orders.Rows.Add(maxOrderID + 1, orderDate, customerID, total);

//            Console.WriteLine($"Добавлен новый заказ для клиента с ID: {customerID}");
//        }

//        // 5. Удаление заказа
//        static void DeleteOrder(DataSet ds, int orderID)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            DataRow orderRow = orders.Rows.Find(orderID);

//            if (orderRow != null)
//            {
//                DateTime orderDate = (DateTime)orderRow["OrderDate"];
//                orderRow.Delete();

//                Console.WriteLine($"Удален заказ с ID: {orderID} от {orderDate.ToShortDateString()}");
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с ID {orderID} не найден.");
//            }
//        }

//        // 6. Изменение ID заказа
//        static void UpdateOrderID(DataSet ds, int oldOrderID, int newOrderID)
//        {
//            DataTable orders = ds.Tables["Заказы"];

//            DataRow orderRow = orders.Rows.Find(oldOrderID);

//            if (orderRow != null)
//            {
//                DateTime orderDate = (DateTime)orderRow["OrderDate"];

//                try
//                {
//                    orderRow.BeginEdit();
//                    orderRow["OrderID"] = newOrderID;
//                    orderRow.EndEdit();

//                    Console.WriteLine($"ID заказа от {orderDate.ToShortDateString()} изменен с {oldOrderID} на {newOrderID}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка при изменении ID заказа: {ex.Message}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"Заказ с ID {oldOrderID} не найден.");
//            }
//        }

//        // 7. Отчёт о безопасности операций
//        static void PrintOperationReport(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ОрдеротовыеДетали"];

//            Console.WriteLine("ОТЧЁТ О БЕЗОПАСНОСТИ ОПЕРАЦИЙ:");
//            Console.WriteLine("-------------------------------------");
//            Console.WriteLine($"Общее количество заказчиков: {customers.Rows.Count}");
//            Console.WriteLine($"Общее количество заказов: {orders.Rows.Count}");
//            Console.WriteLine($"Общее количество деталей заказов: {orderDetails.Rows.Count}");

//            // Подсчёт количества заказов для каждого заказчика
//            var customerOrderCounts = new Dictionary<int, int>();
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int customerID = (int)row["CustomerID"];
//                    if (customerOrderCounts.ContainsKey(customerID))
//                    {
//                        customerOrderCounts[customerID]++;
//                    }
//                    else
//                    {
//                        customerOrderCounts[customerID] = 1;
//                    }
//                }
//            }

//            Console.WriteLine("\nКоличество заказов на заказчика:");
//            foreach (var kvp in customerOrderCounts)
//            {
//                DataRow customerRow = customers.Rows.Find(kvp.Key);
//                if (customerRow != null)
//                {
//                    string customerName = (string)customerRow["CustomerName"];
//                    Console.WriteLine($"{customerName}: {kvp.Value}");
//                }
//            }

//            // Подсчёт количества деталей заказа для каждого заказа
//            var orderDetailCounts = new Dictionary<int, int>();
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int orderID = (int)row["OrderID"];
//                    if (orderDetailCounts.ContainsKey(orderID))
//                    {
//                        orderDetailCounts[orderID]++;
//                    }
//                    else
//                    {
//                        orderDetailCounts[orderID] = 1;
//                    }
//                }
//            }

//            Console.WriteLine("\nКоличество деталей заказа на заказ:");
//            foreach (var kvp in orderDetailCounts)
//            {
//                DataRow orderRow = orders.Rows.Find(kvp.Key);
//                if (orderRow != null)
//                {
//                    DateTime orderDate = (DateTime)orderRow["OrderDate"];
//                    Console.WriteLine($"Заказ от {orderDate.ToShortDateString()}: {kvp.Value}");
//                }
//            }
//        }
//    }
//}


////12
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace RowStateExample
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношение между таблицами
//            CreateRelation(ds);

//            Console.WriteLine("=== РАБОТА С ROWSTATE ДЛЯ УДАЛЯЕМЫХ СТРОК ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 1. Добавление 2 новых товаров
//            Console.WriteLine("1. ДОБАВЛЕНИЕ 2 НОВЫХ ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            AddNewProduct(ds, "Новый товар 1", 1999.99m, 1, 10);
//            AddNewProduct(ds, "Новый товар 2", 2999.99m, 2, 5);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 2. Модификация 2 товаров
//            Console.WriteLine("2. МОДИФИКАЦИЯ 2 ТОВАРОВ:");
//            Console.WriteLine("=====================================");
//            UpdateProduct(ds, 1, "Обновленный смартфон", 25990.00m, 15);
//            UpdateProduct(ds, 2, "Обновленный ноутбук", 65990.00m, 3);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 3. Пометка 3 товаров на удаление
//            Console.WriteLine("3. ПОМЕТКА 3 ТОВАРОВ НА УДАЛЕНИЕ:");
//            Console.WriteLine("=====================================");
//            DeleteProduct(ds, 3);
//            DeleteProduct(ds, 4);
//            DeleteProduct(ds, 5);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 4. Получение всех строк, помеченных на удаление
//            Console.WriteLine("4. ТОВАРЫ, ПОМЕЧЕННЫЕ НА УДАЛЕНИЕ:");
//            Console.WriteLine("=====================================");
//            PrintDeletedProducts(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 5. Отчёт перед удалением
//            Console.WriteLine("5. ОТЧЁТ ПЕРЕД УДАЛЕНИЕМ:");
//            Console.WriteLine("=====================================");
//            PrintDeletionReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 6. Отмена удаления для отдельных строк
//            Console.WriteLine("6. ОТМЕНА УДАЛЕНИЯ ДЛЯ ТОВАРА С ID 3:");
//            Console.WriteLine("=====================================");
//            CancelProductDeletion(ds, 3);
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // 7. Вывод итогового состояния
//            Console.WriteLine("7. ИТОГОВОЕ СОСТОЯНИЕ:");
//            Console.WriteLine("=====================================");
//            PrintDataSetState(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("ProductDataSet");

//            // Таблица Категории
//            DataTable categories = new DataTable("Категории");
//            categories.Columns.Add("CategoryID", typeof(int));
//            categories.Columns.Add("CategoryName", typeof(string));
//            categories.Columns.Add("Описание", typeof(string));
//            categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//            // Таблица Товары
//            DataTable products = new DataTable("Товары");
//            products.Columns.Add("ProductID", typeof(int));
//            products.Columns.Add("ProductName", typeof(string));
//            products.Columns.Add("Price", typeof(decimal));
//            products.Columns.Add("Quantity", typeof(int));
//            products.Columns.Add("CategoryID", typeof(int));
//            products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//            ds.Tables.Add(categories);
//            ds.Tables.Add(products);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            // Добавляем категории
//            categories.Rows.Add(1, "Электроника", "Электронные устройства");
//            categories.Rows.Add(2, "Одежда", "Одежда и обувь");
//            categories.Rows.Add(3, "Книги", "Книги и журналы");

//            // Добавляем товары
//            products.Rows.Add(1, "Смартфон", 29990.00m, 10, 1);
//            products.Rows.Add(2, "Ноутбук", 59990.00m, 5, 1);
//            products.Rows.Add(3, "Наушники", 2990.00m, 20, 1);
//            products.Rows.Add(4, "Футболка", 990.00m, 30, 2);
//            products.Rows.Add(5, "Джинсы", 2490.00m, 15, 2);
//            products.Rows.Add(6, "Кроссовки", 3990.00m, 10, 2);
//            products.Rows.Add(7, "Роман", 490.00m, 50, 3);
//            products.Rows.Add(8, "Учебник", 1290.00m, 20, 3);
//            products.Rows.Add(9, "Журнал", 190.00m, 100, 3);
//        }

//        // Создание отношения между таблицами
//        static void CreateRelation(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            try
//            {
//                DataRelation relation = new DataRelation(
//                    "CategoryProducts",
//                    categories.Columns["CategoryID"],
//                    products.Columns["CategoryID"],
//                    true); // createConstraints = true

//                ds.Relations.Add(relation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношения: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable categories = ds.Tables["Категории"];
//            DataTable products = ds.Tables["Товары"];

//            Console.WriteLine("\nКАТЕГОРИИ:");
//            foreach (DataRow row in categories.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CategoryID"]}: {row["CategoryName"]} ({row["Описание"]})");
//                }
//            }

//            Console.WriteLine("\nТОВАРЫ:");
//            foreach (DataRow row in products.Rows)
//            {
//                string state = "";
//                switch (row.RowState)
//                {
//                    case DataRowState.Added:
//                        state = " [Добавлен]";
//                        break;
//                    case DataRowState.Modified:
//                        state = " [Изменен]";
//                        break;
//                    case DataRowState.Deleted:
//                        state = " [Удален]";
//                        break;
//                }
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["ProductID"]}: {row["ProductName"]}, Цена: {row["Price"]:C}, Кол-во: {row["Quantity"]}, Категория: {row["CategoryID"]}{state}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["ProductID", DataRowVersion.Original]}: {row["ProductName", DataRowVersion.Original]} [Удален]");
//                }
//            }
//        }

//        // 1. Добавление нового товара
//        static void AddNewProduct(DataSet ds, string productName, decimal price, int categoryID, int quantity)
//        {
//            DataTable products = ds.Tables["Товары"];

//            // Находим максимальный ProductID
//            int maxProductID = 0;
//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    int currentID = (int)row["ProductID"];
//                    if (currentID > maxProductID)
//                    {
//                        maxProductID = currentID;
//                    }
//                }
//            }

//            // Добавляем новый товар
//            products.Rows.Add(maxProductID + 1, productName, price, quantity, categoryID);

//            Console.WriteLine($"Добавлен новый товар: {productName}");
//        }

//        // 2. Модификация товара
//        static void UpdateProduct(DataSet ds, int productID, string productName, decimal price, int quantity)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState != DataRowState.Deleted)
//            {
//                productRow.BeginEdit();
//                productRow["ProductName"] = productName;
//                productRow["Price"] = price;
//                productRow["Quantity"] = quantity;
//                productRow.EndEdit();

//                Console.WriteLine($"Товар с ID {productID} обновлен.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или уже удален.");
//            }
//        }

//        // 3. Пометка товара на удаление
//        static void DeleteProduct(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState != DataRowState.Deleted)
//            {
//                productRow.Delete();

//                Console.WriteLine($"Товар с ID {productID} помечен на удаление.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или уже удален.");
//            }
//        }

//        // 4. Получение всех строк, помеченных на удаление
//        static void PrintDeletedProducts(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("Список товаров, помеченных на удаление:");

//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int productID = (int)row["ProductID", DataRowVersion.Original];
//                    string productName = (string)row["ProductName", DataRowVersion.Original];

//                    // Получаем родительскую строку (категорию)
//                    DataRow[] categoryRows = row.GetParentRows(relation, DataRowVersion.Original);

//                    if (categoryRows.Length > 0)
//                    {
//                        DataRow categoryRow = categoryRows[0];
//                        string categoryName = (string)categoryRow["CategoryName"];

//                        Console.WriteLine($"\nТовар: {productName} (ID: {productID})");
//                        Console.WriteLine($"\tКатегория: {categoryName}");
//                    }
//                    else
//                    {
//                        Console.WriteLine($"\nТовар: {productName} (ID: {productID})");
//                        Console.WriteLine("\tКатегория: Неизвестно");
//                    }
//                }
//            }
//        }

//        // 5. Отчёт перед удалением
//        static void PrintDeletionReport(DataSet ds)
//        {
//            DataTable products = ds.Tables["Товары"];
//            DataTable categories = ds.Tables["Категории"];
//            DataRelation relation = ds.Relations["CategoryProducts"];

//            Console.WriteLine("ОТЧЁТ ПЕРЕД УДАЛЕНИЕМ:");

//            // Подсчёт количества товаров на удаление по категориям
//            Dictionary<int, List<DataRow>> deletedProductsByCategory = new Dictionary<int, List<DataRow>>();

//            foreach (DataRow row in products.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int categoryID = (int)row["CategoryID", DataRowVersion.Original];

//                    if (!deletedProductsByCategory.ContainsKey(categoryID))
//                    {
//                        deletedProductsByCategory[categoryID] = new List<DataRow>();
//                    }

//                    deletedProductsByCategory[categoryID].Add(row);
//                }
//            }

//            Console.WriteLine("\nТовары, помеченные на удаление:");
//            foreach (var kvp in deletedProductsByCategory)
//            {
//                int categoryID = kvp.Key;
//                DataRow categoryRow = categories.Rows.Find(categoryID);

//                if (categoryRow != null)
//                {
//                    string categoryName = (string)categoryRow["CategoryName"];
//                    Console.WriteLine($"\nКатегория: {categoryName}");

//                    foreach (DataRow productRow in kvp.Value)
//                    {
//                        string productName = (string)productRow["ProductName", DataRowVersion.Original];
//                        Console.WriteLine($"\tТовар: {productName}");
//                    }
//                }
//            }

//            Console.WriteLine("\nСтатистика по категориям:");
//            foreach (var kvp in deletedProductsByCategory)
//            {
//                int categoryID = kvp.Key;
//                DataRow categoryRow = categories.Rows.Find(categoryID);

//                if (categoryRow != null)
//                {
//                    string categoryName = (string)categoryRow["CategoryName"];
//                    Console.WriteLine($"{categoryName}: {kvp.Value.Count} товаров на удаление");
//                }
//            }
//        }

//        // 6. Отмена удаления для отдельных строк
//        static void CancelProductDeletion(DataSet ds, int productID)
//        {
//            DataTable products = ds.Tables["Товары"];

//            DataRow productRow = products.Rows.Find(productID);

//            if (productRow != null && productRow.RowState == DataRowState.Deleted)
//            {
//                productRow.RejectChanges();

//                Console.WriteLine($"Удаление товара с ID {productID} отменено.");
//            }
//            else
//            {
//                Console.WriteLine($"Товар с ID {productID} не найден или не помечен на удаление.");
//            }
//        }
//    }
//}


////13
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace NewRegistrationsAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== АНАЛИЗ НОВЫХ РЕГИСТРАЦИЙ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Добавление новых регистраций
//            Console.WriteLine("ДОБАВЛЕНИЕ НОВЫХ РЕГИСТРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            AddNewRegistration(ds, 101, "C003", new DateTime(2024, 01, 25), 4.0);
//            AddNewRegistration(ds, 102, "C004", new DateTime(2024, 02, 10), 4.5);
//            AddNewRegistration(ds, 103, "C001", new DateTime(2024, 02, 15), 3.8);
//            AddNewRegistration(ds, 104, "C002", new DateTime(2024, 02, 20), 4.2);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Получение всех новых регистраций
//            Console.WriteLine("НОВЫЕ РЕГИСТРАЦИИ:");
//            Console.WriteLine("=====================================");
//            PrintNewRegistrations(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт о новых регистрациях
//            Console.WriteLine("ОТЧЁТ О НОВЫХ РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintRegistrationReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Статистика по новым регистрациям
//            Console.WriteLine("СТАТИСТИКА ПО НОВЫМ РЕГИСТРАЦИЯМ:");
//            Console.WriteLine("=====================================");
//            PrintRegistrationStatistics(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            Console.WriteLine("\nСТУДЕНТЫ:");
//            foreach (DataRow row in students.Rows)
//            {
//                Console.WriteLine($"{row["StudentID"]}: {row["StudentName"]}, Email: {row["Email"]}");
//            }

//            Console.WriteLine("\nКУРСЫ:");
//            foreach (DataRow row in courses.Rows)
//            {
//                Console.WriteLine($"{row["CourseID"]}: {row["CourseName"]}, Преподаватель: {row["Instructor"]}");
//            }

//            Console.WriteLine("\nРЕГИСТРАЦИИ:");
//            foreach (DataRow row in registration.Rows)
//            {
//                Console.WriteLine($"{row["RegistrationID"]}: Студент: {row["StudentID"]}, Курс: {row["CourseID"]}, Дата: {((DateTime)row["EnrollmentDate"]).ToShortDateString()}, Оценка: {row["Grade"]}");
//            }
//        }

//        // Добавление новой регистрации
//        static void AddNewRegistration(DataSet ds, int studentID, string courseID, DateTime enrollmentDate, double grade)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            // Находим максимальный RegistrationID
//            int maxRegistrationID = 0;
//            foreach (DataRow row in registration.Rows)
//            {
//                int currentID = (int)row["RegistrationID"];
//                if (currentID > maxRegistrationID)
//                {
//                    maxRegistrationID = currentID;
//                }
//            }

//            // Проверяем существование студента и курса
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];

//            DataRow studentRow = students.Rows.Find(studentID);
//            DataRow courseRow = courses.Rows.Find(courseID);

//            if (studentRow == null)
//            {
//                Console.WriteLine($"Ошибка: Студент с ID {studentID} не найден.");
//                return;
//            }

//            if (courseRow == null)
//            {
//                Console.WriteLine($"Ошибка: Курс с ID {courseID} не найден.");
//                return;
//            }

//            // Добавляем новую регистрацию
//            registration.Rows.Add(maxRegistrationID + 1, studentID, courseID, enrollmentDate, grade);

//            Console.WriteLine($"Добавлена новая регистрация: Студент {studentID}, Курс {courseID}");
//        }

//        // Получение всех новых регистраций
//        static void PrintNewRegistrations(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Список новых регистраций:");

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int registrationID = (int)row["RegistrationID"];
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];
//                    DateTime enrollmentDate = (DateTime)row["EnrollmentDate"];
//                    double grade = (double)row["Grade"];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"\nРегистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName} (ID: {studentID})");
//                    Console.WriteLine($"\tКурс: {courseName} (ID: {courseID})");
//                    Console.WriteLine($"\tДата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\tОценка: {grade:F1}");
//                }
//            }
//        }

//        // Отчёт о новых регистрациях
//        static void PrintRegistrationReport(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Отчёт о новых регистрациях:");

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int registrationID = (int)row["RegistrationID"];
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];
//                    DateTime enrollmentDate = (DateTime)row["EnrollmentDate"];
//                    double grade = (double)row["Grade"];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";
//                    string studentEmail = studentRows.Length > 0 ? (string)studentRows[0]["Email"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";
//                    string instructor = courseRows.Length > 0 ? (string)courseRows[0]["Instructor"] : "Неизвестен";

//                    Console.WriteLine($"\nРегистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName} (ID: {studentID}, Email: {studentEmail})");
//                    Console.WriteLine($"\tКурс: {courseName} (ID: {courseID}, Преподаватель: {instructor})");
//                    Console.WriteLine($"\tДата регистрации: {enrollmentDate:dd.MM.yyyy}");
//                    Console.WriteLine($"\tОценка: {grade:F1}");
//                }
//            }
//        }

//        // Статистика по новым регистрациям
//        static void PrintRegistrationStatistics(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            // Подсчёт количества новых регистраций для каждого студента
//            Dictionary<int, int> newRegistrationsByStudent = new Dictionary<int, int>();
//            // Подсчёт количества новых регистраций для каждого курса
//            Dictionary<string, int> newRegistrationsByCourse = new Dictionary<string, int>();

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Added)
//                {
//                    int studentID = (int)row["StudentID"];
//                    string courseID = (string)row["CourseID"];

//                    // Подсчёт для студентов
//                    if (newRegistrationsByStudent.ContainsKey(studentID))
//                    {
//                        newRegistrationsByStudent[studentID]++;
//                    }
//                    else
//                    {
//                        newRegistrationsByStudent[studentID] = 1;
//                    }

//                    // Подсчёт для курсов
//                    if (newRegistrationsByCourse.ContainsKey(courseID))
//                    {
//                        newRegistrationsByCourse[courseID]++;
//                    }
//                    else
//                    {
//                        newRegistrationsByCourse[courseID] = 1;
//                    }
//                }
//            }

//            // Вывод статистики по студентам
//            Console.WriteLine("Количество новых регистраций по студентам:");
//            DataTable students = ds.Tables["Студенты"];
//            foreach (var kvp in newRegistrationsByStudent)
//            {
//                DataRow studentRow = students.Rows.Find(kvp.Key);
//                if (studentRow != null)
//                {
//                    string studentName = (string)studentRow["StudentName"];
//                    Console.WriteLine($"\t{studentName}: {kvp.Value}");
//                }
//            }

//            // Вывод статистики по курсам
//            Console.WriteLine("\nКоличество новых регистраций по курсам:");
//            DataTable courses = ds.Tables["Курсы"];
//            foreach (var kvp in newRegistrationsByCourse)
//            {
//                DataRow courseRow = courses.Rows.Find(kvp.Key);
//                if (courseRow != null)
//                {
//                    string courseName = (string)courseRow["CourseName"];
//                    Console.WriteLine($"\t{courseName}: {kvp.Value}");
//                }
//            }

//            // Общее количество новых регистраций
//            int totalNewRegistrations = newRegistrationsByStudent.Sum(kvp => kvp.Value);
//            Console.WriteLine($"\nОбщее количество новых регистраций: {totalNewRegistrations}");
//        }
//    }
//}


////14
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace ModifiedRegistrationsAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения DataRelation
//            CreateRelations(ds);

//            Console.WriteLine("=== АНАЛИЗ ИЗМЕНЁННЫХ РЕГИСТРАЦИЙ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Модификация нескольких регистраций
//            Console.WriteLine("МОДИФИКАЦИЯ РЕГИСТРАЦИЙ:");
//            Console.WriteLine("=====================================");
//            ModifyRegistrationGrade(ds, 1, 5.0);  // Повышаем оценку
//            ModifyRegistrationGrade(ds, 2, 3.0);  // Понижаем оценку
//            ModifyRegistrationGrade(ds, 3, 4.9);  // Оставляем почти без изменений
//            ModifyRegistrationGrade(ds, 4, 4.8);  // Повышаем оценку
//            ModifyRegistrationGrade(ds, 5, 3.5);  // Понижаем оценку
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт об изменённых регистрациях
//            Console.WriteLine("ОТЧЁТ ОБ ИЗМЕНЁННЫХ РЕГИСТРАЦИЯХ:");
//            Console.WriteLine("=====================================");
//            PrintModifiedRegistrations(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Дельта изменений
//            Console.WriteLine("ДЕЛЬТА ИЗМЕНЕНИЙ:");
//            Console.WriteLine("=====================================");
//            PrintGradeChangeDelta(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Валидация перед сохранением
//            Console.WriteLine("ВАЛИДАЦИЯ ПЕРЕД СОХРАНЕНИЕМ:");
//            Console.WriteLine("=====================================");
//            bool isValid = ValidateGrades(ds);
//            Console.WriteLine(isValid ? "Все оценки корректны." : "Обнаружены некорректные оценки.");
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Статистика изменений оценок
//            Console.WriteLine("СТАТИСТИКА ИЗМЕНЕНИЙ ОЦЕНОК:");
//            Console.WriteLine("=====================================");
//            PrintGradeChangeStatistics(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("UniversityDB");

//            // Таблица Студенты
//            DataTable students = new DataTable("Студенты");
//            students.Columns.Add("StudentID", typeof(int));
//            students.Columns.Add("StudentName", typeof(string));
//            students.Columns.Add("Email", typeof(string));
//            students.PrimaryKey = new DataColumn[] { students.Columns["StudentID"] };

//            // Таблица Курсы
//            DataTable courses = new DataTable("Курсы");
//            courses.Columns.Add("CourseID", typeof(string));
//            courses.Columns.Add("CourseName", typeof(string));
//            courses.Columns.Add("Instructor", typeof(string));
//            courses.PrimaryKey = new DataColumn[] { courses.Columns["CourseID"] };

//            // Таблица Регистрация (промежуточная)
//            DataTable registration = new DataTable("Регистрация");
//            registration.Columns.Add("RegistrationID", typeof(int));
//            registration.Columns.Add("StudentID", typeof(int));
//            registration.Columns.Add("CourseID", typeof(string));
//            registration.Columns.Add("EnrollmentDate", typeof(DateTime));
//            registration.Columns.Add("Grade", typeof(double));
//            registration.PrimaryKey = new DataColumn[] { registration.Columns["RegistrationID"] };

//            ds.Tables.Add(students);
//            ds.Tables.Add(courses);
//            ds.Tables.Add(registration);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            // Добавляем студентов
//            students.Rows.Add(101, "Иван Петров", "ivan@example.com");
//            students.Rows.Add(102, "Мария Сидорова", "maria@example.com");
//            students.Rows.Add(103, "Петр Иванов", "petr@example.com");
//            students.Rows.Add(104, "Анна Смирнова", "anna@example.com");

//            // Добавляем курсы
//            courses.Rows.Add("C001", "C# Programming", "Дмитрий Волков");
//            courses.Rows.Add("C002", "Database Design", "Светлана Морозова");
//            courses.Rows.Add("C003", "Web Development", "Алексей Новиков");
//            courses.Rows.Add("C004", "OOP Principles", "Петр Сергеев");

//            // Добавляем регистрации с оценками
//            registration.Rows.Add(1, 101, "C001", new DateTime(2025, 11, 15), 4.5);
//            registration.Rows.Add(2, 101, "C002", new DateTime(2025, 11, 20), 3.8);
//            registration.Rows.Add(3, 101, "C004", new DateTime(2025, 12, 10), 4.9);
//            registration.Rows.Add(4, 102, "C001", new DateTime(2025, 11, 15), 4.8);
//            registration.Rows.Add(5, 102, "C003", new DateTime(2025, 12, 05), 4.2);
//            registration.Rows.Add(6, 103, "C002", new DateTime(2025, 11, 20), 3.5);
//            registration.Rows.Add(7, 103, "C003", new DateTime(2025, 12, 05), 4.0);
//            registration.Rows.Add(8, 103, "C004", new DateTime(2025, 12, 10), 4.7);
//            registration.Rows.Add(9, 104, "C001", new DateTime(2025, 11, 15), 4.6);
//            registration.Rows.Add(10, 104, "C002", new DateTime(2025, 11, 20), 4.3);
//            registration.Rows.Add(11, 104, "C003", new DateTime(2025, 12, 05), 4.1);
//        }

//        // Создание отношений DataRelation
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable students = ds.Tables["Студенты"];
//            DataTable courses = ds.Tables["Курсы"];
//            DataTable registration = ds.Tables["Регистрация"];

//            try
//            {
//                // Отношение: Студенты → Регистрация (один студент → много регистраций)
//                DataRelation studentRegistrationRelation = new DataRelation(
//                    "Students_Registrations",
//                    students.Columns["StudentID"],
//                    registration.Columns["StudentID"],
//                    true);

//                // Отношение: Курсы → Регистрация (один курс → много регистраций)
//                DataRelation courseRegistrationRelation = new DataRelation(
//                    "Courses_Registrations",
//                    courses.Columns["CourseID"],
//                    registration.Columns["CourseID"],
//                    true);

//                ds.Relations.Add(studentRegistrationRelation);
//                ds.Relations.Add(courseRegistrationRelation);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            Console.WriteLine("\nРЕГИСТРАЦИИ:");
//            foreach (DataRow row in registration.Rows)
//            {
//                string state = "";
//                switch (row.RowState)
//                {
//                    case DataRowState.Added:
//                        state = " [Добавлена]";
//                        break;
//                    case DataRowState.Modified:
//                        state = " [Изменена]";
//                        break;
//                    case DataRowState.Deleted:
//                        state = " [Удалена]";
//                        break;
//                }
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["RegistrationID"]}: Студент: {row["StudentID"]}, Курс: {row["CourseID"]}, Дата: {((DateTime)row["EnrollmentDate"]).ToShortDateString()}, Оценка: {row["Grade"]}{state}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["RegistrationID", DataRowVersion.Original]}: Студент: {row["StudentID", DataRowVersion.Original]}, Курс: {row["CourseID", DataRowVersion.Original]}, Оценка: {row["Grade", DataRowVersion.Original]} [Удалена]");
//                }
//            }
//        }

//        // Модификация оценки регистрации
//        static void ModifyRegistrationGrade(DataSet ds, int registrationID, double newGrade)
//        {
//            DataTable registration = ds.Tables["Регистрация"];

//            DataRow registrationRow = registration.Rows.Find(registrationID);

//            if (registrationRow != null)
//            {
//                double oldGrade = (double)registrationRow["Grade"];
//                registrationRow.BeginEdit();
//                registrationRow["Grade"] = newGrade;
//                registrationRow.EndEdit();

//                Console.WriteLine($"Оценка регистрации {registrationID} изменена с {oldGrade:F1} на {newGrade:F1}");
//            }
//            else
//            {
//                Console.WriteLine($"Регистрация с ID {registrationID} не найдена.");
//            }
//        }

//        // Отчёт об изменённых регистрациях
//        static void PrintModifiedRegistrations(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Список изменённых регистраций:");

//            bool hasModified = false;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    hasModified = true;
//                    int registrationID = (int)row["RegistrationID"];
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"Регистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName}");
//                    Console.WriteLine($"\tКурс: {courseName}");
//                    Console.WriteLine($"\tСтарая оценка: {oldGrade:F1}");
//                    Console.WriteLine($"\tНовая оценка: {newGrade:F1}");
//                    Console.WriteLine();
//                }
//            }

//            if (!hasModified)
//            {
//                Console.WriteLine("Нет изменённых регистраций.");
//            }
//        }

//        // Дельта изменений оценок
//        static void PrintGradeChangeDelta(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            DataRelation studentRegistrationRelation = ds.Relations["Students_Registrations"];
//            DataRelation courseRegistrationRelation = ds.Relations["Courses_Registrations"];

//            Console.WriteLine("Дельта изменений оценок:");

//            bool hasChanges = false;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    hasChanges = true;
//                    int registrationID = (int)row["RegistrationID"];
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];
//                    double delta = newGrade - oldGrade;
//                    string deltaSign = delta >= 0 ? "+" : "";

//                    // Получаем информацию о студенте
//                    DataRow[] studentRows = row.GetParentRows(studentRegistrationRelation);
//                    string studentName = studentRows.Length > 0 ? (string)studentRows[0]["StudentName"] : "Неизвестен";

//                    // Получаем информацию о курсе
//                    DataRow[] courseRows = row.GetParentRows(courseRegistrationRelation);
//                    string courseName = courseRows.Length > 0 ? (string)courseRows[0]["CourseName"] : "Неизвестен";

//                    Console.WriteLine($"Регистрация ID: {registrationID}");
//                    Console.WriteLine($"\tСтудент: {studentName}");
//                    Console.WriteLine($"\tКурс: {courseName}");
//                    Console.WriteLine($"\tИзменение оценки: {oldGrade:F1} → {newGrade:F1} ({deltaSign}{delta:F1})");
//                    Console.WriteLine();
//                }
//            }

//            if (!hasChanges)
//            {
//                Console.WriteLine("Нет изменённых оценок.");
//            }
//        }

//        // Валидация оценок перед сохранением
//        static bool ValidateGrades(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            bool isValid = true;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified || row.RowState == DataRowState.Added)
//                {
//                    double grade = (double)row["Grade", DataRowVersion.Current];

//                    if (grade < 2.0 || grade > 5.0)
//                    {
//                        int registrationID = (int)row["RegistrationID"];
//                        Console.WriteLine($"Ошибка валидации: Оценка {grade:F1} для регистрации {registrationID} вне диапазона [2.0, 5.0]");
//                        isValid = false;
//                    }
//                }
//            }

//            return isValid;
//        }

//        // Статистика изменений оценок
//        static void PrintGradeChangeStatistics(DataSet ds)
//        {
//            DataTable registration = ds.Tables["Регистрация"];
//            int increasedCount = 0;
//            int decreasedCount = 0;
//            int unchangedCount = 0;

//            foreach (DataRow row in registration.Rows)
//            {
//                if (row.RowState == DataRowState.Modified)
//                {
//                    double oldGrade = (double)row["Grade", DataRowVersion.Original];
//                    double newGrade = (double)row["Grade", DataRowVersion.Current];

//                    if (newGrade > oldGrade)
//                    {
//                        increasedCount++;
//                    }
//                    else if (newGrade < oldGrade)
//                    {
//                        decreasedCount++;
//                    }
//                    else
//                    {
//                        unchangedCount++;
//                    }
//                }
//            }

//            Console.WriteLine($"Повышено оценок: {increasedCount}");
//            Console.WriteLine($"Понижено оценок: {decreasedCount}");
//            Console.WriteLine($"Без изменений: {unchangedCount}");
//            Console.WriteLine($"Всего изменений: {increasedCount + decreasedCount + unchangedCount}");
//        }
//    }
//}


////15
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace CascadeDeletionAnalysis
//{
//    class Program
//    {
//        static void Main()
//        {
//            // Создаём DataSet и таблицы
//            DataSet ds = CreateDataSet();

//            // Заполняем таблицы тестовыми данными
//            FillTestData(ds);

//            // Создаём отношения с DeleteRule.Cascade
//            CreateRelations(ds);

//            Console.WriteLine("=== КАСКАДНОЕ УДАЛЕНИЕ С ОТСЛЕЖИВАНИЕМ ===\n");

//            // Выводим начальное состояние данных
//            Console.WriteLine("НАЧАЛЬНОЕ СОСТОЯНИЕ ДАННЫХ:");
//            PrintDataSetState(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Удаляем заказчика с ID 1
//            Console.WriteLine("УДАЛЕНИЕ ЗАКАЗЧИКА С ID 1:");
//            Console.WriteLine("=====================================");
//            DeleteCustomer(ds, 1);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Анализ удалённых строк
//            Console.WriteLine("АНАЛИЗ УДАЛЁННЫХ СТРОК:");
//            Console.WriteLine("=====================================");
//            AnalyzeDeletedRows(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Отчёт об удалённых данных
//            Console.WriteLine("ОТЧЁТ ОБ УДАЛЁННЫХ ДАННЫХ:");
//            Console.WriteLine("=====================================");
//            PrintDeletionReport(ds);
//            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
//            Console.ReadKey();
//            Console.Clear();

//            // Полный откат изменений
//            Console.WriteLine("ПОЛНЫЙ ОТКАТ ИЗМЕНЕНИЙ:");
//            Console.WriteLine("=====================================");
//            RejectAllChanges(ds);
//            PrintDataSetState(ds);
//        }

//        // Создание DataSet с таблицами
//        static DataSet CreateDataSet()
//        {
//            DataSet ds = new DataSet("OrderManagement");

//            // Таблица Заказчики
//            DataTable customers = new DataTable("Заказчики");
//            customers.Columns.Add("CustomerID", typeof(int));
//            customers.Columns.Add("CustomerName", typeof(string));
//            customers.Columns.Add("Email", typeof(string));
//            customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//            // Таблица Заказы
//            DataTable orders = new DataTable("Заказы");
//            orders.Columns.Add("OrderID", typeof(int));
//            orders.Columns.Add("OrderDate", typeof(DateTime));
//            orders.Columns.Add("CustomerID", typeof(int));
//            orders.Columns.Add("Total", typeof(decimal));
//            orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//            // Таблица ДеталиЗаказов
//            DataTable orderDetails = new DataTable("ДеталиЗаказов");
//            orderDetails.Columns.Add("DetailID", typeof(int));
//            orderDetails.Columns.Add("OrderID", typeof(int));
//            orderDetails.Columns.Add("ProductID", typeof(int));
//            orderDetails.Columns.Add("Quantity", typeof(int));
//            orderDetails.Columns.Add("Price", typeof(decimal));
//            orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//            ds.Tables.Add(customers);
//            ds.Tables.Add(orders);
//            ds.Tables.Add(orderDetails);

//            return ds;
//        }

//        // Заполнение тестовыми данными
//        static void FillTestData(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            // Добавляем заказчиков
//            customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//            customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//            // Добавляем заказы
//            orders.Rows.Add(1, new DateTime(2023, 10, 15), 1, 1000.00m);
//            orders.Rows.Add(2, new DateTime(2023, 11, 20), 1, 1500.00m);
//            orders.Rows.Add(3, new DateTime(2023, 12, 05), 2, 2000.00m);

//            // Добавляем детали заказов
//            orderDetails.Rows.Add(1, 1, 101, 2, 500.00m);
//            orderDetails.Rows.Add(2, 1, 102, 1, 500.00m);
//            orderDetails.Rows.Add(3, 2, 103, 3, 500.00m);
//            orderDetails.Rows.Add(4, 2, 104, 1, 1000.00m);
//            orderDetails.Rows.Add(5, 3, 105, 1, 2000.00m);
//        }

//        // Создание отношений с DeleteRule.Cascade
//        static void CreateRelations(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            try
//            {
//                // Отношение: Заказчики → Заказы (DeleteRule=Cascade)
//                ForeignKeyConstraint customerOrderConstraint = new ForeignKeyConstraint(
//                    "FK_Customers_Orders",
//                    customers.Columns["CustomerID"],
//                    orders.Columns["CustomerID"]);

//                customerOrderConstraint.DeleteRule = Rule.Cascade;
//                orders.Constraints.Add(customerOrderConstraint);

//                // Отношение: Заказы → ДеталиЗаказов (DeleteRule=Cascade)
//                ForeignKeyConstraint orderDetailConstraint = new ForeignKeyConstraint(
//                    "FK_Orders_OrderDetails",
//                    orders.Columns["OrderID"],
//                    orderDetails.Columns["OrderID"]);

//                orderDetailConstraint.DeleteRule = Rule.Cascade;
//                orderDetails.Constraints.Add(orderDetailConstraint);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка при создании отношений: {ex.Message}");
//            }
//        }

//        // Вывод состояния DataSet
//        static void PrintDataSetState(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("\nЗАКАЗЧИКИ:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["CustomerID"]}: {row["CustomerName"]}, Email: {row["Email"]}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["CustomerID", DataRowVersion.Original]}: {row["CustomerName", DataRowVersion.Original]} [Удален]");
//                }
//            }

//            Console.WriteLine("\nЗАКАЗЫ:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["OrderID"]}: Дата: {((DateTime)row["OrderDate"]).ToShortDateString()}, Заказчик: {row["CustomerID"]}, Сумма: {row["Total"]:C}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["OrderID", DataRowVersion.Original]}: Дата: {((DateTime)row["OrderDate", DataRowVersion.Original]).ToShortDateString()}, Заказчик: {row["CustomerID", DataRowVersion.Original]}, Сумма: {row["Total", DataRowVersion.Original]:C} [Удален]");
//                }
//            }

//            Console.WriteLine("\nДЕТАЛИ ЗАКАЗОВ:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState != DataRowState.Deleted)
//                {
//                    Console.WriteLine($"{row["DetailID"]}: Заказ: {row["OrderID"]}, Продукт: {row["ProductID"]}, Кол-во: {row["Quantity"]}, Цена: {row["Price"]:C}");
//                }
//                else
//                {
//                    Console.WriteLine($"{row["DetailID", DataRowVersion.Original]}: Заказ: {row["OrderID", DataRowVersion.Original]}, Продукт: {row["ProductID", DataRowVersion.Original]}, Кол-во: {row["Quantity", DataRowVersion.Original]}, Цена: {row["Price", DataRowVersion.Original]:C} [Удалена]");
//                }
//            }
//        }

//        // Удаление заказчика
//        static void DeleteCustomer(DataSet ds, int customerID)
//        {
//            DataTable customers = ds.Tables["Заказчики"];

//            DataRow customerRow = customers.Rows.Find(customerID);

//            if (customerRow != null)
//            {
//                string customerName = (string)customerRow["CustomerName"];
//                Console.WriteLine($"Удаляем заказчика: {customerName} (ID: {customerID})");
//                customerRow.Delete();
//            }
//            else
//            {
//                Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            }
//        }

//        // Анализ удалённых строк
//        static void AnalyzeDeletedRows(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("Удалённые заказчики:");
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    string customerName = (string)row["CustomerName", DataRowVersion.Original];
//                    Console.WriteLine($"\t{customerName} (ID: {row["CustomerID", DataRowVersion.Original]})");
//                }
//            }

//            Console.WriteLine("\nУдалённые заказы:");
//            foreach (DataRow row in orders.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int orderID = (int)row["OrderID", DataRowVersion.Original];
//                    DateTime orderDate = (DateTime)row["OrderDate", DataRowVersion.Original];
//                    Console.WriteLine($"\tЗаказ {orderID} от {orderDate:dd.MM.yyyy}");
//                }
//            }

//            Console.WriteLine("\nУдалённые детали заказов:");
//            foreach (DataRow row in orderDetails.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    int detailID = (int)row["DetailID", DataRowVersion.Original];
//                    int orderID = (int)row["OrderID", DataRowVersion.Original];
//                    Console.WriteLine($"\tДеталь {detailID} для заказа {orderID}");
//                }
//            }
//        }

//        // Отчёт об удалённых данных
//        static void PrintDeletionReport(DataSet ds)
//        {
//            DataTable customers = ds.Tables["Заказчики"];
//            DataTable orders = ds.Tables["Заказы"];
//            DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//            Console.WriteLine("ПОЛНЫЙ ОТЧЁТ ОБ УДАЛЁННЫХ ДАННЫХ:");

//            // Собираем информацию об удалённых заказчиках
//            List<DataRow> deletedCustomers = new List<DataRow>();
//            foreach (DataRow row in customers.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted)
//                {
//                    deletedCustomers.Add(row);
//                }
//            }

//            // Для каждого удалённого заказчика собираем информацию о его заказах и деталях
//            foreach (DataRow customerRow in deletedCustomers)
//            {
//                int customerID = (int)customerRow["CustomerID", DataRowVersion.Original];
//                string customerName = (string)customerRow["CustomerName", DataRowVersion.Original];

//                Console.WriteLine($"\nУдалённый заказчик: {customerName} (ID: {customerID})");

//                // Собираем информацию об удалённых заказах этого заказчика
//                List<DataRow> deletedOrders = new List<DataRow>();
//                foreach (DataRow orderRow in orders.Rows)
//                {
//                    if (orderRow.RowState == DataRowState.Deleted &&
//                        (int)orderRow["CustomerID", DataRowVersion.Original] == customerID)
//                    {
//                        deletedOrders.Add(orderRow);
//                    }
//                }

//                Console.WriteLine($"\tКоличество удалённых заказов: {deletedOrders.Count}");

//                // Для каждого удалённого заказа собираем информацию о его деталях
//                foreach (DataRow orderRow in deletedOrders)
//                {
//                    int orderID = (int)orderRow["OrderID", DataRowVersion.Original];
//                    DateTime orderDate = (DateTime)orderRow["OrderDate", DataRowVersion.Original];
//                    decimal total = (decimal)orderRow["Total", DataRowVersion.Original];

//                    Console.WriteLine($"\t\tУдалённый заказ {orderID} от {orderDate:dd.MM.yyyy}, сумма: {total:C}");

//                    // Собираем информацию об удалённых деталях этого заказа
//                    List<DataRow> deletedDetails = new List<DataRow>();
//                    foreach (DataRow detailRow in orderDetails.Rows)
//                    {
//                        if (detailRow.RowState == DataRowState.Deleted &&
//                            (int)detailRow["OrderID", DataRowVersion.Original] == orderID)
//                        {
//                            deletedDetails.Add(detailRow);
//                        }
//                    }

//                    Console.WriteLine($"\t\tКоличество удалённых деталей заказа: {deletedDetails.Count}");

//                    // Выводим информацию о деталях заказа
//                    foreach (DataRow detailRow in deletedDetails)
//                    {
//                        int productID = (int)detailRow["ProductID", DataRowVersion.Original];
//                        int quantity = (int)detailRow["Quantity", DataRowVersion.Original];
//                        decimal price = (decimal)detailRow["Price", DataRowVersion.Original];

//                        Console.WriteLine($"\t\t\tДеталь: Продукт {productID}, Кол-во: {quantity}, Цена: {price:C}");
//                    }
//                }
//            }

//            // Подсчёт общего количества удалённых записей
//            int totalDeletedCustomers = customers.Select(null, null, DataViewRowState.Deleted).Length;
//            int totalDeletedOrders = orders.Select(null, null, DataViewRowState.Deleted).Length;
//            int totalDeletedDetails = orderDetails.Select(null, null, DataViewRowState.Deleted).Length;

//            Console.WriteLine($"\nОБЩАЯ СТАТИСТИКА:");
//            Console.WriteLine($"\tУдалённых заказчиков: {totalDeletedCustomers}");
//            Console.WriteLine($"\tУдалённых заказов: {totalDeletedOrders}");
//            Console.WriteLine($"\tУдалённых деталей заказов: {totalDeletedDetails}");
//        }

//        // Полный откат изменений
//        static void RejectAllChanges(DataSet ds)
//        {
//            Console.WriteLine("Откатываем все изменения...");
//            ds.RejectChanges();
//            Console.WriteLine("Все изменения отменены.");
//        }
//    }
//}


////16
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношения
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ПРОВЕРКИ ССЫЛОЧНОЙ ЦЕЛОСТНОСТИ ===\n");

//        // 1. Попытка добавить товар с несуществующей CategoryID (нарушение целостности)
//        Console.WriteLine("1. ПОПЫТКА ДОБАВИТЬ ТОВАР С НЕСУЩЕСТВУЮЩЕЙ CATEGORYID:");
//        Console.WriteLine("=====================================");
//        TryAddInvalidProduct(ds);
//        Console.WriteLine();

//        // 2. Временно отключим принуждение ограничений для введения нарушений
//        Console.WriteLine("2. ВВЕДЕНИЕ НАРУШЕНИЙ (EnforceConstraints = false):");
//        Console.WriteLine("=====================================");
//        IntroduceViolations(ds);
//        Console.WriteLine();

//        // 3. Проверка целостности и вывод отчета
//        Console.WriteLine("3. ПРОВЕРКА ЦЕЛОСТНОСТИ (CheckReferentialIntegrity):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds);
//        Console.WriteLine();

//        // 4. Исправление нарушений (удаление осиротевших записей)
//        Console.WriteLine("4. ИСПРАВЛЕНИЕ НАРУШЕНИЙ (удаление осиротевших):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds, true, true); // fix with delete
//        CheckReferentialIntegrity(ds); // check again
//        Console.WriteLine();

//        // 5. Введение нарушений снова для демонстрации установки NULL
//        IntroduceViolations(ds);

//        // 6. Исправление нарушений (установка NULL для осиротевших)
//        Console.WriteLine("5. ИСПРАВЛЕНИЕ НАРУШЕНИЙ (установка NULL):");
//        Console.WriteLine("=====================================");
//        CheckReferentialIntegrity(ds, true, false); // fix with set null
//        CheckReferentialIntegrity(ds); // check again
//        Console.WriteLine();

//        // 6. Симуляция отчета перед сохранением в БД
//        Console.WriteLine("6. ОТЧЕТ ПЕРЕД СОХРАНЕНИЕМ В БД:");
//        Console.WriteLine("=====================================");
//        SimulateSaveToDB(ds);
//        Console.WriteLine();
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Категории
//        DataTable categories = new DataTable("Категории");
//        categories.Columns.Add("CategoryID", typeof(int));
//        categories.Columns.Add("CategoryName", typeof(string));
//        categories.PrimaryKey = new DataColumn[] { categories.Columns["CategoryID"] };

//        // Таблица Товары
//        DataTable products = new DataTable("Товары");
//        products.Columns.Add("ProductID", typeof(int));
//        products.Columns.Add("ProductName", typeof(string));
//        products.Columns.Add("CategoryID", typeof(int));
//        products.Columns["CategoryID"].AllowDBNull = true; // Разрешаем NULL для демонстрации
//        products.PrimaryKey = new DataColumn[] { products.Columns["ProductID"] };

//        ds.Tables.Add(categories);
//        ds.Tables.Add(products);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable categories = ds.Tables["Категории"];
//        DataTable products = ds.Tables["Товары"];

//        // Добавляем категории
//        categories.Rows.Add(1, "Электроника");
//        categories.Rows.Add(2, "Книги");

//        // Добавляем товары
//        products.Rows.Add(1, "Ноутбук", 1);
//        products.Rows.Add(2, "Книга по C#", 2);
//    }

//    // Создание отношений
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable categories = ds.Tables["Категории"];
//        DataTable products = ds.Tables["Товары"];

//        // Отношение: Категории → Товары (один ко многим)
//        DataRelation rel = new DataRelation(
//            "FK_Products_Categories",
//            categories.Columns["CategoryID"],
//            products.Columns["CategoryID"],
//            true // Создаём ForeignKeyConstraint
//        );
//        ds.Relations.Add(rel);

//        // Настраиваем ForeignKeyConstraint
//        ForeignKeyConstraint fk = (ForeignKeyConstraint)products.Constraints["FK_Products_Categories"];
//        fk.DeleteRule = Rule.Cascade; // Для демонстрации, но не обязательно
//        fk.UpdateRule = Rule.Cascade;
//    }

//    // 1. Попытка добавить invalid товар
//    static void TryAddInvalidProduct(DataSet ds)
//    {
//        DataTable products = ds.Tables["Товары"];
//        try
//        {
//            products.Rows.Add(3, "Неверный товар", 99); // Несуществующая категория
//            Console.WriteLine("Товар добавлен без ошибки (не должно произойти)");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Нарушение целостности поймано:");
//            Console.WriteLine(ex.Message);
//        }
//    }

//    // 2. Введение нарушений (EnforceConstraints = false)
//    static void IntroduceViolations(DataSet ds)
//    {
//        ds.EnforceConstraints = false;
//        DataTable products = ds.Tables["Товары"];

//        // Добавляем осиротевший товар
//        products.Rows.Add(4, "Осиротевший товар", 100); // Несуществующая категория

//        // Добавляем товар с NULL в CategoryID
//        products.Rows.Add(5, "Товар с NULL", null);

//        Console.WriteLine("Нарушения введены (orphan и NULL)");
//    }

//    // 3. Метод проверки целостности
//    static void CheckReferentialIntegrity(DataSet ds, bool fix = false, bool deleteOrphans = true)
//    {
//        StringBuilder report = new StringBuilder();
//        report.AppendLine("Отчет о нарушениях ссылочной целостности:");
//        report.AppendLine("=========================================");

//        List<DataRow> rowsToDelete = new List<DataRow>();
//        List<(DataRow, string)> rowsToSetNull = new List<(DataRow, string)>();

//        bool hasViolations = false;

//        foreach (DataRelation rel in ds.Relations)
//        {
//            DataTable child = rel.ChildTable;
//            string fkColumnName = rel.ChildColumns[0].ColumnName;
//            string relationName = rel.RelationName;

//            report.AppendLine($"Отношение: {relationName} (Родитель: {rel.ParentTable.TableName}, Дитя: {child.TableName})");

//            foreach (DataRow row in child.Rows)
//            {
//                if (row.RowState == DataRowState.Deleted) continue;

//                object fkValue = row[fkColumnName];

//                if (fkValue == DBNull.Value || fkValue == null)
//                {
//                    hasViolations = true;
//                    report.AppendLine($" - NULL в колонке ограничения: Таблица {child.TableName}, PK {row[child.PrimaryKey[0].ColumnName]}, FK {fkColumnName} = NULL");
//                }
//                else
//                {
//                    DataRow[] parents = row.GetParentRows(relationName);
//                    if (parents.Length == 0)
//                    {
//                        hasViolations = true;
//                        report.AppendLine($" - Осиротевшая запись: Таблица {child.TableName}, PK {row[child.PrimaryKey[0].ColumnName]}, FK {fkColumnName} = {fkValue}");

//                        if (fix)
//                        {
//                            if (deleteOrphans)
//                            {
//                                rowsToDelete.Add(row);
//                            }
//                            else if (child.Columns[fkColumnName].AllowDBNull)
//                            {
//                                rowsToSetNull.Add((row, fkColumnName));
//                            }
//                            else
//                            {
//                                report.AppendLine($"   ! Невозможно установить NULL: колонка {fkColumnName} не позволяет NULL");
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        if (!hasViolations)
//        {
//            report.AppendLine("Нарушений не найдено.");
//        }

//        Console.WriteLine(report.ToString());

//        if (fix)
//        {
//            // Исправляем
//            foreach (DataRow row in rowsToDelete)
//            {
//                row.Delete();
//                Console.WriteLine($"Удалена осиротевшая запись: Таблица {row.Table.TableName}, PK {row[row.Table.PrimaryKey[0].ColumnName, DataRowVersion.Original]}");
//            }

//            foreach (var (row, colName) in rowsToSetNull)
//            {
//                row[colName] = DBNull.Value;
//                Console.WriteLine($"Установлен NULL для осиротевшей записи: Таблица {row.Table.TableName}, PK {row[row.Table.PrimaryKey[0].ColumnName]}");
//            }
//        }
//    }

//    // 6. Симуляция сохранения в БД с проверкой
//    static void SimulateSaveToDB(DataSet ds)
//    {
//        // Перед "сохранением" проверяем
//        CheckReferentialIntegrity(ds);

//        // Симулируем сохранение (в реальности - адаптеры и т.д.)
//        try
//        {
//            ds.EnforceConstraints = true; // Включаем обратно для симуляции
//            ds.AcceptChanges();
//            Console.WriteLine("Данные 'сохранены' в БД без нарушений.");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine("Ошибка при 'сохранении' в БД:");
//            Console.WriteLine(ex.Message);
//        }
//        finally
//        {
//            ds.EnforceConstraints = false; // Сбрасываем для дальнейших тестов, если нужно
//        }
//    }
//}


////17
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношение
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ФИЛЬТРАЦИИ ДАННЫХ ЧЕРЕЗ ОТНОШЕНИЯ ===\n");

//        // 1. Все заказы конкретного заказчика
//        Console.WriteLine("1. ВСЕ ЗАКАЗЫ КОНКРЕТНОГО ЗАКАЗЧИКА:");
//        Console.WriteLine("=====================================");
//        GetOrdersForCustomer(ds, 1);
//        Console.WriteLine();

//        // 2. Заказы, сделанные после определенной даты
//        Console.WriteLine("2. ЗАКАЗЫ ПОСЛЕ ОПРЕДЕЛЕННОЙ ДАТЫ:");
//        Console.WriteLine("=====================================");
//        DateTime afterDate = new DateTime(2023, 6, 1);
//        GetOrdersAfterDate(ds, afterDate);
//        Console.WriteLine();

//        // 3. Заказы на сумму больше указанной
//        Console.WriteLine("3. ЗАКАЗЫ НА СУММУ БОЛЬШЕ УКАЗАННОЙ:");
//        Console.WriteLine("=====================================");
//        decimal minAmount = 150.00m;
//        GetOrdersAboveAmount(ds, minAmount);
//        Console.WriteLine();

//        // 4. Комбинированные фильтры (заказы клиента X после даты Y с суммой > Z)
//        Console.WriteLine("4. КОМБИНИРОВАННЫЕ ФИЛЬТРЫ:");
//        Console.WriteLine("=====================================");
//        int customerId = 1;
//        DateTime dateY = new DateTime(2023, 4, 1);
//        decimal amountZ = 100.00m;
//        GetFilteredOrders(ds, customerId, dateY, amountZ);
//        Console.WriteLine();

//        // 5. Сортировка результатов (например, заказы по дате descending)
//        Console.WriteLine("5. ЗАКАЗЫ С СОРТИРОВКОЙ ПО ДАТЕ (DESC):");
//        Console.WriteLine("=====================================");
//        GetSortedOrders(ds, "OrderDate DESC");
//        Console.WriteLine();

//        // 6. Обработка пустых результатов
//        Console.WriteLine("6. ПРИМЕР ПУСТОГО РЕЗУЛЬТАТА:");
//        Console.WriteLine("=====================================");
//        GetOrdersForCustomer(ds, 999); // Несуществующий заказчик
//        Console.WriteLine();

//        // Для отображения в DataGridView (пример, как привязать DataView к grid)
//        // В реальном Windows Forms приложении:
//        // DataGridView grid = new DataGridView();
//        // DataView dv = new DataView(ds.Tables["Заказы"]);
//        // grid.DataSource = dv;
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Заказчики
//        DataTable customers = new DataTable("Заказчики");
//        customers.Columns.Add("CustomerID", typeof(int));
//        customers.Columns.Add("Name", typeof(string));
//        customers.Columns.Add("Email", typeof(string));
//        customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//        // Таблица Заказы
//        DataTable orders = new DataTable("Заказы");
//        orders.Columns.Add("OrderID", typeof(int));
//        orders.Columns.Add("CustomerID", typeof(int));
//        orders.Columns.Add("OrderDate", typeof(DateTime));
//        orders.Columns.Add("Amount", typeof(decimal));
//        orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//        ds.Tables.Add(customers);
//        ds.Tables.Add(orders);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        // Добавляем заказчиков
//        customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//        customers.Rows.Add(2, "Мария Петрова", "maria@example.com");
//        customers.Rows.Add(3, "Петр Сидоров", "petr@example.com");

//        // Добавляем заказы
//        orders.Rows.Add(1, 1, new DateTime(2025, 1, 15), 100.00m);
//        orders.Rows.Add(2, 1, new DateTime(2025, 5, 20), 200.00m);
//        orders.Rows.Add(3, 2, new DateTime(2025, 3, 10), 150.00m);
//        orders.Rows.Add(4, 2, new DateTime(2025, 7, 5), 300.00m);
//        orders.Rows.Add(5, 3, new DateTime(2025, 2, 25), 120.00m);
//        orders.Rows.Add(6, 3, new DateTime(2025, 8, 15), 250.00m);
//    }

//    // Создание отношения
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        // Отношение: Заказчики → Заказы (один заказчик → много заказов)
//        DataRelation rel = new DataRelation(
//            "FK_Customers_Orders",
//            customers.Columns["CustomerID"],
//            orders.Columns["CustomerID"],
//            true
//        );
//        ds.Relations.Add(rel);
//    }

//    // 1. Все заказы конкретного заказчика (используя GetChildRows)
//    static void GetOrdersForCustomer(DataSet ds, int customerID)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];

//        DataRow customerRow = customers.Rows.Find(customerID);

//        if (customerRow == null)
//        {
//            Console.WriteLine($"Заказчик с ID {customerID} не найден.");
//            return;
//        }

//        Console.WriteLine($"Заказчик: {customerRow["Name"]}");
//        Console.WriteLine($"Email: {customerRow["Email"]}");
//        Console.WriteLine("\nЗаказы:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        DataRow[] orderRows = customerRow.GetChildRows("FK_Customers_Orders");

//        if (orderRows.Length == 0)
//        {
//            Console.WriteLine("У заказчика нет заказов.");
//            return;
//        }

//        foreach (DataRow orderRow in orderRows)
//        {
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine();
//        }
//    }

//    // 2. Заказы после определенной даты (используя DataView)
//    static void GetOrdersAfterDate(DataSet ds, DateTime afterDate)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"OrderDate > '#{afterDate:MM/dd/yyyy}#'"; // Формат для RowFilter

//        Console.WriteLine($"Заказы после {afterDate:dd.MM.yyyy}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов после указанной даты.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }

//    // 3. Заказы на сумму больше указанной (используя DataView)
//    static void GetOrdersAboveAmount(DataSet ds, decimal minAmount)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"Amount > {minAmount.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

//        Console.WriteLine($"Заказы с суммой > {minAmount:F2}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов с суммой больше указанной.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }

//    // 4. Комбинированные фильтры (заказы клиента X после даты Y с суммой > Z)
//    static void GetFilteredOrders(DataSet ds, int customerID, DateTime dateY, decimal amountZ)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.RowFilter = $"CustomerID = {customerID} AND OrderDate > '#{dateY:MM/dd/yyyy}#' AND Amount > {amountZ.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

//        Console.WriteLine($"Заказы клиента {customerID} после {dateY:dd.MM.yyyy} с суммой > {amountZ:F2}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов, удовлетворяющих условиям.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine();
//        }
//    }

//    // 5. Заказы с сортировкой (используя DataView.Sort)
//    static void GetSortedOrders(DataSet ds, string sortExpression)
//    {
//        DataTable orders = ds.Tables["Заказы"];

//        DataView dv = new DataView(orders);
//        dv.Sort = sortExpression;

//        Console.WriteLine($"Все заказы, отсортированные по {sortExpression}:");
//        Console.WriteLine("─────────────────────────────────────────────────");

//        if (dv.Count == 0)
//        {
//            Console.WriteLine("Нет заказов.");
//            return;
//        }

//        foreach (DataRowView rowView in dv)
//        {
//            DataRow orderRow = rowView.Row;
//            Console.WriteLine($" • Заказ ID: {orderRow["OrderID"]}");
//            Console.WriteLine($"   Дата: {(DateTime)orderRow["OrderDate"]:dd.MM.yyyy}");
//            Console.WriteLine($"   Сумма: {orderRow["Amount"]:F2}");
//            Console.WriteLine($"   Заказчик ID: {orderRow["CustomerID"]}");
//            Console.WriteLine();
//        }
//    }
//}


////20
//using System;
//using System.Data;
//using System.IO;
//using System.Text;

//class Program
//{
//    static void Main()
//    {
//        // Создаём DataSet и таблицы
//        DataSet ds = CreateDataSet();

//        // Заполняем таблицы тестовыми данными
//        FillTestData(ds);

//        // Создаём отношения
//        CreateRelationships(ds);

//        Console.WriteLine("=== ДЕМОНСТРАЦИЯ ЭКСПОРТА ИЕРАРХИЧЕСКИХ ДАННЫХ ===\n");

//        // 1. Экспорт в XML с сохранением иерархии
//        Console.WriteLine("1. ЭКСПОРТ В XML:");
//        Console.WriteLine("=====================================");
//        ExportToXml(ds, "hierarchy.xml");
//        Console.WriteLine();

//        // 2. Рекурсивный обход иерархии и вывод в консоль
//        Console.WriteLine("2. РЕКУРСИВНЫЙ ОБХОД ИЕРАРХИИ:");
//        Console.WriteLine("=====================================");
//        TraverseHierarchy(ds.Tables["Заказчики"], 0);
//        Console.WriteLine();

//        // 3. Экспорт в CSV с иерархией (индентированный)
//        Console.WriteLine("3. ЭКСПОРТ В CSV С ИЕРАРХИЕЙ:");
//        Console.WriteLine("=====================================");
//        ExportToHierarchicalCsv(ds.Tables["Заказчики"], "hierarchy.csv");
//        Console.WriteLine();

//        // 4. Экспорт в JSON с иерархией
//        Console.WriteLine("4. ЭКСПОРТ В JSON:");
//        Console.WriteLine("=====================================");
//        string json = ExportToJson(ds.Tables["Заказчики"]);
//        File.WriteAllText("hierarchy.json", json);
//        Console.WriteLine("Экспортировано в hierarchy.json");
//        Console.WriteLine("Фрагмент JSON:");
//        Console.WriteLine(json.Substring(0, Math.Min(200, json.Length)) + "...");
//        Console.WriteLine();

//        // 5. Импорт из XML обратно в DataSet
//        Console.WriteLine("5. ИМПОРТ ИЗ XML:");
//        Console.WriteLine("=====================================");
//        DataSet importedDs = ImportFromXml("hierarchy.xml");
//        if (importedDs != null)
//        {
//            Console.WriteLine("Импорт успешен. Проверка данных:");
//            TraverseHierarchy(importedDs.Tables["Заказчики"], 0);
//        }
//        Console.WriteLine();

//        // 6. Обработка ошибок (пример с несуществующим файлом)
//        Console.WriteLine("6. ОБРАБОТКА ОШИБОК:");
//        Console.WriteLine("=====================================");
//        ImportFromXml("nonexistent.xml");
//        Console.WriteLine();
//    }

//    // Создание DataSet с таблицами
//    static DataSet CreateDataSet()
//    {
//        DataSet ds = new DataSet("ShopDB");

//        // Таблица Заказчики
//        DataTable customers = new DataTable("Заказчики");
//        customers.Columns.Add("CustomerID", typeof(int));
//        customers.Columns.Add("Name", typeof(string));
//        customers.Columns.Add("Email", typeof(string));
//        customers.PrimaryKey = new DataColumn[] { customers.Columns["CustomerID"] };

//        // Таблица Заказы
//        DataTable orders = new DataTable("Заказы");
//        orders.Columns.Add("OrderID", typeof(int));
//        orders.Columns.Add("CustomerID", typeof(int));
//        orders.Columns.Add("OrderDate", typeof(DateTime));
//        orders.Columns.Add("Amount", typeof(decimal));
//        orders.PrimaryKey = new DataColumn[] { orders.Columns["OrderID"] };

//        // Таблица Детали Заказов
//        DataTable orderDetails = new DataTable("ДеталиЗаказов");
//        orderDetails.Columns.Add("DetailID", typeof(int));
//        orderDetails.Columns.Add("OrderID", typeof(int));
//        orderDetails.Columns.Add("ProductName", typeof(string));
//        orderDetails.Columns.Add("Quantity", typeof(int));
//        orderDetails.Columns.Add("Price", typeof(decimal));
//        orderDetails.PrimaryKey = new DataColumn[] { orderDetails.Columns["DetailID"] };

//        ds.Tables.Add(customers);
//        ds.Tables.Add(orders);
//        ds.Tables.Add(orderDetails);

//        return ds;
//    }

//    // Заполнение тестовыми данными
//    static void FillTestData(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];
//        DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//        // Добавляем заказчиков
//        customers.Rows.Add(1, "Иван Иванов", "ivan@example.com");
//        customers.Rows.Add(2, "Мария Петрова", "maria@example.com");

//        // Добавляем заказы
//        orders.Rows.Add(1, 1, new DateTime(2025, 1, 15), 100.00m);
//        orders.Rows.Add(2, 1, new DateTime(2025, 5, 20), 200.00m);
//        orders.Rows.Add(3, 2, new DateTime(2025, 3, 10), 150.00m);

//        // Добавляем детали заказов
//        orderDetails.Rows.Add(1, 1, "Товар A", 2, 30.00m);
//        orderDetails.Rows.Add(2, 1, "Товар B", 1, 40.00m);
//        orderDetails.Rows.Add(3, 2, "Товар C", 3, 50.00m);
//        orderDetails.Rows.Add(4, 3, "Товар D", 1, 150.00m);
//    }

//    // Создание отношений
//    static void CreateRelationships(DataSet ds)
//    {
//        DataTable customers = ds.Tables["Заказчики"];
//        DataTable orders = ds.Tables["Заказы"];
//        DataTable orderDetails = ds.Tables["ДеталиЗаказов"];

//        // Отношение: Заказчики → Заказы
//        DataRelation rel1 = new DataRelation(
//            "FK_Customers_Orders",
//            customers.Columns["CustomerID"],
//            orders.Columns["CustomerID"],
//            true
//        );
//        ds.Relations.Add(rel1);

//        // Отношение: Заказы → ДеталиЗаказов
//        DataRelation rel2 = new DataRelation(
//            "FK_Orders_OrderDetails",
//            orders.Columns["OrderID"],
//            orderDetails.Columns["OrderID"],
//            true
//        );
//        ds.Relations.Add(rel2);
//    }

//    // Экспорт в XML с сохранением схемы и отношений
//    static void ExportToXml(DataSet ds, string filePath)
//    {
//        try
//        {
//            ds.WriteXml(filePath, XmlWriteMode.WriteSchema);
//            Console.WriteLine($"Данные экспортированы в XML: {filePath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка экспорта в XML: {ex.Message}");
//        }
//    }

//    // Рекурсивный метод для обхода иерархии (для консольного вывода)
//    static void TraverseHierarchy(DataTable table, int level)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            string indent = new string(' ', level * 2);
//            Console.WriteLine($"{indent}Таблица: {table.TableName}");
//            foreach (DataColumn col in table.Columns)
//            {
//                Console.WriteLine($"{indent}  {col.ColumnName}: {row[col]}");
//            }
//            Console.WriteLine();

//            // Получаем дочерние отношения
//            foreach (DataRelation rel in table.ChildRelations)
//            {
//                DataRow[] childRows = row.GetChildRows(rel);
//                foreach (DataRow childRow in childRows)
//                {
//                    TraverseHierarchyRecursive(childRow, level + 1, rel.ChildTable);
//                }
//            }
//        }
//    }

//    static void TraverseHierarchyRecursive(DataRow row, int level, DataTable table)
//    {
//        string indent = new string(' ', level * 2);
//        Console.WriteLine($"{indent}Таблица: {table.TableName}");
//        foreach (DataColumn col in table.Columns)
//        {
//            Console.WriteLine($"{indent}  {col.ColumnName}: {row[col]}");
//        }
//        Console.WriteLine();

//        // Рекурсия для следующих уровней
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            foreach (DataRow childRow in childRows)
//            {
//                TraverseHierarchyRecursive(childRow, level + 1, rel.ChildTable);
//            }
//        }
//    }

//    // Экспорт в CSV с иерархией (индентированный текст)
//    static void ExportToHierarchicalCsv(DataTable table, string filePath)
//    {
//        try
//        {
//            StringBuilder sb = new StringBuilder();
//            BuildHierarchicalCsv(table, 0, sb);
//            File.WriteAllText(filePath, sb.ToString());
//            Console.WriteLine($"Данные экспортированы в CSV: {filePath}");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка экспорта в CSV: {ex.Message}");
//        }
//    }

//    static void BuildHierarchicalCsv(DataTable table, int level, StringBuilder sb)
//    {
//        foreach (DataRow row in table.Rows)
//        {
//            string indent = new string(' ', level * 2);
//            sb.AppendLine($"{indent}Таблица: {table.TableName}");
//            foreach (DataColumn col in table.Columns)
//            {
//                sb.AppendLine($"{indent}  {col.ColumnName},{row[col]}");
//            }
//            sb.AppendLine();

//            // Дочерние
//            foreach (DataRelation rel in table.ChildRelations)
//            {
//                DataRow[] childRows = row.GetChildRows(rel);
//                foreach (DataRow childRow in childRows)
//                {
//                    BuildHierarchicalCsvRecursive(childRow, level + 1, rel.ChildTable, sb);
//                }
//            }
//        }
//    }

//    static void BuildHierarchicalCsvRecursive(DataRow row, int level, DataTable table, StringBuilder sb)
//    {
//        string indent = new string(' ', level * 2);
//        sb.AppendLine($"{indent}Таблица: {table.TableName}");
//        foreach (DataColumn col in table.Columns)
//        {
//            sb.AppendLine($"{indent}  {col.ColumnName},{row[col]}");
//        }
//        sb.AppendLine();

//        // Рекурсия
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            foreach (DataRow childRow in childRows)
//            {
//                BuildHierarchicalCsvRecursive(childRow, level + 1, rel.ChildTable, sb);
//            }
//        }
//    }

//    // Экспорт в JSON (рекурсивный)
//    static string ExportToJson(DataTable table)
//    {
//        StringBuilder sb = new StringBuilder();
//        sb.AppendLine("[");
//        bool first = true;
//        foreach (DataRow row in table.Rows)
//        {
//            if (!first) sb.AppendLine(",");
//            first = false;
//            BuildJsonRecursive(row, table, sb, 0);
//        }
//        sb.AppendLine("]");
//        return sb.ToString();
//    }

//    static void BuildJsonRecursive(DataRow row, DataTable table, StringBuilder sb, int level)
//    {
//        string indent = new string(' ', level * 2);
//        sb.Append($"{indent}{{");
//        bool firstCol = true;
//        foreach (DataColumn col in table.Columns)
//        {
//            if (!firstCol) sb.Append(",");
//            firstCol = false;
//            string value = row[col].ToString().Replace("\"", "\\\"");
//            sb.Append($"\"{col.ColumnName}\":\"{value}\"");
//        }

//        // Дочерние таблицы
//        foreach (DataRelation rel in table.ChildRelations)
//        {
//            DataRow[] childRows = row.GetChildRows(rel);
//            if (childRows.Length > 0)
//            {
//                sb.Append($",\"{rel.ChildTable.TableName}\":[");
//                bool firstChild = true;
//                foreach (DataRow childRow in childRows)
//                {
//                    if (!firstChild) sb.Append(",");
//                    firstChild = false;
//                    BuildJsonRecursive(childRow, rel.ChildTable, sb, level + 1);
//                }
//                sb.Append("]");
//            }
//        }
//        sb.Append("}");
//    }

//    // Импорт из XML
//    static DataSet ImportFromXml(string filePath)
//    {
//        try
//        {
//            DataSet ds = new DataSet();
//            ds.ReadXml(filePath, XmlReadMode.ReadSchema);
//            Console.WriteLine($"Данные импортированы из XML: {filePath}");
//            return ds;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Ошибка импорта из XML: {ex.Message}");
//            return null;
//        }
//    }
//}