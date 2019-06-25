using HotelBookingApp.Models;
using System.Collections.Generic;
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

        public async void Create(UserModel user)
        {
            UserModel userInDb = await applicationContext.Users
                .FindAsync(user.Email);

            if (userInDb == null)
            {
                await applicationContext.Users.AddAsync(user);
                long id = await applicationContext.SaveChangesAsync();
            }
            else
            {
                throw new System.Exception($"User with email {user.Email} already exists.");
            }
        }

        public async void Delete(long id)
        {
            applicationContext.Users.Remove(GetById(id).Result);
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
                ?? throw new KeyNotFoundException($"User with {id} is not found.");
        }

        public async void Update(UserModel userParam)
        {
            applicationContext.Users.Update(userParam);
            long id = await applicationContext.SaveChangesAsync();
        }
    }
}
