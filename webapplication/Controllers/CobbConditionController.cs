using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class CobbConditionController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: CobbCondition
        public ActionResult Index()
        {
            var model = db.CobbConditions.ToList();
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var cobbCondition = db.CobbConditions.Where(i => i.Id == id).FirstOrDefault();
            return Json(new
            {
                Id = cobbCondition.Id,
                Name = cobbCondition.Name,
                NameFr = cobbCondition.NameFr,
                IsActive = cobbCondition.IsActive
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var cobbCondition = db.CobbConditions.Where(i => i.Id == id).FirstOrDefault();
                db.CobbConditions.Remove(cobbCondition);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CobbConditions.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(CobbCondition model)
        {
            if (model.Id > 0)
            {
                var cobbCondition = db.CobbConditions.Where(i => i.Id == model.Id).FirstOrDefault();
                cobbCondition.Name = model.Name;
                cobbCondition.NameFr = model.NameFr;
                cobbCondition.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.CobbConditions.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CobbConditions.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}