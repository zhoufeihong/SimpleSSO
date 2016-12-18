using System;
using System.Data.Entity.ModelConfiguration;
using SimpleSSO.Domain.System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleSSO.EFRepositories.Map
{
    public class AppMap : EntityTypeConfiguration<App>
    {
        public AppMap()
        {
            this.ToTable("Sys_App");
            this.HasKey(p => p.ID);
            this.Property(p => p.ClientID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.Name).HasMaxLength(200);
            this.Property(p => p.ClientSecret).HasMaxLength(200);
            this.Property(p => p.CreatedBy).HasMaxLength(50);
            this.Property(p => p.LastUpdatedBy).HasMaxLength(50);
            this.Property(p => p.IconUrl).HasMaxLength(200);
            this.Property(p => p.ReturnUrl).HasMaxLength(200);
        }
    }
}