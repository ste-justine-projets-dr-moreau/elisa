using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Cities");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Province_Id).HasColumnName("Province_Id");
            Property(t => t.Region_Id).HasColumnName("Region_Id");

            // Relationships
            HasRequired(t => t.Province)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.Province_Id);
            HasRequired(t => t.Region)
                .WithMany(t => t.Cities)
                .HasForeignKey(d => d.Region_Id);

        }
    }
}
