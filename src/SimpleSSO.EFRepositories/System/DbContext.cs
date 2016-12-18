using System.Data.Entity;
using SimpleSSO.Domain.System;

namespace SimpleSSO.EFRepositories
{
    partial class SimpleDbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<App> Apps { get; set; }

        public DbSet<Role> Roles { get; set; }

        private void MapSystemModuleEntities(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Sys_User");
        }
    }
}
