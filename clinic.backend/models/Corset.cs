using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;


namespace Clinic.BackEnd.Models
{
    public class Corset : Entity
    {
        [Display(ResourceType = typeof(Resources.Corset), Name = "Start")]
        public DateTime? Start { get; set; }
        [Display(ResourceType = typeof(Resources.Corset), Name = "End")]
        public DateTime? End { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Corset), Name = "Comment")]
        public string Comment { get; set; }

        public int Participant_Id { get; set; }
        public int CorsetType_Id { get; set; }

        public virtual Participant Participant { get; set; }
        public virtual CorsetType CorsetType { get; set; }

    }
}
