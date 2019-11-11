using Microsoft.AspNet.Identity;
using Moses_Bugtracker.Helpers;
using Moses_Bugtracker.Models;
using Moses_Bugtracker.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Moses_Bugtracker.Controllers
{
    public class MemberController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Member
        [Authorize]
        public ActionResult EditProfile()
        {
            //Grab the user Id of the currently logged in User
            var userId = User.Identity.GetUserId();
            var member = db.Users.Select(u => new MemberViewModel
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarPath,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Id = u.Id
            }).FirstOrDefault(u => u.Id == userId);
            
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(MemberViewModel member)
        {
            var user = db.Users.Find(member.Id);
            user.FirstName = member.FirstName;
            user.LastName = member.LastName;
            user.DisplayName = member.DisplayName;
            user.PhoneNumber = member.PhoneNumber;
            user.Email = member.Email;
            user.UserName = member.Email;

            if(member.Avatar != null)
            {
                if (FileUploadValidator.IsWebFriendlyImage(member.Avatar))
                {
                    var fileName = Path.GetFileName(member.Avatar.FileName);
                    member.Avatar.SaveAs(Path.Combine(Server.MapPath("~/Avatars/"), fileName));
                    user.AvatarPath = "/Avatars/" + fileName;
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}