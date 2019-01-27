namespace Moses_Bugtracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
            var userId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("DemoProjectManager@mailinator.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            userId = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");

            userId = userManager.FindByEmail("DemoSubmitter@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");
        }
    }
}
