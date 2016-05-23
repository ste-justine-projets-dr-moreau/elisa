using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class DrugHistoryMap : EntityTypeConfiguration<DrugHistory>
    {
        public DrugHistoryMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("DrugHistories");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Dosage).HasColumnName("Dosage");
            Property(t => t.Dosage).HasColumnName("StartDate");
            Property(t => t.Dosage).HasColumnName("EndDate");
            Property(t => t.Comment).HasColumnName("Comment");

            Property(t => t.Drug_Id).HasColumnName("Drug_Id");
            Property(t => t.Participant_Id).HasColumnName("Participant_Id");
            
            // Relationships

            HasOptional(t => t.Drug)
                .WithMany(t => t.DrugHistories)
                .HasForeignKey(d => d.Drug_Id);

            HasRequired(t => t.Participant)
                .WithMany(t => t.DrugHistories)
                .HasForeignKey(d => d.Participant_Id);



        }
    }
}
