using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;

namespace CarRental.Web.Controllers
{
    //[Authorize]
    public class CarController : BaseController
    {
        public CarController(IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCar()
        {
            return View();
        }
    }
}