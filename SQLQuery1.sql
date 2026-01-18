CREATE DATABASE TestDB;
GO

USE TestDB;
GO

CREATE TABLE Employees (
    EmployeeID int IDENTITY(1,1) PRIMARY KEY,
    FirstName nvarchar(50) NULL,
    LastName nvarchar(50) NULL,
    Salary decimal(12,2) NULL,
    BirthDate date NULL,
    PhoneNumber varchar(20) NULL,
    IsActive bit NULL,
    Department nvarchar(100) NULL,
    ManagerID int NULL,
    HireDate date NULL
);
GO

-- Добавим тестовые данные с кучей NULL
INSERT INTO Employees (FirstName, LastName, Salary, BirthDate, PhoneNumber, IsActive, Department, ManagerID, HireDate) VALUES
('Петя', 'Петров', 95000.00, '1990-05-12', '+7(999)111-22-33', 1, 'Разработка', NULL, '2021-03-15'),
('Вера', NULL, NULL, NULL, NULL, 0, NULL, 1, NULL),
('Вика', 'Смирнова', 120000.00, '1988-11-30', NULL, 1, 'Аналитика', 1, '2019-07-20'),
('Никита', 'Пупкин', 78000.00, '1995-02-28', '+7(915)555-44-33', 1, 'Тестирование', 2, '2023-01-10'),
(NULL, 'Иванов', 60000.00, '1980-09-05', '84951234567', NULL, 'HR', NULL, '2017-11-01');
GO