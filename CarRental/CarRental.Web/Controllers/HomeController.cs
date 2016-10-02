using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;

namespace CarRental.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IBaseRepository<Car> _carRepository;

        public HomeController(IBaseRepository<Car> carRepository,
            IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _carRepository = carRepository;
        }

        public ActionResult Index()
        {
            var user = _carRepository.GetAll().OrderByDescending(m => m.)
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}