using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Service.Interfaces;
using CarRental.Web.Models.ViewModel;
using Newtonsoft.Json;

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
                var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == model.Email);
                if (user != null && IsUserValid(user, model.Password))
                {
                    List<Role> listRole = new List<Role>();
                    var userRoles = GetUserRoles(user.Username);
                    var val = userRoles.Select(r => r.Name);
                    
                    var rolesArray = new string[] { val.ToString() };
                    int i = 5;

                    var principalModel = new PrincipalModel
                    {
                        ID = user.ID,
                        Email = user.Email,
                        Username = user.Username,
                        Roles = userRoles.Select(x => x.Name).ToArray()
                    };

                    var userData = JsonConvert.SerializeObject(principalModel, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    var authenticationTicket = new FormsAuthenticationTicket(
                        1,
                        user.Username,
                        DateTime.Now,
                        DateTime.Now.AddHours(1),
                        model.RememberMe,
                        userData);

                    var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
                    var httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    Response.Cookies.Add(httpCookie);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
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

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        public List<Role> GetUserRoles(string username)
        {
            List<Role> _result = new List<Role>();
            var existingUser = _userRepository.GetAll().FirstOrDefault(x => x.Username == username);

            if (existingUser != null)
            {
                foreach (var userRole in existingUser.UserRoles)
                {
                    _result.Add(userRole.Role);
                }
            }

            return _result.Distinct().ToList();
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

        private bool IsPasswordValid(User user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.Password);
        }

        private bool IsUserValid(User user, string password)
        {
            if (IsPasswordValid(user, password))
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}