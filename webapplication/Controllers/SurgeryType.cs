using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class SurgeryTypeController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: SurgeryType
        public ActionResult Index()
        {
            var model = db.SurgeryTypes.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var surgeryType = db.SurgeryTypes.Where(i => i.Id == id).FirstOrDefault();
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                        Id = surgeryType.Id, 
                        Name = surgeryType.Name,
                        NameFr = surgeryType.NameFr,
                        IsActive = surgeryType.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var surgeryType = db.SurgeryTypes.Where(i => i.Id == id).FirstOrDefault();
                db.SurgeryTypes.Remove(surgeryType);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SurgeryTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(SurgeryType model)
        {
            if (model.Id > 0)
            {
                var surgeryType = db.SurgeryTypes.Where(i => i.Id == model.Id).FirstOrDefault();
                surgeryType.Name = model.Name;
                surgeryType.NameFr = model.NameFr;
                surgeryType.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.SurgeryTypes.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.SurgeryTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}