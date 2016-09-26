using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using CarRental.Data.App_Data;

namespace CarRental.Web.Structure.Authorize
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (Roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public int ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string[] Roles { get; set; }
    }
}