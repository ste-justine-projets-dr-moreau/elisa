using System.Linq;
using System.Web.Mvc;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Context;

namespace WebApplication.Controllers
{
    public class CityController : Controller
    {
        #region Private Variables

        private ClinicContext db = new ClinicContext();

        #endregion

        #region Actions

        #region Index

        // GET: /City/
        public ActionResult Index()
        {
            ViewBag.Province_Id = new SelectList(db.Provinces, "Id", "Name");
            ViewBag.Region_Id = new SelectList(db.Regions, "Id", "Name");
            var model = db.Cities.ToList().OrderBy(x => x.Name);
            return View(model);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult Get(int id)
        {
            var city = db.Cities.Where(i => i.Id == id).FirstOrDefault();
            return Json(new
            {
                Id = city.Id,
                Name = city.Name,
                Province_Id = city.Province_Id,
                Region_Id = city.Region_Id
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        [HttpPost]
        public PartialViewResult Delete(int id)
        {
            if (id > 0)
            {
                var city = db.Cities.Where(i => i.Id == id).FirstOrDefault();
                db.Cities.Remove(city);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Cities.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #region Save

        [HttpPost]
        public PartialViewResult Save(City model)
        {

  
            if (model.Id > 0)
            {
                var city = db.Cities.Where(i => i.Id == model.Id).FirstOrDefault();
                city.Name = model.Name;
                city.Province_Id = model.Province_Id;
                city.Region_Id = model.Region_Id;
                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                db.Cities.Add(model);
                db.SaveChanges(User.Identity.Name);
            }

            var result = db.Cities.ToList();
            return PartialView("_List", result);
        }

        #endregion

        #endregion

    }
}
