using System;
using System.Collections.Generic;

namespace Hopital.SteJustine.BackEnd.Models
{
    public  class StudyQuestionAppointment
    {
        public StudyQuestionAppointment()
        {
            this.AppointmentAnswers = new List<AppointmentAnswer>();
        }

        public int Id { get; set; }
        public int Question_Id { get; set; }
        public int Study_Id { get; set; }
        public virtual IList<AppointmentAnswer> AppointmentAnswers { get; set; }
        public virtual Question Question { get; set; }
        public virtual Study Study { get; set; }
    }
}
