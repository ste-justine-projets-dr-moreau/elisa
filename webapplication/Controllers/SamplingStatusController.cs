using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class SamplingStatusController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: SamplingSTatus
        public ActionResult Index()
        {
            var model = db.SamplingStatus.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var samplingstatus = db.SamplingStatus.Where(i => i.Id == id).FirstOrDefault();
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                           Id = samplingstatus.Id,
                           Name = samplingstatus.Name,
                           NameFr = samplingstatus.NameFr,
                           IsActive = samplingstatus.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var samplingstatus = db.SamplingStatus.Where(i => i.Id == id).FirstOrDefault();
                db.SamplingStatus.Remove(samplingstatus);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SamplingStatus.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(SamplingStatu model)
        {
            if (model.Id > 0)
            {
                var samplingstatus = db.SamplingStatus.Where(i => i.Id == model.Id).FirstOrDefault();
                samplingstatus.Name = model.Name;
                samplingstatus.NameFr = model.NameFr;
                samplingstatus.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.SamplingStatus.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SamplingStatus.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}