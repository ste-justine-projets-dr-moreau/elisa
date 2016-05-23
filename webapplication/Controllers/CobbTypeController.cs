using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class CobbTypeController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: CobbType
        public ActionResult Index()
        {
            var model = db.CobbTypes.ToList();
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var cobbType = db.CobbTypes.Where(i => i.Id == id).FirstOrDefault();
            return Json(new
            {
                Id = cobbType.Id,
                Name = cobbType.Name,
                NameFr = cobbType.NameFr,
                ShortName = cobbType.ShortName,
                IsActive = cobbType.IsActive
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var cobbType = db.CobbTypes.Where(i => i.Id == id).FirstOrDefault();
                db.CobbTypes.Remove(cobbType);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CobbTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(CobbType model)
        {
            if (model.Id > 0)
            {
                var cobbType = db.CobbTypes.Where(i => i.Id == model.Id).FirstOrDefault();
                cobbType.Name = model.Name;
                cobbType.NameFr = model.NameFr;
                cobbType.IsActive = model.IsActive;
                cobbType.ShortName = model.ShortName;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.CobbTypes.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CobbTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}