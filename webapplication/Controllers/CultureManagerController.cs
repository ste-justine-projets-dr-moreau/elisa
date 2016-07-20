using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;

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
            Language language = null;
            Language english = _db.Languages.Single(l => l.Id == 1);
            Language french = _db.Languages.Single(l => l.Id == 2);


            if (lang == "fr")
            {
                english.Name = "Anglais";
                french.Name = "Français";
                language = french;
            }
            else // lang == "en"
            {
                english.Name = "English";
                french.Name = "French";
                language = english;
            }

            user.Language = language;
            _db.SaveChanges(User.Identity.Name);

            Session["Language"] = cultureInfo.Name;

            return Redirect(returnUrl);
        }

	}
}