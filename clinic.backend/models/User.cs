using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class User : Entity
    {
        public User()
        {
            Appointments = new List<Appointment>();
            ResultFiles = new List<ResultFile>();
            Results = new List<Result>();
            Roles = new List<Role>();
            //ParticipantsCreated = new List<Participant>();
            //ParticipantsTreated = new List<Participant>();

        }

        
        [Display(ResourceType = typeof(Resources.User), Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(ResourceType = typeof(Resources.User), Name = "LastName")]
        public string LastName { get; set; }
        [Display(ResourceType = typeof(Resources.User), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.User), Name = "Nt")]
        public string NT { get; set; }
        [Display(ResourceType = typeof(Resources.User), Name = "IsActive")]
        public bool IsActive { get; set; }
        public int? Hospital_Id { get; set; }
        public int? Language_Id { get; set; }
       
        public virtual Hospital Hospital { get; set; }
        public virtual Language Language { get; set; }

        public virtual IList<Appointment> Appointments { get; set; }
        public virtual IList<ResultFile> ResultFiles { get; set; }
        public virtual IList<Result> Results { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual IList<Participant> ParticipantsTreated { get; set; }
        public virtual IList<Participant> ParticipantsCreated { get; set; }

        [Display(ResourceType = typeof(Resources.User), Name = "FullName")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
