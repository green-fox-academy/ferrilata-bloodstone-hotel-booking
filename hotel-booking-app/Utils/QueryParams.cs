using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Utils
{
    public class QueryParams
    {
        public string OrderBy { get; set; } = "Name";
        public bool Desc { get; set; } = false;
        public int currentPage { get; set; } = 1;
    }
}
