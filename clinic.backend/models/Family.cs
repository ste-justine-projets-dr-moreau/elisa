using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Clinic.BackEnd.Models.Base;


namespace Clinic.BackEnd.Models
{
    public class Family : IDescribable
    {
        [Display(ResourceType = typeof(Resources.Family), Name = "Id")]
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Family), Name = "Name")]
        public string Name { get; set; }
        public int? Participant_Id { get; set; }
        [ForeignKey("Participant_Id")]
        public virtual Participant Participant { get; set; }
        public virtual IList<Participant> Participants { get; set; }

        #region Implementation of IDescribableEntity

        public string Describe()
        {
            string description = GetType().GetProperties().Aggregate("{ ",
                                                                     (current, property) =>
                                                                     current +
                                                                     string.Format("{0} : \"{1}\", ", property.Name, property.GetValue(this, null)));

            if (description.Length > 2)
                description = description.Substring(0, description.Length - 2) + " }";

            return description;
        }

        #endregion
    }
}
