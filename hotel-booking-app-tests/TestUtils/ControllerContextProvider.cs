using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBookingAppTests.TestUtils
{
    class ControllerContextProvider
    {
        public static ControllerContext GetDefault()
        {
            return GetDefault("Username", "userid");
        }

        public static ControllerContext GetDefault(string userName, string id)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, id)
            }, "mock"));

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}
