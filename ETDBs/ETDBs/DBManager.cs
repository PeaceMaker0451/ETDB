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
            EnsureTablesExist();
        }

        private void EnsureTablesExist()
        {

            using (var dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var command = dbConnection.CreateCommand();

                command.CommandText = @"
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
                StatusID INT FOREIGN KEY REFERENCES Statuses(StatusID)
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='EmployeesEvents' and xtype='U')
        BEGIN
            CREATE TABLE EmployeesEvents (
                EventID INT PRIMARY KEY IDENTITY,
                EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                EventName NVARCHAR(50),
                ToNext INT NOT NULL,
                IsMonths BIT NOT NULL,
                StartDate DATE NOT NULL,
                OneTime BIT NOT NULL,
                Expired INT NOT NULL
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TitlesEvents' and xtype='U')
        BEGIN
            CREATE TABLE TitlesEvents (
                EventID INT PRIMARY KEY IDENTITY,
                JobTitleID INT FOREIGN KEY REFERENCES JobTitles(JobTitleID),
                EventName NVARCHAR(50),
                ToNext INT NOT NULL,
                IsMonths BIT NOT NULL,
                OneTime BIT NOT NULL,
            );
        END;

        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TitlesEventsDates' and xtype='U')
        BEGIN
            CREATE TABLE TitlesEventsDates (
                EventID INT FOREIGN KEY REFERENCES TitlesEvents(EventID),
                EmployeeID INT FOREIGN KEY REFERENCES Employees(EmployeeID),
                StartDate DATE NOT NULL,
                Expired INT NOT NULL
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

        public DataTable GetAllEmployeesEventsWithAttributes(DataTable filteredEmployeesData)
        {
            var employeeIds = filteredEmployeesData.AsEnumerable().Select(row => (int)row["EmployeeID"]).ToList();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Получение уникальных имен атрибутов
                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                // Запрос для событий сотрудников
                var employeeEventsQuery = @"
        SELECT e.EmployeeID, e.FullName, j.Title AS JobTitle, 
               ee.EventID, ee.EventName, ee.StartDate, 
               ee.OneTime, ee.Expired, ee.ToNext, ee.IsMonths
        FROM Employees e
        JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
        JOIN EmployeesEvents ee ON e.EmployeeID = ee.EmployeeID
        WHERE e.EmployeeID IN (" + string.Join(",", employeeIds) + ")";

                // Запрос для событий должностей
                var titleEventsQuery = @"
        SELECT e.EmployeeID, e.FullName, j.Title AS JobTitle, 
               te.EventID, te.EventName, ted.StartDate, 
               te.OneTime, ted.Expired, te.ToNext, te.IsMonths
        FROM Employees e
        JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
        JOIN TitlesEvents te ON j.JobTitleID = te.JobTitleID
        JOIN TitlesEventsDates ted ON ted.EventID = te.EventID AND ted.EmployeeID = e.EmployeeID
        WHERE e.EmployeeID IN (" + string.Join(",", employeeIds) + ")";

                // Формирование основного DataTable с учетом атрибутов
                var combinedEventsTable = new DataTable();
                combinedEventsTable.Columns.Add("EmployeeID", typeof(int));
                combinedEventsTable.Columns.Add("EmployeeName", typeof(string));
                combinedEventsTable.Columns.Add("JobTitle", typeof(string));
                combinedEventsTable.Columns.Add("EventID", typeof(int));
                combinedEventsTable.Columns.Add("EventName", typeof(string));
                combinedEventsTable.Columns.Add("EventDate", typeof(DateTime));
                combinedEventsTable.Columns.Add("OneTime", typeof(bool));
                combinedEventsTable.Columns.Add("Expired", typeof(int));
                combinedEventsTable.Columns.Add("ToNext", typeof(int));
                combinedEventsTable.Columns.Add("IsMonths", typeof(bool));
                combinedEventsTable.Columns.Add("IsTitleEvent", typeof(bool));

                foreach (var attributeName in attributeNames)
                {
                    combinedEventsTable.Columns.Add(attributeName, typeof(string));
                }

                // Заполнение таблицы данными о событиях сотрудников
                var employeeEventsCommand = new SqlCommand(employeeEventsQuery, connection);
                var employeeEventsReader = employeeEventsCommand.ExecuteReader();

                while (employeeEventsReader.Read())
                {
                    var row = combinedEventsTable.NewRow();
                    row["EmployeeID"] = employeeEventsReader["EmployeeID"];
                    row["EmployeeName"] = employeeEventsReader["FullName"];
                    row["JobTitle"] = employeeEventsReader["JobTitle"];
                    row["EventID"] = employeeEventsReader["EventID"];
                    row["EventName"] = employeeEventsReader["EventName"];
                    row["EventDate"] = employeeEventsReader["StartDate"];
                    row["OneTime"] = employeeEventsReader["OneTime"];
                    row["Expired"] = employeeEventsReader["Expired"];
                    row["ToNext"] = employeeEventsReader["ToNext"];
                    row["IsMonths"] = employeeEventsReader["IsMonths"];
                    row["IsTitleEvent"] = false;
                    combinedEventsTable.Rows.Add(row);
                }
                employeeEventsReader.Close();

                // Заполнение таблицы данными о событиях должностей
                var titleEventsCommand = new SqlCommand(titleEventsQuery, connection);
                var titleEventsReader = titleEventsCommand.ExecuteReader();

                while (titleEventsReader.Read())
                {
                    var row = combinedEventsTable.NewRow();
                    row["EmployeeID"] = titleEventsReader["EmployeeID"];
                    row["EmployeeName"] = titleEventsReader["FullName"];
                    row["JobTitle"] = titleEventsReader["JobTitle"];
                    row["EventID"] = titleEventsReader["EventID"];
                    row["EventName"] = titleEventsReader["EventName"];
                    row["EventDate"] = titleEventsReader["StartDate"];
                    row["OneTime"] = titleEventsReader["OneTime"];
                    row["Expired"] = titleEventsReader["Expired"];
                    row["ToNext"] = titleEventsReader["ToNext"];
                    row["IsMonths"] = titleEventsReader["IsMonths"];
                    row["IsTitleEvent"] = true;
                    combinedEventsTable.Rows.Add(row);
                }
                titleEventsReader.Close();

                // Запрос и добавление атрибутов
                var attributesQuery = @"
        SELECT ea.EmployeeID, ea.AttributeName, ea.AttributeValue
        FROM EmployeeAttributes ea
        WHERE ea.EmployeeID IN (" + string.Join(",", employeeIds) + ")";
                var attributesCommand = new SqlCommand(attributesQuery, connection);
                var attributesReader = attributesCommand.ExecuteReader();

                var attributesDict = new Dictionary<int, Dictionary<string, string>>();

                while (attributesReader.Read())
                {
                    var employeeId = (int)attributesReader["EmployeeID"];
                    var attributeName = (string)attributesReader["AttributeName"];
                    var attributeValue = (string)attributesReader["AttributeValue"];

                    if (!attributesDict.ContainsKey(employeeId))
                    {
                        attributesDict[employeeId] = new Dictionary<string, string>();
                    }
                    attributesDict[employeeId][attributeName] = attributeValue;
                }
                attributesReader.Close();

                // Добавление атрибутов к существующим строкам
                foreach (DataRow row in combinedEventsTable.Rows)
                {
                    var employeeId = (int)row["EmployeeID"];
                    if (attributesDict.ContainsKey(employeeId))
                    {
                        foreach (var attributeName in attributeNames)
                        {
                            row[attributeName] = attributesDict[employeeId].ContainsKey(attributeName)
                                ? attributesDict[employeeId][attributeName]
                                : null;
                        }
                    }
                }

                return combinedEventsTable;
            }
        }

        public DataTable GetAllEmployeesEvents(DataTable filteredEmployeesData)
        {
            var employeeIds = filteredEmployeesData.AsEnumerable().Select(row => (int)row["EmployeeID"]).ToList();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var employeeEventsQuery = @"
            SELECT e.EmployeeID, e.FullName, j.Title AS JobTitle, 
                   ee.EventID, ee.EventName, ee.StartDate, 
                   ee.OneTime, ee.Expired, ee.ToNext, ee.IsMonths
            FROM Employees e
            JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
            JOIN EmployeesEvents ee ON e.EmployeeID = ee.EmployeeID
            WHERE e.EmployeeID IN (" + string.Join(",", employeeIds) + ")";

                var titleEventsQuery = @"
            SELECT e.EmployeeID, e.FullName, j.Title AS JobTitle, 
                   te.EventID, te.EventName, ted.StartDate, 
                   te.OneTime, ted.Expired, te.ToNext, te.IsMonths
            FROM Employees e
            JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
            JOIN TitlesEvents te ON j.JobTitleID = te.JobTitleID
            JOIN TitlesEventsDates ted ON ted.EventID = te.EventID AND ted.EmployeeID = e.EmployeeID
            WHERE e.EmployeeID IN (" + string.Join(",", employeeIds) + ")";

                var combinedEventsTable = new DataTable();
                combinedEventsTable.Columns.Add("EmployeeID", typeof(int));
                combinedEventsTable.Columns.Add("EmployeeName", typeof(string));
                combinedEventsTable.Columns.Add("JobTitle", typeof(string));
                combinedEventsTable.Columns.Add("EventID", typeof(int));
                combinedEventsTable.Columns.Add("EventName", typeof(string));
                combinedEventsTable.Columns.Add("EventDate", typeof(DateTime));
                combinedEventsTable.Columns.Add("OneTime", typeof(bool));
                combinedEventsTable.Columns.Add("Expired", typeof(int));
                combinedEventsTable.Columns.Add("ToNext", typeof(int));
                combinedEventsTable.Columns.Add("IsMonths", typeof(bool));
                combinedEventsTable.Columns.Add("IsTitleEvent", typeof(bool));

                var employeeEventsCommand = new SqlCommand(employeeEventsQuery, connection);
                var employeeEventsReader = employeeEventsCommand.ExecuteReader();

                while (employeeEventsReader.Read())
                {
                    combinedEventsTable.Rows.Add(
                        employeeEventsReader["EmployeeID"],
                        employeeEventsReader["FullName"],
                        employeeEventsReader["JobTitle"],
                        employeeEventsReader["EventID"],
                        employeeEventsReader["EventName"],
                        employeeEventsReader["StartDate"],
                        employeeEventsReader["OneTime"],
                        employeeEventsReader["Expired"],
                        employeeEventsReader["ToNext"],
                        employeeEventsReader["IsMonths"],
                        false
                    );
                }
                employeeEventsReader.Close();

                var titleEventsCommand = new SqlCommand(titleEventsQuery, connection);
                var titleEventsReader = titleEventsCommand.ExecuteReader();

                while (titleEventsReader.Read())
                {
                    combinedEventsTable.Rows.Add(
                        titleEventsReader["EmployeeID"],
                        titleEventsReader["FullName"],
                        titleEventsReader["JobTitle"],
                        titleEventsReader["EventID"],
                        titleEventsReader["EventName"],
                        titleEventsReader["StartDate"],
                        titleEventsReader["OneTime"],
                        titleEventsReader["Expired"],
                        titleEventsReader["ToNext"],
                        titleEventsReader["IsMonths"],
                        true
                    );
                }
                titleEventsReader.Close();

                return combinedEventsTable;
            }
        }

        public DataTable GetAllEmployeeEventsTable(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        SELECT EventID, EventName, StartDate, ToNext, IsMonths, OneTime, Expired
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
        SELECT EventID, EventName, StartDate, ToNext, IsMonths, OneTime, Expired
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

        public int AddEmployeeEvent(int employeeId, string eventName, DateTime startDate, int timeToNext, bool isMonths, bool oneTime, int expired)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        INSERT INTO EmployeesEvents (EmployeeID, EventName, StartDate, ToNext, IsMonths, OneTime, Expired)
        OUTPUT INSERTED.EventID
        VALUES (@EmployeeID, @EventName, @StartDate, @ToNext, @IsMonths, @OneTime, @Expired)", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@OneTime", oneTime);
                command.Parameters.AddWithValue("@Expired", expired);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);

                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateEmployeeEvent(int employeeId, int eventId, string eventName, DateTime startDate, int timeToNext, bool isMonths, bool oneTime, int expired)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        UPDATE EmployeesEvents
        SET EventName = @EventName, StartDate = @StartDate, ToNext = @ToNext, IsMonths = @IsMonths, OneTime = @OneTime, Expired = @Expired
        WHERE EmployeeID = @EmployeeID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@OneTime", oneTime);
                command.Parameters.AddWithValue("@Expired", expired);
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
        SELECT EventID, EventName, ToNext, IsMonths, OneTime
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
        SELECT EventID, EventName, ToNext, IsMonths, OneTime
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
               MAX(ted.StartDate) AS StartDate,
               te.OneTime,
               MAX(ted.Expired) AS Expired, 
               te.EventName, 
               te.ToNext, 
               te.IsMonths
        FROM TitlesEvents te
        LEFT JOIN TitlesEventsDates ted 
            ON ted.EventID = te.EventID AND ted.EmployeeID = @EmployeeID
        WHERE te.JobTitleID = (SELECT JobTitleID FROM Employees WHERE EmployeeID = @EmployeeID)
        GROUP BY te.EventID, te.OneTime, te.EventName, te.ToNext, te.IsMonths", connection);

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
        SELECT te.EventID,
               COALESCE(ted.StartDate, NULL) AS StartDate,
               te.EventName,
               te.ToNext,
               te.IsMonths,
               te.OneTime,
               ted.Expired
        FROM TitlesEvents te
        LEFT JOIN TitlesEventsDates ted 
            ON ted.EventID = te.EventID 
            AND ted.EmployeeID = @EmployeeID
        WHERE te.EventID = @EventID", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
        }

        public int AddJobTitleEvent(int jobTitleId, string eventName, int timeToNext, bool isMonths, bool oneTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        INSERT INTO TitlesEvents (JobTitleID, EventName, ToNext, IsMonths, OneTime)
        OUTPUT INSERTED.EventID
        VALUES (@JobTitleID, @EventName, @ToNext, @IsMonths, @OneTime)", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);
                command.Parameters.AddWithValue("@OneTime", oneTime);

                return (int)command.ExecuteScalar();
            }
        }

        public void UpdateJobTitleEvent(int jobTitleId, int eventId, string eventName, int timeToNext, bool isMonths, bool oneTime)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        UPDATE TitlesEvents
        SET EventName = @EventName, ToNext = @ToNext, IsMonths = @IsMonths, OneTime = @OneTime
        WHERE JobTitleID = @JobTitleID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@EventName", eventName);
                command.Parameters.AddWithValue("@ToNext", timeToNext);
                command.Parameters.AddWithValue("@IsMonths", isMonths);
                command.Parameters.AddWithValue("@OneTime", oneTime);

                command.ExecuteNonQuery();
            }
        }

        public void AddOrUpdateJobTitleEventDate(int employeeId, int eventId, DateTime startDate, int expired)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
        IF EXISTS (SELECT 1 FROM TitlesEventsDates WHERE EmployeeID = @EmployeeID AND EventID = @EventID)
            UPDATE TitlesEventsDates SET StartDate = @StartDate, Expired = @Expired
            WHERE EmployeeID = @EmployeeID AND EventID = @EventID
        ELSE
            INSERT INTO TitlesEventsDates (EmployeeID, EventID, StartDate, Expired) 
            VALUES (@EmployeeID, @EventID, @StartDate, @Expired)", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@Expired", expired);

                command.ExecuteNonQuery();
            }
        }
        public void AddOrUpdateEmployeeAttribute(int employeeId, string attributeName, string attributeValue)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Если employeeId равен 0, найти первого сотрудника
                if (employeeId == 0)
                {
                    var findFirstEmployeeCommand = new SqlCommand("SELECT TOP 1 EmployeeID FROM Employees", connection);
                    var result = findFirstEmployeeCommand.ExecuteScalar();

                    if (result == null)
                    {
                        throw new InvalidOperationException("В базе данных нет сотрудников.");
                    }

                    employeeId = (int)result;
                }

                // Проверяем, существует ли указанный сотрудник
                var checkEmployeeCommand = new SqlCommand("SELECT COUNT(*) FROM Employees WHERE EmployeeID = @EmployeeID", connection);
                checkEmployeeCommand.Parameters.AddWithValue("@EmployeeID", employeeId);

                int employeeExists = (int)checkEmployeeCommand.ExecuteScalar();
                if (employeeExists == 0)
                {
                    throw new InvalidOperationException("Сотрудник с указанным ID не существует.");
                }

                // MERGE для добавления или обновления атрибута
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

        //public void DeleteAttributeGlobally(string attributeName)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        var command = new SqlCommand(@"
        //    DELETE FROM EmployeeAttributes 
        //    WHERE AttributeName = @AttributeName", connection);

        //        command.Parameters.AddWithValue("@AttributeName", attributeName);

        //        int rowsAffected = command.ExecuteNonQuery();

        //        if (rowsAffected == 0)
        //        {
        //            MessageBox.Show("Атрибут с таким именем не найден.");
        //        }
        //        else
        //        {
        //            MessageBox.Show($"Удалено {rowsAffected} записей.");
        //        }
        //    }
        //}

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

            DataRow row = attributesTable.Rows[0];

            foreach (DataColumn column in attributesTable.Columns)
            {
                string attributeName = column.ColumnName;

                if (attributeName.StartsWith("EmployeeID", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

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

                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

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

                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                var dataTable = new DataTable();
                dataTable.Columns.Add("EmployeeID", typeof(int));

                foreach (var attributeName in attributeNames)
                {
                    dataTable.Columns.Add(attributeName, typeof(string));
                }

                var newRow = dataTable.NewRow();
                newRow["EmployeeID"] = DBNull.Value;
                foreach (var attributeName in attributeNames)
                {
                    newRow[attributeName] = string.Empty;
                }

                dataTable.Rows.Add(newRow);

                return dataTable;
            }
        }

        public DataTable GetAllEmployeesWithAttributes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT DISTINCT AttributeName FROM EmployeeAttributes", connection);
                var reader = command.ExecuteReader();
                var attributeNames = new List<string>();

                while (reader.Read())
                {
                    attributeNames.Add(reader.GetString(0));
                }
                reader.Close();

                var selectQuery = @"
                SELECT e.EmployeeID, e.FullName, j.Title AS JobTitle, s.StatusName AS Status";

                foreach (var attributeName in attributeNames)
                {
                    selectQuery += $", MAX(CASE WHEN ea.AttributeName = '{attributeName}' THEN ea.AttributeValue ELSE NULL END) AS [{attributeName}]";
                }

                selectQuery += @"
                FROM Employees e
                JOIN JobTitles j ON e.JobTitleID = j.JobTitleID
                JOIN Statuses s ON e.StatusID = s.StatusID
                LEFT JOIN EmployeeAttributes ea ON e.EmployeeID = ea.EmployeeID
                GROUP BY e.EmployeeID, e.FullName, j.Title, s.StatusName";

                command = new SqlCommand(selectQuery, connection);
                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int AddEmployee(int jobTitleId, string fullName, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
                INSERT INTO Employees (JobTitleID, FullName, StatusID) 
                OUTPUT INSERTED.EmployeeID 
                VALUES (@JobTitleID, @FullName, @StatusID);", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
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

                adapter.Fill(dataTable);

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
SELECT e.EmployeeID, e.FullName,
       e.JobTitleID, 
       e.StatusID
FROM Employees e
WHERE e.EmployeeID = @EmployeeID", connection);

                command.Parameters.AddWithValue("@EmployeeID", employeeId);

                var adapter = new SqlDataAdapter(command);
                var dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count == 0)
                {
                    throw new InvalidOperationException($"Сотрудник с ID {employeeId} не найден.");
                }

                return dataTable;
            }
        }

        public void UpdateEmployee(int employeeId, int jobTitleId, string fullName, int statusId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand(@"
            UPDATE Employees
            SET JobTitleID = @JobTitleID,
                FullName = @FullName,
                StatusID = @StatusID
            WHERE EmployeeID = @EmployeeID", connection);
                command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                command.Parameters.AddWithValue("@FullName", fullName);
                command.Parameters.AddWithValue("@StatusID", statusId);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.ExecuteNonQuery();
            }
        }

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

                var checkTitleCommand = new SqlCommand("SELECT COUNT(*) FROM JobTitles WHERE Title = @Title", connection);
                checkTitleCommand.Parameters.AddWithValue("@Title", title);

                int titleExists = (int)checkTitleCommand.ExecuteScalar();
                if (titleExists > 0)
                {
                    throw new InvalidOperationException("Должность с таким названием уже существует.");
                }

                var command = new SqlCommand("INSERT INTO JobTitles (Title) OUTPUT INSERTED.JobTitleID VALUES (@Title);", connection);
                command.Parameters.AddWithValue("@Title", title);
                return (int)command.ExecuteScalar();
            }
        }

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

        public void DeleteEmployee(int employeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    // Удаляем связанные записи из таблицы EmployeesEvents
                    var command = new SqlCommand("DELETE FROM EmployeesEvents WHERE EmployeeID = @EmployeeID", connection, transaction);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.ExecuteNonQuery();

                    // Удаляем связанные записи из таблицы TitlesEventsDates
                    command = new SqlCommand("DELETE FROM TitlesEventsDates WHERE EmployeeID = @EmployeeID", connection, transaction);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.ExecuteNonQuery();

                    // Удаляем связанные записи из таблицы EmployeeAttributes
                    command = new SqlCommand("DELETE FROM EmployeeAttributes WHERE EmployeeID = @EmployeeID", connection, transaction);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.ExecuteNonQuery();

                    // Наконец, удаляем запись из Employees
                    command = new SqlCommand("DELETE FROM Employees WHERE EmployeeID = @EmployeeID", connection, transaction);
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteJobTitle(int jobTitleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    // Удаляем записи из TitlesEvents, связанные с JobTitleID
                    var command = new SqlCommand("DELETE FROM TitlesEvents WHERE JobTitleID = @JobTitleID", connection, transaction);
                    command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                    command.ExecuteNonQuery();

                    // Удаляем всех сотрудников с данным JobTitleID
                    var getEmployeesCommand = new SqlCommand("SELECT EmployeeID FROM Employees WHERE JobTitleID = @JobTitleID", connection, transaction);
                    getEmployeesCommand.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                    using (var reader = getEmployeesCommand.ExecuteReader())
                    {
                        var employeeIds = new List<int>();
                        while (reader.Read())
                        {
                            employeeIds.Add(reader.GetInt32(0));
                        }

                        foreach (var employeeId in employeeIds)
                        {
                            DeleteEmployee(employeeId);
                        }
                    }

                    // Удаляем JobTitle
                    command = new SqlCommand("DELETE FROM JobTitles WHERE JobTitleID = @JobTitleID", connection, transaction);
                    command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteEmployeeAttributeGlobally(string attributeName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand(@"
        DELETE FROM EmployeeAttributes 
        WHERE AttributeName = @AttributeName", connection);

                command.Parameters.AddWithValue("@AttributeName", attributeName);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteEvent(int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    // Delete related TitleEventDates
                    var command = new SqlCommand("DELETE FROM EmployeesEvents WHERE EventID = @EventID", connection, transaction);
                    command.Parameters.AddWithValue("@EventID", eventId);
                    command.ExecuteNonQuery();

                    //// Delete the event itself
                    //command = new SqlCommand("DELETE FROM Events WHERE EventID = @EventID", connection, transaction);
                    //command.Parameters.AddWithValue("@EventID", eventId);
                    //command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DeleteTitleEventDate(int employeeId, int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("DELETE FROM TitlesEventsDates WHERE EmployeeID = @EmployeeID AND EventID = @EventID", connection);
                command.Parameters.AddWithValue("@EmployeeID", employeeId);
                command.Parameters.AddWithValue("@EventID", eventId);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteJobTitleEvent(int jobTitleId, int eventId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();

                try
                {
                    // Delete related TitleEventDates for the specified JobTitle and Event
                    var command = new SqlCommand(@"
                DELETE FROM TitlesEventsDates
                WHERE EventID = @EventID 
                AND EmployeeID IN (SELECT EmployeeID FROM Employees WHERE JobTitleID = @JobTitleID)", connection, transaction);

                    command.Parameters.AddWithValue("@EventID", eventId);
                    command.Parameters.AddWithValue("@JobTitleID", jobTitleId);
                    command.ExecuteNonQuery();

                    // Delete the event itself from TitlesEvents
                    command = new SqlCommand(@"
                DELETE FROM TitlesEvents
                WHERE EventID = @EventID", connection, transaction);

                    command.Parameters.AddWithValue("@EventID", eventId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
