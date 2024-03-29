using HotelBookingApp.Models.Account;
using HotelBookingApp.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HotelBookingApp.Models.HotelModels
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarRating { get; set; }
        public string TimeZoneId { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public PropertyType PropertyType { get; set; }
        public int PropertyTypeId { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public PaginatedList<Review> Reviews { get; set; }
        public bool Thumbnail { get; set; } = false;
        public string ThumbnailUrl { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public string ShortDescription
        {
            get
            {
                int maxLength = 300;
                return Description != null && Description.Length > maxLength
                    ? Description.Substring(0, maxLength) + "..."
                    : Description;
            }
        }

        public double AvgReviewRating
        {
            get
            {
                if (Reviews == null || !Reviews.Any())
                {
                    return 0;
                }
                return Reviews.Average(r => r.Rating);
            }
        }
    }
}
