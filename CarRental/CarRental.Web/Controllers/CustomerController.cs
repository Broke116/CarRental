using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Web.Models.ViewModel;

namespace CarRental.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IBaseRepository<Customer> _customerRepository;

        public CustomerController(IBaseRepository<Customer> customerRepository,
            IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _customerRepository = customerRepository;
        }

        public ActionResult Index()
        {
            var customer = _customerRepository.GetAll()
                .OrderByDescending(m => m.CreatedDate)
                .Select(c => new CustomerViewModel
                {
                    ID = c.ID,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    Email = c.Email,
                    Gender = c.Gender,
                    Location = c.Location,
                    City = c.City,
                    Country = c.Country
                }).ToList();

            return View(customer);
        }

        [HttpGet]
        public ActionResult GetCustomerDetail(int id)
        {
            var customer = _customerRepository.GetAll()
                .Where(c => c.ID == id).Select(c => new CustomerViewModel
                {
                    ID = c.ID,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    Email = c.Email,
                    Gender = c.Gender,
                    Mobile = c.Mobile,
                    Location = c.Location,
                    City = c.City,
                    Country = c.Country,
                    BirthDate = c.BirthDate,
                    CitizenNumber = c.CitizenNumber,
                    PassportNumber = c.PassportNumber,
                    LicenseOrigin = c.LicenseOrigin,
                    UniqueKey = c.UniqueKey
                }).FirstOrDefault();

            /*var customer = _customerRepository.FindBy(c => c.ID == id).FirstOrDefault();
            var model = Mapper.Map<Customer, CustomerViewModel>(customer);*/

            return PartialView("Partials/_EditCustomerPartial", customer);
        }

        [HttpPost]
        public ActionResult UpdateCustomerInfo(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updatedCustomer = new Customer
                    {
                        ID = model.ID,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Email = model.Email,
                        Gender = model.Gender,
                        Mobile = model.Mobile,
                        Location = model.Location,
                        City = model.City,
                        Country = model.Country,
                        BirthDate = model.BirthDate,
                        CitizenNumber = model.CitizenNumber,
                        PassportNumber = model.PassportNumber,
                        LicenseOrigin = model.LicenseOrigin,
                        UniqueKey = model.UniqueKey
                    };

                    _customerRepository.Update(updatedCustomer);
                    _unitOfWork.Commit();
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
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["errorMessage"] = errors;
                return PartialView("Partials/_EditCustomerPartial", model);
            }
            return RedirectToAction("Index", "Customer"); 
        }

        #region register customer
        public ActionResult RegisterCustomer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCustomer(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    #region if customer still exists, it won't add

                    var customer = _customerRepository.GetAll().FirstOrDefault(c => c.Email == model.Email);
                    if (customer != null)
                    {
                        ViewData["errorMessage"] = "customer already exists";
                        return View(model);
                    }

                    #endregion

                    #region create a new customer
                    var newCustomer = new Customer()
                    {
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Email = model.Email,
                        CitizenNumber = model.CitizenNumber,
                        BirthDate = model.BirthDate,
                        City = model.City,
                        Country = model.Country,
                        Gender = model.Gender,
                        LicenseOrigin = model.LicenseOrigin,
                        Mobile = model.Mobile,
                        PassportNumber = model.PassportNumber,
                        Location = model.Location,
                        UniqueKey = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };
                    #endregion

                    ViewData["errorMessage"] = null;
                    ViewData["successMessage"] = "Ok";

                    _customerRepository.Add(newCustomer);
                    _unitOfWork.Commit();
                    return RedirectToAction("Index", "Customer");
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
            else
            {
                var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            }

            return View(model);
        }
        #endregion
    }
}