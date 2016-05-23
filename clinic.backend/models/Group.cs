using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Group : Entity
    {
        public Group()
        {
            Participants = new List<Participant>();
        }

        
         [Display(ResourceType = typeof(Resources.Group), Name = "Name")]
        public string Name { get; set; }
         [Display(ResourceType = typeof(Resources.Group), Name = "NameFr")]
         public string NameFr { get; set; }
         [Display(ResourceType = typeof(Resources.Group), Name = "IsActive")]
         public bool IsActive { get; set; }
        public virtual IList<Participant> Participants { get; set; }
    }
}
