namespace Moses_Bugtracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Moses_Bugtracker.Helpers;
    using Moses_Bugtracker.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<Moses_Bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Moses_Bugtracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                   new TicketPriority { Name = "Low" },
                   new TicketPriority { Name = "Medium" },
                   new TicketPriority { Name = "High" },
                   new TicketPriority { Name = "Immediate" }
               );
            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Name = "New" },
                    new TicketStatus { Name = "Closed" },
                    new TicketStatus { Name = "Archived" },
                    new TicketStatus { Name = "Assigned" },
                    new TicketStatus { Name = "UnAssigned" }
                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Name = "Bug" },
                    new TicketType { Name = "Typo" },
                    new TicketType { Name = "Request for Documentation" },
                    new TicketType { Name = "Request for Functionality" },
                    new TicketType { Name = "Request for Discussion" }
                );





            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //We want to create an Admin role ONLY if one doestn't  already exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
               roleManager.Create(new IdentityRole { Name = "Admin" });
            }
               
            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {

               roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }
               
            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
               roleManager.Create(new IdentityRole { Name = "Developer" });

            }
                
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {

               roleManager.Create(new IdentityRole { Name = "Submitter" });
            }



            #region User Seeding section

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //Create an Admin user
            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@mailinator.com",
                    Email = "DemoAdmin@mailinator.com",
                    FirstName = "Moses",
                    LastName = "DeSaussure",
                    DisplayName = "The Golden Boy",
                    AvatarPath = "/Avatars/default.png"
                }, "Abc&123!");

            }

            //Create an Project Manager
            if (!context.Users.Any(u => u.Email == "DemoProjectManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoProjectManager@mailinator.com",
                    Email = "DemoProjectManager@mailinator.com",
                    FirstName = "Moses",
                    LastName = "DeSaussure",
                    DisplayName = "The Golden Boy",
                     AvatarPath = "/Avatars/default.png"
                }, "Abc&123!");

            }

            //Create an Developer
            if (!context.Users.Any(u => u.Email == "DemoDeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@mailinator.com",
                    Email = "DemoDeveloper@mailinator.com",
                    FirstName = "Moses",
                    LastName = "DeSaussure",
                    DisplayName = "The Golden Boy",
                    AvatarPath = "/Avatars/default.png"
                }, "Abc&123!");

            }

            //Creater an Submitter
            if (!context.Users.Any(u => u.Email == "DemoSubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@mailinator.com",
                    Email = "DemoSubmitter@mailinator.com",
                    FirstName = "Moses",
                    LastName = "DeSaussure",
                    DisplayName = "The Golden Boy",
                    AvatarPath = "/Avatars/default.png"
                }, "Abc&123!");

            }
            var demoAdminId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(demoAdminId, "Admin");

            var demoPMId = userManager.FindByEmail("DemoProjectManager@mailinator.com").Id;
            userManager.AddToRole(demoPMId, "Project Manager");

            var demoDevId = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(demoDevId, "Developer");

            var demoSubId = userManager.FindByEmail("DemoSubmitter@mailinator.com").Id;
            userManager.AddToRole(demoSubId, "Submitter");

            #endregion

            #region Project Seeding section
            context.Projects.AddOrUpdate(
                p => p.Name,
                    new Project { Name = "Seeded Project 1" },
                    new Project { Name = "Seeded Project 2" }                  
                );

            context.SaveChanges();

            //Now that I have saved the Projects to the DB I need to get their Id's by Name
            var project1Id = context.Projects.FirstOrDefault(p => p.Name == "Seeded Project 1").Id;
            var project2Id = context.Projects.FirstOrDefault(p => p.Name == "Seeded Project 2").Id;

            var projHelper = new ProjectsHelper();
            projHelper.AddUserToProject(demoPMId, project1Id);
            projHelper.AddUserToProject(demoSubId, project1Id);
            projHelper.AddUserToProject(demoDevId, project1Id);
            projHelper.AddUserToProject(demoPMId, project2Id);
            projHelper.AddUserToProject(demoSubId, project2Id);
            projHelper.AddUserToProject(demoDevId, project2Id);
            #endregion

            #region Ticket Seeding section
            var priorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Medium").Id;
            var statusId = context.TicketStatus.FirstOrDefault(t => t.Name == "New").Id;
            var typeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Bug").Id;

            context.Tickets.AddOrUpdate(
                t => t.Title,
                    new Ticket
                    {
                        Title = "Seeded Ticket 1: Project 1",
                        ProjectId = project1Id,
                        Created = DateTime.Now,
                        AssignedToUserId = demoDevId,
                        OwnerUserId = demoSubId,
                        TicketStatusId = statusId,
                        TicketTypeId = typeId,
                        TicketPriorityId = priorityId
                    },
                     new Ticket
                     {
                         Title = "Seeded Ticket 2: Project 2",
                         ProjectId = project2Id,
                         Created = DateTime.Now,
                         AssignedToUserId = demoDevId,
                         OwnerUserId = demoSubId,
                         TicketStatusId = statusId,
                         TicketTypeId = typeId,
                         TicketPriorityId = priorityId
                     }
                );

            context.SaveChanges();
            #endregion

            #region Ticket Comment Seeding section

            #endregion


            #region Ticket Attachment Seeding section
            #endregion


            #region Ticket Notification Seeding section
            #endregion

            #region Ticket History section
            #endregion

        }
    }
}
