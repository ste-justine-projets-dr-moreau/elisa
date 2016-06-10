using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class AnalytiqueController : Controller
    {
        private readonly ClinicContext db = new ClinicContext();

        public ActionResult Index()
        {
            ViewBag.AverageCobbComputed = false;

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