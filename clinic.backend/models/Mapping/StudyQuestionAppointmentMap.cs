using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Hopital.SteJustine.BackEnd.Models.Mapping
{
    public class StudyQuestionAppointmentMap : EntityTypeConfiguration<StudyQuestionAppointment>
    {
        public StudyQuestionAppointmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("StudyQuestionAppointments");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Question_Id).HasColumnName("Question_Id");
            this.Property(t => t.Study_Id).HasColumnName("Study_Id");

            // Relationships
            this.HasRequired(t => t.Question)
                .WithMany(t => t.StudyQuestionAppointments)
                .HasForeignKey(d => d.Question_Id);
            this.HasRequired(t => t.Study)
                .WithMany(t => t.StudyQuestionAppointments)
                .HasForeignKey(d => d.Study_Id);

        }
    }
}
