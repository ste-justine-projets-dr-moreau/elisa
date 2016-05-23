using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Sampling : Entity
    {
        public Sampling()
        {
            Results = new List<Result>();
        }

        
        [Display(ResourceType = typeof(Resources.Sampling), Name = "BarCode")]
        public string BarCode { get; set; }
        [Display(ResourceType = typeof(Resources.Sampling), Name = "Iteration")]
        public int Iteration { get; set; }
        [Display(ResourceType = typeof(Resources.Sampling), Name = "Location")]
        public string Location { get; set; }
        [Display(ResourceType = typeof(Resources.Sampling), Name = "Comment")]
        public string Comment { get; set; }
        
        public int SamplingType_Id { get; set; }
        public int SamplingStatus_Id { get; set; }
        public int Appointment_Id { get; set; }
        
        public virtual Appointment Appointment { get; set; }
        public virtual IList<Result> Results { get; set; }
        public virtual SamplingStatu SamplingStatu { get; set; }
        public virtual SamplingType SamplingType { get; set; }
    }
}
