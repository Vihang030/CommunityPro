namespace CommunityPro.DAL.SecurityMigrations
{
    using CommunityPro.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CommunityPro.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DAL\SecurityMigrations";
        }

        protected override void Seed(CommunityPro.DAL.ApplicationDbContext context)
        {
            //Create a Role Manager
            var roleManager = new RoleManager<IdentityRole>(new
                                          RoleStore<IdentityRole>(context));
            //Create Role Admin if it does not exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Admin"));
            }
            //Create Role Supervisor if it does not exist
            if (!context.Roles.Any(r => r.Name == "Managers"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Manager"));
            }
            //Create Role Security if it does not exist
            if (!context.Roles.Any(r => r.Name == "Applicant"))
            {
                var roleresult = roleManager.Create(new IdentityRole("Applicant"));
            }
           
            //Create a User Manager
            var manager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //-----------------------------------------
            //Now the Admin user named admin1 with password password
            var adminuser = new ApplicationUser
            {
                UserName = "admin@outlook.com",
                Email = "admin@outlook.com"
            };

            //Assign admin user to role
            if (!context.Users.Any(u => u.UserName == "admin@outlook.com"))
            {
                manager.Create(adminuser, "password");
                manager.AddToRole(adminuser.Id, "Admin");
            }
            //-----------------------------------------------------
            //Now the Security user named security1 with password password
            var manageruser = new ApplicationUser
            {
                UserName = "manager@outlook.com",
                Email = "manager@outlook.com"
            };

            //Assign security user to role
            if (!context.Users.Any(u => u.UserName == "manager@outlook.com"))
            {
                manager.Create(manageruser, "password");
                manager.AddToRole(manageruser.Id, "Manager");
            }
            //-----------------------------------------------------
            //Now the Supervisor user named supervisor1 with password password
            var applicantsuser = new ApplicationUser
            {
                UserName = "singh@outlook.com",
                Email = "singh@outlook.com"
            };

            //Assign supervisor user to role
            if (!context.Users.Any(u => u.UserName == "singh@outlook.com"))
            {
                manager.Create(applicantsuser, "password");
                manager.AddToRole(applicantsuser.Id, "Applicant");
            }
            //-----------------------------------------------------
           
        }
    }
}
