using System.Collections.Generic;
using System.Data.Entity.Validation;
using DesignHubSite.Models;
using Microsoft.AspNet.Identity;

namespace DesignHubSite.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DesignHubSite.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DesignHubSite.Models.ApplicationDbContext context)
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

            if (!context.Users.Any())
            {
                System.Diagnostics.Debug.WriteLine("INSIDE");
                var hasher = new PasswordHasher();
                try
                {
                    // users

                    var users = new List<ApplicationUser> {
                        new ApplicationUser
                        {
                            PasswordHash = hasher.HashPassword("123456"),
                            Email = "kulfon@gmail.com",
                            UserName = "kulfon@gmail.com",
                            SecurityStamp = Guid.NewGuid().ToString()
                        },
                        new ApplicationUser
                        {
                            PasswordHash = hasher.HashPassword("123456"),
                            Email = "zaba@gmail.com",
                            UserName = "zaba@gmail.com",
                            SecurityStamp = Guid.NewGuid().ToString()
                        }
                        };

                    users.ForEach(user => context.Users.AddOrUpdate(user));

                    // images

                    Image image = new Image { Name = "Cat picture", Description = "none" };

                    context.Images.Add(image);

                    context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    System.Diagnostics.Debug.WriteLine("EXC: ");
                    foreach (var result in e.EntityValidationErrors)
                    {
                        foreach (var error in result.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                        }
                    }

                }
            }

        }
    }
}
