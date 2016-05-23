using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using WebApplication.Helpers;

namespace WebApplication.Controllers
{
    public class ResultController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: Result
        public ActionResult Index(int id)
        {
            ViewBag.SamplingId = id;
            var results = db.Results.Where(a => a.Sampling_Id == id).OrderBy(x => x.LabTest.SortOrder

    );
            return PartialView("_Index",results.ToList());
        }

        // GET: Result/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Result result = db.Results.Find(id);
        //    if (result == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(result);
        //}

        // GET: Result/Create
        public ActionResult Create(int samplingId)
        {

            if (Session["Language"].ToString()  == "en" )
                {
            ViewBag.LabTest_Id = new SelectList(db.LabTests, "Id", "Name");
            }       
                else
            {
                 ViewBag.LabTest_Id = new SelectList(db.LabTests, "Id", "NameFr");
            }

            var currentUser = db.Users.First(x => x.NT == User.Identity.Name);
            
            var result = new Result 
            {
            Sampling_Id = samplingId,
            User_Id = currentUser.Id,
            Date = DateTime.Now
            };

            ModelState.Clear();
            return PartialView("_create", result);
        }

        // POST: Result/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Labresult,User_Id,LabTest_Id,Sampling_Id, SubSample")] Result result)
        
        {
            if (ModelState.IsValid)
            {
                db.Results.Add(result);
                db.SaveChanges(User.Identity.Name);
                ViewBag.SamplingId = result.Sampling_Id;
               var results = db.Results.Where(a => a.Sampling_Id == result.Sampling_Id);
               return Json(new { success = true, html = this.RenderPartialViewToString("_Index", results) });
                //return Json(new { success = true, html = "test" });
 }
            ViewBag.LabTest_Id = new SelectList(db.LabTests, "Id", "Name", result.LabTest_Id);
            return PartialView("_Create", result);
        }

        // GET: Result/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.LabTest_Id = new SelectList(db.LabTests, "Id", "Name", result.LabTest_Id);
            return PartialView("_edit", result);
        }

        // POST: Result/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Labresult,User_Id,LabTest_Id,Sampling_Id, SubSample")] Result result)
        {
            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges(User.Identity.Name);
                ViewBag.SamplingId = result.Sampling_Id;
                var results = db.Results.Include(a => a.LabTest).Where(a => a.Sampling_Id == result.Sampling_Id);
                return Json(new { success = true, html = this.RenderPartialViewToString("_index", results) });
            }
            ViewBag.LabTest_Id = new SelectList(db.LabTests, "Id", "Name", result.LabTest_Id);
            return PartialView("_edit", result);
        }

        // GET: Result/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }

        // POST: Result/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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
