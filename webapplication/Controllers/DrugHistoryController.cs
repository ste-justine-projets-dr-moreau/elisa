using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Context;

namespace WebApplication.Controllers
{
    public class DrugHistoryController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: /DrugHistory/
        public ActionResult Index()
        {
            var drughistories = db.DrugHistories.Include(d => d.Drug).Include(d => d.Participant);
            return View(drughistories.ToList());
        }

        // GET: /AppointmenetDrug/
        public ActionResult IndexAppointment(int id)
        {
            ViewBag.ParticipantId = id;
            var drugHistories = db.DrugHistories.Where(a => a.Participant_Id == id);
            return PartialView("_IndexAppointment", drugHistories.ToList());

        }
        // GET: /DrugHistory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugHistory drughistory = db.DrugHistories.Find(id);
            if (drughistory == null)
            {
                return HttpNotFound();
            }
            return View(drughistory);
        }

        // GET: /DrugHistory/Create
        public ActionResult Create()
        {
            ViewBag.Drug_Id = new SelectList(db.Drugs, "Id", "Name");
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName");
            return View();
        }

        // POST: /DrugHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Dosage,StartDate,EndDate,Comment,Participant_Id,Drug_Id")] DrugHistory drughistory)
        {
            if (ModelState.IsValid)
            {
                db.DrugHistories.Add(drughistory);
                db.SaveChanges(User.Identity.Name);
                return RedirectToAction("Index");
            }

            ViewBag.Drug_Id = new SelectList(db.Drugs, "Id", "Name", drughistory.Drug_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", drughistory.Participant_Id);
            return View(drughistory);
        }

        // GET: /DrugHistory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugHistory drughistory = db.DrugHistories.Find(id);
            if (drughistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.Drug_Id = new SelectList(db.Drugs, "Id", "Name", drughistory.Drug_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", drughistory.Participant_Id);
            return View(drughistory);
        }

        // POST: /DrugHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Dosage,StartDate,EndDate,Comment,Participant_Id,Drug_Id")] DrugHistory drughistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drughistory).State = EntityState.Modified;
                db.SaveChanges(User.Identity.Name);
                return RedirectToAction("Index");
            }
            ViewBag.Drug_Id = new SelectList(db.Drugs, "Id", "Name", drughistory.Drug_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", drughistory.Participant_Id);
            return View(drughistory);
        }

        // GET: /DrugHistory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrugHistory drughistory = db.DrugHistories.Find(id);
            if (drughistory == null)
            {
                return HttpNotFound();
            }
            return View(drughistory);
        }

        // POST: /DrugHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DrugHistory drughistory = db.DrugHistories.Find(id);
            db.DrugHistories.Remove(drughistory);
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
    }
}
