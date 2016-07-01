using Clinic.BackEnd.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.BackEnd.Models
{
    public class ParticipantIdTrauma : IDescribable
    {
        public double IdParticipantPourTrauma { get; set; }
        public string TraumaId { get; set; }

        public string Describe()
        {
            throw new NotImplementedException();
        }
    }
}
