using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Cobb : Entity

    {

        
        [Display(ResourceType = typeof(Resources.Cobb), Name = "Angle")]
        public int? Angle { get; set; }
        [Display(ResourceType = typeof(Resources.Cobb), Name = "IsRight")]
        public bool? IsRight { get; set; }
        
        public int Appointment_Id { get; set; }
        public int? CobbType_Id { get; set; }

        public virtual CobbType CobbType { get; set; }
        public virtual Appointment Appointment { get; set; }

    }
}
