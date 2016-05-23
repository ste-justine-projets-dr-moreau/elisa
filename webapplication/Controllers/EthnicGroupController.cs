using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class EthnicGroupController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: EthnicGroups
        public ActionResult Index()
        {
            var model = db.EthnicGroups.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var ethnicGroup = db.EthnicGroups.Where(i => i.Id == id).FirstOrDefault();
            //return Json(ethnicGroup, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                        Id = ethnicGroup.Id, 
                        Name = ethnicGroup.Name,
                        NameFr = ethnicGroup.NameFr,
                        IsActive = ethnicGroup.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var ethnicGroup = db.EthnicGroups.Where(i => i.Id == id).FirstOrDefault();
                db.EthnicGroups.Remove(ethnicGroup);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.EthnicGroups.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(EthnicGroup model)
        {
            if (model.Id > 0)
            {
                var ethnicGroup = db.EthnicGroups.Where(i => i.Id == model.Id).FirstOrDefault();
                ethnicGroup.Name = model.Name;
                ethnicGroup.NameFr = model.NameFr;
                ethnicGroup.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.EthnicGroups.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.EthnicGroups.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}