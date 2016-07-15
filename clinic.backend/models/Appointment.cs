using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.BackEnd.Models
{
    public  class Appointment : Entity
    {
        public Appointment()
        {
            CobbConditions = new List<CobbCondition>();
        }

        
        [Display(ResourceType = typeof(Resources.Appointment), Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Hour")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public String Hour { get; set; }

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Height")]
        public decimal? Height { get; set; }

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Anovulant")]
        public bool? Anovulant { get; set; }  

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Pain")]
        public bool? IsPain { get; set; } 

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Smoker")]
        public bool? IsSmoker { get; set; } 

        [Display(ResourceType = typeof(Resources.Appointment), Name = "SmokePerDay")]
        public int? SmokePerDay { get; set; } 

        [Display(ResourceType = typeof(Resources.Appointment), Name = "Risser")]
        public decimal? Risser { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Appointment), Name = "Comment")]
        public string Comment { get; set; }

        public decimal? TheBMI { get; set; }

        [NotMapped]
        public int? ParticipantAgeAtAppointment { get; set; }

        public int Participant_Id { get; set; }
        public int User_Id { get; set; }       
 
        public virtual Participant Participant { get; set; }
        public virtual User User { get; set; }

        public virtual IList<Sampling> Samplings { get; set; }
        public virtual IList<Cobb> Cobbs { get; set; }
        [Display(ResourceType = typeof(Resources.Appointment), Name = "CobbCondition")]
        public virtual IList<CobbCondition> CobbConditions { get; set; }
        

    }
}
