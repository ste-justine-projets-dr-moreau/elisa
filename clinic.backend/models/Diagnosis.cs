using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Diagnosis : Entity
    {
        public Diagnosis()
        {
            Participants = new List<Participant>();
        }

        
        [Display(ResourceType = typeof(Resources.Diagnosis), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Diagnosis), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.Diagnosis), Name = "IsActive")]
        public bool IsActive { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }

}