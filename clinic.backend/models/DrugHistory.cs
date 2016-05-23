using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class DrugHistory : Entity

    {

        
        [Display(ResourceType = typeof(Resources.DrugHistory), Name = "Dosage")]
        public String Dosage { get; set; }
        [Display(ResourceType = typeof(Resources.DrugHistory), Name = "StartDate")]
        public DateTime StartDate  { get; set; }
        [Display(ResourceType = typeof(Resources.DrugHistory), Name = "EndDate")]
        public DateTime EndDate { get; set; }
        [Display(ResourceType = typeof(Resources.DrugHistory), Name = "Comment")]
        public String Comment { get; set; }

        public int Participant_Id { get; set; }
        public int? Drug_Id { get; set; }

        public virtual Drug Drug { get; set; }
        public virtual Participant Participant { get; set; }

    }
}
