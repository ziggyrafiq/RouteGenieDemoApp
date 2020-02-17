namespace RouteGenieDemoApp.Infrastructure.Migrations
{
    using RouteGenieDemoApp.Infrastructure.Models;
    using RouteGenieDemoApp.Resources;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RouteGenieDemoApp.Infrastructure.DbEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true
                ;
        }

        protected override void Seed(RouteGenieDemoApp.Infrastructure.DbEntities context)
        {
#if DEBUG
            var executeSeed = false;
            if (executeSeed)
            {
                try
                {
                    context.Roles.AddOrUpdate(o => o.Name,
                        new Role { Name = AppTerms.UserRoleMaster, Description=AppMessages.UserRoleMaster },
                        new Role { Name = AppTerms.UserRoleAdmin, Description=AppMessages.UserRoleAdmin },
                        new Role { Name = AppTerms.UserRoleStandard, Description = AppMessages.UserRoleStandard }
                    );

                    context.SaveChanges();

                    //admin password is Password123!
                    context.Users.AddOrUpdate(o => o.Email,
                       new User { FirstName = "Ziggy", LastName = "Rafiq", Email = "admin@ziggyrafiq.com", RoleID = context.Roles.ToList()[0].RoleID, Password = "kPWCyi2KIbzTdZJPBce7yZPGE6U=", Salt = "bXmBeuR66NcqC+bzZeOH1g==" }

                    );


                }
                catch (Exception ex)
                {
                    string ahhh = AppMessages.StandardError;
                }
            }
#endif
        }
    }
}
