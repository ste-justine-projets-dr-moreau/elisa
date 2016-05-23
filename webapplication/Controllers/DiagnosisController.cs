using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class DiagnosisController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: Diagnoses
        public ActionResult Index()
        {
            var model = db.Diagnoses.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var diagnosis = db.Diagnoses.Where(i => i.Id == id).FirstOrDefault();
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                        Id = diagnosis.Id, 
                        Name = diagnosis.Name,
                        NameFr = diagnosis.NameFr,
                        IsActive = diagnosis.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var diagnosis = db.Diagnoses.Where(i => i.Id == id).FirstOrDefault();
                db.Diagnoses.Remove(diagnosis);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Diagnoses.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(Diagnosis model)
        {
            if (model.Id > 0)
            {
                var diagnosis = db.Diagnoses.Where(i => i.Id == model.Id).FirstOrDefault();
                diagnosis.Name = model.Name;
                diagnosis.NameFr = model.NameFr;
                diagnosis.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.Diagnoses.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Diagnoses.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}