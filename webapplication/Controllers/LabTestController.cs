using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class LabTestController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: LabTest
        public ActionResult Index()
        {
            var model = db.LabTests.ToList();
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var labtest = db.LabTests.Where(i => i.Id == id).FirstOrDefault();
            return Json(new
            {
                Id = labtest.Id,
                Name = labtest.Name,
                NameFr = labtest.NameFr,
                SortOrder = labtest.SortOrder,
                IsActive = labtest.IsActive
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var labtest = db.LabTests.Where(i => i.Id == id).FirstOrDefault();
                db.LabTests.Remove(labtest);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.LabTests.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(LabTest model)
        {
            if (model.Id > 0)
            {
                var labtest = db.LabTests.Where(i => i.Id == model.Id).FirstOrDefault();
                labtest.Name = model.Name;
                labtest.NameFr = model.NameFr;
                labtest.IsActive = model.IsActive;
                labtest.SortOrder = model.SortOrder;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.LabTests.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.LabTests.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}