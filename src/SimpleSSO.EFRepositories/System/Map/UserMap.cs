using System;
using System.Data.Entity.ModelConfiguration;
using SimpleSSO.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSSO.EFRepositories.Map
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.ToTable("Sys_User");
            this.HasKey(p => p.ID);
            this.Property(p => p.UserID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Name).HasMaxLength(200);
            this.Property(p => p.Mobile).HasMaxLength(200);
            this.Property(p => p.Email).HasMaxLength(200);
            this.Property(p => p.CreatedBy).HasMaxLength(50);
            this.Property(p => p.LastUpdatedBy).HasMaxLength(50);
            this.Property(p => p.RealName).HasMaxLength(200);
            this.Property(p => p.Password).HasMaxLength(200);
            this.Property(p => p.Salt).HasMaxLength(200);
        }
    }
}