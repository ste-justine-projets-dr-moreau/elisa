using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Hospital : Entity
    {
        public Hospital()
        {
            Participants = new List<Participant>();
            Users = new List<User>();
        }

        
        [Display(ResourceType = typeof(Resources.Hospital), Name = "Name")]
        public string Name { get; set; }
        public virtual IList<Participant> Participants { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
