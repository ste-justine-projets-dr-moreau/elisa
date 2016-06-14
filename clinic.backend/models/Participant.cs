using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Models.Base;

namespace Clinic.BackEnd.Models
{
    public  class Participant : IDescribable
    {

        [Display(ResourceType = typeof(Resources.Participant), Name = "RandomId")]
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "LastName")]
        public string LastName { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        //[Display(ResourceType = typeof(Resources.Participant), Name = "RandomId")]
        //public int RandomId { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "IsMale")]
        public bool IsMale { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "StreetAdress")]
        public string StreetAdress { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "Telephone")]
        public string Telephone { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "MCH")]
        public int? MCH { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "HSJ")]
        public int? HSJ { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "CHS")]
        public int? CHS { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "IsConsent")]
        public bool? IsConsent { get; set; }
        //[DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Participant), Name = "Comment")]
        public string Comment { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "IsMotherSmoking")]
        public bool? IsMotherSmoking { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "MotherSmokingNumber")]
        public int? MotherSmokingNumber { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "MotherSmokingAge")]
        public int? MotherSmokingAge{ get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "IsFatherSmoking")]
        public bool? IsFatherSmoking { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "FatherSmokingNumber")]
        public int? FatherSmokingNumber { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "FatherSmokingAge")]
        public int? FatherSmokingAge { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "ParticipantSmokingAge")]
        public int? ParticipantSmokingAge { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "MedicalHistoryOther")]
        public string MedicalHistoryOther { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "Menarche")]
        public DateTime? Menarche { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "SurgeryDate")]
        public DateTime? SurgeryDate { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "EthnicPrecision")]
        public String EthnicPrecision { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "FamilyHistory")]
        public String FamilyHistory { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "Medication")]
        public String Medication { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "DomesticAnimals")]
        public String DomesticAnimals { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "SchoolYear")]
        public String SchoolYear { get; set; }


        public int City_Id { get; set; }        
        public int? Group_Id { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "Doctor_Id")]
        public int? Doctor_Id { get; set; }
        public int Creator_Id { get; set; }

       [Display(ResourceType = typeof(Resources.Participant), Name = "Family")]
        public int? Family_Id  { get; set; }

       public IList<Family> Families { get; set; }
        public int? FamilyRole_Id { get; set; }
        public int? EthnicGroup_Id { get; set; }
         [Display(ResourceType = typeof(Resources.Participant), Name = "SurgeryType")]
        public int? SurgeryType_Id { get; set; }

        public virtual City City { get; set; }
        public virtual Group Group { get; set; }
        public virtual User Doctor { get; set; }
        public virtual User Creator { get; set; }
        public virtual Family Family { get; set; }
        public virtual FamilyRole FamilyRole { get; set; }
        public virtual EthnicGroup EthnicGroup { get; set; }
        public virtual SurgeryType SurgeryType { get; set; }

        public virtual IList<Appointment> Appointments { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "Diagnosis")]
        public virtual IList<Diagnosis> Diagnoses { get; set; }
        [Display(ResourceType = typeof(Resources.Participant), Name = "MedicalHistory")]
        public virtual IList<MedicalHistory> MedicalHistories { get; set; }
        public virtual IList<Corset> Corsets { get; set; }
        public virtual IList<DrugHistory> DrugHistories { get; set; }

        [Display(ResourceType = typeof(Resources.Participant), Name = "FullName")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }


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
