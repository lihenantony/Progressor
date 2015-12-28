using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace Progressor.Models
{
    public class Task : IComparable<Task>
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int ID { get; set; }

        [Required]
        public string userID { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Exceed maximum length (20 characters)")]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Display(Name = "Progress")]
        [Range(0, 1000)]
        public int? progressIndex { get; set; }

        [Display(Name = "Progress Max")]
        [Range(0, 1000)]
        public int? progressMax { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? dueDate { get; set; }

        [Display(Name = "Create Date")]
        [DataType(DataType.Date)]
        public DateTime createDate { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }

        [Display(Name = "Complete Date")]
        [DataType(DataType.Date)]
        public DateTime completeDate { get; set; }

        [Display(Name = "Difficulty")]
        [Range(1, 10)]
        public int? difficultyIndex { get; set; }

        [Display(Name = "Importance")]
        [Range(1, 10)]
        public int? importanceIndex { get; set; }
        
        [Display(Name = "Status")]
        public string taskStatus { get; set; }

        public void refreshStatus()
        {
            if (taskStatus == "Onhold")
                return;

            if (progressIndex.HasValue && progressMax.HasValue)
            {
                if (progressIndex == 0)
                {
                    taskStatus = "Not Started";
                }
                else if (progressIndex >= progressMax)
                {
                    taskStatus = "Completed";
                }
                else
                {
                    taskStatus = "In Progress";
                }
            }
            else
            {
                taskStatus = "Unknown";
            }
        }

        public static Task createNewTask (NewTaskViewModel newtask)
        {
            Task t = new Task();
            t.name = newtask.name;
            t.userID = HttpContext.Current.User.Identity.GetUserId();
            
            if (newtask.progressMax != null)
            {
                t.progressMax = newtask.progressMax.Value;
                t.taskStatus = "Not Started";
            }
            else
            {
                t.taskStatus = "Unknown";
            }

            t.progressIndex = 0;

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
            
            return t;
        }

        public int CompareTo(Task other)
        {
            return other.getPriorityIndex() - getPriorityIndex();
        }
        
        public int getPriorityIndex()
        {
            if (taskStatus == "Completed" || taskStatus == "Onhold")
                return 0;

            double p = 1.0;

            // from due dates
            if (dueDate.HasValue)
            {
                double d = ((DateTime.Now - createDate).TotalDays + 1) / (dueDate - createDate).Value.TotalDays * 10;
                p *= d;
            }

            // from progress
            if (progressMax.HasValue && progressIndex.HasValue)
            {
                double t = (1.0 * progressMax.Value - progressIndex.Value) / (progressMax.Value) * 10.0 + 1;
                p *= t;
            }

            // from diff index
            if (difficultyIndex.HasValue)
                p *= 11 - difficultyIndex.Value;

            // from importance index
            if (importanceIndex.HasValue)
                p *= importanceIndex.Value;

            return (int)p;
        }

    }

    public class NewTaskViewModel
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Exceed maximum length (20 characters)")]
        [Display(Name = "Name")]
        public string name { get; set; }
        
        [Display(Name = "Progress Max")]
        [Range(0,1000)]
        public int? progressMax { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? dueDate { get; set; }

        [Display(Name = "Difficulty")]
        [Range(1,10)]
        public int? difficultyIndex { get; set; }

        [Display(Name = "Importance")]
        [Range(1, 10)]
        public int? importanceIndex { get; set; }
    }



    public class TaskDBContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
    }
}