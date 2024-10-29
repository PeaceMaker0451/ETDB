using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETDBs
{
    internal class DBInitializer
    {
        private string connectionString;
        private string _databaseName;
        public DBInitializer(string _connectionString, string dbName)
        {
            connectionString = _connectionString;
            _databaseName = dbName;
        }
        public void EnsureDatabaseAndTables()
        {
            if (!DatabaseExists())
            {
                CreateDatabase();
                CreateTables();
            }
        }

        private bool DatabaseExists()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = $"SELECT database_id FROM sys.databases WHERE Name = '{_databaseName}'";
                var command = new SqlCommand(query, connection);

                connection.Open();
                return command.ExecuteScalar() != null;
            }
        }

        private void CreateDatabase()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand($"CREATE DATABASE [{_databaseName}]", connection);

                connection.Open();
                command.ExecuteNonQuery();

                MessageBox.Show("База данных создана.");
            }
        }

        private void CreateTables()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string createTableQuery = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[JobTitles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[JobTitles] (
        [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
        [JobTitle] NVARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employees] (
        [Id] INT IDENTITY (1, 1) NOT NULL,
        [FullName] NVARCHAR (50) NOT NULL,
        [JobTitleID] INT,
        [BirthDate] DATETIME NULL,
        FOREIGN KEY (JobTitleID) REFERENCES JobTitles(Id)
    );
END";

                var command = new SqlCommand(createTableQuery, connection);

                connection.Open();
                command.ExecuteNonQuery();

                MessageBox.Show("Таблицы созданы.");
            }
        }
    }
}
