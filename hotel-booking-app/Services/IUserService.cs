using HotelBookingApp.Models;
using System.Collections.Generic;

namespace HotelBookingApp.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        UserModel GetById(long id);
        UserModel Create(UserModel user);
        void Update(UserModel userParam);
        void Delete(long id);
    }
}
