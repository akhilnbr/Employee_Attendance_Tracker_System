using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Employee_Attendence_Tracker.Models;

namespace Employee_Attendence_Tracker
{
    public class DbModel:DbContext
    {
        public DbModel() : base("DefaultConnection")
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LogDetails> Logs { get; set; }
 
    }
}