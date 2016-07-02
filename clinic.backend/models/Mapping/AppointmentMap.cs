using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class AppointmentMap : EntityTypeConfiguration<Appointment>
    {
        public AppointmentMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Appointments");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.Hour).HasColumnName("Hour");
            Property(t => t.Height).HasColumnName("Height");
            Property(t => t.Weight).HasColumnName("Weight");
            Property(t => t.Anovulant).HasColumnName("Anovulant"); 
            Property(t => t.IsPain).HasColumnName("IsPain");
            Property(t => t.IsSmoker).HasColumnName("IsSmoker");
            Property(t => t.SmokePerDay).HasColumnName("SmokePerDay");
            Property(t => t.Comment).HasColumnName("Comment");
            Property(t => t.Participant_Id).HasColumnName("Participant_Id");
            Property(t => t.TheBMI).HasColumnName("TheBMI");
           
            // Relationships
            HasRequired(t => t.Participant)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.Participant_Id);

            HasMany(t => t.Cobbs)
                .WithRequired(t => t.Appointment)
                .HasForeignKey(d => d.Appointment_Id)
                    .WillCascadeOnDelete(true);

            HasMany(t => t.Samplings)
                .WithRequired(t => t.Appointment)
                .HasForeignKey(d => d.Appointment_Id).WillCascadeOnDelete(false);

            HasRequired(t => t.User)
                .WithMany(t => t.Appointments)
                .HasForeignKey(d => d.User_Id);

            HasMany(x => x.CobbConditions).WithMany(x => x.Appointments);
        }
    }
}
