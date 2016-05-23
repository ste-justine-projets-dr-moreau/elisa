using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class EthnicGroup : Entity
    {
        public EthnicGroup()
        {
            Participants = new List<Participant>();
        }

        
         [Display(ResourceType = typeof(Resources.EthnicGroup), Name = "Name")]
         public string Name { get; set; }
         [Display(ResourceType = typeof(Resources.EthnicGroup), Name = "NameFr")]
         public string NameFr { get; set; }
         [Display(ResourceType = typeof(Resources.EthnicGroup), Name = "IsPrecision")]
         public bool ? IsPrecision { get; set; }
         [Display(ResourceType = typeof(Resources.EthnicGroup), Name = "IsActive")]
         public bool IsActive { get; set; }
         public virtual IList<Participant> Participants { get; set; }
    }
}
