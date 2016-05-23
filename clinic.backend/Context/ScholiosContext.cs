using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Auditing;
using Clinic.BackEnd.Models.Base;
using Clinic.BackEnd.Models.Mapping;

namespace Clinic.BackEnd.Context
{
    public class ClinicContext : DbContext
    {
        static ClinicContext()
        {
            Database.SetInitializer<ClinicContext>(null);
        }

        public ClinicContext()
            : base("Name=ClinicContext")
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<ResultFile> ResultFiles { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Sampling> Samplings { get; set; }
        public DbSet<SamplingStatu> SamplingStatus { get; set; }
        public DbSet<SamplingType> SamplingTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<EthnicGroup> EthnicGroups { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyRole> FamilRoles { get; set; }
        public DbSet<CobbType> CobbTypes { get; set; }
        public DbSet<Cobb> Cobbs { get; set; }
        public DbSet<SurgeryType> SurgeryTypes { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<CorsetType> CorsetTypes { get; set; }
        public DbSet<Corset> Corsets { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<DrugHistory> DrugHistories { get; set; }
        public DbSet<CobbCondition> CobbConditions { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AppointmentMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new HospitalMap());
            modelBuilder.Configurations.Add(new LabTestMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new ProvinceMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new ResultFileMap());
            modelBuilder.Configurations.Add(new ResultMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SamplingMap());
            modelBuilder.Configurations.Add(new SamplingStatuMap());
            modelBuilder.Configurations.Add(new SamplingTypeMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new DiagnosisMap());
            modelBuilder.Configurations.Add(new MedicalHistoryMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new EthnicGroupMap());
            modelBuilder.Configurations.Add(new FamilyMap());
            modelBuilder.Configurations.Add(new FamilyRoleMap());
            modelBuilder.Configurations.Add(new CobbTypeMap());
            modelBuilder.Configurations.Add(new CobbMap());
            modelBuilder.Configurations.Add(new SurgeryTypeMap());
            modelBuilder.Configurations.Add(new LanguageMap());
            modelBuilder.Configurations.Add(new CorsetMap());
            modelBuilder.Configurations.Add(new CorsetTypeMap());
            modelBuilder.Configurations.Add(new DrugMap());
            modelBuilder.Configurations.Add(new DrugHistoryMap());


        }

        public int SaveChanges(string nt)
        {
            var addedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Added).ToList();

            // Get all Deleted/Modified entities (not Unmodified or Detached)
            foreach (var ent in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                // For each changed record, get the audit record entries and add them
                foreach (var auditLog in GetAuditRecordsForChange(ent, nt))
                {
                    Logs.Add(auditLog);
                }
            }



            // Call the original SaveChanges(), which will save both the changes made and the audit records
            var rowsModified = base.SaveChanges();

            // Get all Added entities (not Unmodified or Detached)
            foreach (var ent in addedEntities)
            {
                ent.State = EntityState.Added;
                // For each changed record, get the audit record entries and add them
                foreach (var auditLog in GetAuditRecordsForChange(ent, nt))
                {
                    Logs.Add(auditLog);
                }

                ent.State = EntityState.Unchanged;
            }


            return rowsModified + base.SaveChanges();
        }

        private IEnumerable<Log> GetAuditRecordsForChange(DbEntityEntry dbEntry, string nt)
        {
            var result = new List<Log>();

            var changeTime = DateTime.Now;

            // Get the Table() attribute, if one exists
            var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;

            // Get table name (if it has a Table attribute, use that, otherwise get the pluralized name)
            // ReSharper disable once PossibleNullReferenceException
            string entityName = tableAttr != null
                                   ? tableAttr.Name
                                   : dbEntry.Entity.GetType().BaseType != null && dbEntry.Entity.GetType().BaseType != typeof(Entity)
                                         ? dbEntry.Entity.GetType().BaseType.Name
                                         : dbEntry.Entity.GetType().Name;

            // Get primary key value (If you have more than one key column, this will need to be adjusted)
            string keyName = dbEntry.Entity.GetType().GetProperties().Single(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Any()).Name;

            if (dbEntry.State == EntityState.Added)
            {
                // For Inserts, just add the whole record
                // If the entity implements IDescribableEntity, use the description from Describe(), otherwise use ToString()
                var describableEntity = dbEntry.CurrentValues.ToObject() as IDescribable;
                
                result.Add(new Log
                {
                    Nt = nt,
                    EventDate = changeTime,
                    EventType = "A", // Added
                    EntityName = entityName,
                    RecordId = dbEntry.CurrentValues.GetValue<object>(keyName).ToString(),  // Again, adjust this if you have a multi-column key
                    ColumnName = "*ALL",    // Or make it nullable, whatever you want
                    NewValue = describableEntity != null ? describableEntity.Describe() : dbEntry.CurrentValues.ToObject().ToString()
                });
            }
            else if (dbEntry.State == EntityState.Deleted)
            {
                // Same with deletes, do the whole record, and use either the description from Describe() or ToString()
                var describableEntity = dbEntry.OriginalValues.ToObject() as IDescribable;
                result.Add(new Log
                {
                    Nt = nt,
                    EventDate = changeTime,
                    EventType = "D", // Deleted
                    EntityName = entityName,
                    RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                    ColumnName = "*ALL",
                    OriginalValue = describableEntity != null ? describableEntity.Describe() : dbEntry.OriginalValues.ToObject().ToString()
                });

            }
            else if (dbEntry.State == EntityState.Modified)
            {
                result.AddRange(
                    dbEntry.OriginalValues.PropertyNames.Where(
                        propertyName =>
                            !Equals(dbEntry.OriginalValues.GetValue<object>(propertyName),
                                dbEntry.CurrentValues.GetValue<object>(propertyName))).Select(propertyName => new Log
                                {
                                    Nt = nt,
                                    EventDate = changeTime,
                                    EventType = "M", // Modified
                                    EntityName = entityName,
                                    RecordId = dbEntry.OriginalValues.GetValue<object>(keyName).ToString(),
                                    ColumnName = propertyName,
                                    OriginalValue =
                                        dbEntry.OriginalValues.GetValue<object>(propertyName) == null
                                            ? null
                                            : dbEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                    NewValue =
                                        dbEntry.CurrentValues.GetValue<object>(propertyName) == null
                                            ? null
                                            : dbEntry.CurrentValues.GetValue<object>(propertyName).ToString()
                                }));
            }

            return result;
        }


    }
}
