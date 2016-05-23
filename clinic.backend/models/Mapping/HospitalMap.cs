using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class HospitalMap : EntityTypeConfiguration<Hospital>
    {
        public HospitalMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Hospitals");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
        }
    }
}
