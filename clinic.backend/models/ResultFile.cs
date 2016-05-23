using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class ResultFile : Entity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public DateTime Date { get; set; }
        public int User_Id { get; set; }
        public int LabTest_Id { get; set; }
        public virtual LabTest LabTest { get; set; }
        public virtual User User { get; set; }

        public byte[] FileData { get; set; }
    }
}
