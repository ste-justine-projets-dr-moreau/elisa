using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class SamplingStatu : Entity
    {
        public SamplingStatu()
        {
            Samplings = new List<Sampling>();
        }

        
        [Display(ResourceType = typeof(Resources.SamplignStatu), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.SamplignStatu), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.SamplignStatu), Name = "IsActive")]
        public bool IsActive { get; set; }
        public virtual IList<Sampling> Samplings { get; set; }
    }
}
