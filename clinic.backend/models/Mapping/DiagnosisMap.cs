using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class DiagnosisMap : EntityTypeConfiguration<Diagnosis>
    {
        public DiagnosisMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Diagnoses");

            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.NameFr).HasColumnName("NameFr");
            Property(t => t.IsActive).HasColumnName("IsActive");

            HasMany(x => x.Participants).WithMany(x => x.Diagnoses);
        }
    }
}
