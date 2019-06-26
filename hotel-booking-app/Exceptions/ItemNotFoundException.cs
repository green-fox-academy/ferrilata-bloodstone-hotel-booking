using System;

namespace HotelBookingApp.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException()
        {
        }

        public ItemNotFoundException(string message) : base(message)
        {
        }

        public ItemNotFoundException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
