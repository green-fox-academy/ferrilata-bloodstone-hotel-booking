using HotelBookingApp.Models.Account;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.HotelModels
{
    public class Review
    {
        public const int MIN_RATING = 1;
        public const int DEFAULT_RATING = 5;
        public const int MAX_RATING = 10;
        public const int MAX_COMMENT_LEN = 200;

        public int ReviewId { get; set; }

        [StringLength(MAX_COMMENT_LEN)]
        public string Comment { get; set; }

        [Required]
        [Range(MIN_RATING, MAX_RATING)]
        public int Rating { get; set; } = DEFAULT_RATING;
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
