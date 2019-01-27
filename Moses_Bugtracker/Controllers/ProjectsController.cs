using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moses_Bugtracker.Models;
using Moses_Bugtracker.Helpers;
using Microsoft.AspNet.Identity;

namespace Moses_Bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private UserRolesHelper rolesHelper = new UserRolesHelper();

        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            var projects = new List<Project>();
            var userId = User.Identity.GetUserId();

            if (User.IsInRole("Admin"))
            {
                projects = db.Projects.ToList();
            }
            else if (User.IsInRole("Project Manager") || User.IsInRole("Developer") || User.IsInRole("Submitter"))
            {
                projects = projHelper.ListUserProjects(userId).ToList();
            }
           
            return View(projects);

            //return View(db.Projects.ToList());
        }

        // GET: Projects
        [Authorize(Roles="Project Manager, Admin")]
        public ActionResult AllIndex()
        {
            return View("Index", db.Projects.ToList());
        }
 
        [Authorize(Roles = "Project Manager, Admin")]        
        public ActionResult Assign (int id)
        {
            //Step 1: Determine who is on the proejct
            var projectUserIds = projHelper.UsersOnProject(id).Select(u => u.Id).ToList();


            ViewBag.AssignableUsers = new MultiSelectList(db.Users.ToList(), "Id", "Email", projectUserIds);
            return View(db.Projects.Find(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Project Manager, Admin")]
        public ActionResult Assign(int Id, List<string> AssignableUsers)
        {
            if(AssignableUsers == null)
            {
                ModelState.AddModelError("AssignableUsers", "Please make at least one selection.");

                //Use the new UserRolesHelper to determine who occupies the role of Developer and push that list into a MultiSelectList
                var projectUserIds = projHelper.UsersOnProject(Id).Select(u => u.Id).ToList();
                ViewBag.AssignableUsers = new MultiSelectList(db.Users.ToList(), "Id", "Email", projectUserIds);
                return View(db.Projects.Find(Id));
            }

            //If we are here that means the user made a valid selection and we need to assign the selected users to the project
            //We will accomplish this by using the ProjectHelper...

            //Step 1: Remove all developers from the project
            foreach(var user in projHelper.UsersOnProject(Id).ToList())
            {
                projHelper.RemoveUserFromProject(user.Id, Id);
            }
         
            //Step 2: Add all the selected developers to the Project
            foreach(var userId in AssignableUsers)
            {
                projHelper.AddUserToProject(userId, Id);
            }

            return RedirectToAction("Index");
        }


            // GET: Projects/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = ("Admin,Project Manager"))]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = ("Admin,Project Manager"))]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]      
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
