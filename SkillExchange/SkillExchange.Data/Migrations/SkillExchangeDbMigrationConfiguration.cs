namespace SkillExchange.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity.Migrations;
    using Context;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class SkillExchangeDbMigrationConfiguration : DbMigrationsConfiguration<SkillExchangeDbContext>
    {
        public SkillExchangeDbMigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(SkillExchangeDbContext context)
        {
            if (!context.Users.Any())
            {
                this.CreateUsers(context);
                this.AddAdminUser(context);
            }
        }

        private void CreateUsers(SkillExchangeDbContext context)
        {
            var usersdata = new UserData[]
                {
                    new UserData
                    {
                        Username = "katherina",
                        FirstName = "Katherina",
                        LastName = "Ivanova"
                    },
                    new UserData
                    {
                        Username = "maria",
                        FirstName = "Maria",
                        LastName = "Deneva"
                    },
                    new UserData
                    {
                        Username = "nikola",
                        FirstName = "Nikola",
                        LastName = "Penev"
                    },
                    new UserData
                    {
                        Username = "georgi",
                        FirstName = "Georgi",
                        LastName = "Tenev"
                    },
                    new UserData
                    {
                        Username = "denislav",
                        FirstName = "Denislav",
                        LastName = "Benev"
                    },
                    new UserData
                    {
                        Username = "iliana",
                        FirstName = "Iliana",
                        LastName = "Geneva"
                    }
                };

            var users = new List<User>();
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 2,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            foreach (var userData in usersdata)
            {
                var user = new User
                {
                    UserName = userData.Username,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Username + "@gmail.com",
                };

                var password = "123456";
                var userCreateResult = userManager.Create(user, password);
                if (userCreateResult.Succeeded)
                {
                    users.Add(user);
                }
                else
                {
                    throw new Exception(string.Join("; ", userCreateResult.Errors));
                }
            }
        }

        private void AddAdminUser(SkillExchangeDbContext context)
        {
            var adminExists = context.Users.FirstOrDefault(u => u.UserName == "admin") != null;
            if (!adminExists)
            {
                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 2,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequireUppercase = false,
                };

                // Create admin user
                var admin = new User
                {
                    FirstName = "Admin",
                    UserName = "admin",
                    Email = "admin@abv.bg"
                };

                var adminPassword = "123456";
                var adminUserCreateResult = userManager.Create(admin, adminPassword);
                if (!adminUserCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", adminUserCreateResult.Errors));
                }

                // Create "Administrator" role
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var roleCreateResult = roleManager.Create(new IdentityRole("Administrator"));
                if (!roleCreateResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", roleCreateResult.Errors));
                }

                // Add admin user to "Administrator" role
                var adminUser = context.Users.First(user => user.UserName == "admin");
                var addAdminRoleResult = userManager.AddToRole(adminUser.Id, "Administrator");
                if (!addAdminRoleResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
                }
            }
        }
    }
}
