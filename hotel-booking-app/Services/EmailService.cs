using System.Text;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.EmailModels;
using HotelBookingApp.Models.HotelModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Services
{
    public class EmailService : IEmailService
    {
        private ISendGridClient client;
        private ApplicationContext context;
        //private SendGridClient client;
        private string apiKey;

        public EmailService(ApplicationContext context)
        {
            this.context = context;
            apiKey = System.Environment.GetEnvironmentVariable("SendGridApiKey");
        }

        public async Task<Response> SendMailAsync(Email email)
        {
            client = new SendGridClient("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");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(email.From),
                Subject = email.Subject,
                PlainTextContent = email.Body
            };
            msg.AddTo(new EmailAddress(email.To));
            var response = await client.SendEmailAsync(msg);
            return response;
        }

        public async Task<string> ConvertReservationToBodyAsync(Reservation reservation)
        {
            reservation.Room = context.Rooms
                .Include(room => room.RoomBeds)
                .ThenInclude(roomBed => roomBed.Bed)
                .Where(room => room.RoomId == reservation.RoomId)
                .FirstOrDefault();
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
            body.Append($"Bed type:{reservation.Room.RoomBeds.ToString()}\n");
            return body.ToString();
        }
    }
}
