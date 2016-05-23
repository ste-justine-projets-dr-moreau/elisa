using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using System.Linq;

using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class DrugController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: Diagnoses
        public ActionResult Index()
        {
            var model = db.Drugs.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var drug = db.Drugs.Where(i => i.Id == id).FirstOrDefault();
            //return Json(diagnosis, JsonRequestBehavior.AllowGet);
            return Json(drug, JsonRequestBehavior.AllowGet);


        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var drug = db.Drugs.Where(i => i.Id == id).FirstOrDefault();
                db.Drugs.Remove(drug);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Drugs.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(Drug model)
        {
            if (model.Id > 0)
            {
                var drug = db.Drugs.Where(i => i.Id == model.Id).FirstOrDefault();
                drug.Name = model.Name;
                drug.Provider = model.Provider;
                drug.IsActive = model.IsActive;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.Drugs.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Drugs.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion
    }
}