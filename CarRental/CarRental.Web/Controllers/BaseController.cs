using System.Web.Mvc;
using CarRental.Web.Structure.Authorize;

namespace CarRental.Web.Controllers
{
    public class BaseController : Controller
    {
        protected new virtual CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}