using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Drug : Entity
    {
        public Drug()
        {
            DrugHistories = new List<DrugHistory>();
        }

        
        [Display(ResourceType = typeof(Resources.Drug), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Drug), Name = "Provider")]
        public string Provider { get; set; }
        [Display(ResourceType = typeof(Resources.Drug), Name = "IsActive")]
        public bool IsActive { get; set; }

        public virtual IList<DrugHistory> DrugHistories { get; set; }
    }
}
