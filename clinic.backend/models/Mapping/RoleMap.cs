using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Clinic.BackEnd.Models;

namespace Clinic.BackEnd.Models.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Roles");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameFr).HasColumnName("NameFr");
            Property(t => t.IsActive).HasColumnName("IsActive");
            HasMany(t => t.Users).WithMany(t => t.Roles);

        }
    }
}
