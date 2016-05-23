using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class DrugMap : EntityTypeConfiguration<Drug>
    {
        public DrugMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Drugs");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Name).HasColumnName("Name");
            Property(t => t.Provider).HasColumnName("Provider");
            Property(t => t.IsActive).HasColumnName("IsActive");
            // Relationships



        }
    }
}
