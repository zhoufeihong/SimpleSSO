using System;
using System.Data.Entity;
using System.Reflection;
using Wechart.EFRepositories.Migrations;

namespace SimpleSSO.EFRepositories
{
    public partial class SimpleDbContext : DbContext
    {
        public SimpleDbContext()
            : base("SimpleSSODB")
        {

            //this.Database.Log = LogUtil.Debug;
            //Database.SetInitializer<SimpleDbContext>(null);
            Database.SetInitializer(new SimpleDbContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MapSystemModuleEntities(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
