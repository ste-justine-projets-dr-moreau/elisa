using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class GroupController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: Groups
        public ActionResult Index()
        {
            var model = db.Groups.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var group = db.Groups.Where(i => i.Id == id).FirstOrDefault();
            //return Json(group, JsonRequestBehavior.AllowGet);
            return Json(new 
                       {
                        Id = group.Id, 
                        Name = group.Name,
                        NameFr = group.NameFr,
                        IsActive = group.IsActive
                       }, JsonRequestBehavior.AllowGet);



        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var group = db.Groups.Where(i => i.Id == id).FirstOrDefault();
                db.Groups.Remove(group);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Groups.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(Group model)
        {
            if (model.Id > 0)
            {
                var group = db.Groups.Where(i => i.Id == model.Id).FirstOrDefault();
                group.Name = model.Name;
                group.NameFr = model.NameFr;
                group.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.Groups.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Groups.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}