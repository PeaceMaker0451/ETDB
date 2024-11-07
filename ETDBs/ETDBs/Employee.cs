using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETDBs
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string FullName { get; set; }
        public string DisplayText => $"{FullName} - ({JobTitle})";

        public static List<Employee> GetEmployees(DBManager manager)
        {
            DataTable employeeTable = manager.GetAllEmployeesWithAttributes(); // Метод, возвращающий DataTable с сотрудниками
            var employees = new List<Employee>();

            foreach (DataRow row in employeeTable.Rows)
            {
                employees.Add(new Employee
                {
                    EmployeeID = Convert.ToInt32(row["EmployeeID"]),
                    JobTitle = row["JobTitle"].ToString(),
                    FullName = row["FullName"].ToString()
                });
            }

            return employees;
        }
    }
}
