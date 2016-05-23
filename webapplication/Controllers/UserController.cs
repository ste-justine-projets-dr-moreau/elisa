using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Clinic.BackEnd.Models;
using Clinic.BackEnd.Context;
using System.Linq.Dynamic;
using PagedList;
using System.Text;


namespace WebApplication.Controllers
{
    public class UserController : Controller
    {
        private ClinicContext db = new ClinicContext();

        // GET: /User/
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Hospital);
            return View(users.ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            ViewBag.Hospital_Id = new SelectList(db.Hospitals, "Id", "Name");
            ViewBag.Language_Id = new SelectList(db.Languages, "Id", "Name");
            var roles = db.Roles.ToList().Select(role => new SelectListItem
            {
                Selected = false,
                Text = role.Name,
                Value = role.Id.ToString()
            }).ToList();

            ViewBag.Roles = roles;

            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include="Id,FirstName,LastName,Email,NT,Language,IsActive,Hospital_Id")] User user)
        public ActionResult Create(User user, int[] rolesForUser)
        {
            if (user.Language_Id != null && ModelState.IsValid)
            {
                if (rolesForUser != null)
                    {
                        var selectedRoles = db.Roles.Where(x => rolesForUser.Contains(x.Id)).ToList();
                        foreach (var selectedRole in selectedRoles)
                        {
                            selectedRole.Users.Add(user);
                        }

                        user.Roles = selectedRoles;
                    }
                db.Users.Add(user);
                db.SaveChanges(User.Identity.Name);
                return RedirectToAction("Index");
            }

            ViewBag.Hospital_Id = new SelectList(db.Hospitals, "Id", "Name", user.Hospital_Id);
            ViewBag.Language_Id = new SelectList(db.Languages, "Id", "Name", user.Language_Id); 
            var roles = db.Roles.ToList().Select(role => new SelectListItem
            {
                Selected = user.Roles != null && user.Roles.Contains(role),
                Text = role.Name,
                Value = role.Id.ToString()
            }).ToList();

            ViewBag.Roles = roles;

            return View(user);
         }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
           
            ViewBag.Hospital_Id = new SelectList(db.Hospitals, "Id", "Name", user.Hospital_Id);
            ViewBag.Language_Id = new SelectList(db.Languages, "Id", "Name", user.Language_Id); 
            var roles = db.Roles.ToList().Select(role => new SelectListItem
            {
                Selected = user.Roles != null && user.Roles.Contains(role),
                Text = role.Name,
                Value = role.Id.ToString()
            }).ToList();

            ViewBag.Roles = roles;

            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public ActionResult Edit([Bind(Include="Id,FirstName,LastName,Email,NT,Language,IsActive,Hospital_Id")] User user)      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(User user, int[] rolesForUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var UserRoles = this.db.Users.Include(a => a.Roles).SingleOrDefault(a => a.Id == user.Id);
                
                if ( UserRoles.Roles.ToList() != null)
                    {
                        foreach (var deleteRoles in UserRoles.Roles.ToList())
                        {
                            UserRoles.Roles.Remove(deleteRoles);
                        }
                    }
                if (rolesForUser != null)
                    {
                        var selectedRoles = db.Roles.Where(x => rolesForUser.Contains(x.Id)).ToList();
                        foreach (var selectedRole in selectedRoles)
                        {
                            selectedRole.Users.Add(user);
                        }
                   
                user.Roles = selectedRoles;
                   }
                  
                //db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //db.SaveChanges(User.Identity.Name);

                return RedirectToAction("Index");
            }
            
            ViewBag.Hospital_Id = new SelectList(db.Hospitals, "Id", "Name", user.Hospital_Id);
            ViewBag.Language_Id = new SelectList(db.Languages, "Id", "Name", user.Language_Id);
            var roles = db.Roles.ToList().Select(role => new SelectListItem
            {
                Selected = user.Roles.Contains(role),
                Text = role.Name,
                Value = role.Id.ToString()
            }).ToList();
            
            if (roles != null)
            {
                ViewBag.Roles = roles;
            }



            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges(User.Identity.Name);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
