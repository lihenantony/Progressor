using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Progressor.Models
{
    public class Task
    {
        public string taskID { get; set; }
        public string userID { get; set; }
        public string name { get; set; }
        public int progressIndex { get; set; }
        public int progressMax { get; set; }
        public DateTime dueDate { get; set; }
        public DateTime createDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime completeDate { get; set; }
        public int difficultyIndex { get; set; }
        public int importanceIndex { get; set; }
        public string taskStatus { get; set; }

    }

    public class TaskDBContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
    }
}