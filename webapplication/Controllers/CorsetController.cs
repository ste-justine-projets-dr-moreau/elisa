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
    public class CorsetController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: /Corset/
        public ActionResult Index()
        {
            var corsets = db.Corsets.Include(c => c.CorsetType).Include(c => c.Participant);
            return View(corsets.ToList());
        }

        // GET: /AppointmenetCorset/
        public ActionResult IndexAppointment(int id)
        {
            ViewBag.ParticipantId = id;
            var corsets = db.Corsets.Where(a => a.Participant_Id == id);
            return PartialView("_IndexAppointment", corsets.ToList());

        }

        // GET: /Corset/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corset corset = db.Corsets.Find(id);
            if (corset == null)
            {
                return HttpNotFound();
            }
            return View(corset);
        }

        // GET: /Corset/Create
        public ActionResult Create()
        {
            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", "Name");
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName");
            return View();
        }

        // POST: /Corset/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Start,End,Comment,Participant_Id,CorsetType_Id")] Corset corset)
        {
            if (ModelState.IsValid)
            {
                db.Corsets.Add(corset);
                db.SaveChanges(User.Identity.Name);
                return RedirectToAction("Index");
            }

            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", "Name", corset.CorsetType_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", corset.Participant_Id);
            return View(corset);
        }

        // GET: /Corset/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corset corset = db.Corsets.Find(id);
            if (corset == null)
            {
                return HttpNotFound();
            }
            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", "Name", corset.CorsetType_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", corset.Participant_Id);
            return View(corset);
        }

        // POST: /Corset/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Start,End,Comment,Participant_Id,CorsetType_Id")] Corset corset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corset).State = EntityState.Modified;
                db.SaveChanges(User.Identity.Name);
                return RedirectToAction("Index");
            }
            ViewBag.CorsetType_Id = new SelectList(db.CorsetTypes, "Id", "Name", corset.CorsetType_Id);
            ViewBag.Participant_Id = new SelectList(db.Participants, "Id", "FirstName", corset.Participant_Id);
            return View(corset);
        }

        // GET: /Corset/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corset corset = db.Corsets.Find(id);
            if (corset == null)
            {
                return HttpNotFound();
            }
            return View(corset);
        }

        // POST: /Corset/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Corset corset = db.Corsets.Find(id);
            db.Corsets.Remove(corset);
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
