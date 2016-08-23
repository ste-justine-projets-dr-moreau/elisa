using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using MathNet.Numerics.Statistics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class AnalytiqueController : Controller
    {
        private readonly ClinicContext db = new ClinicContext();

        public ActionResult Index()
        {
            FillDropDownList();

            
            ViewBag.AverageCobbComputed = false;
            ViewBag.descriptivesStatsComputed = false;

            return View();
        }

        [HttpPost]
        public ActionResult ComputeAverageCobb(bool sexFilter, String sex, bool ageFilter, int ageBegin, int ageEnd)
        {

            List<Participant> participants = db.Participants.ToList();

            if (sexFilter)
            {
                bool isMale = sex == "1" ? true : false;
                participants = participants
                                    .Where(p => p.IsMale == isMale)
                                    .ToList();
            }

            if (ageFilter)
            {
                participants = participants
                                    .Where(p =>
                                        p.DOB.HasValue ?
                                            ((DateTime.Now.Year - p.DOB.Value.Year) >= ageBegin &&
                                            (DateTime.Now.Year - p.DOB.Value.Year) <= ageEnd)
                                            : false
                                    ).ToList();
            }

            if (true)
            {
                var resultToDisplay = from p in participants
                             join a in db.Appointments
                                on p.Id equals a.Participant_Id
                             join c in db.Cobbs
                                on a.Id equals c.Appointment_Id
                             where c.Angle.HasValue
                             select new
                             {
                                 Id = p.Id,
                                 Appointment_Id = a.Id,
                                 Cobb_Id = c.Id,
                                 Angle = c.Angle
                             };

                var result = from p in participants
                             join a in db.Appointments
                                on p.Id equals a.Participant_Id
                             join c in db.Cobbs
                                on a.Id equals c.Appointment_Id
                             where c.Angle.HasValue
                             select new {
                                 Id = p.Id,
                                 Angle = c.Angle
                             };

                var groupResult = result
                            .GroupBy(e => e.Id)
                            .Select(e =>
                                new {
                                    Id = e.Select(f => f.Id).First(),
                                    Angle = e.Max(x => x.Angle)
                                }
                            );

                var average = groupResult.Average(e => e.Angle);

                
                ViewBag.AverageCobbComputed = true;
                ViewBag.AverageCobb = average;
                ViewBag.ParticipantCobbs = resultToDisplay.ToList();

            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult ComputeDescriptivesStats()
        {
            var particpantAges = db.Participants
                        .Where(p => p.DOB.HasValue)
                        .Select(p => (double)(DateTime.Now.Year - p.DOB.Value.Year));

            var statistics = new DescriptiveStatistics(particpantAges);

            ViewBag.statMin = statistics.Minimum;
            ViewBag.statMax = statistics.Maximum;
            ViewBag.statMedian = Statistics.Median(particpantAges);
            ViewBag.statMean = statistics.Mean;
            ViewBag.statStandardDeviation = statistics.StandardDeviation;
            ViewBag.statVariance = statistics.Variance;

            ViewBag.AverageCobbComputed = false;
            ViewBag.descriptivesStatsComputed = true;

            return View("Index");
        }

        public ActionResult AnalyzeData(int? diagnosis, int? gender, int? functionalGroup, int? filterBegin, int? filterEnd, int? studyVariable, string submitButton)
        {
            FillDropDownList();


            bool isAgeField = studyVariable == 1;
            bool isCobbAngleField = studyVariable == 2;
            bool isOPNField = studyVariable == 3;

            bool isExcelOperation = submitButton.ToLower() == "excel";

            bool isFilterMustBeApply = filterBegin.HasValue && filterEnd.HasValue;

            ICollection<Participant> participantWithDiagnosisList;

            if (diagnosis.HasValue)
            {
                participantWithDiagnosisList = db.Diagnoses
                                                    .Single(e => e.Id == diagnosis)
                                                    .Participants;
            }
            else
            {
                participantWithDiagnosisList = db.Participants.ToList();
            }

            if (gender.HasValue) {
                if (gender.Value == 1) // Homme
                {
                    participantWithDiagnosisList = participantWithDiagnosisList
                                                        .Where(e => e.IsMale).ToList();
                }
                else if (gender.Value == 2) // Femme
                {
                    participantWithDiagnosisList = participantWithDiagnosisList
                                    .Where(e => !e.IsMale).ToList();
                }
            }

            if (functionalGroup.HasValue) {
                participantWithDiagnosisList = participantWithDiagnosisList
                                                .Where(e => e.FunctionalGroup == functionalGroup).ToList();
            }


            #region Report Linq Query

            var participantDiagnosisLists = db.Participants
                .Select(e => e.Diagnoses.Select(f => new {
                    Participant_Id = e.Id,
                    Diagnosis_Id = f.Id,
                    DiagnosisName = f.Name
                })).ToList();

            var participantDiagnosis = participantDiagnosisLists.SelectMany(x => x).ToList();


            var queryResult =

                        from participant in participantWithDiagnosisList

                        join pd in participantDiagnosis
                            on participant.Id equals pd.Participant_Id

                        join a in db.Appointments
                            on participant.Id equals a.Participant_Id

                        join c in db.Cobbs
                                on a.Id equals c.Appointment_Id into cobbGroup
                        from cobb in cobbGroup.DefaultIfEmpty(new Cobb())

                        join ct in db.CobbTypes
                                 on cobb.CobbType_Id equals ct.Id into cobbTypeGroup
                        from cobbType in cobbTypeGroup.DefaultIfEmpty(new CobbType())

                        join b in db.Corsets
                                 on participant.Id equals b.Participant_Id into braceGroup
                        from brace in braceGroup.DefaultIfEmpty(new Corset())

                        join bt in db.CorsetTypes
                                 on brace.CorsetType_Id equals bt.Id into braceTypeGroup
                        from braceType in braceTypeGroup.DefaultIfEmpty(new CorsetType())

                        join s in db.Samplings
                                 on a.Id equals s.Appointment_Id into samplingGroup
                        from sampling in samplingGroup.DefaultIfEmpty(new Sampling())

                        join r in db.Results
                                 on sampling.Id equals r.Sampling_Id into resultGroup
                        from aResult in resultGroup.DefaultIfEmpty(new Result())

                        join lt in db.LabTests
                                 on aResult.LabTest_Id equals lt.Id into labTestGroup
                        from labTest in labTestGroup.DefaultIfEmpty(new LabTest())

                        select new
                        {

                            Family = participant.Family_Id,
                            Random = participant.Id,
                            Gender = participant.IsMale ? "M" : "F",
                            DOB = participant.DOB.HasValue ? participant.DOB.Value.Date.ToString("dd/MM/yyyy") : "",
                            Appointment_Id = a.Id,
                            Collection_Date = a.Date.HasValue ? a.Date.Value.Date.ToString("dd/MM/yyyy") : "",
                            Collection_Hour = a.Hour != null ? a.Hour : "",
                            Age = participant.DOB.HasValue ? ((double)(DateTime.Now.Year - participant.DOB.Value.Year)).ToString() : "",
                            FunctionalGroup = participant.FunctionalGroup.HasValue ? participant.FunctionalGroup.Value.ToString() : "",
                            Diagnosis = pd == null ? "" : pd.DiagnosisName,


                            Cobb_Id = cobb.Id <= 0 ? "" : cobb.Id.ToString(),
                            Cobb = cobb.Id <= 0 ? "" : cobb.Angle.ToString(),
                            CobbDirection = cobb.IsRight.HasValue ?
                                                         (cobb.IsRight.Value ? "Right" : "Left")
                                                     :
                                                         "",
                            CobbType_Id = cobbType.Id <= 0 ? "" : cobbType.Id.ToString(),
                            CobbType = cobbType.Id <= 0 ? "" : cobbType.Name,


                            Corset_Id = brace.Id <= 0 ? "" : brace.Id.ToString(),
                            CorsetStart = (brace.Id <= 0
                                                     ? "" : brace.Start.HasValue ? brace.Start.Value.Date.ToString("dd/MM/yyyy") : ""),
                            CorsetEnd = (brace.Id <= 0
                                                     ? "" : brace.End.HasValue ? brace.End.Value.Date.ToString("dd/MM/yyyy") : ""),
                            BraceType_Id = braceType.Id <= 0 ? "" : braceType.Id.ToString(),


                            Sampling_Id = sampling.Id <= 0 ? "" : sampling.Id.ToString(),
                            LabTest_Id = labTest.Id,
                            LabTest = labTest.Id <= 0 ? "" : labTest.Name,
                            Result_Id = aResult.Id <= 0 ? "" : aResult.Id.ToString(),
                            LabResult = aResult.Labresult

                        };

            queryResult = queryResult.ToList();
            #endregion


            DescriptiveStatistics statistics = null;
            List<double> inputToStatistics = new List<double>();

            if (isAgeField)
            {
                if (isFilterMustBeApply)
                {
                    participantWithDiagnosisList = participantWithDiagnosisList
                                                        .Where(e => e.DOB.HasValue)
                                                        .Where(e =>
                                                            ((double)(DateTime.Now.Year - e.DOB.Value.Year)) >= filterBegin &&
                                                            ((double)(DateTime.Now.Year - e.DOB.Value.Year)) <= filterEnd
                                                        ).ToList();
                }

                inputToStatistics = participantWithDiagnosisList
                                            .Where(p => p.DOB.HasValue)
                                            .Select(p => (double)(DateTime.Now.Year - p.DOB.Value.Year))
                                            .ToList();
            }

            if (isCobbAngleField)
            {
                queryResult = queryResult
                                .Where(e => e.Cobb != String.Empty && Convert.ToDouble(e.Cobb) > 0);

                if (isFilterMustBeApply)
                {
                    queryResult = queryResult.Where(e =>
                        Convert.ToInt32(e.Cobb) >= filterBegin &&
                        Convert.ToInt32(e.Cobb) <= filterEnd
                    );
                }

                var groupResult = queryResult
                                    .GroupBy(e => e.Random)
                                    .Select(e =>
                                        new {
                                            Id = e.Select(f => f.Random).First(),
                                            Cobb = e.Max(x => Convert.ToDouble(x.Cobb))
                                        }
                                    );

                inputToStatistics = groupResult.Select(e => e.Cobb).ToList();
            }

            if (isOPNField)
            {
                queryResult = queryResult
                    .Where(e => e.LabTest_Id == 25)     // (OPN) (ng/ml) === 25
                    .Where(e => e.LabResult.HasValue && e.LabResult.Value > 0);

                if (isFilterMustBeApply)
                {
                    queryResult = queryResult.Where(e =>
                        Convert.ToInt32(e.LabResult) >= filterBegin &&
                        Convert.ToInt32(e.LabResult) <= filterEnd
                    );
                }

                var groupResult = queryResult
                    .GroupBy(e => e.Random)
                    .Select(e =>
                        new {
                            Id = e.Select(f => f.Random).First(),
                            LabResult = e.Max(x => Convert.ToDouble(x.LabResult))
                        }
                    );

                inputToStatistics = groupResult.Select(e => e.LabResult).ToList();
            }

            statistics = new DescriptiveStatistics(inputToStatistics);

            ViewBag.analyzeStatMin = statistics.Minimum;
            ViewBag.analyzeStatMax = statistics.Maximum;
            ViewBag.analyzeStatMedian = Statistics.Median(inputToStatistics);
            ViewBag.analyzeStatMean = statistics.Mean;
            ViewBag.analyzeStatStandardDeviation = statistics.StandardDeviation;
            ViewBag.analyzeStatVariance = statistics.Variance;
            ViewBag.analyzeStatSampleSize = inputToStatistics.Count();

            ViewBag.analyzeStatComputed = true;

            #region Excel part

            if (isExcelOperation)
            {
                var CSVContent = queryResult;

                StringBuilder sb = new StringBuilder();

                sb.Append("Family,");
                sb.Append("Random,");
                sb.Append("Gender,");
                sb.Append("DOB,");

                sb.Append("Appointment_Id,");
                sb.Append("Collection Date,");
                sb.Append("Collection Hour,");
                sb.Append("Age,");
                sb.Append("Functional Group,");
                sb.Append("Diagnosis,");

                sb.Append("Cobb_Id,");
                sb.Append("Cobb Angle,");
                sb.Append("Cobb Direction,");
                sb.Append("Cobb Type,");

                sb.Append("Brace Id,");
                sb.Append("Brace Start,");
                sb.Append("Brace End,");

                sb.Append("Sampling_Id,");
                sb.Append("LabTest,");
                sb.Append("Result_Id,");
                sb.Append("Lab Result,");

                sb.Append("\r\n");

                foreach (var item in CSVContent)
                {
                    sb.Append(item.Family.ToString() + ',');
                    sb.Append(item.Random.ToString() + ',');
                    sb.Append(item.Gender.ToString() + ',');
                    sb.Append(item.DOB.ToString() + ',');

                    sb.Append(item.Appointment_Id.ToString() + ',');
                    sb.Append(item.Collection_Date.ToString() + ',');
                    sb.Append(item.Collection_Hour.ToString() + ",");
                    sb.Append(item.Age.ToString() + ',');
                    sb.Append(item.FunctionalGroup.ToString() + ',');
                    sb.Append(item.Diagnosis.ToString() + ',');

                    sb.Append(item.Cobb_Id.ToString() + ',');
                    sb.Append(item.Cobb.ToString() + ',');
                    sb.Append(item.CobbDirection.ToString() + ',');
                    sb.Append(item.CobbType.ToString().Trim() + ',');

                    sb.Append(item.Corset_Id.ToString() + ',');
                    sb.Append(item.CorsetStart.ToString() + ',');
                    sb.Append(item.CorsetEnd.ToString() + ',');

                    sb.Append(item.Sampling_Id.ToString() + ',');
                    sb.Append(item.LabTest.ToString() + ',');
                    sb.Append(item.Result_Id.ToString() + ',');
                    sb.Append(item.LabResult.ToString() + ',');

                    sb.Append("\r\n");
                }

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=SqlExport.csv");
                Response.Charset = "";
                Response.ContentType = "application/text";
                Response.Output.Write(sb.ToString());
                Response.Flush();
                Response.End();
            }

            #endregion


            ViewBag.AverageCobbComputed = false;
            ViewBag.descriptivesStatsComputed = false;

            return View("Index");
        }

        private void FillDropDownList()
        {
            List<SelectListItem> diagnosisList = new List<SelectListItem>();
            List<SelectListItem> functionalGroupList = new List<SelectListItem>();
            List<SelectListItem> genderList = new List<SelectListItem>();
            List<SelectListItem> studyVariableList = new List<SelectListItem>();


            // Diagnoses
            var diagnoses = db.Diagnoses.Select(e => new {
                Id = e.Id,
                Name = e.Name
            });

            foreach (var diagnosis in diagnoses)
            {
                diagnosisList.Add(new SelectListItem()
                {
                    Text = diagnosis.Name,
                    Value = diagnosis.Id.ToString()
                });
            }

            // Functional Group
            var functionalGroups = db.Participants
                                        .Where(e => e.FunctionalGroup.HasValue && e.FunctionalGroup != 0)
                                        .OrderBy(e => e.FunctionalGroup)
                                        .Select(e => new
                                        {
                                            Id = e.FunctionalGroup,
                                            Name = e.FunctionalGroup
                                        })
                                        .Distinct()
                                        .OrderBy(e => e.Id);

            foreach (var functionalGroup in functionalGroups)
            {
                functionalGroupList.Add(new SelectListItem() {
                    Text = functionalGroup.Name.ToString(),
                    Value = functionalGroup.Id.ToString()
                });
            }

            // Gender
            genderList.Add(new SelectListItem() { Text = "Homme", Value = "1" });
            genderList.Add(new SelectListItem() { Text = "Femme", Value = "2" });

            // Study Variable
            studyVariableList.Add(new SelectListItem() { Text = "Age", Value = "1" });
            studyVariableList.Add(new SelectListItem() { Text = "Angle de Cobb", Value = "2" });
            studyVariableList.Add(new SelectListItem() { Text = "OPN", Value = "3" });


            ViewBag.diagnosisList = diagnosisList;
            ViewBag.functionalGroupList = functionalGroupList;
            ViewBag.genderList = genderList;
            ViewBag.studyVariableList = studyVariableList;
        }

        #region Ajax call (JSON)

        public ActionResult GetData()
        {
            var participants = db.Participants

                                    .Select(e => new {
                                        City = e.City.Name,
                                        Sex = e.IsMale ? "M" : "F",
                                        Number = 1
                                    })
                                    .ToList();

            return Json(participants, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetData2()
        {
            var cobbs = db.Cobbs
                            .Where(e => e.IsRight.HasValue && e.CobbType != null)
                            .Select(e => new {
                                Angle = e.Angle,
                                IsRight = e.IsRight,
                                CobbType = e.CobbType.Name.Trim(),
                                Number = 1
                            })
                            .Take(200)
                            .ToList();

            return Json(cobbs, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}