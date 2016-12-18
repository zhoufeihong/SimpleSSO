namespace Wechart.EFRepositories.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sys_User",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    UserID = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 200),
                    Password = c.String(maxLength: 200),
                    Salt = c.String(maxLength: 200),
                    IsLocked = c.Boolean(nullable: false),
                    RealName = c.String(maxLength: 200),
                    Email = c.String(maxLength: 200),
                    Mobile = c.String(maxLength: 200),
                    LastLoginDate = c.DateTime(),
                    CreatedOn = c.DateTime(),
                    CreatedBy = c.String(maxLength: 50),
                    LastUpdatedOn = c.DateTime(),
                    LastUpdatedBy = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.ID, clustered: false);

            CreateTable(
                "dbo.Sys_Role",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    Name = c.String(maxLength: 200),
                    CreatedOn = c.DateTime(),
                    CreatedBy = c.String(),
                    LastUpdatedOn = c.DateTime(),
                    LastUpdatedBy = c.String(),
                })
                 .PrimaryKey(t => t.ID, clustered: false);

            CreateTable(
                "dbo.Sys_App",
                c => new
                {
                    ID = c.Guid(nullable: false),
                    Name = c.String(maxLength: 200),
                    ClientID = c.Int(nullable: false, identity: true),
                    ClientSecret = c.String(maxLength: 200),
                    ReturnUrl = c.String(maxLength: 200),
                    IsCredible = c.Boolean(nullable: false),
                    IconUrl = c.String(maxLength: 200),
                    CreatedOn = c.DateTime(),
                    CreatedBy = c.String(maxLength: 50),
                    LastUpdatedOn = c.DateTime(),
                    LastUpdatedBy = c.String(maxLength: 50),
                })
                 .PrimaryKey(t => t.ID, clustered: false); ;

            CreateTable(
                "dbo.Sys_UserRoleMap",
                c => new
                {
                    RoleID = c.Guid(nullable: false),
                    UserID = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.RoleID, t.UserID })
                .ForeignKey("dbo.Sys_Role", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.Sys_User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.UserID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Sys_UserRoleMap", "UserID", "dbo.Sys_User");
            DropForeignKey("dbo.Sys_UserRoleMap", "RoleID", "dbo.Sys_Role");
            DropIndex("dbo.Sys_UserRoleMap", new[] { "UserID" });
            DropIndex("dbo.Sys_UserRoleMap", new[] { "RoleID" });
            DropTable("dbo.Sys_UserRoleMap");
            DropTable("dbo.Sys_App");
            DropTable("dbo.Sys_Role");
            DropTable("dbo.Sys_User");
        }
    }
}
