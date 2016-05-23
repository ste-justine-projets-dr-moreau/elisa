using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class ProvinceMap : EntityTypeConfiguration<Province>
    {
        public ProvinceMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Provinces");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Country_Id).HasColumnName("Country_Id");

            // Relationships
            HasRequired(t => t.Country)
                .WithMany(t => t.Provinces)
                .HasForeignKey(d => d.Country_Id);

        }
    }
}
