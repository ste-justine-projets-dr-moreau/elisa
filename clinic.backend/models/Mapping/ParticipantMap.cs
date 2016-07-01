using System.Data.Entity.ModelConfiguration;

namespace Clinic.BackEnd.Models.Mapping
{
    public class ParticipantMap : EntityTypeConfiguration<Participant>
    {
        public ParticipantMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            //Property(t => t.FirstName)
            //    .IsRequired();

            //Property(t => t.LastName)
            //    .IsRequired();

            // Table & Column Mappings
            ToTable("Participants");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.DOB).HasColumnName("DOB");
            //Property(t => t.RandomId).HasColumnName("RandomId");
            Property(t => t.IsMale).HasColumnName("IsMale");
            Property(t => t.StreetAdress).HasColumnName("StreetAdress");
            Property(t => t.Telephone).HasColumnName("Telephone");
            Property(t => t.HSJ).HasColumnName("HSJ");
            Property(t => t.MCH).HasColumnName("MCH");
            Property(t => t.CHS).HasColumnName("CHS");
            Property(t => t.IsConsent).HasColumnName("IsConsent");
            Property(t => t.IsMotherSmoking).HasColumnName("IsMotherSmoking");
            Property(t => t.MotherSmokingNumber).HasColumnName("MotherSmokingNumber");
            Property(t => t.MotherSmokingAge).HasColumnName("MotherSmokingAge");
            Property(t => t.IsFatherSmoking).HasColumnName("IsFatherSmoking");
            Property(t => t.FatherSmokingNumber).HasColumnName("FatherSmokingNumber");
            Property(t => t.FatherSmokingAge).HasColumnName("FatherSmokingAge");
            Property(t => t.ParticipantSmokingAge).HasColumnName("ParticipantSmokingAge");
            Property(t => t.Comment).HasColumnName("Comment");
            Property(t => t.EthnicPrecision).HasColumnName("EthnicPrecision");
            Property(t => t.SurgeryDate).HasColumnName("SurgeryDate");
            
            Property(t => t.FamilyHistory).HasColumnName("FamilyHistory");
            Property(t => t.Medication).HasColumnName("Medication");
            Property(t => t.DomesticAnimals).HasColumnName("DomesticAnimals");
            Property(t => t.SchoolYear).HasColumnName("SchoolYear");
            




            Property(t => t.City_Id).HasColumnName("City_Id");
            Property(t => t.Group_Id).HasColumnName("Group_Id");
            Property(t => t.Doctor_Id).HasColumnName("Doctor_Id");
            Property(t => t.Creator_Id).HasColumnName("Creator_Id");
            Property(t => t.Family_Id).HasColumnName("Family_Id");
            Property(t => t.FamilyRole_Id).HasColumnName("FamilyRole_Id");
            Property(t => t.EthnicGroup_Id).HasColumnName("EthnicGroup_Id");
            Property(t => t.SurgeryType_Id).HasColumnName("SurgeryType_Id");
            
            
           
            // Relationships
            HasRequired(t => t.City)
                .WithMany(t => t.Participants)
                .HasForeignKey(d => d.City_Id);
            
            HasOptional(t => t.Group)
                .WithMany(t => t.Participants)
                .HasForeignKey(d => d.Group_Id);
            
            HasOptional(t => t.Doctor)
               .WithMany(t => t.ParticipantsTreated)
               .HasForeignKey(d => d.Doctor_Id).WillCascadeOnDelete(false);

            HasRequired(t => t.Creator)
                .WithMany(t => t.ParticipantsCreated)
                .HasForeignKey(t => t.Creator_Id).WillCascadeOnDelete(false);

            HasOptional(t => t.Family)
             .WithMany(t => t.Participants)
             .HasForeignKey(d => d.Family_Id)
             .WillCascadeOnDelete(true);

            HasOptional(t => t.FamilyRole)
                .WithMany(t => t.Participants)
                .HasForeignKey(d => d.FamilyRole_Id);

            HasOptional(t => t.EthnicGroup)
                .WithMany(t => t.Participants)
                .HasForeignKey(d => d.EthnicGroup_Id);
            
            HasMany(x => x.Diagnoses)
                .WithMany(x => x.Participants);

            HasOptional(t => t.SurgeryType)
                .WithMany(t => t.Participants)
                .HasForeignKey(d => d.SurgeryType_Id);

            HasMany(x => x.Appointments)
                .WithRequired(x => x.Participant)
                .HasForeignKey(x => x.Participant_Id)
                .WillCascadeOnDelete(true);

            HasMany(t => t.Families)
                .WithOptional(t => t.Participant)
                .HasForeignKey(t => t.Participant_Id);
                

            HasMany(t => t.Corsets).WithRequired(t => t.Participant).HasForeignKey(t => t.Participant_Id).WillCascadeOnDelete(true);
            HasMany(t => t.DrugHistories).WithRequired(t => t.Participant).HasForeignKey(t => t.Participant_Id).WillCascadeOnDelete(true);
        }
    }
}
