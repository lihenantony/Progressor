using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Progressor.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;

namespace Progressor.Controllers
{
    public class TasksController : Controller
    {
        private TaskDBContext db = new TaskDBContext();

        // GET: Tasks
        public ActionResult Index()
        {
            List<Task> tasklist = new List<Task>();
            foreach (Task t in db.Tasks.ToList())
            {
                if (t.userID == HttpContext.User.Identity.GetUserId())
                {
                    tasklist.Add(t);
                }
            }
            for (int i=0;i<tasklist.Count;++i)
            {
                tasklist[i].refreshStatus();
            }
            tasklist.Sort();
            return View(tasklist);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewTaskViewModel newtask)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(Task.createNewTask(newtask));
                try {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");
            }

            return View(newtask);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            //db.Tasks.Remove(task);
            //db.SaveChanges();
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                //db.Tasks.Add(task);

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Update original values from the database 
                        //var entry = ex.Entries.Single();
                        //entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed);

                return RedirectToAction("Index");
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Tasks/UpdateProgress/5
        public ActionResult UpdateProgress(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
                return HttpNotFound();

            return View(task);
        }

        // POST: Tasks/UpdateProgress/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProgress(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                //db.Tasks.Add(task);

                bool saveFailed;
                do
                {
                    saveFailed = false;
                    try
                    {
                        db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;
                        
                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed);

                return RedirectToAction("Index");
            }
            return View(task);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
