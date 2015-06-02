﻿namespace SkillExchange.Data.Migrations
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
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SkillExchangeDbContext context)
        {
            if (!context.Users.Any())
            {
                this.CreateTowns(context);
                this.CreateUsers(context);
                this.AddAdminUser(context);
                this.CreateSkillCategories(context);
                this.CreateExchangeTypes(context);
                this.CreateSkills(context);
                this.CreateUserSkills(context);
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
                        Username = "m.deneva",
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

            var randomGenerator = new Random();
            foreach (var userData in usersdata)
            {
                var user = new User
                {
                    UserName = userData.Username,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName,
                    Email = userData.Username + "@gmail.com",
                    Town = context.Towns
                        .Find(
                            randomGenerator.Next(context.Towns.Select(t => t.Id).Min(),
                            context.Towns.Select(t => t.Id).Max()))
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

        private void CreateSkillCategories(SkillExchangeDbContext context)
        {
            context.SkillCategories.AddOrUpdate(
                new SkillCategory
                {
                    Name = "Art and Design"
                },
                new SkillCategory
                {
                    Name = "Services"
                },
                new SkillCategory
                {
                    Name = "Computers"
                },
                new SkillCategory
                {
                    Name = "Cooking"
                }, new SkillCategory
                {
                    Name = "Languages"
                });

            context.SaveChanges();
        }

        private void CreateExchangeTypes(SkillExchangeDbContext context)
        {
            context.ExchangeTypes.AddOrUpdate(
                new ExchangeType
                {
                    Name = "Offering"
                },
                new ExchangeType
                {
                    Name = "Seeking"
                });
        }

        private void CreateSkills(SkillExchangeDbContext context)
        {
            context.Skills.AddOrUpdate(
                new Skill
                {
                    Name = "gardening",
                    Category = context.SkillCategories.First(c => c.Name == "Services")
                },
                new Skill
                {
                    Name = "photoshop",
                    Category = context.SkillCategories.First(c => c.Name == "Computers")
                },
                new Skill
                {
                    Name = "french",
                    Category = context.SkillCategories.First(c => c.Name == "Languages")
                },
                new Skill
                {
                    Name = "soup",
                    Category = context.SkillCategories.First(c => c.Name == "cooking")
                },
                new Skill
                {
                    Name = "quilling",
                    Category = context.SkillCategories.First(c => c.Name == "Art and Design")
                });

            context.SaveChanges();
        }

        private void CreateTowns(SkillExchangeDbContext context)
        {
            context.Towns.AddOrUpdate(
                new Town
                {
                    Name = "Sofia"
                },
                new Town
                {
                    Name = "Stara Zagora"
                },
                new Town
                {
                    Name = "Plovdiv"
                },
                new Town
                {
                    Name = "Varna"
                },
                new Town
                {
                    Name = "Pleven"
                },
                new Town
                {
                    Name = "Vidin"
                });

            context.SaveChanges();
        }

        private void CreateUserSkills(ISkillExchangeDbContext context)
        {
            context.UserSkills.AddOrUpdate(
                new UserSkill
                {
                    UserId = context.Users.First(u => u.UserName == "katherina").Id,
                    SkillId = context.Skills.First(s => s.Name == "gardening").Id,
                    ExchangeTypeId = 1
                },
                new UserSkill
                {
                    UserId = context.Users.First(u => u.UserName == "nikola").Id,
                    SkillId = context.Skills.First(s => s.Name == "photoshop").Id,
                    ExchangeTypeId = 1
                },
                new UserSkill
                {
                    UserId = context.Users.First(u => u.UserName == "nikola").Id,
                    SkillId = context.Skills.First(s => s.Name == "french").Id,
                    ExchangeTypeId = 1
                },
                new UserSkill
                {
                    UserId = context.Users.First(u => u.UserName == "iliana").Id,
                    SkillId = context.Skills.First(s => s.Name == "quilling").Id,
                    ExchangeTypeId = 2
                },
                new UserSkill
                {
                    UserId = context.Users.First(u => u.UserName == "iliana").Id,
                    SkillId = context.Skills.First(s => s.Name == "photoshop").Id,
                    ExchangeTypeId = 2
                });

            context.SaveChanges();
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
                    LastName = "Admin",
                    UserName = "administrator",
                    Email = "admin@abv.bg",
                    TownId = 1
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
                var adminUser = context.Users.First(user => user.UserName == "administrator");
                var addAdminRoleResult = userManager.AddToRole(adminUser.Id, "Administrator");
                if (!addAdminRoleResult.Succeeded)
                {
                    throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
                }
            }
        }
    }
}
