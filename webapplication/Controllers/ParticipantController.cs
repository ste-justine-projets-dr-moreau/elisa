using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using WebApplication.Helpers;


namespace WebApplication.Controllers
{
    //soit  docteur soit researcher --> authorized
    //[Authorize(Roles = "Doctor,Researcher")]
    public class ParticipantController : Controller
    {
        private readonly ClinicContext db = new ClinicContext();

        #region Participant

        // GET: /Participant/
        public ActionResult Index()
        {
            var model = db.Participants.ToList().OrderBy(x => x.Id);
            var participantsWithTrauma = db.ParticipantIdTraumas.ToList();

            foreach (var item in model) {
                ParticipantIdTrauma particpantIdTrauma = participantsWithTrauma.SingleOrDefault(e => e.IdParticipantPourTrauma == item.Id);

                if (particpantIdTrauma != null)
                {
                    item.IdToDisplay = particpantIdTrauma.TraumaId;
                }
                else
                {
                    item.IdToDisplay = item.Id.ToString();
                }
                
            }

            return View(model);
        }

        //dois etre administrator pour entré dans "create"
         //[Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.City_Id = new SelectList(db.Cities.OrderBy(x => x.Name), "Id", "Name");
            ViewBag.Family_Id = new SelectList(db.Families.OrderBy(x => x.Name), "Id", "Name");

            ViewBag.Doctor_Id = new SelectList(db.Users.Include(x => x.Roles).Where(x => x.Roles.Any(j => j.Name == "Doctor")), "Id", "FullName");
            ViewBag.Group_Id = new SelectList(db.Groups.OrderBy(x => x.Name), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            ViewBag.FamilyRole_Id = new SelectList(db.FamilRoles.OrderBy(x => x.Name), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            ViewBag.EthnicGroup_Id = new SelectList(db.EthnicGroups.OrderBy(x => x.Name), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            ViewBag.SurgeryType_Id = new SelectList(db.SurgeryTypes.OrderBy(x => x.Name), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

            var diagnoses = db.Diagnoses.ToList().Select(diagnosis => new SelectListItem
            {
                Selected = false,
                Text = Session["Language"].ToString().ToLower() == "en" ? diagnosis.Name : diagnosis.NameFr,
                Value = diagnosis.Id.ToString()
            }).ToList();

            ViewBag.Diagnoses = diagnoses;

            var medicalhistories = db.MedicalHistories.ToList().Select(medicalhistory => new SelectListItem
            {
                Selected = false,
                Text = Session["Language"].ToString().ToLower() == "en" ? medicalhistory.Name : medicalhistory.NameFr,
                Value = medicalhistory.Id.ToString()
            }).ToList();

            ViewBag.MedicalHistories = medicalhistories;


            return View();
        }

        // POST: /Participant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Participant participant, int[] diagnosesForParticipant, int[] medicalhistoriesForParticipant)
        {
            if (ModelState.IsValid)
            {
                AddNewEntityIfNecessary(participant);

                if (diagnosesForParticipant != null)
                {
                    var selectedDiagnoses = db.Diagnoses.Where(x => diagnosesForParticipant.Contains(x.Id)).ToList();
                    foreach (var selectedDiagnosis in selectedDiagnoses)
                    {
                        selectedDiagnosis.Participants.Add(participant);
                    }
                    participant.Diagnoses = selectedDiagnoses;
                }

                if (medicalhistoriesForParticipant != null)
                {
                    var selectedMedicalhistories = db.MedicalHistories.Where(x => medicalhistoriesForParticipant.Contains(x.Id)).ToList();
                    foreach (var selectedMedicalhistory in selectedMedicalhistories)
                    {
                        selectedMedicalhistory.Participants.Add(participant);
                    }
                    participant.MedicalHistories = selectedMedicalhistories;
                }

                var currentUser = db.Users.First(x => x.NT == User.Identity.Name);
                participant.Creator_Id = currentUser.Id;

                db.Participants.Add(participant);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException ex)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var failure in ex.EntityValidationErrors)
                    {
                        sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                        foreach (var error in failure.ValidationErrors)
                        {
                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                            sb.AppendLine();
                        }
                    }

                    throw new DbEntityValidationException(sb.ToString());
                }


                return RedirectToAction("Index");
            }

            SetupViewBags(participant);

            var diagnoses = db.Diagnoses.ToList().Select(diagnosis => new SelectListItem
            {
                Selected = participant.Diagnoses != null && participant.Diagnoses.Contains(diagnosis),
                Text = diagnosis.Name,
                Value = diagnosis.Id.ToString()
            }).ToList();

            ViewBag.Diagnoses = diagnoses;

            var medicalhistories = db.MedicalHistories.ToList().Select(medicalhistory => new SelectListItem
            {
                Selected = participant.MedicalHistories != null && participant.MedicalHistories.Contains(medicalhistory),
                Text = medicalhistory.Name,
                Value = medicalhistory.Id.ToString()
            }).ToList();

            ViewBag.MedicalHistories = medicalhistories;

            return View(participant);
        }

        // GET: /Participant/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Participant participant = db.Participants
                .Include(x => x.Appointments)
                .Include(x => x.Corsets)
                .Include(x => x.DrugHistories)
                .FirstOrDefault(x => x.Id == id);

            if (participant == null)
            {
                return HttpNotFound();
            }

            ParticipantIdTrauma participantIdTrauma = db.ParticipantIdTraumas.SingleOrDefault(e => e.IdParticipantPourTrauma == participant.Id);

            if (participantIdTrauma != null)
            {
                participant.IdToDisplay = participantIdTrauma.TraumaId;
            }
            else
            {
                participant.IdToDisplay = participant.Id.ToString();
            }


            SetupViewBags(participant);

            return View(participant);
        }
        
        // POST: /Participant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Participant participant, int[] diagnosesForParticipant, int[] medicalhistoriesForParticipant)
        {
            // Bug fix
            // Pour l'edition avec NULL comme "Family role".
            // Le champ "Family role" est Nullable, mais le ModelState detecte une erreur meme si la valeur est NULL (ne devrait pas).
            if (participant.FamilyRole_Id == null)
            {
                participant.FamilyRole = null;
                ModelState.Remove("FamilyRole.Id");
            }


            if (ModelState.IsValid)
            {
                AddNewEntityIfNecessary(participant);

                db.Entry(participant).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var participantDiagnoses = db.Participants.Include(a => a.Diagnoses)
                    .SingleOrDefault(a => a.Id == participant.Id);

                if (participantDiagnoses != null)
                {
                    foreach (var deleteDiagnoses in participantDiagnoses.Diagnoses.ToList())
                    {
                        participantDiagnoses.Diagnoses.Remove(deleteDiagnoses);
                    }
                }

                if (diagnosesForParticipant != null)
                {

                    var selectedDiagnoses = db.Diagnoses.Where(x => diagnosesForParticipant.Contains(x.Id)).ToList();
                    foreach (var selectedDiagnosis in selectedDiagnoses)
                    {
                        selectedDiagnosis.Participants.Add(participant);
                    }

                    participant.Diagnoses = selectedDiagnoses;
                }


                var participantMedicalHistories = db.Participants.Include(a => a.MedicalHistories).SingleOrDefault(a => a.Id == participant.Id);

                if (participantMedicalHistories != null)
                {
                    foreach (var deleteMedicalHistories in participantMedicalHistories.MedicalHistories.ToList())
                    {
                        participantMedicalHistories.MedicalHistories.Remove(deleteMedicalHistories);
                    }
                }

                if (medicalhistoriesForParticipant != null)
                {

                    var selectedMedicalHistories = db.MedicalHistories.Where(x => medicalhistoriesForParticipant.Contains(x.Id)).ToList();
                    foreach (var selectedMedicalHistory in selectedMedicalHistories)
                    {
                        selectedMedicalHistory.Participants.Add(participant);
                    }

                    participant.MedicalHistories = selectedMedicalHistories;
                }


                await db.SaveChangesAsync();

                
                //db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(participant);
        }

        [HttpPost]
        public void DeleteParticipant(int participantId)
        {
            var participant =
                db.Participants.Include(p => p.Appointments.Select(a => a.Samplings))
                    .Include(p => p.Families)
                    .First(p => p.Id == participantId);

            var samplings = participant.Appointments.SelectMany(a => a.Samplings).ToList();
            foreach (var sampling in samplings)
            {
                db.Samplings.Remove(sampling);
            }
            db.SaveChanges(User.Identity.Name);

            foreach (var family in participant.Families.ToList())
            {
                family.Participant_Id = null;
            }

            db.SaveChanges(User.Identity.Name);

            db.Participants.Remove(participant);
            db.SaveChanges(User.Identity.Name);

        }

        // POST: /Participant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Participant participant = await db.Participants.FindAsync(id);
            db.Participants.Remove(participant);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region Corset

        public ActionResult GetNewCorset(int participantId)
        {
            ViewBag.CorsetTypes = db.CorsetTypes.ToList().Select(st =>
                new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = Session["Language"].ToString().ToLower() == "en" ? st.Name : st.NameFr,
                });


            return PartialView("_CreateOrEditCorset", new Corset { Participant_Id = participantId });

        }

        public ActionResult GetCorset(int CorsetId)
        {
            var Corset = db.Corsets.Include(s => s.CorsetType).First(s => s.Id == CorsetId);

            ViewBag.CorsetTypes = db.CorsetTypes.ToList().Select(st =>
                new SelectListItem
                {
                    Value = st.Id.ToString(),
                    Text = Session["Language"].ToString().ToLower() == "en" ? st.Name : st.NameFr,
                    Selected = st.Id == Corset.CorsetType_Id
                });

            return PartialView("_CreateOrEditCorset", Corset);
        }

        public ActionResult GetCorsetForDeletion(int CorsetId)
        {
            var Corset = db.Corsets.Include(s => s.CorsetType).First(s => s.Id == CorsetId);

            return PartialView("_ConfirmDeleteCorset", Corset);
        }

        [HttpPost]
        public ActionResult DeleteCorset(int CorsetId)
        {
            var Corset = db.Corsets.Find(CorsetId);
            db.Corsets.Remove(Corset);
            db.SaveChanges(User.Identity.Name);

            ViewBag.ApppointmentId = Corset.Participant_Id;

            var participant =
                db.Participants
                .Include(a => a.Corsets.Select(s => s.CorsetType))
                    .First(a => a.Id == Corset.Participant_Id);

            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id",
                Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

            return Json(new { success = true, html = this.RenderPartialViewToString("_ListCorsets", participant) });
        }

        [HttpPost]
        public ActionResult CreateOrEditCorset(Corset Corset)
        {
            if (ModelState.IsValid)
            {
                if (Corset.Id == 0)
                {
                    db.Corsets.Add(Corset);
                }
                else
                {
                    var CorsetFromDb = db.Corsets.First(s => s.Id == Corset.Id);
                    CorsetFromDb.CorsetType_Id = Corset.CorsetType_Id;
                    CorsetFromDb.Start = Corset.Start;
                    CorsetFromDb.End = Corset.End;
                    CorsetFromDb.Comment = Corset.Comment;
                }

                db.SaveChanges(User.Identity.Name);
                ViewBag.ParticipantId = Corset.Participant_Id;

                var participant =
                    db.Participants
                        .Include(a => a.Corsets.Select(s => s.CorsetType))
                        .First(a => a.Id == Corset.Participant_Id);

                ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id",
                    Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");

                return Json(new { success = true, html = this.RenderPartialViewToString("_ListCorsets", participant) });
            }

            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", "Name", Corset.CorsetType_Id);

            return Json(new { success = false, message = "form is invalid" });
        }

        #endregion

        #region Sampling

        //Show Sampling
        public ActionResult ShowSampling(int id)
        {
            ViewBag.SamplingId = id;
            var results = db.Samplings.Include(a => a.Appointment).Where(a => a.Appointment.Participant_Id == id);
            return PartialView("_Sampling", results.ToList());
        }
        //Show Appointment
        public ActionResult ShowAppointment(int id)
        {
            var results = db.Appointments.Include(a => a.User).Where(a => a.Participant_Id == id).OrderBy(a => a.Date).ToList();
            int? participantAgeAtAppointment = null;


            foreach (var appointment in results)
            {
                if (appointment.Date.HasValue && appointment.Participant.DOB.HasValue)
                {
                    participantAgeAtAppointment = appointment.Date.Value.Year
                                                        - appointment.Participant.DOB.Value.Year;

                    appointment.ParticipantAgeAtAppointment = participantAgeAtAppointment;
                }
            }

            return PartialView("_Appointment", results);
        }

        #endregion

        #region Add Family

        [HttpPost]
        public JsonResult AddFamily(int id)
        {
            if (id > 0)
            {
                var family = db.Families.FirstOrDefault(i => i.Participant_Id == id);
                if (family == null)
                {
                    family = new Family { Participant_Id = id };
                    db.Families.Add(family);
                    db.SaveChanges(User.Identity.Name);

                    var familyfromDb = db.Families.Include(f => f.Participant).First(f => f.Id == family.Id);
                    return Json(new
                    {
                        success = true,
                        selectListItem = new SelectListItem
                        {
                            Value = familyfromDb.Id.ToString(),
                            Text = string.Format("{0} - {1}", familyfromDb.Id, familyfromDb.Participant.FullName)
                        }
                    });
                }

                return Json(new { success = false, message = Languages.Global.FamilyAlreadyExists });
            }
            return Json(new { success = false, message = Languages.Global.FamilyNotCreated });
        }

        #endregion

        #region Private method and others
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetupViewBags(Participant participant)
        {

            ViewBag.City_Id = new SelectList(db.Cities.OrderBy(x => x.Name), "Id", "Name", participant.City_Id);
            ViewBag.Doctor_Id = new SelectList(db.Users.Include(x => x.Roles).Where(x => x.Roles.Any(j => j.Name == "Doctor")), "Id", "FullName", participant.Doctor_Id);
            ViewBag.Group_Id = new SelectList(db.Groups.OrderBy(x => x.NameFr), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", participant.Group_Id);

            ViewBag.FamilyRole_Id = new SelectList(db.FamilRoles.OrderBy(x => x.NameFr), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", participant.FamilyRole_Id);
            ViewBag.EthnicGroup_Id = new SelectList(db.EthnicGroups.OrderBy(x => x.NameFr), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", participant.EthnicGroup_Id);
            ViewBag.SurgeryType_Id = new SelectList(db.SurgeryTypes.OrderBy(x => x.NameFr), "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr", participant.SurgeryType_Id);
            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", Session["Language"].ToString().ToLower() == "en" ? "Name" : "NameFr");
            var diagnoses = db.Diagnoses.ToList().Select(diagnosis => new SelectListItem
            {
                Selected = participant.Diagnoses.Contains(diagnosis),
                Text = Session["Language"].ToString().ToLower() == "en" ? diagnosis.Name : diagnosis.NameFr,
                Value = diagnosis.Id.ToString()
            }).ToList();
            ViewBag.Diagnoses = diagnoses;


            var medicalhistories = db.MedicalHistories.ToList().Select(medicalhistory => new SelectListItem
            {
                Selected = participant.MedicalHistories.Contains(medicalhistory),
                Text = Session["Language"].ToString().ToLower() == "en" ? medicalhistory.Name : medicalhistory.NameFr,
                Value = medicalhistory.Id.ToString()
            }).ToList();

            ViewBag.MedicalHistories = medicalhistories;


            ViewBag.Family_Id =
                
                db.Families.Include(f => f.Participant).OrderBy(f => f.Id).ToList().Select(f => new SelectListItem
                {
                    Value = f.Id.ToString(),
                    Text = string.Format("{0} - {1}", f.Id.ToString(), f.Participant_Id == null ? "" : f.Participant.FullName ),
                    Selected = f.Id == participant.Family_Id
                });
        }

        private void AddNewEntityIfNecessary(Participant participant)
        {
            bool cityMustBeAdded = participant.City_Id < 1;
            bool familyMustBeAdded = participant.Family_Id < 1;
            bool familyRoleMustBeAdded = participant.FamilyRole_Id < 1;

            if (cityMustBeAdded)
            {
                City newCity = new City
                {
                    Name = participant.City.Name,
                    Province = db.Provinces.Single(e => e.Id == 2),
                    Region = db.Regions.Single(e => e.Id == 18)
                };
                db.Cities.Add(newCity);
                db.SaveChanges();

                participant.City = newCity;
                participant.City_Id = newCity.Id;
            }
            else
            {
                participant.City = db.Cities.Single(c => c.Id == participant.City_Id);
            }

            if (familyMustBeAdded)
            {
                Family newFamily = new Family
                {
                    Name = participant.Family.Name
                };
                db.Families.Add(newFamily);
                db.SaveChanges();

                participant.Family = newFamily;
                participant.Family_Id = newFamily.Id;
            }

            if (familyRoleMustBeAdded)
            {
                FamilyRole newFamilyRole = new FamilyRole
                {
                    Name = participant.FamilyRole.Name,
                    NameFr = participant.FamilyRole.Name
                };
                db.FamilRoles.Add(newFamilyRole);
                db.SaveChanges();

                participant.FamilyRole = newFamilyRole;
                participant.FamilyRole_Id = newFamilyRole.Id;
            }
        }

        #endregion
    }
}
