using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class ResultFileMap : EntityTypeConfiguration<ResultFile>
    {
        public ResultFileMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("ResultFiles");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.ContentType).HasColumnName("ContentType");
            Property(t => t.Date).HasColumnName("Date");
            Property(t => t.FileData).HasColumnName("FileData");
            Property(t => t.User_Id).HasColumnName("User_Id");
            Property(t => t.LabTest_Id).HasColumnName("LabTest_Id");

            // Relationships
            HasRequired(t => t.LabTest)
                .WithMany(t => t.ResultFiles)
                .HasForeignKey(d => d.LabTest_Id);
            HasRequired(t => t.User)
                .WithMany(t => t.ResultFiles)
                .HasForeignKey(d => d.User_Id);

        }
    }
}
