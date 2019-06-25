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

        public UserModel Create(UserModel user)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            applicationContext.Users.Remove(GetById(id).Result);
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
            await applicationContext.SaveChangesAsync();
        }
    }
}
