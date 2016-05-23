using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Clinic.BackEnd.Models.Base
{
    public class Entity : IDescribable
    {
        [Key]
        public int Id { get; set; } 


        
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