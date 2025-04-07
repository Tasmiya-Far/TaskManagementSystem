using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Core.Intefaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserDetailsByID(Guid id);
        Task<User> GetUserDetailsByUserName(string username);
    }
}
