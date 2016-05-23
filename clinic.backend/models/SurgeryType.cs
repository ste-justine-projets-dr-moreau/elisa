using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class SurgeryType : Entity
    {
        public SurgeryType()
        {
            Participants = new List<Participant>();
        }

        
        [Display(ResourceType = typeof(Resources.SurgeryType), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.SurgeryType), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.SurgeryType), Name = "IsActive")]
        public bool IsActive { get; set; }
        
        public virtual IList<Participant> Participants { get; set; }
    }
}
