using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using MathNet.Numerics.Statistics;
using System;
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

        public ActionResult AnalyzeData(int? diagnosis, int? gender, int? studyVariable, string submitButton)
        {
            RedirectToAction("ReportToExcel", new { var1 = "test" });

            bool isAISDiagnose = diagnosis == 1;
        
            bool isAgeField = false;
            bool isCobbAngleField = false;

            bool isExcelOperation = submitButton.ToLower() == "excel";

            var participantWithDiagnosisList = db.Diagnoses
                                                    .Single(e => e.Id == diagnosis)
                                                    .Participants;

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

            if (isAISDiagnose)
            {

                isAgeField = studyVariable == 1;
                isCobbAngleField = studyVariable == 2;

                if (isAgeField)
                {

                    var participantAgeList = participantWithDiagnosisList
                                                .Where(p => p.DOB.HasValue)
                                                .Select(p => (double)(DateTime.Now.Year - p.DOB.Value.Year));

                    var statistics = new DescriptiveStatistics(participantAgeList);

                    ViewBag.analyzeStatMin = statistics.Minimum;
                    ViewBag.analyzeStatMax = statistics.Maximum;
                    ViewBag.analyzeStatMedian = Statistics.Median(participantAgeList);
                    ViewBag.analyzeStatMean = statistics.Mean;
                    ViewBag.analyzeStatStandardDeviation = statistics.StandardDeviation;
                    ViewBag.analyzeStatVariance = statistics.Variance;
                    ViewBag.analyzeStatSampleSize = participantAgeList.Count();

                    ViewBag.analyzeStatComputed = true;

                    if (isExcelOperation) {

                        var CSVContent = participantWithDiagnosisList
                                                .Where(p => p.DOB.HasValue)
                                                .Select(p => new {
                                                    
                                                    Id = p.Id,
                                                    FirstName = p.FirstName,
                                                    LastName = p.LastName,
                                                    DOB = p.DOB.Value,
                                                    Age = (double)(DateTime.Now.Year - p.DOB.Value.Year)
                                                });

                        StringBuilder sb = new StringBuilder();

                        sb.Append("Id," );
                        sb.Append("FirstName,");
                        sb.Append("LastName,");
                        sb.Append("DOB,");
                        sb.Append("Age,");

                        sb.Append("\r\n");

                        foreach (var item in CSVContent) {
                            sb.Append(item.Id.ToString() + ',');
                            sb.Append(item.FirstName + ',');
                            sb.Append(item.LastName + ',');
                            sb.Append(item.DOB.ToString() + ',');
                            sb.Append(item.Age.ToString() + ',');

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
                }

                if (isCobbAngleField)
                {
                    var result = from p in participantWithDiagnosisList
                                 join a in db.Appointments
                                    on p.Id equals a.Participant_Id
                                 join c in db.Cobbs
                                    on a.Id equals c.Appointment_Id
                                 where c.Angle.HasValue
                                 select new
                                 {
                                     Id = p.Id,
                                     Angle = c.Angle,
                                     FirstName = p.FirstName,
                                     LastName = p.LastName,
                                     DOB = p.DOB.Value,
                                     Age = (double)(DateTime.Now.Year - p.DOB.Value.Year)
                                 };

                    result = result.ToList();

                    var groupResult = result
                                .GroupBy(e => e.Id)
                                .Select(e =>
                                    new {
                                        Id = e.Select(f => f.Id).First(),
                                        Angle = (double) e.Max(x => x.Angle)
                                    }
                                );

                    var participantCobbAngleList = groupResult.Select(e => e.Angle).ToList();

                    var statistics = new DescriptiveStatistics(participantCobbAngleList);

                    ViewBag.analyzeStatMin = statistics.Minimum;
                    ViewBag.analyzeStatMax = statistics.Maximum;
                    ViewBag.analyzeStatMedian = Statistics.Median(participantCobbAngleList);
                    ViewBag.analyzeStatMean = statistics.Mean;
                    ViewBag.analyzeStatStandardDeviation = statistics.StandardDeviation;
                    ViewBag.analyzeStatVariance = statistics.Variance;
                    ViewBag.analyzeStatSampleSize = participantCobbAngleList.Count();

                    ViewBag.analyzeStatComputed = true;

                    if (isExcelOperation)
                    {
                        var CSVContent = result;

                        StringBuilder sb = new StringBuilder();

                        sb.Append("Id,");
                        sb.Append("FirstName,");
                        sb.Append("LastName,");
                        sb.Append("DOB,");
                        sb.Append("Age,");
                        sb.Append("Cobb Angle,");

                        sb.Append("\r\n");

                        foreach (var item in CSVContent)
                        {
                            sb.Append(item.Id.ToString() + ',');
                            sb.Append(item.FirstName + ',');
                            sb.Append(item.LastName + ',');
                            sb.Append(item.DOB.ToString() + ',');
                            sb.Append(item.Age.ToString() + ',');
                            sb.Append(item.Angle.ToString() + ",");

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
                }

            }

            ViewBag.AverageCobbComputed = false;
            ViewBag.descriptivesStatsComputed = false;

            return View("Index");
        }

        public void ReportToExcel(string var1)
        {

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