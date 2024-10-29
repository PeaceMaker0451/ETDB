using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    public class DBManager
    {
        private readonly string _connectionString;

        public DBManager(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabaseAndTables();
        }

        // Метод для проверки и создания базы данных и таблиц
        private void EnsureDatabaseAndTables()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // SQL-запрос для создания таблиц
                command.CommandText = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeesEventsDB')
                BEGIN
                    CREATE DATABASE EmployeesEventsDB;
                END;
                USE EmployeesEventsDB;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Statuses' and xtype='U')
                BEGIN
                    CREATE TABLE Statuses (
                        StatusID INT PRIMARY KEY IDENTITY,
                        StatusName NVARCHAR(50) NOT NULL
                    );

                    -- Вставка начальных значений статусов
                    INSERT INTO Statuses (StatusName) VALUES 
                    ('Не нанят'), 
                    ('Нанят'), 
                    ('Уволен'), 
                    ('Переведен');
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='JobTitles' and xtype='U')
                BEGIN
                    CREATE TABLE JobTitles (
                        JobTitleID INT PRIMARY KEY IDENTITY,
                        Title NVARCHAR(100) NOT NULL
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' and xtype='U')
                BEGIN
                    CREATE TABLE Employees (
                        EmployeeID INT PRIMARY KEY IDENTITY,
                        JobTitleID INT FOREIGN KEY REFERENCES JobTitles(JobTitleID),
                        FullName NVARCHAR(100) NOT NULL,
                        HireDate DATE NULL,
                        StatusID INT FOREIGN KEY REFERENCES Statuses(StatusID)  -- Внешний ключ для статуса
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='JobTitlesEvents' and xtype='U')
                BEGIN
                    CREATE TABLE JobTitlesEvents (
                        EventID INT PRIMARY KEY IDENTITY,
                        JobTitleID INT FOREIGN KEY REFERENCES JobTitles(JobTitleID),
                        EventName NVARCHAR(100) NOT NULL,
                        Type NVARCHAR(50) NOT NULL,
                        FrequencyDays INT NOT NULL,
                        NextEventDate DATE
                    );
                END;

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeesEvents' and xtype='U')
                BEGIN
                    CREATE TABLE EmployeesEvents (
                        EventID INT PRIMARY KEY IDENTITY,
                        EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                        EventName NVARCHAR(100) NOT NULL,
                        Type NVARCHAR(50) NOT NULL,
                        FrequencyDays INT NOT NULL,
                        NextEventDate DATE
                    );
                END;";

                command.ExecuteNonQuery();
            }
        }

        // Метод для добавления нового статуса
        public int AddStatus(string statusName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Statuses (StatusName) OUTPUT INSERTED.StatusID VALUES (@StatusName);", connection);
                command.Parameters.AddWithValue("@StatusName", statusName);
                return (int)command.ExecuteScalar();
            }
        }

        // Метод для получения всех статусов
        public DataTable GetAllStatuses()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Statuses", connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Метод для добавления новой должности
        public int AddJobTitle(string title)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO JobTitles (Title) OUTPUT INSERTED.JobTitleID VALUES (@Title);", connection);
                command.Parameters.AddWithValue("@Title", title);
                return (int)command.ExecuteScalar();
            }
        }

        // Метод для добавления нового работника
        public int AddEmployee(int jobTitleId, string fullName, DateTime hireDate, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Employees (JobTitleID, FullName, HireDate, StatusID) OUTPUT INSERTED.EmployeeID VALUES (@JobTitleID, @FullName, @HireDate, @StatusID);", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@HireDate", hireDate);
                command.Parameters.AddWithValue("@StatusID", statusId);  // Используем ID статуса
                return (int)command.ExecuteScalar();
            }
        }

        // Метод для получения всех работников с информацией о статусах
        public DataTable GetAllEmployees()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT e.EmployeeID, e.FullName, e.HireDate, 
                   j.Title AS JobTitle, s.StatusName AS Status
            FROM Employees e
            JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
            JOIN Statuses s ON e.StatusID = s.StatusID", connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Метод для редактирования работника
        public void UpdateEmployee(int employeeId, int jobTitleId, string fullName, DateTime hireDate, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                UPDATE Employees
                SET JobTitleID = @JobTitleID,
                    FullName = @FullName,
                    HireDate = @HireDate,
                    StatusID = @StatusID
                WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@HireDate", hireDate);
                command.Parameters.AddWithValue("@StatusID", statusId);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.ExecuteNonQuery();
            }
        }

        // Метод для удаления работника
        public void DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.ExecuteNonQuery();
            }
        }

        // Метод для редактирования должности
        public void UpdateJobTitle(int jobTitleId, string newTitle)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE JobTitles SET Title = @Title WHERE JobTitleID = @JobTitleID", connection);
                command.Parameters.AddWithValue("@Title", newTitle);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.ExecuteNonQuery();
            }
        }

        // Метод для удаления должности
        public void DeleteJobTitle(int jobTitleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM JobTitles WHERE JobTitleID = @JobTitleID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.ExecuteNonQuery();
            }
        }
    }
}
