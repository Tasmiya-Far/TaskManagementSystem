using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.Intefaces;
using TaskManagementSystem.Core.Dbcontext;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
        }
        


        public async Task<User> GetUserDetailsByUserName(string username)
        {
            var user = new User();
            try
            {
                user = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserRepository/GetUserDetailsByUserName failed with an exception: " + ex.Message);
                return user;
            }
        }

        public async Task<User> GetUserDetailsByID(Guid id)
        {
            var user = new User();
            try
            {
                user = await _context.Users.Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("UserRepository/GetUserDetailsByID failed with an exception: " + ex.Message);
                return user;
            }
        }
    }
}
