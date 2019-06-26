using HotelBookingApp.Models.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext applicationContext;

        public UserService(ApplicationContext context)
        {
            this.applicationContext = context;
        }

        public async Task Create(UserModel user)
        {
            var userList = applicationContext.Users.ToList();
            if (Exists(user.Email))
            {
                throw new System.Exception($"User with email {user.Email} already exists.");
            }
            await applicationContext.Users.AddAsync(user);
            long id = await applicationContext.SaveChangesAsync();
        }

        public bool Exists(string email)
        {
            return applicationContext.Users.Where(u => u.Email == email)
                .FirstOrDefault() == null ? false : true;
        }

        public async Task Delete(long id)
        {
            UserModel user = applicationContext.Users
                .Where(u => u.Id == id)
                .SingleOrDefault();
            applicationContext.Users.Remove(user);
            await applicationContext.SaveChangesAsync();
        }

        public IEnumerable<UserModel> GetAll()
        {
            return applicationContext.Users;
        }

        public async Task<UserModel> GetById(long id)
        {
            return await applicationContext.Users
                .FindAsync(id)
                ?? throw new KeyNotFoundException($"User with id: {id} is not found.");
        }

        public async Task Update(UserModel userParam)
        {
            applicationContext.Users.Update(userParam);
            long id = await applicationContext.SaveChangesAsync();
        }
    }
}
