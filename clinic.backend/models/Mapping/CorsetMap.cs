using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class CorsetMap : EntityTypeConfiguration<Corset>
    {
        public CorsetMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Corsets");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Start).HasColumnName("Start");
            Property(t => t.End).HasColumnName("End");
            Property(t => t.Comment).HasColumnName("Comment");

            Property(t => t.CorsetType_Id).HasColumnName("CorsetType_Id");
            Property(t => t.Participant_Id).HasColumnName("Participant_Id");
           // Relationships

            HasRequired(t => t.CorsetType)
                .WithMany(t => t.Corsets)
                .HasForeignKey(d => d.CorsetType_Id);

            HasRequired(t => t.Participant)
                .WithMany(t => t.Corsets)
                .HasForeignKey(d => d.Participant_Id);
        }
    }
}
