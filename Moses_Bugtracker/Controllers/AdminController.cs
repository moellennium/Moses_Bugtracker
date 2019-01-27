using Moses_Bugtracker.Helpers;
using Moses_Bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moses_Bugtracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();

        public ActionResult ProjectAssign()
        {
           

            //step 1: Load a viewbag property with a selectlist of projects
            ViewBag.ProjectId = new SelectList(db.Projects.ToList(), "Id", "Name");

            //step 2: Load a viewBag property with a multiselectlist of users
            ViewBag.UserIds = new MultiSelectList(db.Users.ToList(), "Id", "Email");


            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssign(int ProjectId, List<string> UserIds)
        {
            //Here is where the action happens
            //step 1: Unassign all users from the specific project
            foreach(var user in projHelper.UsersOnProject(ProjectId).ToList())
            {
                //we remove each user from the project
                projHelper.RemoveUserFromProject(user.Id, ProjectId);
            }

            //step 2: Assign the incoming users to the specific project
            if(UserIds != null)
            {
                foreach(var userId in UserIds)
                {
                    projHelper.AddUserToProject(userId, ProjectId);
                }

            }
            
            //step 3: send the user on their way...
            return RedirectToAction("Index", "Projects");

        }

        
    }
}