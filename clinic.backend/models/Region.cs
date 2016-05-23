using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Region : Entity
    {
        public Region()
        {
            Cities = new List<City>();
        }

        
         [Display(ResourceType = typeof(Resources.Region), Name = "Name")]
        public string Name { get; set; }
        public virtual IList<City> Cities { get; set; }
    }
}
