using Clinic.BackEnd.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class AnalytiqueController : Controller
    {
        private readonly ClinicContext db = new ClinicContext();

        // GET: Analytique
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            var participants = db.Participants

                                    .Select(e => new {
                                        City = e.City.Name,
                                        Sex = e.IsMale ? "M" : "F",
                                        Number = 1
                                    })
                                    .Take(200)
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
                                CobbType = e.CobbType.Name.Trim()

                            })
                            .Take(200)
                            .ToList();

            return Json(cobbs, JsonRequestBehavior.AllowGet);
        }

    }
}