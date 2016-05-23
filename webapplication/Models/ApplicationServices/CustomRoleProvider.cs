using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using Clinic.BackEnd.Context;

namespace WebApplication.Models.ApplicationServices
{
    public class CustomRoleProvider : RoleProvider
    {
        private ClinicContext _db;

        #region Overrides of RoleProvider

        public override bool IsUserInRole(string username, string roleName)
        {
            _db = new ClinicContext();
            var user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.NT == username);
            if (user == null)
                return false;


            return
                user.Roles.Any(
                    x => x.Name == roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            _db = new ClinicContext();
            var user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.NT == username);
            if (user != null)
                return
                    user.Roles.Select(
                        x => x.Name).ToArray();

            return new string[] { };
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return _db.Roles.Any(r => r.Name == roleName);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            _db = new ClinicContext();
            var role = _db.Roles.Include(u => u.Users).FirstOrDefault(u => u.Name == roleName);
            if (role != null)
                return
                    role.Users.Select(x => x.NT).ToArray();

            return new string[]{};
        }

        public override string[] GetAllRoles()
        {
            _db = new ClinicContext();
            return _db.Roles.Select(x => x.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            _db = new ClinicContext();
            if (string.IsNullOrEmpty(usernameToMatch))
                return
                    _db.Roles.Include(r => r.Users).Where(x => x.Name == roleName).SelectMany(
                        x => x.Users).Select(x => x.NT).
                        ToArray();

            return
               _db.Roles.Include(r => r.Users).Where(x => x.Name == roleName).SelectMany(
                    x => x.Users).Where(x => x.NT.Contains(usernameToMatch)).Select(x => x.NT).
                    ToArray();
        }

        public override string ApplicationName { get; set; }

        #endregion
    }
}