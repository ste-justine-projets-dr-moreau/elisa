using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;
using Clinic.BackEnd.Context;

namespace WebApplication.Controllers
{
    public class CultureManagerController : Controller
    {
        private readonly ClinicContext _db;
        public CultureManagerController()
        {
            _db = new ClinicContext();
        }
        //
        // GET: /CultureManager/
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            var cultureInfo = new CultureInfo(lang);
            var user = _db.Users.First(x => x.NT == User.Identity.Name);

            user.Language.Name = cultureInfo.Name;
            _db.SaveChanges(User.Identity.Name);

            Session["Language"] = cultureInfo.Name;

            return Redirect(returnUrl);
        }

	}
}