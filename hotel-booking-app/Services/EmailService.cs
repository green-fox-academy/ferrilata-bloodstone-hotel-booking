using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRoomService roomService;
        private ISendGridClient client;
        private readonly ApplicationContext context;
        private readonly string apiKey;
        private readonly IBedService bedService;

        public EmailService(ApplicationContext context, IConfiguration configuration, IRoomService roomService, IBedService bedService)
        {
            this.bedService = bedService;
            this.roomService = roomService;
            this.context = context;
            apiKey = configuration.GetConnectionString("SendGridApiKey");
        }

        public async Task<Response> SendEmailAsync(Reservation reservation, string userEmail)
        {
            var options = new SendGridClientOptions { ApiKey = apiKey};
            client = new SendGridClient(options);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("hotel-booking@bloodstone.com", "Hotel Booking"),
                Subject = "Reservation Confirmed",
                PlainTextContent = await ConvertReservationToPlainTextAsync(reservation),
                HtmlContent = await ConvertReservationToHtmlAsync(reservation)
            };
            msg.AddTo(new EmailAddress(userEmail));
            msg.SetFooterSetting(
                     true,
                     html: "<hr><div>© 2019 - Hotel Booking</div>",
                     text: "© 2019 - Hotel Booking");
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public async Task<string> ConvertReservationToHtmlAsync(Reservation reservation)
        {
            var body = new StringBuilder();
            reservation.Room = await roomService.FindRoomWithAllProperties(reservation.RoomId);
            body.Append($"Thank you for your booking!");
            body.Append("<br><br>");
            body.Append("Your reservation was successful!");
            body.Append("<br><br>");
            body.Append("<div>");
            body.Append("<div style=\"" + "float: left; margin - right: 5px;" + "\">");
            body.Append("<img src=\"" + reservation.Room.Hotel.ThumbnailUrl + "\" width=\"" + 96 + "\" + /></div><br><h3>" + reservation.Room.Hotel.Name + "</h3> ");
            body.Append("<br>");
            body.Append($"<b>Address:</b> " +
                $"{reservation.Room.Hotel.Location.Address}, " +
                $"{reservation.Room.Hotel.Location.City}, " +
                $"{reservation.Room.Hotel.Location.Country}");
            body.Append("</div><br><br>");
            body.Append($"<b>Room:</b> {reservation.Room.Name}");
            body.Append("<br>");
            body.Append($"<b>Number of guests:</b> {reservation.GuestNumber} person(s)");
            body.Append("<br>");
            body.Append($"<b>Guests:</b> {reservation.GuestNames}");
            body.Append("<br>");
            body.Append($"<b>Number of nights:</b> {reservation.NumberOfNights}");
            body.Append("<br>");
            body.Append($"<b>Check-in on:</b> {reservation.FromDate} ({reservation.FromDate.DayOfWeek})");
            body.Append("<br>");
            body.Append($"<b>Check-out on:</b> {reservation.ToDate} ({reservation.ToDate.DayOfWeek})");
            body.Append("<br><br>");
            body.Append($"<b>Details of the room:</b>");
            body.Append("<br>");
            body.Append($"<b>Name:</b> {reservation.Room.Name}");
            body.Append("<br>");
            body.Append($"<b>Price</b>: ${reservation.Room.Price}");
            body.Append("<br>");
            body.Append($"<b>Bed type:</b> {bedService.GetBedTypesAsString(reservation.Room.RoomBeds)}");
            body.Append("<br>");
            return body.ToString();
        }

        public async Task<string> ConvertReservationToPlainTextAsync(Reservation reservation)
        {
            var body = new StringBuilder();
            reservation.Room = await roomService.FindRoomWithAllProperties(reservation.RoomId);
            body.Append($"Thank you for your booking!");
            body.Append("\n\n\n");
            body.Append("Your reservation was successful!");
            body.Append("\n\n\n");
            body.Append($"Hotel: {reservation.Room.Hotel.Name}");
            body.Append("\n\n");
            body.Append($"Address: " +
                $"{reservation.Room.Hotel.Location.Address}, " +
                $"{reservation.Room.Hotel.Location.City}, " +
                $"{reservation.Room.Hotel.Location.Country}");
            body.Append("\n\n\n");
            body.Append($"Room: {reservation.Room.Name}");
            body.Append("\n\n");
            body.Append($"Number of guests: {reservation.GuestNumber} person(s)");
            body.Append("\n\n");
            body.Append($"Guests: {reservation.GuestNames}");
            body.Append("\n\n");
            body.Append($"Number of nights: {reservation.NumberOfNights}");
            body.Append("\n\n");
            body.Append($"Check-in on: {reservation.FromDate} ({reservation.FromDate.DayOfWeek})");
            body.Append("\n\n");
            body.Append($"Check-out on: {reservation.ToDate} ({reservation.ToDate.DayOfWeek})");
            body.Append("\n\n\n");
            body.Append($"Details of the room:");
            body.Append("\n\n");
            body.Append($"Name: {reservation.Room.Name}");
            body.Append("\n\n\n");
            body.Append($"Price: ${reservation.Room.Price}");
            body.Append("\n\n\n");
            body.Append($"Bed type: {bedService.GetBedTypesAsString(reservation.Room.RoomBeds)}");
            body.Append("\n\n");
            return body.ToString();
        }
    }
}
