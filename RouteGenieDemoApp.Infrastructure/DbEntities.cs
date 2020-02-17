using RouteGenieDemoApp.Infrastructure.Entity;
using RouteGenieDemoApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RouteGenieDemoApp.Infrastructure
{
    public partial class DbEntities : DbContext
    {

        public DbEntities()
      : this(null)
        { }

        public static string _testersConnectionString = ConfigurationManager.ConnectionStrings["RouteGenieDemoAppTst"].ConnectionString.ToString();
        public static string _developmentConnectionString = ConfigurationManager.ConnectionStrings["RouteGenieDemoAppDev"].ConnectionString.ToString();

        public DbEntities(string username) : base(_developmentConnectionString)
        {
          //  Database.SetInitializer<DbEntities>(new MigrateDatabaseToLatestVersion<DbEntities, FS.Portal.Infrastructure.Migrations.Configuration>());

            var objectContextAdapter = this as IObjectContextAdapter;
            objectContextAdapter.ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = true;


            Username = string.IsNullOrWhiteSpace(username) ? "Unknown User" : username;
        }

        public string Username { get; private set; }


        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public static DbEntities Create()
        {
            return new DbEntities(null);
        }

        public static DbEntities Create(string username)
        {
            return new DbEntities(username);
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 




        }
       
        public override int SaveChanges()
        {
            UpdateTracking();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateTracking();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTracking()
        {
            foreach (var change in ChangeTracker.Entries<EntityBase>())
            {
                if (change.State == EntityState.Added)
                {
                    change.Entity.CreatedDate = DateTime.Now;
                    change.Entity.CreatedBy = Username;
                }
                else
                {
                    if (change.Property("CreatedDate").IsModified)
                    {
                        change.Property("CreatedDate").CurrentValue = change.Property("CreatedDate").OriginalValue;
                        change.Property("CreatedDate").IsModified = false;
                    }
                }
                change.Entity.LastModifiedDate = DateTime.Now;
                change.Entity.LastModifiedBy = Username;
            }
        }
    }
}
