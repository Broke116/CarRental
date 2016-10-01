using System;
using System.IO;
using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Web.Models.ViewModel;

namespace CarRental.Web.Controllers
{
    //[Authorize]
    public class CarController : BaseController
    {
        private readonly IBaseRepository<Car> _carRepository;
        public CarController(IBaseRepository<Car> carRepository,
            IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _carRepository = carRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCar(RegisterCarViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #region car image upload
                    string pathUrl = "";

                    if (model.Image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.Image.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                        if (System.IO.File.Exists(path)) // if file exists throw an error
                        {
                            ViewData["errorMessage"] = "The file has been already uploaded";
                            return View();
                        }
                        model.Image.SaveAs(path);
                        pathUrl = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    #endregion

                    #region capacity string to int conversion
                    int capacity = 0;
                    if (model.Capacity == "Two")
                        capacity = 2;
                    else if (model.Capacity == "Four")
                        capacity = 4;
                    else if (model.Capacity == "Five")
                        capacity = 5;
                    else
                        capacity = 9;
                    #endregion

                    #region car values added to entity model
                    var newCar = new Car // image stock
                    {
                        Title = model.Title,
                        Capacity = capacity,
                        Location = model.Location,
                        City = model.City,
                        County = model.County,
                        Description = model.Description,
                        FuelType = model.FuelType,
                        GearType = model.GearType,
                        GroupType = model.GroupType,
                        UniqueId = Guid.NewGuid(),
                        Image = pathUrl,
                        InsuranceType = model.InsuranceType,
                        Price = model.Price,
                        Rating = model.Rating
                    };
                    #endregion

                    #region add stock value
                    for (int i = 0; i < model.Stock; i++)
                    {
                        Stock stock = new Stock()
                        {
                            IsAvailable = 1,
                            Car = newCar,
                            UniqueKey = Guid.NewGuid()
                        };
                        newCar.Stocks.Add(stock);
                    }
                    #endregion

                    ViewData["successMessage"] = "Ok";

                    _carRepository.Add(newCar);
                    _unitOfWork.Commit();
                    //return RedirectToAction("CarList","Car")// this will be added when carlist add
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewData["errorMessage"] = ex.Message;
                    var exception = new Error()
                    {
                        DateCreated = DateTime.Now,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    };
                }
            }
            return View();
        }
    }
}