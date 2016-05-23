using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Table & Column Mappings
            ToTable("Users");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.NT).HasColumnName("NT");
            Property(t => t.IsActive).HasColumnName("IsActive");
            Property(t => t.Hospital_Id).HasColumnName("Hospital_Id");
            Property(t => t.Language_Id).HasColumnName("Language_Id");

            // Relationships
            HasOptional(t => t.Hospital)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.Hospital_Id);
            HasRequired(t => t.Language)
                .WithMany(t => t.Users)
                .HasForeignKey(d => d.Language_Id);

            HasMany(t => t.Roles).WithMany(t => t.Users);

            HasMany(t => t.ParticipantsCreated)
                .WithRequired(t => t.Creator)
                .HasForeignKey(t => t.Creator_Id);

            HasMany(t => t.ParticipantsTreated)
                .WithOptional(t => t.Doctor)
                .HasForeignKey(t => t.Doctor_Id);


        }
    }
}
