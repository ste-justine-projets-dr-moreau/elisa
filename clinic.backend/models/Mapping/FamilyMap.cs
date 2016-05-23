using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Clinic.BackEnd.Models;

namespace Clinic.BackEnd.Models.Mapping
{
    public class FamilyMap : EntityTypeConfiguration<Family>
    {
        public FamilyMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Families");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Participant_Id).HasColumnName("Participant_Id");

            // Relationships
            HasOptional(t => t.Participant)
                .WithMany(t => t.Families)
                .HasForeignKey(t => t.Participant_Id);

            HasMany(t => t.Participants)
                .WithOptional(t => t.Family)
                .HasForeignKey(t =>t.Family_Id)
                .WillCascadeOnDelete(true);


        }
    }
}
