using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Clinic.BackEnd.Context;
using WebApplication.Helpers;
using Appointment = Clinic.BackEnd.Models.Appointment;
using Cobb = Clinic.BackEnd.Models.Cobb;
using Sampling = Clinic.BackEnd.Models.Sampling;

namespace WebApplication.Controllers
{
    public class AppointmentController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: /Appointment/
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Participant).Include(a => a.User);
            return View(appointments.ToList());
        }

        // GET: /Appointment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var appointment =
                db.Appointments.Include(a => a.Participant)
                    .Include(a => a.User)
                    .Include(x => x.Samplings)
                    .First(x => x.Id == id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: /Appointment/Create
        public ActionResult Create(int? participant_Id)
        {

            var currentUser = db.Users.First(x => x.NT == User.Identity.Name);

            var appointment = new Appointment
            {
                Participant_Id = participant_Id.Value,
                User_Id = currentUser.Id,
                Date = DateTime.Now
            };

            return View(appointment);
        }

        // POST: /Appointment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment, int? participant_Id)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                db.SaveChanges(User.Identity.Name);

                return Redirect(Url.Content("~/Appointment/Edit/" + appointment.Id));
            }


            return View(appointment);
        }

        // GET: /Appointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var appointment = db.Appointments
                .Include(x => x.Samplings.Select(j => j.Results))
                .Include(y => y.Cobbs)
                .Include(z => z.Participant)
                .First(x => x.Id == id);

            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", appointment.Participant_Id);
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName", appointment.User_Id);
            ViewBag.CobbTypes = db.CobbTypes;
            ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id",
                Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id",
                Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

            if (Session["Language"].ToString().ToLower() == "en")
            {


                var cobbConditions = db.CobbConditions.ToList().Select(cobbcondition => new SelectListItem
                {
                    Selected = appointment.CobbConditions.Contains(cobbcondition),
                    Text = cobbcondition.Name,
                    Value = cobbcondition.Id.ToString()
                }).ToList();

                ViewBag.CobbCondition = cobbConditions;
            }
            else
            {

                var cobbConditions = db.CobbConditions.ToList().Select(cobbcondition => new SelectListItem
                {
                    Selected = appointment.CobbConditions.Contains(cobbcondition),
                    Text = cobbcondition.NameFr,
                    Value = cobbcondition.Id.ToString()
                }).ToList();

                ViewBag.CobbCondition = cobbConditions;

            }

            return View(appointment);
        }


        public ActionResult GetNewSampling(int appointmentId)
        {
            ViewBag.SamplingTypes = db.SamplingTypes.ToList().Select(st =>
                new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = Session["Language"].ToString().ToLower() == "en" ? st.Name : st.NameFr,
                });

            ViewBag.SamplingStatuses = db.SamplingStatus.ToList().Select(ss => 
                new SelectListItem
                {
                    Value = ss.Id.ToString(),
                    Text = Session["Language"].ToString().ToLower() == "en" ? ss.Name : ss.NameFr,
                });

            return PartialView("_CreateOrEditSampling", new Sampling{ Appointment_Id = appointmentId});

        }
        public ActionResult GetSampling(int samplingId)
        {
            var sampling = db.Samplings.Include(s => s.SamplingType).Include(s => s.SamplingStatu).Include(s => s.Results).First(s => s.Id == samplingId);

            ViewBag.SamplingTypes = db.SamplingTypes.ToList().Select(st =>
                new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = Session["Language"].ToString().ToLower() == "en" ? st.Name : st.NameFr,
                    Selected = st.Id == sampling.SamplingType_Id
                });

            ViewBag.SamplingStatuses = db.SamplingStatus.ToList().Select(ss => new SelectListItem
            {
                Value = ss.Id.ToString(),
                Text = Session["Language"].ToString().ToLower() == "en" ? ss.Name : ss.NameFr,
                Selected = ss.Id == sampling.SamplingStatus_Id
            });

            return PartialView("_CreateOrEditSampling", sampling);
        }

        public ActionResult GetSamplingForDeletion(int samplingId)
        {
            var sampling = db.Samplings.Include(s => s.SamplingType).Include(s => s.SamplingStatu).First(s => s.Id == samplingId);

            return PartialView("_ConfirmDeleteSampling", sampling);
        }

        [HttpPost]
        public ActionResult DeleteSampling(int samplingId)
        {
            var sampling = db.Samplings.Find(samplingId);
            db.Samplings.Remove(sampling);
            db.SaveChanges(User.Identity.Name);

            ViewBag.ApppointmentId = sampling.Appointment_Id;

            var appointment =
                db.Appointments.Include(a => a.Samplings.Select(s => s.SamplingStatu))
                    .Include(a => a.Samplings.Select(s => s.SamplingType))
                    .First(a => a.Id == sampling.Appointment_Id);

            ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id",
                Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id",
                Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

            return Json(new { success = true, html = this.RenderPartialViewToString("_ListSamplings", appointment) });
        }


        [HttpPost]
        public ActionResult CreateOrEditSampling(Sampling sampling)
        {
            if (ModelState.IsValid)
            {
                if (sampling.Id == 0)
                {
                    db.Samplings.Add(sampling);
                }
                else
                {
                    var samplingFromDb = db.Samplings.First(s => s.Id == sampling.Id);
                    samplingFromDb.SamplingStatus_Id = sampling.SamplingStatus_Id;
                    samplingFromDb.SamplingType_Id = sampling.SamplingType_Id;

                    samplingFromDb.BarCode = sampling.BarCode;
                    samplingFromDb.Iteration = sampling.Iteration;
                    samplingFromDb.Location = sampling.Location;
                    samplingFromDb.Comment = sampling.Comment;
                    samplingFromDb.Results = sampling.Results;

                }
                db.SaveChanges(User.Identity.Name);
                ViewBag.ApppointmentId = sampling.Appointment_Id;

                var appointment =
                    db.Appointments.Include(a => a.Samplings.Select(s => s.SamplingStatu))
                        .Include(a => a.Samplings.Select(s => s.SamplingType))
                        .First(a => a.Id == sampling.Appointment_Id);

                ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id",
                    Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
                ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id",
                    Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

                return Json(new { success = true, html = this.RenderPartialViewToString("_ListSamplings", appointment) });
            }

            ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id", "Name", sampling.SamplingStatus_Id);
            ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id", "Name", sampling.SamplingType_Id);

            return Json(new { success = false, message = "form is invalid" });
        }

        // POST: /Appointment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(Appointment appointment, int[] CobbConditionsForCobb)
        public ActionResult Edit(Appointment appointment, int[] cobbConditionsForCobb)
        {
            if (ModelState.IsValid)
            {
                // db.Entry(appointment).State = EntityState.Modified;
                //await db.SaveChangesAsync();

                var appointmentFromDb = db.Appointments.Include(a => a.Cobbs).Include(a => a.CobbConditions).First(x => x.Id == appointment.Id);

                appointmentFromDb.Date = appointment.Date;
                appointmentFromDb.Height = appointment.Height;
                appointmentFromDb.Weight = appointment.Weight;
                appointmentFromDb.Comment = appointment.Comment;

                appointmentFromDb.Participant_Id = appointment.Participant_Id;
                appointmentFromDb.User_Id = appointment.User_Id;

                if (appointment.Weight.HasValue && appointment.Height.HasValue)
                {
                    if (appointment.Weight.Value > 0 && appointment.Height > 0)
                    {
                        decimal? BMIValue = (appointment.Weight /
                            ((appointment.Height / 100) * (appointment.Height / 100)));

                        appointmentFromDb.TheBMI = BMIValue;
                    }
                }


                if (appointment.Cobbs != null)
                {
                    if (appointment.Cobbs.Count > 0)
                    {
                        foreach (var cobb in appointment.Cobbs)
                        {
                            if (appointmentFromDb.Cobbs.Any(x => x.Id == cobb.Id) || cobb.Id != 0)
                            {
                                appointmentFromDb.Cobbs.First(x => x.Id == cobb.Id).CobbType_Id = cobb.CobbType_Id;
                                appointmentFromDb.Cobbs.First(x => x.Id == cobb.Id).Angle = cobb.Angle;
                                appointmentFromDb.Cobbs.First(x => x.Id == cobb.Id).IsRight = cobb.IsRight;
                            }
                            else
                            {
                                appointmentFromDb.Cobbs.Add(cobb);
                            }
                        }


                        var cobbIds = appointment.Cobbs.Select(x => x.Id).ToList();
                        foreach (
                            var cobbFromDbDeleted in appointmentFromDb.Cobbs.Where(x => !cobbIds.Contains(x.Id)).ToList())
                        {
                            appointmentFromDb.Cobbs.Remove(cobbFromDbDeleted);
                            db.Entry(cobbFromDbDeleted).State = EntityState.Deleted;
                        }
                    }
                }
                if (appointment.Cobbs == null)
                {
                    var appointmentCobbs = db.Appointments.Include(a => a.Cobbs).SingleOrDefault(a => a.Id == appointment.Id);

                    if (appointmentCobbs != null)
                    {
                        foreach (var deleteCobb in appointmentCobbs.Cobbs.ToList())
                        {
                            db.Cobbs.Remove(deleteCobb);
                        }
                    }
                }


                if (appointmentFromDb.CobbConditions != null)
                {
                    foreach (var cobbCondition in appointmentFromDb.CobbConditions.ToList())
                    {
                        appointmentFromDb.CobbConditions.Remove(cobbCondition);
                    }
                }

                if (cobbConditionsForCobb != null)
                {
                    var cobbConditionsToAdd = db.CobbConditions.Where(x => cobbConditionsForCobb.Contains(x.Id)).ToList();
                    foreach (var cobbConditionToAdd in cobbConditionsToAdd)
                    {
                        cobbConditionToAdd.Appointments.Add(appointmentFromDb);
                        appointmentFromDb.CobbConditions.Add(cobbConditionToAdd);
                    }
                }



                //await db.SaveChangesAsync();
                db.SaveChanges(User.Identity.Name);
                return Redirect(Url.Content("~/Participant/Edit/" + appointment.Participant_Id + "#tabs-2"));

                //return RedirectToAction("Index");
            }
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", appointment.Participant_Id);
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName", appointment.User_Id);
            ViewBag.SamplingTypes = db.SamplingTypes;

            return View(appointment);
        }


        public ActionResult CobbEntryRow(int appointmentId)
        {
            ViewBag.CobbTypes = db.CobbTypes;
            var cobb = new Cobb()
            {
                Appointment_Id = appointmentId
            };

            return PartialView("_CobbEntryEditor", cobb);
        }



        // GET: /Appointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: /Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
            db.SaveChanges(User.Identity.Name);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Create Iteration

        [HttpPost]
        public JsonResult CreateIteration(int id)
        {
            string result = "Couldn't able to create.";
            if (id > 0)
            {
                var sampling = db.Samplings.Where(i => i.Id == id).FirstOrDefault();
                if (sampling != null)
                {
                    Sampling samplingIteration = DuplicateSamplingRecord(sampling);
                    samplingIteration.Iteration = sampling.Iteration + 1;
                    db.Samplings.Add(samplingIteration);
                    db.SaveChanges(User.Identity.Name);
                    result = "Sampling iteration added successfully";
                }
                else
                {
                    result = "Sampling doesn't exists.";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Generate Sampling

        [HttpPost]
        public JsonResult GenerateSampling(int id, int barCode)
        {
            string result = "Couldn't able to create.";
            if (id > 0)
            {
                var samplingEntity = db.Samplings.Where(i => i.Id == id).FirstOrDefault();
                if (samplingEntity != null)
                {
                    // 1-Plasma / Starting Bar Code
                    Sampling sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "1-Plasma " + barCode;
                    sampling.SamplingType_Id = 4;
                    db.Samplings.Add(sampling);

                    // 2-Plasma / Starting Bar Code + 1
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "2-Plasma " + (barCode + 1);
                    sampling.SamplingType_Id = 4;
                    db.Samplings.Add(sampling);

                    // 3-Red Cell / Starting Bar Code + 2
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "3-Red Cell " + (barCode + 2);
                    sampling.SamplingType_Id = 1;
                    db.Samplings.Add(sampling);

                    // 4-Red Cell / Starting Bar Code + 3
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "4-Red Cell " + (barCode + 3);
                    sampling.SamplingType_Id = 1;
                    db.Samplings.Add(sampling);

                    // 5-Red Cell / Starting Bar Code + 4 
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "5-Red Cell " + (barCode + 4);
                    sampling.SamplingType_Id = 1;
                    db.Samplings.Add(sampling);

                    // 6-White Cell / Starting Bar Code + 5
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "6-White Cell " + (barCode + 5);
                    sampling.SamplingType_Id = 1;
                    db.Samplings.Add(sampling);

                    // 7-White Cell / Starting Bar Code + 6
                    sampling = DuplicateSamplingRecord(samplingEntity);
                    sampling.BarCode = "7-White Cell " + (barCode + 6);
                    sampling.SamplingType_Id = 1;
                    db.Samplings.Add(sampling);

                    samplingEntity.SamplingStatus_Id = 2; // The System update the Original Sampling Status to "Consume"

                    db.SaveChanges(User.Identity.Name);
                    result = "Sampling iteration added successfully";
                }
                else
                {
                    result = "Sampling doesn't exists.";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Helper Methods

        public Sampling DuplicateSamplingRecord(Sampling entity)
        {
            Sampling sampling = new Sampling();
            sampling.BarCode = entity.BarCode;
            sampling.Iteration = entity.Iteration;
            sampling.Location = entity.Location;
            sampling.Comment = entity.Comment;
            sampling.SamplingType_Id = entity.SamplingType_Id;
            sampling.SamplingStatus_Id = entity.SamplingStatus_Id;
            sampling.Appointment_Id = entity.Appointment_Id;
            return sampling;
        }

        #endregion
    }
}