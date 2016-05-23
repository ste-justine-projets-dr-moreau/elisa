using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Role : Entity
    {
        
        [Display(ResourceType = typeof(Resources.Role), Name = "Name")]

        public string Name { get; set; }
        [Display(ResourceType = typeof(Resources.Role), Name = "NameFr")]
        public string NameFr { get; set; }
        public virtual IList<User> Users { get; set; }
        [Display(ResourceType = typeof(Resources.Role), Name = "IsActive")]
        public bool IsActive { get; set; }
    }
}
