using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class SamplingTypeController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: SamplingType
        public ActionResult Index()
        {
            var model = db.SamplingTypes.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var samplingtype = db.SamplingTypes.Where(i => i.Id == id).FirstOrDefault();
           return Json(new 
                       {
                           Id = samplingtype.Id,
                           Name = samplingtype.Name,
                           NameFr = samplingtype.NameFr,
                           IsActive = samplingtype.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var samplingtype = db.SamplingTypes.Where(i => i.Id == id).FirstOrDefault();
                db.SamplingTypes.Remove(samplingtype);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SamplingTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(SamplingType model)
        {
            if (model.Id > 0)
            {
                var samplingtype = db.SamplingTypes.Where(i => i.Id == model.Id).FirstOrDefault();
                samplingtype.Name = model.Name;
                samplingtype.NameFr = model.NameFr;
                samplingtype.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.SamplingTypes.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SamplingTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}