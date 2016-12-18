using System;
using System.Data.Entity.ModelConfiguration;
using SimpleSSO.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSSO.EFRepositories.Map
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            this.ToTable("Sys_Role");
            this.HasKey(p => p.ID);
            this.Property(p => p.Name).HasMaxLength(200);
            this.HasMany(x => x.Users).
            WithMany(x => x.Roles).
            Map(m => m.ToTable("Sys_UserRoleMap").
                MapLeftKey("RoleID").
                MapRightKey("UserID"));
        }
    }
}