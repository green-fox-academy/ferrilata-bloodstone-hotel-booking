using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IDateTimeService
    {
       IEnumerable<TimeZoneInfo> GetTimeZones();
    }
}
