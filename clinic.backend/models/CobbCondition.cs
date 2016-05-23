using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class CobbCondition : Entity
    {
        public CobbCondition()
        {
            Appointments = new List<Appointment>();
        }

        
        [Display(ResourceType = typeof(Resources.CorsetType), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.CorsetType), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.CorsetType), Name = "IsActive")]
        public bool IsActive { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
