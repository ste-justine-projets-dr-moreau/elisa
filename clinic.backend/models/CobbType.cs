using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class CobbType : Entity
    {
        //public CobbType()
        //{
        //    Appointments= new List<CobbType>();
        //}
        public CobbType()
        {
            Cobbs = new List<Cobb>();
        }

        
        [Display(ResourceType = typeof(Resources.CobbType), Name = "Name")]
        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.CobbType), Name = "NameFr")]
        public string NameFr { get; set; }
        [Display(ResourceType = typeof(Resources.CobbType), Name = "IsActive")]
        public bool IsActive { get; set; }
        [Display(ResourceType = typeof(Resources.CobbType), Name = "ShortName")]
        public string ShortName { get; set; }
        
        public virtual IList<Cobb> Cobbs { get; set; }
    }
}
