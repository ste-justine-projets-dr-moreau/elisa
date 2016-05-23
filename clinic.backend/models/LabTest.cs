using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class LabTest : Entity
    {
        public LabTest()
        {
            ResultFiles = new List<ResultFile>();
            Results = new List<Result>();
        }

        
        [Display(ResourceType = typeof(Resources.Labtest), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Labtest), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.Labtest), Name = "IsActive")]
        public bool IsActive { get; set; }
        [Display(ResourceType = typeof(Resources.Labtest), Name = "SortOrder")]
        public int? SortOrder { get; set; }
        public virtual IList<ResultFile> ResultFiles { get; set; }
        public virtual IList<Result> Results { get; set; }
    }
}
