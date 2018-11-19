namespace Remake_CB_4_Project_1.Migrations
{
    using System;
    using Remake_CB_4_Project_1.Persistance;
    using System.Data.Entity;
    using Remake_CB_4_Project_1.Core.Domain;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MessageAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MessageAppContext context)
        {

            context.Users.AddOrUpdate(a => a.Name,
                new User
                {
                    Name = "admin",
                    Password = "-487706753",
                    Saltword = "gLtpVb1",
                    AccessLevel = 5
                });



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
        }
    }
}
