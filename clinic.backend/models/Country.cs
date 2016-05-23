using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Country : Entity
    {
        public Country()
        {
            Provinces = new List<Province>();
        }

        
        [Display(ResourceType = typeof(Resources.Country), Name = "Name")]
        public string Name { get; set; }
        public virtual IList<Province> Provinces { get; set; }
    }
}
