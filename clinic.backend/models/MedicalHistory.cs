using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class MedicalHistory : Entity
    {
        public MedicalHistory()
        {
            Participants = new List<Participant>();
        }

        
        [Display(ResourceType = typeof(Resources.MedicalHistory), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.MedicalHistory), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.MedicalHistory), Name = "IsActive")]
        public bool IsActive { get; set; }
        
        public virtual ICollection<Participant> Participants { get; set; }
    }

}