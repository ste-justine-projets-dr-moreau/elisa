using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Province : Entity
    {
        public Province()
        {
            Cities = new List<City>();
        }

        
        [Display(ResourceType = typeof(Resources.Province), Name = "Name")]
        public string Name { get; set; }
        public int Country_Id { get; set; }
        public virtual IList<City> Cities { get; set; }
        public virtual Country Country { get; set; }
    }
}
