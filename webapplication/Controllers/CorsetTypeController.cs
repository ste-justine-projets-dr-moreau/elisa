using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class CorsetTypeController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: CorsetType
        public ActionResult Index()
        {
            var model = db.CorsetTypes.ToList();
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var corsetType = db.CorsetTypes.Where(i => i.Id == id).FirstOrDefault();
            return Json(new
            {
                Id = corsetType.Id,
                Name = corsetType.Name,
                NameFr = corsetType.NameFr,
                IsActive = corsetType.IsActive
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var corsetType = db.CorsetTypes.Where(i => i.Id == id).FirstOrDefault();
                db.CorsetTypes.Remove(corsetType);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CorsetTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(CorsetType model)
        {
            if (model.Id > 0)
            {
                var corsetType = db.CorsetTypes.Where(i => i.Id == model.Id).FirstOrDefault();
                corsetType.Name = model.Name;
                corsetType.NameFr = model.NameFr;
                corsetType.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.CorsetTypes.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.CorsetTypes.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}