using System;
using System.Linq;
using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Service.Interfaces;
using CarRental.Web.Models.ViewModel;

namespace CarRental.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region variables
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IEncryptionService _encryptionService;
        #endregion

        public AccountController(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository,
            IBaseRepository<UserRole> userRoleRepository, IEncryptionService encryptionService,
            IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _encryptionService = encryptionService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model;
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _userRepository.GetAll().FirstOrDefault(x => x.Username == model.Username);

                if (existingUser != null)
                {
                    throw new Exception("Username is already in use");
                }

                var passwordSalt = _encryptionService.CreateSalt();

                var user = new User()
                {
                    Username = model.Username,
                    Salt = passwordSalt,
                    Email = model.Email,
                    IsLocked = 0,
                    Password = _encryptionService.EncryptPassword(model.Password, passwordSalt),
                    CreatedDate = DateTime.Now,
                    UniqueId = Guid.NewGuid()
                };

                _userRepository.Add(user);
                _unitOfWork.Commit();

                int[] roles = new int[] {3};

                if (roles != null || roles.Length > 0)
                {
                    foreach (var role in roles)
                    {
                        addUserToRole(user, role);
                    }
                }

                _unitOfWork.Commit();
            }

            return View(model);
        }

        #region extension methods
        private void addUserToRole(User user, int roleId)
        {
            var role = _roleRepository.GetSingle(roleId);
            if (role == null)
                throw new ApplicationException("Role doesn't exist.");

            var userRole = new UserRole()
            {
                RoleId = role.ID,
                UserId = user.ID
            };
            _userRoleRepository.Add(userRole);
        }
        #endregion
    }

}