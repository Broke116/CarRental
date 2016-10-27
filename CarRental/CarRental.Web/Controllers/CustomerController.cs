using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

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
                        UniqueKey = Guid.NewGuid()
                    };

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

            return View(model);
        }
    }
}