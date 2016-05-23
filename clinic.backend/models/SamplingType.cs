using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class SamplingType : Entity
    {
        public SamplingType()
        {
            Samplings = new List<Sampling>();
        }

        
        [Display(ResourceType = typeof(Resources.SamplignType), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.SamplignType), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.SamplignType), Name = "IsActive")]
        public bool IsActive { get; set; }
        public virtual IList<Sampling> Samplings { get; set; }
    }
}
