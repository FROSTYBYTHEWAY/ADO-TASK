[README_ADO-TASK.md](https://github.com/user-attachments/files/26333372/README_ADO-TASK.md)
# ADO-TASK — C# + ADO.NET + MS SQL Server

Учебный проект по подключению C#-приложений к базам данных через ADO.NET.  
Курс «Разработчик программного обеспечения» · Академия ТОП · 3 курс

---

## О проекте

Практические задания по работе с ADO.NET — технологии доступа к данным в .NET-приложениях. Проект демонстрирует навыки подключения к SQL Server, выполнения запросов и управления данными из кода на C#.

## Темы

- Подключение к MS SQL Server через `SqlConnection`
- Выполнение запросов: `SqlCommand`, `ExecuteReader`, `ExecuteNonQuery`, `ExecuteScalar`
- Чтение данных через `SqlDataReader`
- Работа с таблицами и столбцами через ADO.NET
- Подготовка к экзаменационным билетам (Билет 7)

## Технологии

![C#](https://img.shields.io/badge/C%23-.NET-blue)
![ADO.NET](https://img.shields.io/badge/ADO.NET-Data%20Access-orange)
![SQL Server](https://img.shields.io/badge/MS%20SQL%20Server-Database-red)

| Технология    | Описание                        |
|---------------|---------------------------------|
| C#            | Язык программирования           |
| ADO.NET       | Технология доступа к данным     |
| MS SQL Server | СУБД                            |
| T-SQL         | Язык запросов                   |
| Visual Studio | IDE                             |

## Структура репозитория

```
📄 02-12.cs / 03-12.cs / 04-12.cs  — задания по датам (декабрь)
📄 BILET-7-NOMER-1.cs              — экзаменационный билет 7, задание 1
📄 BILET-7-NOMER-2.cs              — экзаменационный билет 7, задание 2
📄 SQLQuery1.sql                   — SQL-скрипты для создания тестовой БД
📄 Вопросы Билет 7.docx            — теоретические вопросы к экзамену
```

## Запуск

1. Клонировать репозиторий:
   ```bash
   git clone https://github.com/FROSTYBYTHEWAY/ADO-TASK.git
   ```
2. Открыть `.cs`-файл в Visual Studio
3. В строке подключения указать свой сервер:
   ```csharp
   string connectionString = "Server=localhost;Database=ИМЯ_БД;Trusted_Connection=True;";
   ```
4. Запустить через `F5`

## Навыки

В ходе работы отработаны: подключение C#-приложения к реляционной БД, выполнение CRUD-операций через ADO.NET, работа с `SqlDataReader`, параметризованные запросы, базовое проектирование схемы БД.

---

> Студент: FROSTYBTW · [GitHub](https://github.com/FROSTYBYTHEWAY)
