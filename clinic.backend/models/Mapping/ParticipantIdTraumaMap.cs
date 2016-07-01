using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.BackEnd.Models.Mapping
{
    public class ParticipantIdTraumaMap : EntityTypeConfiguration<ParticipantIdTrauma>
    {
        public ParticipantIdTraumaMap()
        {
            HasKey(t => t.IdParticipantPourTrauma);

            ToTable("Participants-ID-Trauma");

            Property(t => t.IdParticipantPourTrauma).HasColumnName("IdParticipant-pour-Trauma");
            Property(t => t.TraumaId).HasColumnName("TraumaId");

        }
    }
}
