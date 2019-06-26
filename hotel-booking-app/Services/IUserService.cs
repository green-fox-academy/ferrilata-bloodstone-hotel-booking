using HotelBookingApp.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAll();
        Task<UserModel> GetById(long id);
        Task Create(UserModel user);
        Task Update(UserModel userParam);
        Task Delete(long id);
        bool Exists(string email);
    }
}
