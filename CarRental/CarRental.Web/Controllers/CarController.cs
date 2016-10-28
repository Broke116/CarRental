using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
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

        #region add car section
        public ActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCar(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #region if car still exists, it won't add
                    var car = _carRepository.GetAll().FirstOrDefault(c => c.Title == model.Title);
                    if (car != null)
                    {
                        ViewData["errorMessage"] = "car already exists";
                        return View(model);
                    }
                    #endregion

                    #region car image upload
                    var pathUrl = "";

                    if (model.Image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.Image.FileName);
                        if (fileName != null)
                        {
                            var path = Path.Combine(Server.MapPath("~/Content/images/uploads"), fileName);

                            var absolutePath = Path.GetFullPath(path);
                            if (System.IO.File.Exists(absolutePath)) // if file exists throw an error
                            {
                                ViewData["errorMessage"] = "This file already exists";
                                return View();
                            }
                            model.Image.SaveAs(absolutePath);
                            pathUrl = path.Substring(path.LastIndexOf("\\") + 1);
                        }
                    }
                    #endregion

                    #region capacity string to int conversion
                    /*int capacity = 0;
                    switch (model.Capacity)
                    {
                        case "Two":
                            capacity = 2;
                            break;
                        case "Four":
                            capacity = 4;
                            break;
                        case "Five":
                            capacity = 5;
                            break;
                        default:
                            capacity = 9;
                            break;
                    }*/
                    #endregion

                    #region car values added to entity model
                    var newCar = new Car // image stock
                    {
                        Title = model.Title,
                        Capacity = ConvertCapacity(model),
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
                        Rating = model.Rating,
                        CreatedDate = DateTime.Now
                    };
                    #endregion

                    #region add stock value
                    Stock stock = new Stock()
                    {
                        IsAvailable = 1,
                        Quantity = model.Stock,
                        CarId = newCar.ID,
                        UniqueKey = Guid.NewGuid()
                    };
                    newCar.Stocks.Add(stock);
                    #endregion

                    ViewData["successMessage"] = "Ok";

                    _carRepository.Add(newCar);
                    _unitOfWork.Commit();
                    return RedirectToAction("Index", "Car");
                    //return RedirectToAction("Index", "Home");
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
                    _errorsRepository.Add(exception);
                    _unitOfWork.Commit();
                }
            }
            return View();
        }
        #endregion

        public ActionResult Detail(int id)
        {
            CarViewModel model;
            var car = _carRepository.FindBy(c => c.ID == id).FirstOrDefault();
            if (car != null)
            {
                model = new CarViewModel()
                {
                    Id = car.ID,
                    Title = car.Title,
                    Capacity = car.Capacity.ToString(),
                    Location = car.Location,
                    City = car.City,
                    County = car.County,
                    Description = car.Description,
                    FuelType = car.FuelType,
                    GearType = car.GearType,
                    GroupType = car.GroupType,
                    ImageUrl = car.Image,
                    InsuranceType = car.InsuranceType,
                    Price = car.Price,
                    Rating = car.Rating,
                    GetStock = car.Stocks,
                    CreatedDate = car.CreatedDate
                };
            }
            else
            {
                throw new NotImplementedException();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #region image

                    var pathUrl = "";

                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.Image.FileName);
                        if (fileName != null)
                        {
                            var path = Path.Combine(Server.MapPath("~/Content/images/uploads"), fileName);

                            var absolutePath = Path.GetFullPath(path);
                            if (System.IO.File.Exists(absolutePath)) // if file exists throw an error
                            {
                                ViewData["errorMessage"] = "This file already exists";
                                return View();
                            }
                            model.Image.SaveAs(absolutePath);
                            pathUrl = path.Substring(path.LastIndexOf("\\") + 1);
                        }
                    }

                    #endregion

                    #region updatedcar
                    var updatedCar = new Car
                    {
                        Title = model.Title,
                        Capacity = ConvertCapacity(model),
                        Location = model.Location,
                        City = model.City,
                        County = model.County,
                        Description = model.Description,
                        FuelType = model.FuelType,
                        GearType = model.GearType,
                        GroupType = model.GroupType,
                        Image = pathUrl,
                        InsuranceType = model.InsuranceType,
                        Price = model.Price,
                        Rating = model.Rating,
                        CreatedDate = model.CreatedDate
                    };
                    #endregion

                    #region add stock value

                    Stock stock = new Stock()
                    {
                        IsAvailable = 1,
                        Quantity = model.Stock,
                        CarId = updatedCar.ID,
                        UniqueKey = Guid.NewGuid()
                    };
                    updatedCar.Stocks.Add(stock);

                    #endregion

                    ViewData["successMessage"] = "Ok";

                    _carRepository.Update(updatedCar);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index", "Home");
                }
                catch (ValidationException ex)
                {
                    ViewData["errorMessage"] = ex.Message;
                    var exception = new Error()
                    {
                        DateCreated = DateTime.Now,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    };
                    _errorsRepository.Add(exception);
                    _unitOfWork.Commit();
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["errorMessage"] = errors;
                return View();
            }
            return View(model);
        }

        #region helpers
        public int ConvertCapacity(CarViewModel model)
        {
            int capacity;
            switch (model.Capacity)
            {
                case "Two":
                    capacity = 2;
                    break;
                case "Four":
                    capacity = 4;
                    break;
                case "Five":
                    capacity = 5;
                    break;
                default:
                    capacity = 9;
                    break;
            }
            return capacity;
        }
        
        #endregion
    }
}