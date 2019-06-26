using System;

namespace HotelBookingApp.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
