using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserDetails(string Username);
        Task<bool> CheckUserNameExists(string username);
        UserViewModel AddUserDetails(UserViewModel user);
    }
}
