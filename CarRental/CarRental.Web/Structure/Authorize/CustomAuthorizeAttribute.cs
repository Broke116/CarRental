using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CarRental.Data.App_Data;

namespace CarRental.Web.Structure.Authorize
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string UserConfigKey { get; set; }
        public string RoleConfigKey { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                var authorizedUser = ConfigurationManager.AppSettings[UserConfigKey];
                var authorizedRoles = ConfigurationManager.AppSettings[RoleConfigKey];

                Users = string.IsNullOrEmpty(Users) ? authorizedUser : Users;
                Roles = string.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;

                if (!string.IsNullOrEmpty(Roles))
                {
                    if (!CurrentUser.IsInRole(Roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }

                if (!string.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(CurrentUser.ID.ToString()))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }
            }
        }
    }
}