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
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserModel> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public UserModel GetById(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(UserModel userParam)
        {
            throw new System.NotImplementedException();
        }
    }
}
