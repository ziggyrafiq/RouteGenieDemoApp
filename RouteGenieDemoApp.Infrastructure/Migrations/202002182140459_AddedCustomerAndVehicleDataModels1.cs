namespace RouteGenieDemoApp.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCustomerAndVehicleDataModels1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.Int(nullable: false, identity: true),
                        Forename = c.String(),
                        Surname = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        VehicleID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        RegistrationNumber = c.String(),
                        RegistrationDate = c.DateTime(nullable: false),
                        Manufacturer = c.String(),
                        Model = c.String(),
                        EngineSize = c.Long(nullable: false),
                        InteriorColour = c.String(),
                    })
                .PrimaryKey(t => t.VehicleID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Vehicles", new[] { "CustomerID" });
            DropTable("dbo.Vehicles");
            DropTable("dbo.Customers");
        }
    }
}
