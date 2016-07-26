using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Context;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    public class SamplingController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: /Sampling/
        public ActionResult Index()
        {
            var samplings = db.Samplings.Include(s => s.Appointment.Participant).Include(s => s.SamplingStatu).Include(s => s.SamplingType);
            return View(samplings.ToList());
        }

        // GET: /AppointmenetSampling/
        public ActionResult IndexSampling(int id)
        {
            ViewBag.ApppointmentId = id;
            var samplings = db.Samplings.Include(s => s.Appointment.Participant).Include(s => s.SamplingStatu).Include(s => s.SamplingType).Where(a => a.Appointment_Id == id);
            return PartialView("_IndexAppointment", samplings.ToList());

        }

        // GET: /Sampling/Details/5
        public ActionResult Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sampling sampling = db.Samplings.Find(id);
            if (sampling == null)
            {
                return HttpNotFound();
            }

            if (returnUrl != null)
                ViewBag.ReturnUrl = returnUrl;

            return View(sampling);
        }

        // GET: /Sampling/Create
        public ActionResult Create(int appointmentId)
        {
            ViewBag.Appointment_Id = appointmentId;
            ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id", "Name");
            ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id", "Name");
            return PartialView("_Create");
        }

        // POST: /Sampling/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Sampling sampling)
        {
            if (ModelState.IsValid)
            {
                db.Samplings.Add(sampling);
                db.SaveChanges(User.Identity.Name);
                ViewBag.ApppointmentId = sampling.Appointment_Id;
                var samplings = db.Samplings.Include(s => s.Appointment.Participant).Include(s => s.SamplingStatu).Include(s => s.SamplingType).Where(a => a.Appointment_Id == sampling.Appointment_Id);
                
                return Json(new { success = true, html = this.RenderPartialViewToString("_IndexAppointment", samplings) });
            }
 
            ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id", "Name", sampling.SamplingStatus_Id);
            ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id", "Name", sampling.SamplingType_Id);

            return View(sampling);
        }

        // GET: /Sampling/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sampling sampling = db.Samplings.Include(s => s.Appointment.Participant).Include(s => s.SamplingStatu).Include(s => s.SamplingType).FirstOrDefault(x => x.Id == id);
            if (sampling == null)
            {
                return HttpNotFound();
            }

                ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id",  Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", sampling.SamplingStatus_Id);
                ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id",  Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", sampling.SamplingType_Id);


            return View(sampling);
        }

        // POST: /Sampling/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "Id,BarCode,Iteration,Location,Comment,SamplingType_Id,SamplingStatus_Id,Appointment_Id")] Sampling sampling, SamplingType samplingType, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AddNewEntityIfNecessary(sampling, samplingType);

                db.Entry(sampling).State = EntityState.Modified;
                db.SaveChanges(User.Identity.Name);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index");
            }

                   if (Session["Language"].ToString().ToLower() == "en")
                    {
                        ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id", "Name", sampling.SamplingStatus_Id);
                        ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id", "Name", sampling.SamplingType_Id);
                    }
                   else
                   {
                       ViewBag.SamplingStatus_Id = new SelectList(db.SamplingStatus, "Id", "NameFr", sampling.SamplingStatus_Id);
                       ViewBag.SamplingType_Id = new SelectList(db.SamplingTypes, "Id", "NameFr", sampling.SamplingType_Id); 
                   }
            return View(sampling);
        }

        // GET: /Sampling/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sampling sampling = db.Samplings.Find(id);
            if (sampling == null)
            {
                return HttpNotFound();
            }
            return View(sampling);
        }

        // POST: /Sampling/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sampling sampling = db.Samplings.Find(id);
            db.Samplings.Remove(sampling);
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

        private void AddNewEntityIfNecessary(Sampling sampling, SamplingType samplingType)
        {
            bool isSamplingTypeNeedToBeAdd = sampling.SamplingType_Id < 1;

            if (isSamplingTypeNeedToBeAdd)
            {
                SamplingType newSamplingType = new SamplingType() {
                    Name = samplingType.Name,
                    NameFr = samplingType.Name
                };

                db.SamplingTypes.Add(newSamplingType);
                db.SaveChanges();

                sampling.SamplingType_Id = newSamplingType.Id;
            }
        }

        #endregion
    }
}
