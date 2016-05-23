using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Clinic.BackEnd.Models;

namespace Clinic.BackEnd.Models.Mapping
{
    public class ResultMap : EntityTypeConfiguration<Result>
    {
        public ResultMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Results");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.Labresult).HasColumnName("Labresult");
            Property(t => t.SubSample).HasColumnName("SubSample");
            Property(t => t.Comment).HasColumnName("Comment");
            Property(t => t.User_Id).HasColumnName("User_Id");
            Property(t => t.LabTest_Id).HasColumnName("LabTest_Id");
            Property(t => t.Sampling_Id).HasColumnName("Sampling_Id");

            // Relationships
            HasRequired(t => t.LabTest)
                .WithMany(t => t.Results)
                .HasForeignKey(d => d.LabTest_Id);
            HasRequired(t => t.Sampling)
                .WithMany(t => t.Results)
                .HasForeignKey(d => d.Sampling_Id);
            HasRequired(t => t.User)
                .WithMany(t => t.Results)
                .HasForeignKey(d => d.User_Id);

        }
    }
}
