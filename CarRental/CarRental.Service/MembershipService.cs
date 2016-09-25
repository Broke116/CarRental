using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Service.Interfaces;

namespace CarRental.Service
{
    public class MembershipService : IMembershipService
    {
        #region vars
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        public MembershipService(IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository,
            IBaseRepository<UserRole> userRoleRepository, IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        public MembershipContext ValidateUser(string username, string password)
        {
            /*var membershipContext = new MembershipContext();

            var user = _userRepository.GetAll().FirstOrDefault(x => x.Username == username);
            if (user != null)
            {
                
            }*/
            throw new System.NotImplementedException();
        }

        public User CreateUser(string username, string email, string password, int[] roles)
        {
            var existingUser = _userRepository.GetAll().FirstOrDefault(x => x.Username == username);

            if (existingUser != null)
            {
                throw new Exception("Username is already in use");
            }

            var passwordSalt = _encryptionService.CreateSalt();

            var user = new User()
            {
                Username = username,
                Salt = passwordSalt,
                Email = email,
                IsLocked = 0,
                Password = _encryptionService.EncryptPassword(password, passwordSalt),
                CreatedDate = DateTime.Now
            };

            _userRepository.Add(user);
            _unitOfWork.Commit();

            if (roles != null || roles.Length > 0)
            {
                foreach (var role in roles)
                {
                    addUserToRole(user, role);
                }
            }

            _unitOfWork.Commit();

            return user;
        }

        public User GetUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public List<Role> GetUserRoles(string username)
        {
            throw new System.NotImplementedException();
        }

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.Password);
        }

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
    }
}
