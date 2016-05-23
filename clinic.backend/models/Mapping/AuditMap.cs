using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Clinic.BackEnd.Models.Auditing;

namespace Clinic.BackEnd.Models.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Logs", "Auditing");
        }
    }
}
