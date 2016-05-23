using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Clinic.BackEnd.Context;
using Clinic.BackEnd.Models;
using WebApplication.Models.Binders;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableDateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(float), new FloatModelBinder());
            ModelBinders.Binders.Add(typeof(string), new StringModelBinder());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start()
        {
            var db = new ClinicContext();

            User user = db.Users.Include("Roles").FirstOrDefault(x => x.NT == User.Identity.Name);
            if (user == null)
            {
                user = new User
                       {
                           IsActive = true,
                           Language_Id = 1,
                           NT = User.Identity.Name
                       };
                db.Users.Add(user);
                db.SaveChanges(User.Identity.Name);
            }

            Session["UserRoles"] = user.Roles.Select(x => x.Name).ToList();
        }


        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session == null) return;
            CultureInfo ci;

            // A retirer pour Windows Authentification
            if (Session["Language"] == null)

            {
                var db = new ClinicContext();
                var user = db.Users.First(x => x.NT == User.Identity.Name);
                if (!string.IsNullOrEmpty(user.Language.Name))
                {
                    Session["Language"] = user.Language.Name;
                    ci = new CultureInfo(user.Language.Name);
                }
                else
                {
                    var langName = "en";

                    //Try to get values from Accept lang HTTP header
                    if (HttpContext.Current.Request.UserLanguages != null &&
                        HttpContext.Current.Request.UserLanguages.Length != 0)
                    {
                        //Gets accepted list 
                        langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 2);
                    }
                    ci = new CultureInfo(langName);
                    user.Language.Name = ci.Name;
                    Session["Language"] = ci.Name;
                }

                db.SaveChanges(User.Identity.Name);
            }
            else
            {
                ci = new CultureInfo((string)Session["Language"]);
            }

            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }

    }
}
