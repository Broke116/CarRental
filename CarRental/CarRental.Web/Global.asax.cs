using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using CarRental.Web.Models.ViewModel;
using CarRental.Web.Structure.Authorize;
using Newtonsoft.Json;

namespace CarRental.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            Bootstrapper.Run();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    var serializeModel = JsonConvert.DeserializeObject<PrincipalModel>(authTicket.UserData);
                    var newUser = new CustomPrincipal(authTicket.Name)
                    {
                        ID = serializeModel.ID,
                        Email = serializeModel.Email,
                        Username = serializeModel.Username,
                        Roles = serializeModel.Roles
                    };

                    HttpContext.Current.User = newUser;
                }
            }

        }
    }
}
