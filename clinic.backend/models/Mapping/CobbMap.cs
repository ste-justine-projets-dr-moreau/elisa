using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class CobbMap : EntityTypeConfiguration<Cobb>
    {
        public CobbMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Cobbs");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Angle).HasColumnName("Angle");
            Property(t => t.IsRight).HasColumnName("IsRight");


           Property(t => t.CobbType_Id).HasColumnName("CobbType_Id");
            // Relationships

            HasOptional(t => t.CobbType)
                .WithMany(t => t.Cobbs)
                .HasForeignKey(d => d.CobbType_Id);



        }
    }
}
