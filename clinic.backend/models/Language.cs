using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public class Language : Entity
    {
        public Language()
        {
            Users = new List<User>();
        }

        
        [Display(ResourceType = typeof(Resources.Language), Name = "Name")]
        public string Name { get; set; }
        public virtual IList<User> Users { get; set; }
    }
}
