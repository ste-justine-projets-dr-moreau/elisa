using System;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models.Auditing
{
    public class Log
    {
        public int Id { get; set; }

        public string Nt { get; set; }

        public string EventType { get; set; }
        public DateTime? EventDate { get; set; }

        public string EntityName { get; set; }
        public string RecordId { get; set; }
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
    }
}