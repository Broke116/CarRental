using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Web.Structure.Authorize;

namespace CarRental.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IBaseRepository<Error> _errorsRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork)
        {
            _errorsRepository = errorsRepository;
            _unitOfWork = unitOfWork;
        }

        protected new virtual CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}