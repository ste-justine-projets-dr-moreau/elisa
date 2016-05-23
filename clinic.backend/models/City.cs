using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class City : Entity
    {
        public City()
        {
            Participants = new List<Participant>();
        }

        
        [Display(ResourceType = typeof(Resources.City), Name = "Name")]
        public string Name { get; set; }
        public int Province_Id { get; set; }
        public int Region_Id { get; set; }
        public virtual Province Province { get; set; }
        public virtual Region Region { get; set; }
        public virtual IList<Participant> Participants { get; set; }
    }
}
