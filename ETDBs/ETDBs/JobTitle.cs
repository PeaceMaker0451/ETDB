using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETDBs
{
    public class JobTitle
    {
        public int JobTitleID { get; set; }
        public string JobTitleName { get; set; }

        public static List<JobTitle> GetJobTitles(DBManager dbManager)
        {
            DataTable titlesTable = dbManager.GetAllJobTitles();
            var jobTitles = new List<JobTitle>();

            foreach (DataRow row in titlesTable.Rows)
            {
                jobTitles.Add(new JobTitle
                {
                    JobTitleID = Convert.ToInt32(row["JobTitleID"]),
                    JobTitleName = row["Title"].ToString(),
                });
            }

            return jobTitles;
        }
    }
}
