using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Services
{
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public UserViewModel AddUserDetails(UserViewModel user)
        {
            try
            {
                //Add new user
                var userdtl = _mapper.Map<UserViewModel, User>(user);
                UnitOfWork.User.Add(userdtl);
                UnitOfWork.Complete();

                _logger.LogDebug("User details saved successfully for ID: " + user.Id);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserService/AddUserDetails failed with an exception: " + ex.Message);
                return null;
            }
        }

        public async Task<UserViewModel> GetUserDetails(string Username)
        {
            try
            {
                var UserVM = new UserViewModel();
               var User = await UnitOfWork.User.GetUserDetailsByUserName(Username);

                if (User != null)
                {
                    UserVM.Id = User.Id;
                    UserVM.Username = User.Username;
                    UserVM.PasswordHash = User.PasswordHash;
                    UserVM.Email = User.Email;
                }
                    return UserVM;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserService/GetUserDetails failed with an exception : " + ex.Message);
                return null;
            }
        }

        public async Task<bool> CheckUserNameExists(string username)
        {
            try
            {
                var UserVM = new UserViewModel();
                var User = await UnitOfWork.User.GetUserDetailsByUserName(username);

                if (User != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserService/CheckUserNameExists failed with an exception : " + ex.Message);
                return false;
            }
        }
    }
}
