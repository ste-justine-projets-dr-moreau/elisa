using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Base;


namespace Clinic.BackEnd.Models
{
    public class FamilyRole : Entity
    {


        
        [Display(ResourceType = typeof(Resources.FamilyRole), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.FamilyRole), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.FamilyRole), Name = "IsActive")]
        public bool IsActive { get; set; }
        public virtual IList<Participant> Participants { get; set; }
    }
}
