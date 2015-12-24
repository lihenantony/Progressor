using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace Progressor.Models
{
    public class Task
    {
        [Key]
        public int ID { get; set; }
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

        public static Task createNewTask (NewTaskViewModel newtask)
        {
            Task t = new Task();
            t.name = newtask.name;
            t.userID = HttpContext.Current.User.Identity.GetUserId();
            t.progressIndex = 0;
            if (newtask.progressMax != null)
            {
                t.progressMax = newtask.progressMax.Value;
            }

            if (newtask.dueDate != null)
            {
                t.dueDate = newtask.dueDate.Value;
            }
            t.createDate = DateTime.Now;
            if (newtask.difficultyIndex != null)
            {
                t.difficultyIndex = newtask.difficultyIndex.Value;
            }
            if (newtask.importanceIndex != null)
            {
                t.importanceIndex = newtask.importanceIndex.Value;
            }
            t.taskStatus = "Not Started";
            return t;
        }

    }

    public class NewTaskViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Exceed maximum length (20 characters)")]
        [Display(Name = "Name")]
        public string name { get; set; }
        
        [Display(Name = "Progress Max")]
        [Range(1,1000)]
        public int? progressMax { get; set; }

        [Display(Name = "Due Date")]
        public DateTime? dueDate { get; set; }

        [Display(Name = "Difficulty Index")]
        [Range(1,10)]
        public int? difficultyIndex { get; set; }

        [Display(Name = "Importance Index")]
        [Range(1, 10)]
        public int? importanceIndex { get; set; }
    }

    public class TaskDBContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
    }
}