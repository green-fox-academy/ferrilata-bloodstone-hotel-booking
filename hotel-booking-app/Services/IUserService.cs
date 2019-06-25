using HotelBookingApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        Task<UserModel> GetById(long id);
        UserModel Create(UserModel user);
        void Update(UserModel userParam);
        void Delete(long id);
    }
}
