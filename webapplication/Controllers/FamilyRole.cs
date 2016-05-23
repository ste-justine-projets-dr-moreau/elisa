using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class FamilyRoleController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: FamilyRole
        public ActionResult Index()
        {
            var model = db.FamilRoles.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var familyRole = db.FamilRoles.Where(i => i.Id == id).FirstOrDefault();
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                        Id = familyRole.Id, 
                        Name = familyRole.Name,
                        NameFr = familyRole.NameFr,
                        IsActive = familyRole.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var familyRole = db.FamilRoles.Where(i => i.Id == id).FirstOrDefault();
                db.FamilRoles.Remove(familyRole);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.FamilRoles.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(FamilyRole model)
        {
            if (model.Id > 0)
            {
                var familyRole = db.FamilRoles.Where(i => i.Id == model.Id).FirstOrDefault();
                familyRole.Name = model.Name;
                familyRole.NameFr = model.NameFr;
                familyRole.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.FamilRoles.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.FamilRoles.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}