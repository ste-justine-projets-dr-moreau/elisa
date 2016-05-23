using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Result : Entity
    {
        
        [Display(ResourceType = typeof(Resources.Result), Name = "Date")]
        public DateTime Date { get; set; }
        [Display(ResourceType = typeof(Resources.Result), Name = "Labresult")]
        public float? Labresult { get; set; }
        [Display(ResourceType = typeof(Resources.Result), Name = "SubSample")]
        public string SubSample { get; set; }
        [Display(ResourceType = typeof(Resources.Result), Name = "Comment")]
        public string Comment { get; set; }
        public int User_Id { get; set; }
        public int LabTest_Id { get; set; }
        public int Sampling_Id { get; set; }
        public virtual LabTest LabTest { get; set; }
        public virtual Sampling Sampling { get; set; }
        public virtual User User { get; set; }
    }
}
