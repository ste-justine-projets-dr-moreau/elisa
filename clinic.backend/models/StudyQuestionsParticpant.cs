using System;
using System.Collections.Generic;

namespace Hopital.SteJustine.BackEnd.Models
{
    public  class StudyQuestionsParticpant
    {
        public StudyQuestionsParticpant()
        {
            this.ParticipantsAnswers = new List<ParticipantsAnswer>();
        }

        public int Id { get; set; }
        public int Study_Id { get; set; }
        public int Question_Id { get; set; }
        public virtual IList<ParticipantsAnswer> ParticipantsAnswers { get; set; }
        public virtual Question Question { get; set; }
        public virtual Study Study { get; set; }
    }
}
