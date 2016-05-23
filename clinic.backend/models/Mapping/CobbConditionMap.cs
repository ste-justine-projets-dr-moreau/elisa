using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class CobbConditionMap : EntityTypeConfiguration<CobbCondition>
    {
        public CobbConditionMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("CobbConditions");

            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameFr).HasColumnName("NameFr");
            Property(t => t.IsActive).HasColumnName("IsActive");

            HasMany(x => x.Appointments).WithMany(x => x.CobbConditions);
        }
    }
}
