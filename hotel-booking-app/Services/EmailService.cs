using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class EmailService : IEmailService
    {
        private ISendGridClient client;
        private ApplicationContext context;
        private string apiKey;

        public EmailService(ApplicationContext context, IConfiguration configuration)
        {
            this.context = context;
            apiKey = configuration.GetConnectionString("SendGridApiKey");
        }

        public async Task<Response> SendMailAsync(Reservation reservation, string userEmail)
        {
            var options = new SendGridClientOptions();
            options.ApiKey = apiKey;
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
                     "Some Footer HTML",
                     "<strong><hr><div>© 2019 - Hotel Booking</div></strong>");
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public async Task<string> ConvertReservationToHtmlAsync(Reservation reservation)
        {
            reservation.Room = await context.Rooms
                .Include(room => room.Hotel)
                .ThenInclude(hotel => hotel.Location)
                .Include(room => room.RoomBeds)
                .ThenInclude(roomBed => roomBed.Bed)
                .Where(room => room.RoomId == reservation.RoomId)
                .FirstOrDefaultAsync();
            var body = new StringBuilder();
            body.Append($"Thank you for your booking!");
            body.Append("<br><br>");
            body.Append("Your reservation was successful!");
            body.Append("<br><br><div>");
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
            body.Append($"<b>Bed type:</b> {GetBedTypes(reservation.Room.RoomBeds)}");
            body.Append("<br>");
            return body.ToString();
        }

        public async Task<string> ConvertReservationToPlainTextAsync(Reservation reservation)
        {
            reservation.Room = await context.Rooms
                .Include(room => room.RoomBeds)
                .ThenInclude(roomBed => roomBed.Bed)
                .Where(room => room.RoomId == reservation.RoomId)
                .FirstOrDefaultAsync();
            var body = new StringBuilder();
            body.Append($"Thank you for your booking!\n\n");
            body.Append("Your reservation was successful!\n\n");
            body.Append($"Room: {reservation.Room.Name}\n");
            body.Append($"Number of guests: {reservation.GuestNumber} person\n");
            body.Append($"Guests: {reservation.GuestNames}\n");
            body.Append($"Number of nights: {reservation.NumberOfNights}\n");
            body.Append($"Check-in on: {reservation.FromDate} ({reservation.FromDate.DayOfYear})\n");
            body.Append($"Check-out on: {reservation.ToDate} ({reservation.ToDate.DayOfYear})\n");
            body.Append($"Details of the room:\n");
            body.Append($"Name: {reservation.Room.Name}\n");
            body.Append($"Price: {reservation.Room.Price}\n");
            body.Append($"Bed type: {GetBedTypes(reservation.Room.RoomBeds)}");
            return body.ToString();
        }

        private string GetBedTypes(IEnumerable<RoomBed> roomBeds)
        {
            var result = new List<string>();
            roomBeds.ToList().ForEach(bed => result.Add($"{bed.BedNumber} {bed.Bed.Type}"));
            var list = string.Join(",", result.ToArray());
            return list;
        }
    }
}
