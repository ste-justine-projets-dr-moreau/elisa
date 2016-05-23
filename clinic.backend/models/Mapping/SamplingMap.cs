using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class SamplingMap : EntityTypeConfiguration<Sampling>
    {
        public SamplingMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Samplings");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.BarCode).HasColumnName("BarCode");
            Property(t => t.Iteration).HasColumnName("Iteration");
            Property(t => t.Location).HasColumnName("Location");
            Property(t => t.Comment).HasColumnName("Comment");
            Property(t => t.SamplingType_Id).HasColumnName("SamplingType_Id");
            Property(t => t.SamplingStatus_Id).HasColumnName("SamplingStatus_Id");
            Property(t => t.Appointment_Id).HasColumnName("Appointment_Id");
           
            // Relationships
            HasRequired(t => t.SamplingStatu)
                .WithMany(t => t.Samplings)
                .HasForeignKey(d => d.SamplingStatus_Id);
            
            HasRequired(t => t.SamplingType)
                .WithMany(t => t.Samplings)
                .HasForeignKey(d => d.SamplingType_Id);
           
            HasRequired(t => t.Appointment).WithMany(t => t.Samplings).HasForeignKey(t => t.Appointment_Id).WillCascadeOnDelete(false);

            HasMany(t => t.Results).WithRequired(t => t.Sampling).HasForeignKey(t => t.Sampling_Id).WillCascadeOnDelete(true);


        }
    }
}
