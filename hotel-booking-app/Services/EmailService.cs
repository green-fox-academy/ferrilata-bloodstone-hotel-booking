using HotelBookingApp.Models.EmailModels;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Utils.EmailHelpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IRoomService roomService;
        private ISendGridClient client;
        private readonly string apiKey;
        private readonly IBedService bedService;
        private readonly IMemoryCache memoryCache;

        public EmailService(IConfiguration configuration, IRoomService roomService, IBedService bedService, IMemoryCache memoryCache)
        {
            this.bedService = bedService;
            this.roomService = roomService;
            this.memoryCache = memoryCache;
            apiKey = configuration.GetConnectionString("SendGridApiKey");
        }

        public async Task SendEmailAsync(Reservation reservation, string userEmail)
        {
            var options = new SendGridClientOptions { ApiKey = apiKey};
            client = new SendGridClient(options);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("hotel-booking@bloodstone.com", "Hotel Booking"),
                Subject = "Reservation Confirmed",
                PlainTextContent = "Plain text not supported.",
                HtmlContent = await BuildConfirmEmailBody(reservation)
            };
            msg.AddTo(new EmailAddress(userEmail));
            Response response;
            try
            {
                response = await client.SendEmailAsync(msg);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("{0}", "SendEmailAsync Failed");
            }
        }

        public async Task<string> BuildConfirmEmailBody(Reservation reservation)
        {
            var template = "Templates.ReservationConfirmation";
            reservation.Room = await roomService.FindRoomWithAllProperties(reservation.RoomId);

            RazorParser renderer = new RazorParser(typeof(EmailClient).Assembly, memoryCache);
            var body = renderer.UsingTemplateFromEmbedded(template, new ReservationConfirmationEmail
            {
                Reservation = reservation,
                BedTypes = bedService.GetBedTypesAsString(reservation.Room.RoomBeds)
            });
            return body;
        }

        public async Task SendPasswordResetEmailAsync(string password, string userEmail)
        {
            var options = new SendGridClientOptions { ApiKey = apiKey };
            client = new SendGridClient(options);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("hotel-booking@bloodstone.com", "Hotel Booking"),
                Subject = "Password Reset",
                PlainTextContent = "Plain text not supported.",
                HtmlContent = BuildPasswordResetEmailBody(password)
            };
            msg.AddTo(new EmailAddress(userEmail));
            Response response;
            try
            {
                response = await client.SendEmailAsync(msg);
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("{0}", "SendEmailAsync Failed");
            }
        }

        public string BuildPasswordResetEmailBody(string password)
        {
            var template = "Templates.PasswordReset";

            RazorParser renderer = new RazorParser(typeof(EmailClient).Assembly, memoryCache);
            var body = renderer.UsingTemplateFromEmbedded(template, password);
            return body;
        }
    }
}
