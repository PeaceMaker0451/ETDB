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

                INSERT INTO Statuses (StatusName) VALUES 
                ('Не принят'), 
                ('Принят'), 
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
                    MedDate DATE NULL,
                    StatusID INT FOREIGN KEY REFERENCES Statuses(StatusID)
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeesEvents' and xtype='U')
            BEGIN
                CREATE TABLE EmployeesEvents (
                    EventID INT PRIMARY KEY IDENTITY,
                    EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                    EventName NVARCHAR(50),
                    StartDate DATE NOT NULL,
                    ToNext INT NOT NULL,
                    IsMonths BIT NOT NULL
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TitlesEvents' and xtype='U')
            BEGIN
                CREATE TABLE TitlesEvents (
                    EventID INT PRIMARY KEY IDENTITY,
                    JobTitleID INT FOREIGN KEY REFERENCES JobTitles(JobTitleID),
                    EventName NVARCHAR(50),
                    ToNext INT NOT NULL,
                    IsMonths BIT NOT NULL
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TitlesEventsDates' and xtype='U')
            BEGIN
                CREATE TABLE TitlesEventsDates (
                    EventID INT FOREIGN KEY REFERENCES TitlesEvents(EventID),
                    EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                    StartDate DATE NOT NULL,
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeeAttributes' and xtype='U')
            BEGIN
                CREATE TABLE EmployeeAttributes (
                    EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                    AttributeName NVARCHAR(50),
                    AttributeValue NVARCHAR(100),
                    PRIMARY KEY (EmployeeID, AttributeName)
                );
            END;";

                command.ExecuteNonQuery();
            }
        }

        public DataTable GetAllEmployeeEventsTable(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT EventID, EventName, StartDate, ToNext, IsMonths
            FROM EmployeesEvents
            WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public DataTable GetEmployeeEventTable(int employeeId, int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT EventID, EventName, StartDate, ToNext, IsMonths
            FROM EmployeesEvents
            WHERE EmployeeID = @EmployeeID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public int AddEmployeeEvent(int employeeId, string eventName, DateTime startDate, int timeToNext, bool isMonths)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            INSERT INTO EmployeesEvents (EmployeeID, EventName, StartDate, ToNext, IsMonths)
            OUTPUT INSERTED.EventID
            VALUES (@EmployeeID, @EventName, @StartDate, @ToNext, @IsMonths)", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);

                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateEmployeeEvent(int employeeId, int eventId, string eventName, DateTime startDate, int timeToNext, bool isMonths)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            UPDATE EmployeesEvents
            SET EventName = @EventName, StartDate = @StartDate, ToNext = @ToNext, IsMonths = @IsMonths
            WHERE EmployeeID = @EmployeeID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);

                command.ExecuteNonQuery();
            }
        }

        public DataTable GetAllJobTitleEventsTable(int jobTitleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT EventID, EventName, ToNext, IsMonths
            FROM TitlesEvents
            WHERE JobTitleID = @JobTitleID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public DataTable GetJobTitleEventTable(int jobTitleId, int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT EventID, EventName, ToNext, IsMonths
            FROM TitlesEvents
            WHERE JobTitleID = @JobTitleID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@EventID", eventId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public DataTable GetAllEmployeeJobTitleEventsTable(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT te.EventID, 
                   ted.StartDate, 
                   te.EventName, 
                   te.ToNext, 
                   te.IsMonths
            FROM TitlesEvents te
            LEFT JOIN TitlesEventsDates ted 
                ON te.EventID = ted.EventID AND ted.EmployeeID = @EmployeeID
            WHERE te.JobTitleID = (SELECT JobTitleID FROM Employees WHERE EmployeeID = @EmployeeID)",
                    connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public DataTable GetEmployeeJobTitleEventTable(int employeeId, int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            SELECT ted.EventID, ted.StartDate, te.EventName, te.ToNext, te.IsMonths
            FROM TitlesEventsDates ted
            JOIN TitlesEvents te ON ted.EventID = te.EventID
            WHERE ted.EmployeeID = @EmployeeID AND ted.EventID = @EventID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public int AddJobTitleEvent(int jobTitleId, string eventName, int timeToNext, bool isMonths)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            INSERT INTO TitlesEvents (JobTitleID, EventName, ToNext, IsMonths)
            OUTPUT INSERTED.EventID
            VALUES (@JobTitleID, @EventName, @ToNext, @IsMonths)", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);

                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateJobTitleEvent(int jobTitleId, int eventId, string eventName, int timeToNext, bool isMonths)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            UPDATE TitlesEvents
            SET EventName = @EventName, ToNext = @ToNext, IsMonths = @IsMonths
            WHERE JobTitleID = @JobTitleID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);

                command.ExecuteNonQuery();
            }
        }

        public void SetJobTitleEventDate(int employeeId, int eventId, DateTime startDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            IF EXISTS (SELECT 1 FROM TitlesEventsDates WHERE EmployeeID = @EmployeeID AND EventID = @EventID)
                UPDATE TitlesEventsDates SET StartDate = @StartDate WHERE EmployeeID = @EmployeeID AND EventID = @EventID
            ELSE
                INSERT INTO TitlesEventsDates (EmployeeID, EventID, StartDate) VALUES (@EmployeeID, @EventID, @StartDate)", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@StartDate", startDate);

                command.ExecuteNonQuery();
            }
        }

        // Метод для добавления атрибута для сотрудника
        public void AddOrUpdateEmployeeAttribute(int employeeId, string attributeName, string attributeValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Проверяем, существует ли сотрудник с указанным ID
                var checkEmployeeCommand = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE EmployeeID = @EmployeeID", connection);
                checkEmployeeCommand.Parameters.AddWithValue("@EmployeeID", employeeId);

                int employeeExists = (int)checkEmployeeCommand.ExecuteScalar();
                if (employeeExists == 0)
                {
                    throw new InvalidOperationException("Сотрудник с указанным ID не существует.");
                }

                // Если сотрудник существует, выполняем MERGE для обновления или добавления атрибута
                var command = new SqlCommand(@"
        MERGE EmployeeAttributes AS target
        USING (SELECT @EmployeeID AS EmployeeID, @AttributeName AS AttributeName) AS source
        ON target.EmployeeID = source.EmployeeID AND target.AttributeName = source.AttributeName
        WHEN MATCHED THEN 
            UPDATE SET target.AttributeValue = @AttributeValue
        WHEN NOT MATCHED THEN
            INSERT (EmployeeID, AttributeName, AttributeValue)
            VALUES (@EmployeeID, @AttributeName, @AttributeValue);", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@AttributeName", attributeName);
                command.Parameters.AddWithValue("@AttributeValue", attributeValue);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteAttributeGlobally(string attributeName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
            DELETE FROM EmployeeAttributes 
            WHERE AttributeName = @AttributeName", connection);

                command.Parameters.AddWithValue("@AttributeName", attributeName);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    MessageBox.Show("Атрибут с таким именем не найден.");
                }
                else
                {
                    MessageBox.Show($"Удалено {rowsAffected} записей.");
                }
            }
        }

        public void UpdateEmployeeAttributesFromTable(int employeeId, DataTable attributesTable)
        {
            if(attributesTable == null)
            {
                throw new ArgumentNullException("attributes");
            }
            
            if (attributesTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("Таблица атрибутов пуста.");
            }

            // Извлекаем первую строку с атрибутами
            DataRow row = attributesTable.Rows[0];

            foreach (DataColumn column in attributesTable.Columns)
            {
                string attributeName = column.ColumnName;

                // Исключаем столбцы с названиями "EmployeeID" и любые, начинающиеся с "EmployeeID"
                if (attributeName.StartsWith("EmployeeID", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Получаем значение атрибута, если оно не пустое
                string attributeValue = row[column]?.ToString() ?? string.Empty;

                if (!string.IsNullOrEmpty(attributeValue))
                {
                    AddOrUpdateEmployeeAttribute(employeeId, attributeName, attributeValue);
                }
            }
        }

        public DataTable GetEmployeeAttributesById(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем список всех уникальных атрибутов
                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                // Формируем динамический SQL-запрос, выбирающий все возможные атрибуты для конкретного сотрудника
                var selectQuery = $"SELECT {employeeId} AS EmployeeID";

                foreach (var attributeName in attributeNames)
                {
                    selectQuery += $", MAX(CASE WHEN AttributeName = '{attributeName}' THEN AttributeValue ELSE NULL END) AS [{attributeName}]";
                }

                selectQuery += " FROM EmployeeAttributes WHERE EmployeeID = @EmployeeID";

                command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public DataTable GetEmptyAttributesTemplate()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем список всех уникальных атрибутов
                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                // Создаем пустую таблицу с колонками для каждого атрибута
                var dataTable = new DataTable();
                dataTable.Columns.Add("EmployeeID", typeof(int));

                foreach (var attributeName in attributeNames)
                {
                    dataTable.Columns.Add(attributeName, typeof(string));
                }

                // Создаем новую строку и заполняем её пустыми строками
                var newRow = dataTable.NewRow();
                newRow["EmployeeID"] = DBNull.Value; // или 0, если вы хотите, чтобы EmployeeID был равен 0
                foreach (var attributeName in attributeNames)
                {
                    newRow[attributeName] = string.Empty; // Заполняем пустыми строками
                }

                // Добавляем новую строку в таблицу
                dataTable.Rows.Add(newRow);

                return dataTable;
            }
        }

        // Метод для получения всех сотрудников и их атрибутов
        public DataTable GetAllEmployeesWithAttributes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Получаем список всех уникальных атрибутов
                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                // Формируем динамический SQL-запрос с использованием списка атрибутов
                var selectQuery = @"
                SELECT e.EmployeeID, e.FullName, e.HireDate, e.MedDate, j.Title AS JobTitle, s.StatusName AS Status";

                foreach (var attributeName in attributeNames)
                {
                    selectQuery += $", MAX(CASE WHEN ea.AttributeName = '{attributeName}' THEN ea.AttributeValue ELSE NULL END) AS [{attributeName}]";
                }

                selectQuery += @"
                FROM Employees e
                JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
                JOIN Statuses s ON e.StatusID = s.StatusID
                LEFT JOIN EmployeeAttributes ea ON e.EmployeeID = ea.EmployeeID
                GROUP BY e.EmployeeID, e.FullName, e.HireDate, e.MedDate, j.Title, s.StatusName";

                command = new SqlCommand(selectQuery, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Метод для добавления нового сотрудника
        public int AddEmployee(int jobTitleId, string fullName, DateTime hireDate, DateTime medDate, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                INSERT INTO Employees (JobTitleID, FullName, HireDate, MedDate, StatusID) 
                OUTPUT INSERTED.EmployeeID 
                VALUES (@JobTitleID, @FullName, @HireDate, @MedDate, @StatusID);", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@HireDate", hireDate);
                command.Parameters.AddWithValue("@MedDate", medDate);
                command.Parameters.AddWithValue("@StatusID", statusId);
                return (int)command.ExecuteScalar();
            }
        }

        public DataTable GetEmployeeById(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        SELECT e.EmployeeID, e.FullName, e.HireDate, e.MedDate,
               j.JobTitleID, j.Title AS JobTitle, 
               s.StatusID, s.StatusName AS Status
        FROM Employees e
        JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
        JOIN Statuses s ON e.StatusID = s.StatusID
        WHERE e.EmployeeID = @EmployeeID", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();

                // Заполнение DataTable
                adapter.Fill(dataTable);

                // Проверка, была ли найдена запись
                if (dataTable.Rows.Count == 0)
                {
                    throw new InvalidOperationException($"Сотрудник с ID {employeeId} не найден.");
                }

                return dataTable;
            }
        }

        public DataTable GetEmployeeByIdRaw(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
SELECT e.EmployeeID, e.FullName, e.HireDate, e.MedDate,
       e.JobTitleID, 
       e.StatusID
FROM Employees e
WHERE e.EmployeeID = @EmployeeID", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();

                // Заполнение DataTable
                adapter.Fill(dataTable);

                // Проверка, была ли найдена запись
                if (dataTable.Rows.Count == 0)
                {
                    throw new InvalidOperationException($"Сотрудник с ID {employeeId} не найден.");
                }

                return dataTable;
            }
        }

        // Метод для редактирования работника
        public void UpdateEmployee(int employeeId, int jobTitleId, string fullName, DateTime hireDate, DateTime medDate, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            UPDATE Employees
            SET JobTitleID = @JobTitleID,
                FullName = @FullName,
                HireDate = @HireDate,
                MedDate = @MedDate,
                StatusID = @StatusID
            WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@HireDate", hireDate);
                command.Parameters.AddWithValue("@MedDate", hireDate);
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

        // Метод для получения всех сотрудников
        public DataTable GetAllEmployees()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                SELECT e.EmployeeID, e.FullName, e.HireDate, e.MedDate, j.Title AS JobTitle, s.StatusName AS Status
                FROM Employees e
                JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
                JOIN Statuses s ON e.StatusID = s.StatusID", connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        // Пример методов добавления и получения статусов и должностей
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

        public int AddJobTitle(string title)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Проверяем, существует ли должность с указанным названием
                var checkTitleCommand = new SqlCommand("SELECT COUNT(*) FROM JobTitles WHERE Title = @Title", connection);
                checkTitleCommand.Parameters.AddWithValue("@Title", title);

                int titleExists = (int)checkTitleCommand.ExecuteScalar();
                if (titleExists > 0)
                {
                    throw new InvalidOperationException("Должность с таким названием уже существует.");
                }

                // Если должность не существует, добавляем её
                var command = new SqlCommand("INSERT INTO JobTitles (Title) OUTPUT INSERTED.JobTitleID VALUES (@Title);", connection);
                command.Parameters.AddWithValue("@Title", title);
                return (int)command.ExecuteScalar();
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

        public DataTable GetAllJobTitles()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM JobTitles", connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }
}
