using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Clinic.BackEnd.Models;

namespace Clinic.BackEnd.Models.Mapping
{
    public class FamilyRoleMap : EntityTypeConfiguration<FamilyRole>
    {
        public FamilyRoleMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("FamilyRoles");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameFr).HasColumnName("NameFr");
            Property(t => t.IsActive).HasColumnName("IsActive");


            // Relationships


        }
    }
}
